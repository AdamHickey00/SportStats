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

let getPlayerLink (input:DatabaseInput) : Option<string> =
  // get player stat link from search
  let name = input.FirstName + " " + input.LastName
  let baseballSearchUrl = sprintf "http://search.espn.go.com/%s/" (name.Replace(' ', '-'))
  let playersPage = BaseballPlayerSearch.Load(baseballSearchUrl)

  let playerLinks =
    playersPage.Html.Descendants ["a"]
    |> Seq.filter (isPlayer name)
    |> Seq.toList

  match playerLinks with
  | [] -> None
  | firstLink::elements -> Some (firstLink.TryGetAttribute("href").Value.Value())

let getBaseballStat (input:DatabaseInput) : Athlete =
  let link = getPlayerLink input

  match link with
  | None -> { FirstName = input.FirstName
              LastName = input.LastName
              Stat = Homeruns (int 0) }
  | Some linkValue ->
    let playerPage = BaseballPlayerSearch.Load(linkValue)

    // get first row where first column is total
    let statValue =
      playerPage.Html.Descendants ["tr"]
      |> Seq.filter (fun x -> (getTextFromColumn 0 x) = "Total")
      |> Seq.head
      |> (fun x -> getTextFromColumn input.ColumnIndex x)

    { FirstName = input.FirstName
      LastName = input.LastName
      Stat = Homeruns (int statValue) }

let getHomeruns firstName lastName : Athlete =
  let input = { FirstName = firstName; LastName = lastName; ColumnIndex = 8 }
  getBaseballStat input

let getStrikeouts firstName lastName : Athlete =
  let input = { FirstName = firstName; LastName = lastName; ColumnIndex = 11 }
  getBaseballStat input

let getSteals firstName lastName : Athlete =
  let input = { FirstName = firstName; LastName = lastName; ColumnIndex = 12 }
  getBaseballStat input
