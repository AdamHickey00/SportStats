module Types

type StatType =
  | LowestTournament of int
  | LowestRound of int
  | TotalEarnings of string
  | Homeruns of int
  | Strikeouts of int
  | Steals of int

type Athlete = {
  FirstName : string
  LastName : string
  Stat : StatType
}

type IDB =
  abstract member GetLowestTournament : string -> string -> Athlete
  abstract member GetLowestRound : string -> string -> Athlete
  abstract member GetTotalGolfEarnings : string -> string -> Athlete
  abstract member GetHomeruns : string -> string -> Athlete
  abstract member GetStrikeouts : string -> string -> Athlete
  abstract member GetSteals : string -> string -> Athlete
