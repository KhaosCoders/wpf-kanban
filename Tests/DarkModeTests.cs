using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using KC.WPF_Kanban;
using KC.WPF_Kanban.Converter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WPF_Kanban.Tests;

[TestClass]
public class DarkModeTests
{
    private static readonly DarkModeBrushConverter Converter = new DarkModeBrushConverter();

    [TestMethod]
    public void SolidColorPairsMeetContrastTarget() => OnSta(() =>
    {
        Color[] colors =
        [
            Colors.Black, Colors.White, Colors.Red, Colors.Blue, Colors.Lime,
            Colors.Yellow, Colors.Aquamarine, Colors.AliceBlue, Colors.DarkGoldenrod,
            Color.FromRgb(232, 232, 232), Color.FromRgb(240, 244, 251),
            Color.FromRgb(252, 48, 48), Color.FromRgb(252, 88, 88),
            Color.FromRgb(227, 43, 43), Color.FromRgb(255, 224, 204)
        ];
        foreach (Color background in colors)
        {
            var darkBackground = Transform(new SolidColorBrush(background));
            Assert.IsTrue(Luminance(darkBackground.Color) < 0.09);
            foreach (Color foreground in colors)
            {
                var darkForeground = Transform(new SolidColorBrush(foreground), DarkModeBrushRole.Foreground);
                AssertContrast(darkForeground, darkBackground);
            }
        }
    });

    [TestMethod]
    public void BrushesAreProjectedWithoutChangingSources() => OnSta(() =>
    {
        var source = new SolidColorBrush(Color.FromArgb(123, 250, 90, 30))
        {
            Opacity = 0.6,
            Transform = new TranslateTransform(1, 2)
        };
        Color original = source.Color;
        var dark = Transform(source);
        Assert.AreEqual(original, source.Color);
        Assert.AreEqual(original.A, dark.Color.A);
        Assert.AreEqual(source.Opacity, dark.Opacity);
        Assert.AreEqual(source.Transform.Value, dark.Transform.Value);
        Assert.IsTrue(dark.IsFrozen);
        Assert.AreSame(source, Convert(source, false));
        Assert.AreEqual(dark.Color, Transform(source).Color);
        Assert.AreEqual(Colors.Transparent, Transform(Brushes.Transparent).Color);
        Assert.IsNull(Convert(null, true));

        source.Freeze();
        Assert.AreEqual(dark.Color, Transform(source).Color);
        var image = new ImageBrush();
        var visual = new VisualBrush();
        Assert.AreSame(image, Convert(image, true));
        Assert.AreSame(visual, Convert(visual, true));
    });

    [TestMethod]
    public void BothGradientTypesPreserveGeometryAndAlpha() => OnSta(() =>
    {
        GradientBrush[] gradients =
        [
            new LinearGradientBrush(Colors.Red, Colors.Blue, 35),
            new RadialGradientBrush(Colors.Yellow, Colors.Green)
            {
                Center = new Point(0.3, 0.4), RadiusX = 0.7, RadiusY = 0.8
            }
        ];
        foreach (GradientBrush original in gradients)
        {
            original.Opacity = 0.7;
            original.Transform = new RotateTransform(15);
            original.GradientStops[0].Color = Color.FromArgb(80, 255, 0, 0);
            var copy = (GradientBrush)Convert(original, true);
            Assert.AreEqual(original.GetType(), copy.GetType());
            Assert.AreEqual(original.Opacity, copy.Opacity);
            Assert.AreEqual(original.Transform.Value, copy.Transform.Value);
            Assert.AreEqual(original.GradientStops.Count, copy.GradientStops.Count);
            Assert.AreEqual(original.GradientStops[0].Offset, copy.GradientStops[0].Offset);
            Assert.AreEqual(original.GradientStops[0].Color.A, copy.GradientStops[0].Color.A);
            Assert.AreNotEqual(original.GradientStops[0].Color, copy.GradientStops[0].Color);
            if (original is LinearGradientBrush line)
            {
                Assert.AreEqual(line.StartPoint, ((LinearGradientBrush)copy).StartPoint);
                Assert.AreEqual(line.EndPoint, ((LinearGradientBrush)copy).EndPoint);
            }
            if (original is RadialGradientBrush radial)
            {
                Assert.AreEqual(radial.Center, ((RadialGradientBrush)copy).Center);
                Assert.AreEqual(radial.RadiusX, ((RadialGradientBrush)copy).RadiusX);
                Assert.AreEqual(radial.RadiusY, ((RadialGradientBrush)copy).RadiusY);
            }
        }
    });

