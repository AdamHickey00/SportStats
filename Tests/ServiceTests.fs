module ServiceTests

open FsUnit
open Suave.Types
open Xunit

let getResponse content =
  match content with
  | Bytes byteValue -> System.Text.Encoding.ASCII.GetString byteValue
  | _ -> failwith "Bad response"

[<Fact>]
let ``Golf hole in ones Tiger Woods``() =
    let webPart = SportStats.routes

    let query = "firastName=Tiger&lastName=Woods"
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
        //ctx.response.headers
        //|> List.exists(fun (k,v) -> k = "Content-Type" && v = "application/json")
        //|> should equal true

        // check resonse
        getResponse ctx.response.content
        |> should equal "Hello 3"
