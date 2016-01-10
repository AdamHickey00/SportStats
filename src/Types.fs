module Types

type StatType =
  | LowestTournament of int
  | LongestDriveAvg of decimal
  | HomeRuns of int

type Athlete = {
  FirstName : string
  LastName : string
  Stat : StatType
}

type IDB =
  abstract member GetLowestTournament : string -> string -> Athlete
  abstract member GetLongestDriveAvg : string -> string -> Athlete
