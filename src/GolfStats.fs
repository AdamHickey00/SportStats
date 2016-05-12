module GolfStats

open FSharp.Data
open Types
open Utils

type LowestTournamentPage = HtmlProvider<"http://www.golfstats.com/search/?stat=6&player=Tiger+Woods&submit=go">

let lowestRoundMap columnIndex (row:HtmlNode) : int Option =
  let columnValue = getTextFromColumn columnIndex row  
  
  if columnValue.Contains "-" then
    // value like "66-61-68-70=265"
    Some ((columnValue.Split [|'='|]).[0].Split [|'-'|]
           |> Array.map int
           |> Array.min)
  else 
    None 
  
let totalEarningsMap columnIndex (row:HtmlNode) : int Option =
  let columnValue = getTextFromColumn columnIndex row
  Some (int (stripChars ['$'; '~'; ','; ' '] columnValue))

let lowestTournamentMap columnIndex (row:HtmlNode) : int Option =
  Some (int (getTextFromColumn columnIndex row))

let totalEarningsValue (earnings:int) =
  TotalEarnings (Money.formatMoney earnings)

let stat (html:HtmlDocument) (input:GolfInput<int>) =
  let tables = html.Descendants ["table"]
  
  match Seq.length tables with
  | 0 -> Failure RecordNotFound
  | _ -> let value =
           tables
           |> Seq.head
           |> (fun x -> x.Descendants ["tbody"])
           |> Seq.head
           |> (fun x -> x.Descendants ["tr"])
           |> Seq.map (input.MapFunction input.Data.ColumnIndex)
           |> Seq.filter input.FilterFunction
           |> Seq.filter (fun a -> Option.isSome a)
           |> Seq.map (fun a -> match a with 
                                | Some v -> v
                                | None -> failwith "There should only be actual values here.")
           |> input.TotalFunction
         
         Success { FirstName = input.Data.FirstName
                   LastName = input.Data.LastName
                   Stat = input.Data.ValueFunction value }

let getGolfStat (input:GolfInput<int>) =
  let url = sprintf "http://www.golfstats.com/search/?stat=6&player=%s+%s&submit=go" input.Data.FirstName input.Data.LastName
  let stats = LowestTournamentPage.Load(url)
  stat stats.Html input

let filterByValue (f:int -> int -> bool) n x = 
  match x with 
  | Some a -> f a n  
  | None -> false 
      
let getLowestTournament firstName lastName =
  let input = { FirstName = firstName; LastName = lastName; ColumnIndex = 3; ValueFunction = LowestTournament }
  let golfInput = { 
    Data = input; 
    MapFunction = lowestTournamentMap; 
    FilterFunction = filterByValue (<) 0; 
    TotalFunction = Seq.min }

  getGolfStat golfInput

let getLowestRound firstName lastName =
  let input = { FirstName = firstName; LastName = lastName; ColumnIndex = 2; ValueFunction = LowestRound }
  let golfInput = { 
    Data = input; 
    MapFunction = lowestRoundMap; 
    FilterFunction = filterByValue (>) 50; 
    TotalFunction = Seq.min }

  getGolfStat golfInput

let getTotalGolfEarnings firstName lastName =
  let input = { FirstName = firstName; LastName = lastName; ColumnIndex = 4; ValueFunction = totalEarningsValue }
  let golfInput = { 
    Data = input; 
    MapFunction = totalEarningsMap; 
    FilterFunction = filterByValue (>) 0; 
    TotalFunction = Seq.sum }

  getGolfStat golfInput