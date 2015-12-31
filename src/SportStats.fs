module SportStats

open Suave
open Suave.Http
open Suave.Http.Applicatives
open Suave.Http.Successful
open Suave.Types
open Suave.Web

let getName (ctx : HttpContext) =
  let r = ctx.request
  (r.queryParam "firstName"), (r.queryParam "lastName")

let getHoleInOnes (ctx : HttpContext) =
  let firstNameChoice = fst (getName ctx)
  let lastNameChoice = snd (getName ctx)

  (OK "Hello 3") ctx

let routes =
  choose
    [ GET >>=
      choose [ path "/Golf/HoleInOnes" >>= getHoleInOnes ]]

[<EntryPoint>]
let main argv =
    //startWebServer defaultConfig (OK "Hello World22!")
    startWebServer defaultConfig routes
    0
