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

[<Literal>]
let baseballHtml =
  """<html>
         <body>
             <table>
                 <tr>
                    <td>1990</td>
                    <td>SEA</td>
                    <td>1</td> <!-- GP -->
                    <td>2</td> <!-- AB -->
                    <td>3</td> <!-- R -->
                    <td>4</td> <!-- H -->
                    <td>5</td> <!-- 2B -->
                    <td>6</td> <!-- 3B -->
                    <td>7</td> <!-- HR -->
                    <td>8</td> <!-- RBI -->
                    <td>9</td> <!-- BB -->
                    <td>10</td> <!-- SO -->
                    <td>11</td> <!-- SB -->
                    <td>12</td> <!-- CS -->
                    <td>13</td> <!-- AVG -->
                    <td>14</td> <!-- OBP -->
                    <td>15</td> <!-- SLG -->
                    <td>16</td> <!-- OPS -->
                    <td>17</td> <!-- WAR -->
                </tr>
                <tr>
                    <td>Total</td>
                    <td>Total</td>
                    <td>23</td> <!-- GP -->
                    <td>24</td> <!-- AB -->
                    <td>25</td> <!-- R -->
                    <td>26</td> <!-- H -->
                    <td>27</td> <!-- 2B -->
                    <td>28</td> <!-- 3B -->
                    <td>29</td> <!-- HR -->
                    <td>30</td> <!-- RBI -->
                    <td>31</td> <!-- BB -->
                    <td>32</td> <!-- SO -->
                    <td>33</td> <!-- SB -->
                    <td>34</td> <!-- CS -->
                    <td>35</td> <!-- AVG -->
                    <td>36</td> <!-- OBP -->
                    <td>37</td> <!-- SLG -->
                    <td>38</td> <!-- OPS -->
                    <td>39</td> <!-- WAR -->
                </tr>
          </table>
      </body>
  </html>"""

[<Literal>]
let golfHtml =
  """<html>
         <body>
             <table>
                 <tbody>
                     <tr>
                        <td>login</td>
                        <td>Win</td> <!-- Final finish -->
                        <td>61-67-70-71=269</td> <!-- Final score -->
                        <td>-27</td> <!-- Final score to par -->
                        <td>$864,000</td> <!-- Final money -->
                        <td>fedex</td>
                    </tr>
                    <tr>
                        <td>login</td>
                        <td>T15</td> <!-- Final finish -->
                        <td>66-71-70-71=278</td> <!-- Final score -->
                        <td>-28</td> <!-- Final score to par -->
                        <td>$1,997,000</td> <!-- Final money -->
                        <td>fedex</td>
                    </tr>
                    <tr>
                        <td>login</td>
                        <td>Win</td> <!-- Final finish -->
                        <td>72-71-70-71=284</td> <!-- Final score -->
                        <td>-18</td> <!-- Final score to par -->
                        <td>$322,000</td> <!-- Final money -->
                        <td>fedex</td>
                   </tr>
                   <tr>
                        <td>login</td>
                        <td>T33</td> <!-- Final finish -->
                        <td>58-77-64-60=259</td> <!-- Final score -->
                        <td>-17</td> <!-- Final score to par -->
                        <td>$659,000</td> <!-- Final money -->
                        <td>fedex</td>
                   </tr>
               </tbody>
          </table>
      </body>
  </html>"""
