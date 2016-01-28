module StatTests

open FSharp.Data
open FsUnit
open TestUtilities
open Types
open Xunit

[<Fact>]
let ``Baseball home runs``() =
  let input = { FirstName = "Barry"; LastName = "Larkin"; ColumnIndex = 8 }
  let expected = Success { FirstName = "Barry"; LastName = "Larkin"; Stat = Homeruns 29}
  let doc = HtmlDocument.Parse baseballHtml

  (BaseballStats.stat doc input Homeruns) |> should equal expected

[<Fact>]
let ``Baseball steals``() =
  let input = { FirstName = "Ken"; LastName = "Griffey Jr"; ColumnIndex = 12 }
  let expected = Success { FirstName = "Ken"; LastName = "Griffey Jr"; Stat = Steals 33}
  let doc = HtmlDocument.Parse baseballHtml

  (BaseballStats.stat doc input Steals) |> should equal expected

[<Fact>]
let ``Baseball strikeouts``() =
  let input = { FirstName = "Chris"; LastName = "Sabo"; ColumnIndex = 11 }
  let expected = Success { FirstName = "Chris"; LastName = "Sabo"; Stat = Strikeouts 32}
  let doc = HtmlDocument.Parse baseballHtml

  (BaseballStats.stat doc input Strikeouts) |> should equal expected
