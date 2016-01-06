module Database

open FSharp.Data
open Types

type GolfStats = HtmlProvider<"http://www.golfstats.com/search/?stat=6&player=Tiger+Woods&submit=go">

let getLowestTournament firstName lastName : Athlete =

  let url = sprintf "http://www.golfstats.com/search/?stat=6&player=%s+%s&submit=go" "Tiger" "Woods"
  let liveStats = GolfStats.Load(url)

  let lowestScore =
    liveStats.Tables.``Low To Par: Tiger WoodsSavePrintBack``.Rows
    |> Seq.head
    |> (fun x -> x.``Tiger Woods Final Score To Par``)

  { FirstName = firstName
    LastName = lastName
    Accomplishment = LowestTournament (int lowestScore) }

let DB =
  { new IDB with
      member x.GetLowestTournament first last =
        getLowestTournament first last
  }
