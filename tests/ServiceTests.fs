module ServiceTests

open FsUnit
open TestUtilities
open Types
open Xunit

[<Fact>]
let ``Golf lowest tournament total Tiger Woods``() =
  let expectedResponse = "{\"FirstName\":\"Tiger\",\"LastName\":\"Woods\",\"Stat\":{\"Case\":\"LowestTournament\",\"Fields\":[-27]}}"
  let athlete = defaultAthlete "Tiger" "Woods" (LowestTournament -27)

  getResult "Tiger" "Woods" "Golf\LowestTournament" (fakeDB athlete)
  |> validate expectedResponse

[<Fact>]
let ``Golf lowest tournament round Lee Westwood``() =
  let expectedResponse = "{\"FirstName\":\"Lee\",\"LastName\":\"Westwood\",\"Stat\":{\"Case\":\"LowestRound\",\"Fields\":[60]}}"
  let athlete = (defaultAthlete "Lee" "Westwood" (LowestRound 60))

  getResult "Lee" "Westwood" "Golf\LowestRound" (fakeDB athlete)
  |> validate expectedResponse

[<Fact>]
let ``Golf total earnings Phil Mickelson``() =
  let expectedResponse = "{\"FirstName\":\"Phil\",\"LastName\":\"Mickelson\",\"Stat\":{\"Case\":\"TotalEarnings\",\"Fields\":[\"$6,700,931\"]}}"
  let athlete = (defaultAthlete "Phil" "Mickelson" (TotalEarnings "$6,700,931"))

  getResult "Phil" "Mickelson" "Golf\TotalEarnings" (fakeDB athlete)
  |> validate expectedResponse

[<Fact>]
let ``Golf homeruns Ken Griffey``() =
  let expectedResponse = "{\"FirstName\":\"Ken\",\"LastName\":\"Griffey\",\"Stat\":{\"Case\":\"Homeruns\",\"Fields\":[790]}}"
  let athlete = (defaultAthlete "Ken" "Griffey" (Homeruns 790))

  getResult "Ken" "Griffey" "Baseball\Homeruns" (fakeDB athlete)
  |> validate expectedResponse

[<Fact>]
let ``Golf strikeouts Ken Griffey``() =
  let expectedResponse = "{\"FirstName\":\"Ken\",\"LastName\":\"Griffey\",\"Stat\":{\"Case\":\"Strikeouts\",\"Fields\":[1033]}}"
  let athlete = (defaultAthlete "Ken" "Griffey" (Strikeouts 1033))

  getResult "Ken" "Griffey" "Baseball\Strikeouts" (fakeDB athlete)
  |> validate expectedResponse

[<Fact>]
let ``Golf steals Ken Griffey``() =
  let expectedResponse = "{\"FirstName\":\"Ken\",\"LastName\":\"Griffey\",\"Stat\":{\"Case\":\"Steals\",\"Fields\":[234]}}"
  let athlete = (defaultAthlete "Ken" "Griffey" (Steals 234))

  getResult "Ken" "Griffey" "Baseball\Steals" (fakeDB athlete)
  |> validate expectedResponse

[<Fact>]
let ``Golf lowest tournament record not found failure``() =
  let expectedResponse = "Record"
  let athlete = (defaultAthlete "Tiger" "Woods" (LowestTournament -27))

  getResult "Tiger" "Woods" "Golf\LowestTournament" (fakeDB athlete)
  |> validate expectedResponse
