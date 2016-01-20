module SportService

open Monads.Choice
open Newtonsoft.Json
open Suave
open Suave.Operators
open Suave.RequestErrors
open Suave.Successful
open Types

let CORS = Writers.addHeader "Access-Control-Allow-Origin" "*"
let JSON = Writers.setMimeType "application/json"
let okJSON s = OK s >=> JSON >=> CORS

let getName (ctx : HttpContext) =
  let r = ctx.request
  (r.queryParam "firstName"), (r.queryParam "lastName")

let processRoute statFunc (ctx : HttpContext) =
  let input = choice {
    let! firstName = fst (getName ctx)
    let! lastName = snd (getName ctx)
    return (firstName, lastName)
  }

  let result =
    match input with
    | Choice2Of2 err -> BAD_REQUEST err
    | Choice1Of2 (first, last) -> statFunc first last
                                  |> JsonConvert.SerializeObject
                                  |> okJSON

  result ctx

let getLowestTournament (db:IDB) (ctx : HttpContext) =
  processRoute db.GetLowestTournament ctx

let getLowestRound (db:IDB) (ctx : HttpContext) =
  processRoute db.GetLowestRound ctx

let getTotalGolfEarnings (db:IDB) (ctx : HttpContext) =
  processRoute db.GetTotalGolfEarnings ctx

let getHomeruns (db:IDB) (ctx : HttpContext) =
  processRoute db.GetHomeruns ctx

let getStrikeouts (db:IDB) (ctx : HttpContext) =
  processRoute db.GetStrikeouts ctx

let getSteals (db:IDB) (ctx : HttpContext) =
  processRoute db.GetSteals ctx
