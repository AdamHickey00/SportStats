module Utils

let reverse (value : string) =
  value |> Seq.fold (fun acc x -> string x + acc) ""

let combine (value : seq<string>) =
  value |> Seq.fold (fun acc x -> acc + x) ""
