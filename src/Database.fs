module Database

open FSharp.Data
open System.Linq
open Types

type LowestTournamentPage = HtmlProvider<"http://www.golfstats.com/search/?stat=6&player=Tiger+Woods&submit=go">
type LowestRoundPage = HtmlProvider<"http://www.golfstats.com/search/?stat=11&player=Tiger+Woods&submit=go">

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

let getLowestRound firstName lastName : Athlete =
  let url = sprintf "http://www.golfstats.com/search/?stat=11&player=%s+%s&submit=go" firstName lastName
  let stats = LowestRoundPage.Load(url)
  let tables = stats.Html.Descendants ["table"]
  let rowPosition = 1
  let columnPosition = 2

  match Seq.length tables with
  | 0 -> { FirstName = firstName
           LastName = lastName
           Stat = LowestTournament (int 0) }

  | _ -> let tournamentScores =
           tables
           |> Seq.head
           |> (fun x -> (x.Descendants ["tr"]).ElementAt(rowPosition))
           |> (fun x -> (x.Descendants ["td"]).ElementAt(columnPosition))
           |> (fun x -> x.InnerText())

         // value like "66-61-68-70=265"
         let lowestRound =
          (tournamentScores.Split [|'='|]).[0].Split [|'-'|]
          |> Array.map (fun x -> int x)
          |> Array.min

         { FirstName = firstName
           LastName = lastName
           Stat = LowestRound (decimal lowestRound) }

let DB =
  { new IDB with
      member x.GetLowestTournament first last =
        getLowestTournament first last
      member x.GetLowestRound first last =
        getLowestRound first last
  }
