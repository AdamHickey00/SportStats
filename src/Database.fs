module Database

open Types

let getHoleInOnes firstName lastName : Athlete =
  { FirstName = firstName
    LastName = lastName
    Accomplishment = HoleInOnes 7 }
