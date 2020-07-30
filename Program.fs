open Messaging
open Systems
open FSharp.Control.Reactive

[<EntryPoint>]
let main argv =
    let pingPongChannel = MessageChannel.init<PingPongMessage>
    let bigDoinksChannel = MessageChannel.init<BigDoinksMessage>

    let (>>) = Disposable.compose

    // ---------

    let pingPong = 
        pingPongChannel |> Logger.subscribe "Ping Pong Message" 
        >> (pingPongChannel |> Ping.subscribe)
        >> (pingPongChannel |> Pong.subscribe)

    // ---------

    let bigDoinks = 
        bigDoinksChannel |> Logger.subscribe "Big Doinks Message" 
        >> (bigDoinksChannel |> BigDoinks.subscribe)

    // ---------

    let listeners = pingPong >> bigDoinks
    
    pingPongChannel |> MessageChannel.send PingPongMessage.Booted
    bigDoinksChannel |> MessageChannel.send BigDoinksMessage.Booted
    
    listeners.Dispose() // Clean up
    0 // return an integer exit code