    [TestMethod]
    public void ConverterRejectsInvalidConfigurationAndHandlesBindingInitialization() => OnSta(() =>
    {
        Assert.AreSame(DependencyProperty.UnsetValue,
            Converter.Convert([DependencyProperty.UnsetValue, true], typeof(Brush),
                DarkModeBrushRole.Background, CultureInfo.InvariantCulture));
        Assert.ThrowsException<ArgumentException>(() =>
            Converter.Convert(["invalid", true], typeof(Brush),
                DarkModeBrushRole.Background, CultureInfo.InvariantCulture));
        Assert.ThrowsException<ArgumentException>(() =>
            Converter.Convert([Brushes.Red, true], typeof(Brush), null, CultureInfo.InvariantCulture));
    });

    [TestMethod]
    public void ModeIsBindableAndBoardsAreIndependent() => OnSta(() =>
    {
        var board = new KanbanBoard();
        var sibling = new KanbanBoard();
        var nested = new KanbanBoard();
        var content = new StackPanel();
        var outer = new Border { Child = content };
        content.Children.Add(nested);
        KanbanBoard.SetIsDarkMode(outer, true);
        Assert.IsFalse(board.IsDarkMode);
        Assert.IsFalse(sibling.IsDarkMode);
        Assert.IsFalse(nested.IsDarkMode);
        Assert.IsFalse(KanbanBoard.GetIsDarkModeActive(nested));
        Assert.IsTrue(KanbanBoard.GetIsDarkModeActive(content));
        var styled = new KanbanBoard
        {
            Style = new Style(typeof(KanbanBoard))
            {
                Setters = { new Setter(KanbanBoard.IsDarkModeProperty, true) }
            }
        };
        Assert.IsTrue(styled.IsDarkMode);
        Assert.IsTrue(KanbanBoard.GetIsDarkModeActive(styled));

        var source = new CheckBox();
        board.SetBinding(KanbanBoard.IsDarkModeProperty, new Binding("IsChecked") { Source = source });
        source.IsChecked = true;
        Pump();
        Assert.IsTrue(board.IsDarkMode);
        source.IsChecked = false;
        Pump();
        Assert.IsFalse(board.IsDarkMode);
        Assert.IsTrue(BindingOperations.IsDataBound(board, KanbanBoard.IsDarkModeProperty));
    });

    [TestMethod]
    public void JsonRoundtripAndOldModelsPreserveModeAndOriginalColors() => OnSta(() =>
    {
        var board = new KanbanBoard { IsDarkMode = true };
        board.Columns.Add(new KanbanColumn { Caption = "Test", Color = Brushes.Blue });
        string json = board.SaveModel();
        var restored = new KanbanBoard();
        restored.LoadJsonModel(json);
        Assert.IsTrue(restored.IsDarkMode);
        Assert.AreEqual(Colors.Blue, ((SolidColorBrush)restored.Columns[0].Color).Color);
        restored.LoadJsonModel("{\"Title\":\"Old model\"}");
        Assert.IsTrue(restored.IsDarkMode);
        restored.LoadJsonModel("{\"IsDarkMode\":false}");
        Assert.IsFalse(restored.IsDarkMode);
        board.IsDarkMode = false;
        restored.LoadJsonModel(board.SaveModel());
        Assert.IsFalse(restored.IsDarkMode);
    });

