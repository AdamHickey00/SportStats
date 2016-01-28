module TestUtilities

open FsUnit
open Suave.Http
open Types
open Xunit

let defaultAthlete first last stat : Response<Athlete> =
  Success { FirstName = first
            LastName = last
            Stat = stat }

let fakeDB (response:Response<Athlete>) =
  { new IDB with
      member x.GetLowestTournament first last = response
      member x.GetLowestRound first last = response
      member x.GetTotalGolfEarnings first last = response
      member x.GetHomeruns first last = response
      member x.GetStrikeouts first last = response
      member x.GetSteals first last = response
  }

let response content =
  match content with
  | Bytes byteValue -> System.Text.Encoding.ASCII.GetString byteValue
  | _ -> failwith "Bad response"

let result firstName lastName route db =
  let webPart = SportStats.routes db
  let query = sprintf "firstName=%s&lastName=%s" firstName lastName
  let host = sprintf "http://localhost/%s?%s" route query

  let request =
    { HttpRequest.empty with
        ``method`` = HttpMethod.GET
        url = new System.Uri(host); rawQuery = query }

  let context = { HttpContext.empty with request = request }
  webPart context |> Async.RunSynchronously

let context (result:HttpContext option) =
  match result with
  | None -> failwith "Route not found"
  | Some ctx -> ctx

let validateStatus code ctx =
  ctx.response.status |> should equal code
  ctx

let isJson ctx =
  ctx.response.headers
  |> List.exists(fun (k,v) -> k = "Content-Type" && v = "application/json")
  |> should equal true
  ctx

let validateResponse expectedResponse ctx =
  response ctx.response.content
  |> should equal expectedResponse

let validateSuccess expectedResponse (result:HttpContext option) =
  context result
  |> validateStatus HttpCode.HTTP_200
  |> isJson
  |> validateResponse expectedResponse

let validateFailure expectedResponse code (result:HttpContext option) =
  context result
  |> validateStatus code
  |> validateResponse expectedResponse
