﻿namespace AidenDesktop.Views

open Avalonia.Controls
open Avalonia.Markup.Xaml
open Avalonia.Threading
open AidenDesktop.ViewModels

type ChatView() as this =
    inherit UserControl ()

    let mutable viewModel: ChatViewModel option = None

    do
        this.InitializeComponent()
        this.DataContextChanged.Add(fun args ->
            viewModel <- this.DataContext :?> ChatViewModel |> Some
            match viewModel with
            | Some viewModel ->
                viewModel.NewMessageEvent
                |> Observable.subscribe (fun _ ->
                    this.ScrollToBottom())
                |> ignore
            | None -> ())

    member private this.ScrollToBottom() =
        let listBox = this.FindControl<ListBox>("ChatWindow")
        Dispatcher.UIThread.InvokeAsync((fun () ->
            if listBox.ItemCount > 0 then
                let item = listBox.ContainerFromIndex(listBox.ItemCount - 1)
                (item :?> ListBoxItem).BringIntoView()
        ), DispatcherPriority.Loaded) |> ignore

    member private this.InitializeComponent() =
        AvaloniaXamlLoader.Load(this)