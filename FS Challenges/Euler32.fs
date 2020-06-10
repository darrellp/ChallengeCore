module Euler32
open System
open DAP.EulerProblems.Utility

let unitCombos = [|
    (2,3); (2,4); (2,7); (2,8); (2,9); (3,4); (3,6); (3,7); (3,8); (3,9); (4,7); (4,8); (4,9); (6,7); (6,9); (7,8); (7,9); (8,9) |]
    
let nonNegDigits = [| 0; 1; 2; 3; 4; 5; 6; 7; 8; 9 |]
let posDigits = [| 1; 2; 3; 4; 5; 6; 7; 8; 9 |]
let allBut d1 d2 (s:array<'a>) = Array.filter (fun i -> i <> d1 && i <> d2) s
let cross (s:array<'a>) (t:array<'b>) : array<'a * 'b> =
    let ls = Array.length s
    let lt = Array.length t
    Array.init (ls * lt) (fun i -> (s.[i / lt], t.[i % lt]))
    
let rec check (df:array<bool>) (n:int) =
    let digit = n % 10
    match n with
    | 0 -> true
    | _ when digit = 0 || df.[digit] -> false
    | _ -> df.[digit] <- true; check df (n / 10)

let checkDigits a b c =
    let digitsFound = [| for i in 1 .. 10 -> false |]
    (check digitsFound c) && (check digitsFound b) && (check digitsFound a)
    
let feasibleHundreds unit1 unit2 =
    /// First hundred can have a zero in the tens place to take care of the 1 digit * 4 digits = 4 digits case.  Second cannot have
    /// a zero in the hundreds place.
    posDigits |>
        allBut unit1 unit2 |>
        cross (nonNegDigits |> allBut unit1 unit2) |>
        Array.map (fun (a,b) ->
            let m1 = 10 * a + unit1
            let m2 = 10 * b + unit2
            (m1, m2, m1 * m2)) |>
        Array.filter (fun (a, b, c) -> checkDigits a b (c % 100)) |>
        Array.map (fun (a, b, c) -> (a, b))

let allFeasibleHundreds =
    unitCombos |>
    Array.map (fun (a, b) -> (b, a)) |>
    Array.append unitCombos |>
    Array.map (fun (d1, d2) -> feasibleHundreds d1 d2) |>
    Array.concat

let FindPanDigital h1 h2 =
    (if h1 < 10 then [| 10..99 |] else [| 1..9 |]) |>
    Array.map (fun i ->
        let m2 = h2 + i * 100
        (h1, m2, h1 * m2)) |>
    Array.filter (fun (a, b, c) -> c < 10000 && (checkDigits a b c))
        
let answer =
    allFeasibleHundreds |>
    Array.map (fun (a, b) -> FindPanDigital a b) |>
    Array.concat |>
    Array.map (fun (a, b, c) -> c) |>
    Set.ofSeq |>
    Seq.fold (+) 0

#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 32", "https://projecteuler.net/problem=32")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
45228
"
#endif
