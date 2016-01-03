module Types

type AccomplishmentType =
  | HoleInOnes of int
  | HomeRuns of int

type Athlete = {
  FirstName : string
  LastName : string
  Accomplishment : AccomplishmentType
}
