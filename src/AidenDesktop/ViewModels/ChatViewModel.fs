﻿namespace AidenDesktop.ViewModels

open Elmish
open ReactiveElmish
open ReactiveElmish.Avalonia
open DynamicData
open System.Collections.ObjectModel
open System

module Chat =
    type Message = { User: string; Text: string; Alignment: string; Color: string; BorderColor: string; IsMe: bool }

    type Model = { Messages: SourceList<Message> }

    type Msg =
        | SendMessage of string

    let init() =
        let initialMessages =
            [
                { User = "Aiden"; Text = "Anomaly detected in in-bound data. It mirrors a previous probe attack that has preceded a DDoS cycle by 20 minutes."
                  Alignment = "Left"; Color = "Glaucous"; BorderColor = "Orange" ; IsMe = false }
                { User = "Me"; Text = "Thanks I see it. Clear the alarm. Is there any news on the wire that this is hitting more than us?"
                  Alignment = "Right"; Color = "Charcoal"; BorderColor = "MidnightBlue" ; IsMe = true }
            ]
        { Messages = SourceList.createFrom initialMessages}

    let update (msg: Msg) (model: Model) =
        match msg with
        | SendMessage text ->
            let msg = { User = "Me"; Text = text; Alignment = "Right"; Color = "White"; BorderColor = "MidnightBlue" ; IsMe  = true }
            // printfn "Message: %A" msg
            {
                Messages = model.Messages |> SourceList.add msg
            }

open Chat

type ChatViewModel() as this =
    inherit ReactiveElmishViewModel()

    let newMessageEvent = new Event<_>()

    let local =
        Program.mkAvaloniaSimple init update
        |> Program.withErrorHandler (fun (_, ex) -> printfn "Error: %s" ex.Message)
        |> Program.mkStore

    do
        // Subscribe to the Messages list's Changed event
        local.Model.Messages.Connect()
            .Subscribe(fun _ -> 
                newMessageEvent.Trigger())
            |> ignore

    member this.MessagesView = this.BindSourceList(local.Model.Messages)

    member this.NewMessageEvent = newMessageEvent.Publish

    member this.SendMessage(message: string) =
        local.Dispatch (SendMessage message)

    static member DesignVM = new ChatViewModel()
