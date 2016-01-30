module StatTests

open FSharp.Data
open FsUnit
open TestUtilities
open Types
open Xunit

[<Fact>]
let ``Baseball home runs``() =
  let input = { FirstName = "Barry"; LastName = "Larkin"; ColumnIndex = 8; ValueFunction = Homeruns }
  let expected = Success { FirstName = "Barry"; LastName = "Larkin"; Stat = Homeruns 29}
  let doc = HtmlDocument.Parse baseballHtml

  (BaseballStats.stat doc input) |> should equal expected

[<Fact>]
let ``Baseball steals``() =
  let input = { FirstName = "Ken"; LastName = "Griffey Jr"; ColumnIndex = 12; ValueFunction = Steals }
  let expected = Success { FirstName = "Ken"; LastName = "Griffey Jr"; Stat = Steals 33}
  let doc = HtmlDocument.Parse baseballHtml

  (BaseballStats.stat doc input) |> should equal expected

[<Fact>]
let ``Baseball strikeouts``() =
  let input = { FirstName = "Chris"; LastName = "Sabo"; ColumnIndex = 11; ValueFunction = Strikeouts }
  let expected = Success { FirstName = "Chris"; LastName = "Sabo"; Stat = Strikeouts 32}
  let doc = HtmlDocument.Parse baseballHtml

  (BaseballStats.stat doc input) |> should equal expected

[<Fact>]
let ``Golf lowest tournament``() =
  let input = { FirstName = "Rory"; LastName = "Mcilroy"; ColumnIndex = 3; ValueFunction = LowestTournament }
  let golfInput = { Data = input; MapFunction = GolfStats.lowestTournamentMap; FilterFunction = (fun x -> x < 0); TotalFunction = Seq.min }
  let expected = Success { FirstName = "Rory"; LastName = "Mcilroy"; Stat = LowestTournament -28}
  let doc = HtmlDocument.Parse golfHtml

  (GolfStats.stat doc golfInput)
  |> should equal expected

[<Fact>]
let ``Golf lowest round``() =
  let input = { FirstName = "Tiger"; LastName = "Woods"; ColumnIndex = 2; ValueFunction = LowestRound }
  let golfInput = { Data = input; MapFunction = GolfStats.lowestRoundMap; FilterFunction = (fun x -> x > 50); TotalFunction = Seq.min }
  let expected = Success { FirstName = "Tiger"; LastName = "Woods"; Stat = LowestRound 58}
  let doc = HtmlDocument.Parse golfHtml

  (GolfStats.stat doc golfInput)
  |> should equal expected

[<Fact>]
let ``Golf total earnings``() =
  let input = { FirstName = "Phil"; LastName = "Mickelson"; ColumnIndex = 4; ValueFunction = GolfStats.totalEarningsValue }
  let golfInput = { Data = input; MapFunction = GolfStats.totalEarningsMap; FilterFunction = (fun x -> x > 0); TotalFunction = Seq.sum }
  let expected = Success { FirstName = "Phil"; LastName = "Mickelson"; Stat = TotalEarnings "$3,842,000"}
  let doc = HtmlDocument.Parse golfHtml

  (GolfStats.stat doc golfInput)
  |> should equal expected

[<Fact>]
let ``Fail lowest tournament record not found``() =
  let input = { FirstName = "Rory"; LastName = "Mcilroy"; ColumnIndex = 3; ValueFunction = LowestTournament }
  let golfInput = { Data = input; MapFunction = GolfStats.lowestTournamentMap; FilterFunction = (fun x -> x < 0); TotalFunction = Seq.min }
  let doc = HtmlDocument.Parse golfFailHtml

  match (GolfStats.stat doc golfInput) with
  | Failure RecordNotFound -> true
  | response -> failwith (sprintf "Expected Failure but found %A" response)

[<Fact>]
let ``Golf lowest round record not found``() =
  let input = { FirstName = "Tiger"; LastName = "Woods"; ColumnIndex = 2; ValueFunction = LowestRound }
  let golfInput = { Data = input; MapFunction = GolfStats.lowestRoundMap; FilterFunction = (fun x -> x > 50); TotalFunction = Seq.min }
  let doc = HtmlDocument.Parse golfFailHtml

  match (GolfStats.stat doc golfInput) with
  | Failure RecordNotFound -> true
  | response -> failwith (sprintf "Expected Failure but found %A" response)

[<Fact>]
let ``Golf total earnings record not found``() =
  let input = { FirstName = "Phil"; LastName = "Mickelson"; ColumnIndex = 4; ValueFunction = GolfStats.totalEarningsValue }
  let golfInput = { Data = input; MapFunction = GolfStats.totalEarningsMap; FilterFunction = (fun x -> x > 0); TotalFunction = Seq.sum }
  let doc = HtmlDocument.Parse golfFailHtml

  match (GolfStats.stat doc golfInput) with
  | Failure RecordNotFound -> true
  | response -> failwith (sprintf "Expected Failure but found %A" response)