    [TestMethod]
    public void CardTemplateUpdatesModeBindingsAndMutableBrushes() => OnSta(() =>
    {
        var card = new KanbanCard
        {
            DescriptionText = "Description", LeftTitle = "Title", TileText = "A",
            Background = Brushes.White, Foreground = Brushes.Black,
            TitleBackground = Brushes.Aquamarine, TileBrush = Brushes.Blue
        };
        var source = new Border { Background = new SolidColorBrush(Colors.Red) };
        card.SetBinding(KanbanCard.TitleBackgroundProperty,
            new Binding("Background") { Source = source });
        var host = new Border { Child = card };
        WithWindow(host, () =>
        {
            var root = (Border)card.Template.FindName("Border", card);
            Assert.AreEqual(Colors.White, ((SolidColorBrush)root.Background).Color);
            KanbanBoard.SetIsDarkMode(host, true);
            Pump();
            Assert.AreEqual(Transform(Brushes.White).Color, ((SolidColorBrush)root.Background).Color);
            AssertContrast((SolidColorBrush)TextElement.GetForeground(root), (SolidColorBrush)root.Background);
            var title = (Border)card.Template.FindName("TitleBorder", card);
            Assert.AreEqual(Transform(Brushes.Red).Color, ((SolidColorBrush)title.Background).Color);
            ((SolidColorBrush)source.Background).Color = Colors.Blue;
            Pump();
            Assert.AreEqual(Transform(Brushes.Blue).Color, ((SolidColorBrush)title.Background).Color);
            source.Background = new SolidColorBrush(Colors.Green);
            Pump();
            Assert.AreEqual(Transform(Brushes.Green).Color, ((SolidColorBrush)title.Background).Color);
            Assert.IsTrue(BindingOperations.IsDataBound(card, KanbanCard.TitleBackgroundProperty));
            KanbanBoard.SetIsDarkMode(host, false);
            Pump();
            Assert.AreSame(card.Background, root.Background);
            Assert.AreSame(source.Background, title.Background);
            KanbanBoard.SetIsDarkMode(host, true);
            Pump();
            Assert.AreEqual(Transform(Brushes.Green).Color, ((SolidColorBrush)title.Background).Color);
            var gradient = new LinearGradientBrush(Colors.Red, Colors.Blue, 90);
            source.Background = gradient;
            Pump();
            gradient.GradientStops[0].Color = Colors.Yellow;
            Pump();
            Assert.AreEqual(((GradientBrush)Convert(gradient, true)).GradientStops[0].Color,
                ((GradientBrush)title.Background).GradientStops[0].Color);
            Assert.AreEqual(Colors.Yellow, gradient.GradientStops[0].Color);
            card.IsDraged = true;
            Pump();
            Assert.AreEqual(0.3, root.Opacity);
            card.IsDraged = false;
            Pump();
            Assert.AreEqual(1.0, root.Opacity);
        });
    });

