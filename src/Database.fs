module Database

open FSharp.Data
open System.Linq
open Types

type LowestTournamentPage = HtmlProvider<"http://www.golfstats.com/search/?stat=6&player=Tiger+Woods&submit=go">

let getTextFromColumn (columnPosition:int) (row:HtmlNode) =
  (row.Descendants ["td"]).ElementAt(columnPosition).InnerText()

let lowestRoundMap (columnPosition:int) (row:HtmlNode) : int =
  let columnValue = getTextFromColumn columnPosition row

  // value like "66-61-68-70=265"
  (columnValue.Split [|'='|]).[0].Split [|'-'|]
  |> Array.map (fun x -> int x)
  |> Array.min

let lowestTournamentMap (columnPosition:int) (row:HtmlNode) : int =
  int (getTextFromColumn columnPosition row)

let getGolfStat firstName lastName columnPosition mapFunc filterFunc (valueFunc: int -> StatType) : Athlete =
  let url = sprintf "http://www.golfstats.com/search/?stat=6&player=%s+%s&submit=go" firstName lastName
  let stats = LowestTournamentPage.Load(url)
  let tables = stats.Html.Descendants ["table"]

  match Seq.length tables with
  | 0 -> { FirstName = firstName
           LastName = lastName
           Stat = LowestTournament (int 0) }

  | _ -> let value =
           tables
           |> Seq.head
           |> (fun x -> x.Descendants ["tbody"])
           |> Seq.head
           |> (fun x -> (x.Descendants ["tr"]))
           |> Seq.map (mapFunc columnPosition)
           |> Seq.filter filterFunc
           |> Seq.min

         { FirstName = firstName
           LastName = lastName
           Stat = valueFunc value }

let getLowestTournament firstName lastName : Athlete =
  getGolfStat firstName lastName 3 lowestTournamentMap (fun x -> x < 0) (fun y -> LowestTournament y)

let getLowestRound firstName lastName : Athlete =
  getGolfStat firstName lastName 2 lowestRoundMap (fun x -> x > 50) (fun y -> LowestRound y)

let DB =
  { new IDB with
      member x.GetLowestTournament first last =
        getLowestTournament first last
      member x.GetLowestRound first last =
        getLowestRound first last
  }
