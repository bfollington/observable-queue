namespace Systems

open Messaging
open FSharp.Control.Reactive


type PingPongMessage =
    | Booted
    | Ping
    | Pong

type BigDoinksMessage =
    | Booted
    | Big
    | Doinks

// Imagine all these are diverse systems across the domain rather than silly contrived examples...

module Logger =
    let subscribe prefix chan = chan |> Observable.subscribe (printfn "%s: %A" prefix)

module Ping =
    let subscribe chan = 
        chan
        |> Observable.filter (function | PingPongMessage.Booted -> true | _ -> false)
        |> Observable.subscribe (fun _ -> chan |> MessageChannel.send Ping)

module Pong =
    let subscribe chan = 
        chan
        |> Observable.filter (function | Ping -> true | _ -> false)
        |> Observable.subscribe (fun _ -> chan |> MessageChannel.send Pong)

module BigDoinks =
    let private big chan = 
        chan
        |> Observable.filter (function | BigDoinksMessage.Booted -> true | _ -> false)
        |> Observable.subscribe (fun _ -> chan |> MessageChannel.send Big)

    let private doinks chan = 
        chan
        |> Observable.filter (function | Big -> true | _ -> false)
        |> Observable.subscribe (fun _ -> chan |> MessageChannel.send Doinks)

    let subscribe chan = Disposable.compose (big chan) (doinks chan)