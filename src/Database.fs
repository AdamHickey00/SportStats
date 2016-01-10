module Database

open FSharp.Data
open System.Linq
open Types

type LowestTournamentPage = HtmlProvider<"http://www.golfstats.com/search/?stat=6&player=Tiger+Woods&submit=go">

let lowestRoundMap (columnPosition:int) (row:HtmlNode) : int =
  let columnValue = (row.Descendants ["td"]).ElementAt(columnPosition).InnerText()

  // value like "66-61-68-70=265"
  (columnValue.Split [|'='|]).[0].Split [|'-'|]
  |> Array.map (fun x -> int x)
  |> Array.min

//let lowestTournamentMap (columnPosition:int) (row:HtmlNode) : int =
//  let columnValue = (row.Descendants ["td"]).ElementAt(columnPosition).InnerText()

  // value like "66-61-68-70=265"
//  (columnValue.Split [|'='|]).[0].Split [|'-'|]
//  |> Array.map (fun x -> int x)
//  |> Array.min

let getGolfStat firstName lastName columnPosition mapFunc filterFunc : Athlete =
  let url = sprintf "http://www.golfstats.com/search/?stat=6&player=%s+%s&submit=go" firstName lastName
  let stats = LowestTournamentPage.Load(url)
  let tables = stats.Html.Descendants ["table"]

  match Seq.length tables with
  | 0 -> { FirstName = firstName
           LastName = lastName
           Stat = LowestTournament (int 0) }

  | _ -> let lowestRound =
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
           Stat = LowestRound lowestRound }

let getLowestTournament firstName lastName : Athlete =
  let url = sprintf "http://www.golfstats.com/search/?stat=6&player=%s+%s&submit=go" firstName lastName
  let stats = LowestTournamentPage.Load(url)
  let tables = stats.Html.Descendants ["table"]
  let rowPosition = 1
  let columnPosition = 3

  match Seq.length tables with
  | 0 -> { FirstName = firstName
           LastName = lastName
           Stat = LowestTournament (int 0) }

  | _ -> let lowestScore =
           tables
           |> Seq.head
           |> (fun x -> (x.Descendants ["tr"]).ElementAt(rowPosition))
           |> (fun x -> (x.Descendants ["td"]).ElementAt(columnPosition))
           |> (fun x -> x.InnerText())

         { FirstName = firstName
           LastName = lastName
           Stat = LowestTournament (int lowestScore) }

let getLowestTournament2 firstName lastName : Athlete =
  getGolfStat firstName lastName 3 lowestRoundMap (fun (x:int) -> x > 50)

let getLowestRound firstName lastName : Athlete =
  getGolfStat firstName lastName 2 lowestRoundMap (fun (x:int) -> x > 50)

let DB =
  { new IDB with
      member x.GetLowestTournament first last =
        getLowestTournament first last
      member x.GetLowestRound first last =
        getLowestRound first last
  }
