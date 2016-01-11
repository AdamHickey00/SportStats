module SportStats

open Suave.Filters
open Suave.WebPart
open Suave.Operators
open Suave.Web
open Types

let routes (db:IDB) =
  choose
    [ GET >=>
      choose [ path "/Golf/LowestTournament" >=> SportService.getLowestTournament db
               path "/Golf/LowestRound" >=> SportService.getLowestRound db
               path "/Golf/TotalEarnings" >=> SportService.getTotalGolfEarnings db ]]

[<EntryPoint>]
let main argv =
    startWebServer defaultConfig (routes Database.DB)
    0
