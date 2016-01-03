module ServiceTests

open FsUnit
open Suave.Types
open Types
open Xunit

let fakeDB accomplishment =
  { new IDB with
      member x.getHoleInOnes first last =
          { FirstName = first
            LastName = last
            Accomplishment = accomplishment }
  }

let getResponse content =
  match content with
  | Bytes byteValue -> System.Text.Encoding.ASCII.GetString byteValue
  | _ -> failwith "Bad response"

[<Fact>]
let ``Golf hole in ones Tiger Woods``() =
  let expectedResponse = "{\"FirstName\":\"Tiger\",\"LastName\":\"Woods\",\"Accomplishment\":{\"Case\":\"HoleInOnes\",\"Fields\":[7]}}"
  let db = fakeDB (HoleInOnes 7)

  let webPart = SportStats.routes db
  let query = "firstName=Tiger&lastName=Woods"
  let host = "http://localhost/Golf/HoleInOnes?" + query
  let request =
    { HttpRequest.empty with
        ``method`` = HttpMethod.GET
        url = new System.Uri(host); rawQuery = query }

  let context = { HttpContext.empty with request = request }
  let result = webPart context |> Async.RunSynchronously

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