    [TestMethod]
    public void BoardTemplatesCoverWarningsCollapseNewItemsAndScrollbars() => OnSta(() =>
    {
        var errors = new BindingErrors();
        PresentationTraceSources.DataBindingSource.Listeners.Add(errors);
        try
        {
            var board = new KanbanBoard { Title = "Board", ShowReloadButton = true };
            var column = new KanbanColumn { Caption = "Column", ColumnValue = "todo", CardLimit = 0, Color = Brushes.Blue };
            var lane = new KanbanSwimlane { Caption = "Lane", LaneValue = "lane", Background = Brushes.Aquamarine };
            board.Columns.Add(column);
            board.Swimlanes.Add(lane);
            WithWindow(board, () =>
            {
                var boardRoot = Descendants<Border>(board).First();
                Assert.AreEqual(Colors.White, ((SolidColorBrush)boardRoot.Background).Color);
                var originalRoot = boardRoot;
                board.IsDarkMode = true;
                Pump();
                Assert.AreSame(originalRoot, Descendants<Border>(board).First());
                Assert.AreEqual(Transform(Brushes.White).Color, ((SolidColorBrush)boardRoot.Background).Color);
                var laneRoot = Descendants<Border>(lane).First();
                Assert.AreEqual(Transform(Brushes.Aquamarine).Color, ((SolidColorBrush)laneRoot.Background).Color);
                AssertContrast((SolidColorBrush)TextElement.GetForeground(laneRoot), (SolidColorBrush)laneRoot.Background);

                var card = new KanbanCard { DescriptionText = "New card", Blockers = new List<KanbanBlocker> { new() { BlockType = "!" } } };
                var cell = Descendants<KanbanBoardCell>(board).First();
                cell.AddCard(new KanbanCardPresenter { Content = card });
                Pump();
                var cardRoot = (Border)card.Template.FindName("Border", card);
                Assert.AreEqual(Transform(new SolidColorBrush(Color.FromRgb(252, 48, 48))).Color,
                    ((SolidColorBrush)cardRoot.Background).Color);
                var description = (TextBlock)card.Template.FindName("Content", card);
                AssertContrast((SolidColorBrush)description.Foreground, (SolidColorBrush)cardRoot.Background);
                Assert.IsTrue(column.IsCardLimitViolated);
                var header = (Border)column.Template.FindName("border", column);
                var caption = (TextBlock)column.Template.FindName("caption", column);
                Assert.AreEqual(Transform(new SolidColorBrush(Color.FromRgb(252, 88, 88))).Color,
                    ((SolidColorBrush)header.Background).Color);
                AssertContrast((SolidColorBrush)caption.Foreground, (SolidColorBrush)header.Background);
                var pill = Descendants<KanbanCardLimitPill>(column).Single();
                var pillBorder = (Border)pill.Template.FindName("border", pill);
                var pillLimit = (TextBlock)pill.Template.FindName("txt3", pill);
                AssertContrast((SolidColorBrush)pillLimit.Foreground, (SolidColorBrush)pillBorder.Background);

                column.IsCollapsed = true;
                Pump();
                var collapsed = Descendants<Border>(column).First();
                Assert.IsTrue(Luminance(((SolidColorBrush)collapsed.Background).Color) < 0.09);
                AssertContrast((SolidColorBrush)TextElement.GetForeground(collapsed), (SolidColorBrush)collapsed.Background);
                column.IsCollapsed = false;
                Pump();

                var reload = Descendants<KanbanBoardReloadButton>(board).Single();
                Assert.IsTrue(KanbanBoard.GetIsDarkModeActive(reload));
                Assert.IsTrue(Descendants<Border>(reload).Any(b => b.Name == "chrome"));
                Assert.AreEqual(1, Descendants<System.Windows.Shapes.Path>(reload).Count());
                var scrollBars = Descendants<ScrollBar>(board).ToList();
                Assert.IsTrue(scrollBars.Any(b => b.Orientation == Orientation.Horizontal));
                Assert.IsTrue(scrollBars.Any(b => b.Orientation == Orientation.Vertical));
                foreach (ScrollBar scrollBar in scrollBars)
                {
                    scrollBar.ApplyTemplate();
                    var track = (Track)scrollBar.Template.FindName("PART_Track", scrollBar);
                    Assert.IsNotNull(track);
                    Assert.IsTrue(Luminance(((SolidColorBrush)((Panel)VisualTreeHelper.GetParent(track)).Background).Color) < 0.09);
                    var line = (RepeatButton)scrollBar.Template.FindName("lineIncrease", scrollBar);
                    Assert.AreEqual(scrollBar.Orientation == Orientation.Horizontal
                        ? ScrollBar.LineRightCommand : ScrollBar.LineDownCommand, line.Command);
                }

                var newColumn = new KanbanColumn { Caption = "New", ColumnValue = "new" };
                board.Columns.Add(newColumn);
                Pump();
                Assert.IsTrue(KanbanBoard.GetIsDarkModeActive(newColumn));
                board.IsDarkMode = false;
                Pump();
                Assert.AreEqual(Colors.White, ((SolidColorBrush)boardRoot.Background).Color);
                Assert.AreSame(Brushes.Aquamarine, laneRoot.Background);
                Assert.IsFalse(Descendants<Border>(reload).Any(b => b.Name == "chrome"));
            });

            Assert.AreEqual("", errors.Messages);
        }
        finally
        {
            PresentationTraceSources.DataBindingSource.Listeners.Remove(errors);
        }
    });

