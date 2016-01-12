#load "Monads.fs"

open Monads.Choice
open System
open System.String

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

let formatMoney (value:string) =
  value.Replace("$", "").Replace(",", "").Replace(" ", "").Replace("~", "")

let stripChars (chars:seq<char>) (value:string) =
  value
  |> Seq.filter (fun x -> not (chars |> Seq.exists (fun y -> y = x)))
  |> System.String.Concat

stripChars ['$'; '~'; ','; ' '] "$8~ 8,000"
