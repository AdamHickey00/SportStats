module SportStats

open Suave
open Suave.Http
open Suave.Http.Applicatives
open Suave.Http.Successful
open Suave.Types
open Suave.Web

//let routes =
//  choose
//    [ GET >>=
//      choose [ pathScan "/Golf/HoleInOnes/%s" >>= ]

[<EntryPoint>]
let main argv =
    startWebServer Suave.Web.defaultConfig (OK "Hello World!")
    0