    [TestMethod]
    public void NestedBoardsKeepTheirOwnModeInsideAnOuterBoard() => OnSta(() =>
    {
        var outer = new KanbanBoard
        {
            IsDarkMode = true,
            TitleControl = new ControlTemplate(typeof(Control))
            {
                VisualTree = new FrameworkElementFactory(typeof(KanbanBoard))
            }
        };
        WithWindow(outer, () =>
        {
            var nested = Descendants<KanbanBoard>(outer).Single();
            Assert.IsFalse(nested.IsDarkMode);
            Assert.IsFalse(KanbanBoard.GetIsDarkModeActive(nested));
            var root = Descendants<Border>(nested).First();
            Assert.AreEqual(Colors.White, ((SolidColorBrush)root.Background).Color);
            Assert.AreEqual(((SolidColorBrush)SystemColors.ControlTextBrush).Color,
                ((SolidColorBrush)TextElement.GetForeground(root)).Color);
            nested.IsDarkMode = true;
            Pump();
            Assert.AreEqual(Transform(Brushes.White).Color, ((SolidColorBrush)root.Background).Color);
        });
    });

    [TestMethod]
    public void DarkScrollbarsPreserveLinePageAndThumbScrolling() => OnSta(() =>
    {
        var board = new KanbanBoard { IsDarkMode = true };
        board.Columns.Add(new KanbanColumn { Caption = "Wide", MinWidth = 1600 });
        for (int i = 0; i < 15; i++)
        {
            board.Swimlanes.Add(new KanbanSwimlane { Caption = $"Lane {i}", MinHeight = 80 });
        }
        WithWindow(board, () =>
        {
            var viewer = (ScrollViewer)board.Template.FindName("KB_ScrollViewer", board);
            viewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            Pump();
            Assert.IsTrue(viewer.ScrollableHeight > 0);
            Assert.IsTrue(viewer.ScrollableWidth > 0);
            var bars = Descendants<ScrollBar>(viewer).ToList();
            foreach (ScrollBar bar in bars)
            {
                var line = (RepeatButton)bar.Template.FindName("lineIncrease", bar);
                var command = (System.Windows.Input.RoutedCommand)line.Command;
                command.Execute(null, line);
                Pump();
                double Offset() => bar.Orientation == Orientation.Vertical ? viewer.VerticalOffset : viewer.HorizontalOffset;
                Assert.IsTrue(Offset() > 0, $"Line scrolling: {bar.Orientation}");
                double previous = Offset();
                var track = (Track)bar.Template.FindName("PART_Track", bar);
                ((System.Windows.Input.RoutedCommand)track.IncreaseRepeatButton.Command)
                    .Execute(null, track.IncreaseRepeatButton);
                Pump();
                Assert.IsTrue(Offset() > previous, $"Page scrolling: {bar.Orientation}");
                if (bar.Orientation == Orientation.Vertical)
                {
                    viewer.ScrollToVerticalOffset(0);
                }
                else
                {
                    viewer.ScrollToHorizontalOffset(0);
                }
                Pump();
                previous = Offset();
                track.Thumb.RaiseEvent(new DragStartedEventArgs(0, 0) { RoutedEvent = Thumb.DragStartedEvent });
                track.Thumb.RaiseEvent(new DragDeltaEventArgs(20, 20) { RoutedEvent = Thumb.DragDeltaEvent });
                track.Thumb.RaiseEvent(new DragCompletedEventArgs(20, 20, false) { RoutedEvent = Thumb.DragCompletedEvent });
                Pump();
                Assert.IsTrue(Offset() > previous, $"Thumb scrolling: {bar.Orientation}");
            }
        });
    });

