module Database

open FSharp.Data
open System.Linq
open Types

type LowestTournamentPage = HtmlProvider<"http://www.golfstats.com/search/?stat=6&player=Tiger+Woods&submit=go">
type BaseballPlayerSearch = HtmlProvider<"http://www.baseballamerica.com/statistics/players/search/?name_last=G&srch=alph">
type BaseballStatPage = HtmlProvider<"http://www.baseballamerica.com/statistics/players/cards/17588">
//value = "/statistics/players/cards/17588"

type Input = {
  FirstName : string
  LastName : string
  ColumnIndex : int
}

let getTextFromColumn columnIndex (row:HtmlNode) =
  (row.Descendants ["td"]).ElementAt(columnIndex).InnerText()

let stripChars (chars:seq<char>) (value:string) =
  value
  |> Seq.filter (fun x -> not (chars |> Seq.exists (fun invalidChar -> invalidChar = x)))
  |> System.String.Concat

let lowestRoundMap columnIndex (row:HtmlNode) : int =
  let columnValue = getTextFromColumn columnIndex row

  // value like "66-61-68-70=265"
  (columnValue.Split [|'='|]).[0].Split [|'-'|]
  |> Array.map int
  |> Array.min

let totalEarningsMap columnIndex (row:HtmlNode) : int =
  let columnValue = getTextFromColumn columnIndex row

  // take only numbers
  int (stripChars ['$'; '~'; ','; ' '] columnValue)

let lowestTournamentMap columnIndex (row:HtmlNode) : int =
  int (getTextFromColumn columnIndex row)

let totalEarningsValue (earnings:int) =
  TotalEarnings (Money.formatMoney earnings)

let isPlayer name (link:HtmlNode)  =
  System.String.Equals(link.InnerText(), name, System.StringComparison.OrdinalIgnoreCase)

let getBaseballStat (input:Input) : Athlete =
  // get player stat link from search page
  let name = input.FirstName + " " + input.LastName
  let lastNameChar = input.LastName.Chars(0)
  let baseballSearchUrl = sprintf "http://www.baseballamerica.com/statistics/players/search/?name_last=%c&srch=alph" lastNameChar
  let playersPage = BaseballPlayerSearch.Load(baseballSearchUrl)
  let tables = playersPage.Html.Descendants ["table"]
  let value =
    tables
      |> Seq.item 2 // take third
      |> (fun x -> x.Descendants ["a"])
      |> Seq.filter (isPlayer name)
      |> Seq.toList

  match value with
  | [] -> { FirstName = input.FirstName
            LastName = input.LastName
            Stat = Homeruns (int 0) }
  | element::elements ->
    let relativePath = element.TryGetAttribute("href").Value
    let link = sprintf "http://www.baseballamerica.com%s" (relativePath.Value())

    printfn "Value = %A" link

    { FirstName = input.FirstName
      LastName = input.LastName
      Stat = Homeruns (int 1) }

let getGolfStat (input:Input) mapFunc filterFunc (valueFunc: int -> StatType) (totalFunc: seq<'a> -> int) : Athlete =
  let url = sprintf "http://www.golfstats.com/search/?stat=6&player=%s+%s&submit=go" input.FirstName input.LastName
  let stats = LowestTournamentPage.Load(url)
  let tables = stats.Html.Descendants ["table"]

  match Seq.length tables with
  | 0 -> { FirstName = input.FirstName
           LastName = input.LastName
           Stat = LowestTournament (int 0) }

  | _ -> let value =
           tables
           |> Seq.head
           |> (fun x -> x.Descendants ["tbody"])
           |> Seq.head
           |> (fun x -> (x.Descendants ["tr"]))
           |> Seq.map (mapFunc input.ColumnIndex)
           |> Seq.filter filterFunc
           |> totalFunc

         { FirstName = input.FirstName
           LastName = input.LastName
           Stat = valueFunc value }

let getLowestTournament firstName lastName : Athlete =
  let input = { FirstName = firstName; LastName = lastName; ColumnIndex = 3 }
  getGolfStat input lowestTournamentMap (fun x -> x < 0) LowestTournament Seq.min

let getLowestRound firstName lastName : Athlete =
  let input = { FirstName = firstName; LastName = lastName; ColumnIndex = 2 }
  getGolfStat input lowestRoundMap (fun x -> x > 50) LowestRound Seq.min

let getTotalGolfEarnings firstName lastName : Athlete =
  let input = { FirstName = firstName; LastName = lastName; ColumnIndex = 4 }
  getGolfStat input totalEarningsMap (fun x -> x > 0) totalEarningsValue Seq.sum

let getHomeruns firstName lastName : Athlete =
  let input = { FirstName = firstName; LastName = lastName; ColumnIndex = 4 }
  getBaseballStat input

let getStrikeouts firstName lastName : Athlete =
  let input = { FirstName = firstName; LastName = lastName; ColumnIndex = 4 }
  getGolfStat input totalEarningsMap (fun x -> x > 0) Strikeouts Seq.sum

let getSteals firstName lastName : Athlete =
  let input = { FirstName = firstName; LastName = lastName; ColumnIndex = 4 }
  getGolfStat input totalEarningsMap (fun x -> x > 0) Steals Seq.sum

let DB =
  { new IDB with
      member x.GetLowestTournament first last =
        getLowestTournament first last
      member x.GetLowestRound first last =
        getLowestRound first last
      member x.GetTotalGolfEarnings first last =
        getTotalGolfEarnings first last

      member x.GetHomeruns first last =
        getHomeruns first last
      member x.GetStrikeouts first last =
        getStrikeouts first last
      member x.GetSteals first last =
        getSteals first last
  }
