module SportStats

open Suave.Filters
open Suave.WebPart
open Suave.Operators
open Suave.Web
open Types

let routes (db:IDB) =
  choose
    [ GET >=>
      choose [ path "/Golf/HoleInOnes" >=> SportService.getHoleInOnes db ]]

[<EntryPoint>]
let main argv =
    startWebServer defaultConfig (routes Database.DB)
    0
