module Database

open FSharp.Data
open System.Linq
open Types

type LowestTournamentPage = HtmlProvider<"http://www.golfstats.com/search/?stat=6&player=Tiger+Woods&submit=go">

type Input = {
  FirstName : string
  LastName : string
  ColumnIndex : int
}

let getTextFromColumn columnIndex (row:HtmlNode) =
  (row.Descendants ["td"]).ElementAt(columnIndex).InnerText()

let stripChars (chars:seq<char>) (value:string) =
  value
  |> Seq.filter (fun x -> not (chars |> Seq.exists (fun invalidChar -> invalidChar = x)))
  |> System.String.Concat

let lowestRoundMap columnIndex (row:HtmlNode) : int =
  let columnValue = getTextFromColumn columnIndex row

  // value like "66-61-68-70=265"
  (columnValue.Split [|'='|]).[0].Split [|'-'|]
  |> Array.map (fun x -> int x)
  |> Array.min

let totalEarningsMap columnIndex (row:HtmlNode) : int =
  let columnValue = getTextFromColumn columnIndex row

  // take only numbers
  int (stripChars ['$'; '~'; ','; ' '] columnValue)

let lowestTournamentMap columnIndex (row:HtmlNode) : int =
  int (getTextFromColumn columnIndex row)

let getGolfStat (input:Input) mapFunc filterFunc (valueFunc: int -> StatType) (totalFunc: seq<'a> -> int) : Athlete =
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
           |> (fun x -> (x.Descendants ["tr"]))
           |> Seq.map (mapFunc input.ColumnIndex)
           |> Seq.filter filterFunc
           |> totalFunc

         { FirstName = input.FirstName
           LastName = input.LastName
           Stat = valueFunc value }

let getLowestTournament firstName lastName : Athlete =
  let input = { FirstName = firstName; LastName = lastName; ColumnIndex = 3 }
  getGolfStat input lowestTournamentMap (fun x -> x < 0) (fun y -> LowestTournament y) Seq.min

let getLowestRound firstName lastName : Athlete =
  let input = { FirstName = firstName; LastName = lastName; ColumnIndex = 2 }
  getGolfStat input lowestRoundMap (fun x -> x > 50) (fun y -> LowestRound y) Seq.min

let getTotalGolfEarnings firstName lastName : Athlete =
  let input = { FirstName = firstName; LastName = lastName; ColumnIndex = 4 }
  getGolfStat input totalEarningsMap (fun x -> x > 0) (fun y -> TotalEarnings y) Seq.sum

let DB =
  { new IDB with
      member x.GetLowestTournament first last =
        getLowestTournament first last
      member x.GetLowestRound first last =
        getLowestRound first last
      member x.GetTotalGolfEarnings first last =
        getTotalGolfEarnings first last
  }
