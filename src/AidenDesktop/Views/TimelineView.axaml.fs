namespace AidenDesktop.Views

open Avalonia
open Avalonia.Controls
open Avalonia.Markup.Xaml

type TimelineView () as this = 
    inherit UserControl ()

    do this.InitializeComponent()

    member private this.InitializeComponent() =
        AvaloniaXamlLoader.Load(this)