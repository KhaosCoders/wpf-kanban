<p style="float:right; width:100px;">
<img src ="https://raw.githubusercontent.com/KhaosCoders/wpf-kanban/master/Logo.png" width=200 />
</p>

# wpf-kanban
A Kanban Board control for WPF 

![Build](https://github.com/KhaosCoders/wpf-kanban/actions/workflows/build.yml/badge.svg)
[![NuGet](https://img.shields.io/nuget/v/wpf-kanban.svg?logo=nuget)](https://www.nuget.org/packages/wpf-kanban/)

## Usage

See sample

### Dark mode

Set `IsDarkMode` on a board, or bind it to a view-model property:

```xml
<kanban:KanbanBoard IsDarkMode="{Binding UseDarkMode}" />
```

The default is `false`. Changing it at runtime updates the built-in templates
without recreating cards. Each board has its own mode; no application-wide or
Windows theme setting is changed. Both sample windows include a live toggle.
Boards default to the system text brush so a nested light board does not inherit
light text from a dark parent. Set `Foreground` on the board to customize it.

Standard colors and custom brushes used by the built-in templates are adapted
automatically: dark surfaces, light text, and recognizable accent/warning colors.
Solid colors and linear/radial gradients retain alpha, opacity, and geometry.
Opaque solid text/background pairs in the built-in templates meet a contrast
ratio of at least 4.5:1. This guarantee does not apply to transparent surfaces,
gradients, images, or arbitrary custom content.

Original property values, shared brushes, and bindings are not modified.
`SaveModel()` includes `IsDarkMode`; `LoadJsonModel()` restores it. Older models
without the property leave the current mode unchanged. Serialized colors remain
the original colors, not their dark-mode projections.

Images, stickers, other brush types (including image/visual brushes), custom
templates, context menus, and the contents of `KanbanPlainCard` are not
automatically recolored. Custom templates
can opt in using the same markup extension (`conv` refers to
`KC.WPF_Kanban.Converter` in assembly `WPF-Kanban`):

```xml
<Border Background="{conv:DarkModeBrush {Binding Background, RelativeSource={RelativeSource TemplatedParent}}}"
        TextElement.Foreground="{conv:DarkModeBrush {Binding Foreground, RelativeSource={RelativeSource TemplatedParent}}, Role=Foreground}" />
```

Use the default `Background` role for surfaces, `Foreground` for text, and
`Accent` for icons, lines, and borders. Always supply the original brush, not an
already converted background. The extension follows the inherited board mode,
available as the read-only attached property `KanbanBoard.IsDarkModeActive`.
The plain-card sample demonstrates explicit opt-in for its custom content.

### Tests

On Windows with the .NET 10 SDK:

```powershell
dotnet test Tests\WPF-Kanban.Tests.csproj --configuration Release
```

# Support this <3

If you like my work, please support this project!  
Donate via [PayPal](https://www.paypal.com/donate?hosted_button_id=37PBGZPHXY8EC)
or become a [Sponsor on GitHub](https://github.com/sponsors/Khaos66)