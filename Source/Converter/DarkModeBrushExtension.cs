using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace KC.WPF_Kanban.Converter;

/// <summary>
/// Applies dark mode to a brush or brush binding in a built-in or custom template.
/// </summary>
[MarkupExtensionReturnType(typeof(object))]
public class DarkModeBrushExtension : MarkupExtension
{
    private static readonly DarkModeBrushConverter Converter = new DarkModeBrushConverter();

    public DarkModeBrushExtension() { }

    public DarkModeBrushExtension(object value) => Value = value;

    [ConstructorArgument("value")]
    public object Value { get; set; }

    public DarkModeBrushRole Role { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var binding = new MultiBinding
        {
            Converter = Converter,
            ConverterParameter = Role,
            Mode = BindingMode.OneWay
        };
        binding.Bindings.Add(Value is BindingBase sourceBinding
            ? sourceBinding
            : new Binding { Source = Value, Mode = BindingMode.OneWay });
        binding.Bindings.Add(new Binding
        {
            Path = new PropertyPath(KanbanBoard.IsDarkModeActiveProperty),
            RelativeSource = RelativeSource.Self,
            Mode = BindingMode.OneWay
        });
        return binding.ProvideValue(serviceProvider);
    }
}
