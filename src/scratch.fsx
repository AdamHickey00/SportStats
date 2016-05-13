#r "../packages/FSharp.Data/lib/net40/FSharp.Data.dll"
  
#load "Types.fs"
#load "Monads.fs"
#load "Utils.fs"
#load "Money.fs"
#load "GolfStats.fs"

open Monads.Choice
open System
open FSharp.Data

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

[<Literal>]
let golfLowRoundEmptyHtml =
  """<html>
         <body>
             <table>
                 <tbody>
                     <tr>
                         <td>login</td>
                         <td>T33</td> <!-- Final finish -->
                         <td>55-33</td> <!-- Final score -->
                         <td>-17</td> <!-- Final score to par -->
                         <td>$659,000</td> <!-- Final money -->
                         <td>fedex</td>
                     </tr>
               </tbody>
          </table>
      </body>
  </html>"""
    
let firstRow html = 
  let doc = HtmlDocument.Parse html
  
  doc.Descendants ["table"]
  |> Seq.head
  |> (fun x -> x.Descendants ["tbody"])
  |> Seq.head
  |> (fun x -> x.Descendants ["tr"])
  |> Seq.head
  
let row = firstRow golfLowRoundEmptyHtml

GolfStats.lowestRoundMap 2 row