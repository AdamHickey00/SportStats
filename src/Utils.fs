module Utils

open FSharp.Data
open System.Linq

let reverse (value : string) =
  value |> Seq.fold (fun acc x -> string x + acc) ""

let combine (value : seq<string>) =
  value |> Seq.fold (fun acc x -> acc + x) ""

let stripChars (chars:seq<char>) (value:string) =
  value
  |> Seq.filter (fun x -> not (chars |> Seq.exists (fun invalidChar -> invalidChar = x)))
  |> Seq.map string
  |> combine

let getTextFromColumn columnIndex (row:HtmlNode) =
  (row.Descendants ["td"]).ElementAt(columnIndex).InnerText()
