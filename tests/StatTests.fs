module StatTests

open FSharp.Data
open FsUnit
open TestUtilities
open Types
open Xunit

[<Fact>]
let ``Baseball home runs``() =
  let input = { FirstName = "Barry"; LastName = "Larkin"; ColumnIndex = 8; ValueFunc = Homeruns }
  let expected = Success { FirstName = "Barry"; LastName = "Larkin"; Stat = Homeruns 29}
  let doc = HtmlDocument.Parse baseballHtml

  (BaseballStats.stat doc input) |> should equal expected

[<Fact>]
let ``Baseball steals``() =
  let input = { FirstName = "Ken"; LastName = "Griffey Jr"; ColumnIndex = 12; ValueFunc = Steals }
  let expected = Success { FirstName = "Ken"; LastName = "Griffey Jr"; Stat = Steals 33}
  let doc = HtmlDocument.Parse baseballHtml

  (BaseballStats.stat doc input) |> should equal expected

[<Fact>]
let ``Baseball strikeouts``() =
  let input = { FirstName = "Chris"; LastName = "Sabo"; ColumnIndex = 11; ValueFunc = Strikeouts }
  let expected = Success { FirstName = "Chris"; LastName = "Sabo"; Stat = Strikeouts 32}
  let doc = HtmlDocument.Parse baseballHtml

  (BaseballStats.stat doc input) |> should equal expected

[<Fact>]
let ``Golf lowest tournament``() =
  let input = { FirstName = "Rory"; LastName = "Mcilroy"; ColumnIndex = 3; ValueFunc = LowestTournament }
  let expected = Success { FirstName = "Rory"; LastName = "Mcilroy"; Stat = LowestTournament -28}
  let doc = HtmlDocument.Parse golfHtml

  (GolfStats.stat doc input GolfStats.lowestTournamentMap (fun x -> x < 0) Seq.min)
  |> should equal expected

[<Fact>]
let ``Golf lowest round``() =
  let input = { FirstName = "Tiger"; LastName = "Woods"; ColumnIndex = 2; ValueFunc = LowestRound }
  let expected = Success { FirstName = "Tiger"; LastName = "Woods"; Stat = LowestRound 58}
  let doc = HtmlDocument.Parse golfHtml

  (GolfStats.stat doc input GolfStats.lowestRoundMap (fun x -> x > 50) Seq.min)
  |> should equal expected

[<Fact>]
let ``Golf total earnings``() =
  let input = { FirstName = "Phil"; LastName = "Mickelson"; ColumnIndex = 4; ValueFunc = GolfStats.totalEarningsValue }
  let expected = Success { FirstName = "Phil"; LastName = "Mickelson"; Stat = TotalEarnings "$3,842,000"}
  let doc = HtmlDocument.Parse golfHtml

  (GolfStats.stat doc input GolfStats.totalEarningsMap (fun x -> x > 0) Seq.sum)
  |> should equal expected

[<Fact>]
let ``Fail lowest tournament record not found``() =
  let input = { FirstName = "Rory"; LastName = "Mcilroy"; ColumnIndex = 3; ValueFunc = LowestTournament }
  let doc = HtmlDocument.Parse golfFailHtml

  match (GolfStats.stat doc input GolfStats.lowestTournamentMap (fun x -> x < 0) Seq.min) with
  | Failure RecordNotFound -> true
  | response -> failwith (sprintf "Expected Failure but found %A" response)

[<Fact>]
let ``Golf lowest round record not found``() =
  let input = { FirstName = "Tiger"; LastName = "Woods"; ColumnIndex = 2; ValueFunc = LowestRound }
  let doc = HtmlDocument.Parse golfFailHtml

  match (GolfStats.stat doc input GolfStats.lowestRoundMap (fun x -> x > 50) Seq.min) with
  | Failure RecordNotFound -> true
  | response -> failwith (sprintf "Expected Failure but found %A" response)

[<Fact>]
let ``Golf total earnings record not found``() =
  let input = { FirstName = "Phil"; LastName = "Mickelson"; ColumnIndex = 4; ValueFunc = GolfStats.totalEarningsValue }
  let doc = HtmlDocument.Parse golfFailHtml

  match (GolfStats.stat doc input GolfStats.totalEarningsMap (fun x -> x > 0) Seq.sum) with
  | Failure RecordNotFound -> true
  | response -> failwith (sprintf "Expected Failure but found %A" response)
