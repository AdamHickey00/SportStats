module ServiceTests

open FsUnit
open Suave.Http
open Types
open Xunit

let fakeDB stat =
  let getAthlete first last =
    { FirstName = first
      LastName = last
      Stat = stat }

  { new IDB with
      member x.GetLowestTournament first last = getAthlete first last
      member x.GetLowestRound first last = getAthlete first last
      member x.GetTotalGolfEarnings first last = getAthlete first last
  }

let getResponse content =
  match content with
  | Bytes byteValue -> System.Text.Encoding.ASCII.GetString byteValue
  | _ -> failwith "Bad response"

let getResult firstName lastName route db =
  let webPart = SportStats.routes db
  let query = sprintf "firstName=%s&lastName=%s" firstName lastName
  let host = sprintf "http://localhost/Golf/%s?%s" route query

  let request =
    { HttpRequest.empty with
        ``method`` = HttpMethod.GET
        url = new System.Uri(host); rawQuery = query }

  let context = { HttpContext.empty with request = request }
  webPart context |> Async.RunSynchronously

let validate expectedResponse result =
  match result with
  | None -> failwith "Route not found"
  | Some ctx ->
    // response good
    ctx.response.status |> should equal HttpCode.HTTP_200

    // contains json header
    ctx.response.headers
    |> List.exists(fun (k,v) -> k = "Content-Type" && v = "application/json")
    |> should equal true

    // check resonse
    getResponse ctx.response.content
    |> should equal expectedResponse

[<Fact>]
let ``Golf lowest tournament total Tiger Woods``() =
  let expectedResponse = "{\"FirstName\":\"Tiger\",\"LastName\":\"Woods\",\"Stat\":{\"Case\":\"LowestTournament\",\"Fields\":[-27]}}"

  getResult "Tiger" "Woods" "LowestTournament" (fakeDB (LowestTournament -27))
  |> validate expectedResponse

[<Fact>]
let ``Golf lowest tournament round Lee Westwood``() =
  let expectedResponse = "{\"FirstName\":\"Lee\",\"LastName\":\"Westwood\",\"Stat\":{\"Case\":\"LowestRound\",\"Fields\":[60]}}"

  getResult "Lee" "Westwood" "LowestRound" (fakeDB (LowestRound 60))
  |> validate expectedResponse

[<Fact>]
let ``Golf total earnings Phil Mickelson``() =
  let expectedResponse = "{\"FirstName\":\"Phil\",\"LastName\":\"Mickelson\",\"Stat\":{\"Case\":\"TotalEarnings\",\"Fields\":[6700931]}}"

  getResult "Phil" "Mickelson" "TotalEarnings" (fakeDB (TotalEarnings 6700931))
  |> validate expectedResponse