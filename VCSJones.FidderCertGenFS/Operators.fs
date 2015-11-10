namespace VCSJones.FiddlerCertGen
[<AutoOpenAttribute>]
module Operators =
    let inline (|?) (a: 'a option) b = if a.IsSome then a.Value else b
    let inline (|??) (a: 'a option) ((b : 'b, selector : 'a -> 'b)) = if a.IsSome then selector(a.Value) else b

type TryParse<'T> =
    | Fail
    | Success of 'T