module Types

type StatType =
  | LowestTournament of int
  | LowestRound of decimal
  | HomeRuns of int

type Athlete = {
  FirstName : string
  LastName : string
  Stat : StatType
}

type IDB =
  abstract member GetLowestTournament : string -> string -> Athlete
  abstract member GetLowestRound : string -> string -> Athlete
