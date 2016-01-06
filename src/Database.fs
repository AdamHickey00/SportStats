module Database

open FSharp.Data
open FSharp.Data.Runtime
open Types
open System.Linq

open System
open System.Globalization
open System.IO
open System.Text
open System.Text.RegularExpressions
open FSharp.Data
open FSharp.Data.HtmlExtensions
open FSharp.Data.Runtime.StructuralTypes

type GolfStats = HtmlProvider<"http://www.golfstats.com/search/?stat=11&player=Tiger+Woods&submit=go">

let getHoleInOnes firstName lastName : Athlete =

  let url = sprintf "http://www.golfstats.com/search/?stat=11&player=%s+%s&submit=go" "Tiger" "Woods"
  let liveStats = GolfStats.Load(url)

  printfn "tables = %A" (liveStats)

  { FirstName = firstName
    LastName = lastName
    Accomplishment = HoleInOnes 7 }

let DB =
  { new IDB with
      member x.getHoleInOnes first last =
        getHoleInOnes first last
  }
