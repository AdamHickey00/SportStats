module Database

open Types

let DB =
  { new IDB with

      // golf
      member x.GetLowestTournament first last =
        GolfStats.getLowestTournament first last
      member x.GetLowestRound first last =
        GolfStats.getLowestRound first last
      member x.GetTotalGolfEarnings first last =
        GolfStats.getTotalGolfEarnings first last

      // baseball
      member x.GetHomeruns first last =
        BaseballStats.getHomeruns first last
      member x.GetStrikeouts first last =
        BaseballStats.getStrikeouts first last
      member x.GetSteals first last =
        BaseballStats.getSteals first last
  }
