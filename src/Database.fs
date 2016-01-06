module Database

open FSharp.Data
open System.Linq
open Types

type LowestTournamentPage = HtmlProvider<"http://www.golfstats.com/search/?stat=6&player=Tiger+Woods&submit=go">

let rowPosition = 1
let columnPosition = 3

let getLowestTournament firstName lastName : Athlete =

  let url = sprintf "http://www.golfstats.com/search/?stat=6&player=%s+%s&submit=go" firstName lastName
  let stats = LowestTournamentPage.Load(url)
  let tables = stats.Html.Descendants ["table"]

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

let DB =
  { new IDB with
      member x.GetLowestTournament first last =
        getLowestTournament first last
  }
