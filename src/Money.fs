module Money

open Utils

let private addCommas intWithIndex =
  let index = fst intWithIndex
  let value = string (snd intWithIndex)

  if (index > 0 && index % 3 = 0) then
    "," + value
  else
    value

let private addIndexes = Seq.mapi (fun i x -> i,x)

let formatMoney (value:int) =
  "$" + (value.ToString()
         |> reverse
         |> addIndexes
         |> Seq.map addCommas
         |> combine
         |> reverse)
