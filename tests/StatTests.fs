module StatTests

open FsUnit
open Suave.Http
open TestUtilities
open Types
open Xunit

[<Fact>]
let ``Baseball home runs``() =
  BaseballStats.stat
