module Database

open Types

let getHoleInOnes firstName lastName : Athlete =
  { FirstName = firstName
    LastName = lastName
    Accomplishment = HoleInOnes 7 }

let DB =
  { new IDB with
      member x.getHoleInOnes first last =
        getHoleInOnes first last
  }
