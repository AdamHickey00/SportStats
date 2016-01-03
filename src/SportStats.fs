module SportStats

open Suave.Http
open Suave.Http.Applicatives
open Suave.Web

let routes =
  choose
    [ GET >>=
      choose [ path "/Golf/HoleInOnes" >>= SportService.getHoleInOnes ]]

[<EntryPoint>]
let main argv =
    startWebServer defaultConfig routes
    0
