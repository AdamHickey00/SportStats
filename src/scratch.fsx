#load "Monads.fs"

open Monads.Choice

let input first last =
  choice {
    let! startDate = first
    let! endDate = last
    return (startDate, endDate)
  }

let test : Choice<(string * string), string> =
  input (Choice2Of2 "Adam") (Choice1Of2 "Hickey")

let something = "342.0"
decimal something
