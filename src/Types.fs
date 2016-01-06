module Types

type AccomplishmentType =
  | LowestTournament of int
  | HomeRuns of int

type Athlete = {
  FirstName : string
  LastName : string
  Accomplishment : AccomplishmentType
}

type IDB =
  abstract member GetLowestTournament : string -> string -> Athlete
