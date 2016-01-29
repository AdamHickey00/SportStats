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

let stat (html:HtmlDocument) input =
  // get first row where first column is total
  let statValue =
    html.Descendants ["tr"]
    |> Seq.filter (fun x -> (getTextFromColumn 0 x) = "Total")
    |> Seq.head
    |> (fun x -> getTextFromColumn input.ColumnIndex x)

  Success { FirstName = input.FirstName
            LastName = input.LastName
            Stat = input.ValueFunc (int statValue) }

let getBaseballStat (input:DatabaseInput) =
  let link = getPlayerLink input

  match link with
  | None -> Failure RecordNotFound
  | Some linkValue ->
    let playerPage = BaseballPlayerSearch.Load(linkValue)
    stat playerPage.Html input

let getHomeruns firstName lastName =
  let input = { FirstName = firstName; LastName = lastName; ColumnIndex = 8; ValueFunc = Homeruns }
  getBaseballStat input

let getStrikeouts firstName lastName =
  let input = { FirstName = firstName; LastName = lastName; ColumnIndex = 11; ValueFunc = Strikeouts }
  getBaseballStat input 

let getSteals firstName lastName =
  let input = { FirstName = firstName; LastName = lastName; ColumnIndex = 12; ValueFunc = Steals }
  getBaseballStat input