    [TestMethod]
    public void DropTargetPlainCardAndBlockerUseDarkTemplates() => OnSta(() =>
    {
        var panel = new StackPanel();
        var drop = new KanbanCardDropTarget();
        var plain = new KanbanPlainCard();
        var blocker = new KanbanBlocker { Background = Brushes.Red, Foreground = Brushes.Black, BlockType = "!" };
        panel.Children.Add(drop);
        panel.Children.Add(plain);
        panel.Children.Add(blocker);
        KanbanBoard.SetIsDarkMode(panel, true);
        WithWindow(panel, () =>
        {
            var dropBorder = (Border)drop.Template.FindName("Border", drop);
            Assert.IsTrue(Luminance(((SolidColorBrush)dropBorder.Background).Color) < 0.09);
            var plainBorder = (Border)plain.Template.FindName("Border", plain);
            AssertContrast((SolidColorBrush)TextElement.GetForeground(plainBorder), (SolidColorBrush)plainBorder.Background);
            var path = Descendants<System.Windows.Shapes.Path>(blocker).Single();
            var label = Descendants<TextBlock>(blocker).Single();
            AssertContrast((SolidColorBrush)label.Foreground, (SolidColorBrush)path.Fill);
        });
    });

    private static Brush Convert(Brush brush, bool dark, DarkModeBrushRole role = DarkModeBrushRole.Background) =>
        (Brush)Converter.Convert([brush, dark], typeof(Brush), role, CultureInfo.InvariantCulture);

    private static SolidColorBrush Transform(SolidColorBrush brush, DarkModeBrushRole role = DarkModeBrushRole.Background) =>
        (SolidColorBrush)Convert(brush, true, role);

    private static double Luminance(Color color)
    {
        double Linear(byte value) => value / 255.0 <= 0.04045
            ? value / 255.0 / 12.92 : Math.Pow((value / 255.0 + 0.055) / 1.055, 2.4);
        return 0.2126 * Linear(color.R) + 0.7152 * Linear(color.G) + 0.0722 * Linear(color.B);
    }

    private static void AssertContrast(SolidColorBrush foreground, SolidColorBrush background)
    {
        double a = Luminance(foreground.Color), b = Luminance(background.Color);
        double ratio = (Math.Max(a, b) + 0.05) / (Math.Min(a, b) + 0.05);
        Assert.IsTrue(ratio >= 4.5, $"{foreground.Color} on {background.Color}: {ratio:F3}:1");
    }

    private static IEnumerable<T> Descendants<T>(DependencyObject root) where T : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(root, i);
            if (child is T match)
            {
                yield return match;
            }
            foreach (T descendant in Descendants<T>(child))
            {
                yield return descendant;
            }
        }
    }

    private static void WithWindow(UIElement content, Action test)
    {
        var window = new Window
        {
            Content = content, Width = 900, Height = 600, ShowInTaskbar = false,
            WindowStyle = WindowStyle.None, Left = -10000, Top = -10000
        };
        try
        {
            window.Show();
            Pump();
            test();
        }
        finally
        {
            window.Close();
        }
    }

    private static void Pump() =>
        Dispatcher.CurrentDispatcher.Invoke(() => { }, DispatcherPriority.ApplicationIdle);

    private static void OnSta(Action test)
    {
        ExceptionDispatchInfo failure = null;
        var thread = new Thread(() =>
        {
            try { test(); }
            catch (Exception exception) { failure = ExceptionDispatchInfo.Capture(exception); }
            finally { Dispatcher.CurrentDispatcher.InvokeShutdown(); }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        Assert.IsTrue(thread.Join(TimeSpan.FromSeconds(30)), "STA test did not finish.");
        failure?.Throw();
    }

    private sealed class BindingErrors : TraceListener
    {
        public string Messages { get; private set; } = "";
        public override void Write(string message) => Messages += message;
        public override void WriteLine(string message) => Messages += message + Environment.NewLine;
    }
}
