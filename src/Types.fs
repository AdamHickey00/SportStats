module Types

open FSharp.Data

type FailureCode =
  | RecordNotFound

type Response<'a> =
  | Success of 'a
  | Failure of FailureCode

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

type DatabaseInput = {
  FirstName : string
  LastName : string
  ColumnIndex : int
  ValueFunction : int -> StatType
}

type GolfInput<'a> = {
  Data : DatabaseInput
  MapFunction : int -> HtmlNode -> int Option
  FilterFunction : int Option -> bool
  TotalFunction : seq<'a> -> int
}

type IDB =
  abstract member GetLowestTournament : string -> string -> Response<Athlete>
  abstract member GetLowestRound : string -> string -> Response<Athlete>
  abstract member GetTotalGolfEarnings : string -> string -> Response<Athlete>
  abstract member GetHomeruns : string -> string -> Response<Athlete>
  abstract member GetStrikeouts : string -> string -> Response<Athlete>
  abstract member GetSteals : string -> string -> Response<Athlete>
