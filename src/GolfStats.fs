module GolfStats

open FSharp.Data
open Types
open Utils

type LowestTournamentPage = HtmlProvider<"http://www.golfstats.com/search/?stat=6&player=Tiger+Woods&submit=go">

let lowestRoundMap columnIndex (row:HtmlNode) : int =
  let columnValue = getTextFromColumn columnIndex row

  // value like "66-61-68-70=265"
  (columnValue.Split [|'='|]).[0].Split [|'-'|]
  |> Array.map int
  |> Array.min

let totalEarningsMap columnIndex (row:HtmlNode) : int =
  let columnValue = getTextFromColumn columnIndex row
  int (stripChars ['$'; '~'; ','; ' '] columnValue)

let lowestTournamentMap columnIndex (row:HtmlNode) : int =
  int (getTextFromColumn columnIndex row)

let totalEarningsValue (earnings:int) =
  TotalEarnings (Money.formatMoney earnings)

let getGolfStat (input:DatabaseInput) mapFunc filterFunc (valueFunc: int -> StatType) (totalFunc: seq<'a> -> int) : Athlete =
  let url = sprintf "http://www.golfstats.com/search/?stat=6&player=%s+%s&submit=go" input.FirstName input.LastName
  let stats = LowestTournamentPage.Load(url)
  let tables = stats.Html.Descendants ["table"]

  match Seq.length tables with
  | 0 -> { FirstName = input.FirstName
           LastName = input.LastName
           Stat = LowestTournament (int 0) }

  | _ -> let value =
           tables
           |> Seq.head
           |> (fun x -> x.Descendants ["tbody"])
           |> Seq.head
           |> (fun x -> x.Descendants ["tr"])
           |> Seq.map (mapFunc input.ColumnIndex)
           |> Seq.filter filterFunc
           |> totalFunc

         { FirstName = input.FirstName
           LastName = input.LastName
           Stat = valueFunc value }

let getLowestTournament firstName lastName : Athlete =
  let input = { FirstName = firstName; LastName = lastName; ColumnIndex = 3 }
  getGolfStat input lowestTournamentMap (fun x -> x < 0) LowestTournament Seq.min

let getLowestRound firstName lastName : Athlete =
  let input = { FirstName = firstName; LastName = lastName; ColumnIndex = 2 }
  getGolfStat input lowestRoundMap (fun x -> x > 50) LowestRound Seq.min

let getTotalGolfEarnings firstName lastName : Athlete =
  let input = { FirstName = firstName; LastName = lastName; ColumnIndex = 4 }
  getGolfStat input totalEarningsMap (fun x -> x > 0) totalEarningsValue Seq.sum
