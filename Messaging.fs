module Messaging

module MessageChannel =
    let init<'Msg> = Observable.Subject<'Msg>()
    let send<'Msg> msg (s: Observable.Subject<'Msg>) = s.Next msg
