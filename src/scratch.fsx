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

let something = "66-61-68-70=265"
let scores = (something.Split [|'='|]).[0]
let scores2 = scores.Split [|'-'|]
Array.map (fun x -> int x) scores2
|> Array.filter (fun x -> x > 62)
|> Array.min

let moneyInput = "$88,000"
moneyInput.Replace("$", "").Replace(",", "")
