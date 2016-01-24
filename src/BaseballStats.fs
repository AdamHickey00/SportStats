module BaseballStats

open FSharp.Data
open Types
open Utils

type BaseballPlayerSearch = HtmlProvider<"http://search.espn.go.com/ken-griffey-jr/">
type BaseballStatPage = HtmlProvider<"http://espn.go.com/mlb/player/stats/_/id/2148/ken-griffey-jr">

let isPlayer name (link:HtmlNode) =
  let badChars = ['+'; '.']
  let cleaned = stripChars badChars name
  let innerTextCleaned = stripChars badChars (link.InnerText())

  System.String.Equals(innerTextCleaned, cleaned, System.StringComparison.OrdinalIgnoreCase)

let getBaseballStat (input:DatabaseInput) : Athlete =
  // get player stat link from search
  let name = input.FirstName + " " + input.LastName
  let baseballSearchUrl = sprintf "http://search.espn.go.com/%s/" (name.Replace(' ', '-'))
  let playersPage = BaseballPlayerSearch.Load(baseballSearchUrl)

  let value =
    playersPage.Html.Descendants ["a"]
    |> Seq.filter (isPlayer name)
    |> Seq.toList

  match value with
  | [] -> { FirstName = input.FirstName
            LastName = input.LastName
            Stat = Homeruns (int 0) }
  | element::elements ->
    let link = element.TryGetAttribute("href").Value.Value()

    printfn "Value = %s" link

    { FirstName = input.FirstName
      LastName = input.LastName
      Stat = Homeruns (int 1) }

let getHomeruns firstName lastName : Athlete =
  let input = { FirstName = firstName; LastName = lastName; ColumnIndex = 4 }
  getBaseballStat input

let getStrikeouts firstName lastName : Athlete =
  let input = { FirstName = firstName; LastName = lastName; ColumnIndex = 4 }
  getBaseballStat input

let getSteals firstName lastName : Athlete =
  let input = { FirstName = firstName; LastName = lastName; ColumnIndex = 4 }
  getBaseballStat input
