module SportService

open Newtonsoft.Json
open Suave
open Suave.Http
open Suave.Http.Successful
open Suave.Types

let JSON = Writers.setMimeType "application/json"
let okJSON s = OK s >>= JSON

let getName (ctx : HttpContext) =
  let r = ctx.request
  (r.queryParam "firstName"), (r.queryParam "lastName")

let getHoleInOnes (ctx : HttpContext) =
  let firstNameChoice = fst (getName ctx)
  let lastNameChoice = snd (getName ctx)

  let result = Database.getHoleInOnes "Tiger" "Woods"
               |> JsonConvert.SerializeObject
               |> okJSON

  result ctx
