module SportService

open Monads.Choice
open Newtonsoft.Json
open Suave
open Suave.Operators
open Suave.RequestErrors
open Suave.Successful
open Types

let JSON = Writers.setMimeType "application/json"
let okJSON s = OK s >=> JSON

let getName (ctx : HttpContext) =
  let r = ctx.request
  (r.queryParam "firstName"), (r.queryParam "lastName")

let getHoleInOnes (db:IDB) (ctx : HttpContext) =

  let input = choice {
    let! startDate = fst (getName ctx)
    let! endDate = snd (getName ctx)
    return (startDate, endDate)
  }

  let result =
    match input with
    | Choice2Of2 err -> BAD_REQUEST err
    | Choice1Of2 (first, last) -> db.getHoleInOnes first last
                                  |> JsonConvert.SerializeObject
                                  |> okJSON

  result ctx
