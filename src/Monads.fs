module Monads

module Choice =

  let bind m f =
    match m with
    | Choice1Of2 a -> f a
    | Choice2Of2 b -> Choice2Of2 b

  type ChoiceBuilder() =
    member this.Bind(m,f) = bind m f
    member this.Return(x) = Choice1Of2 x
    member this.ReturnFrom(m) = m
    member this.Zero() = Choice1Of2 ()

  let choice = new ChoiceBuilder()
