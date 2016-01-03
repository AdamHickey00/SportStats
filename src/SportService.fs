module SportService

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

  (okJSON "Hello 3") ctx
