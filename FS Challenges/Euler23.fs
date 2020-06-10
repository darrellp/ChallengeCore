module Euler23
open System
open DAP.EulerProblems.Utility

let allFactors2 n =
    let top = n / 2
    seq {
        for index in [1 .. top] do
            if n % index = 0 then
                yield index
        if n <> 1 then yield n
        }
    
let isAbundant n =
    (allFactors n |> Seq.fold (+) 0) - n > n

let nTop = 28128
let abundants = nonNegs |> skip 3 |> Seq.filter isAbundant |> truncateIf (fun n -> n > 28128) |> Seq.toArray

let makeMapping (arr:array<int>) =
    let rec marker (m:array<bool>) i = 
        if i < 0 then m
        else
            m.[arr.[i]] <- true
            marker m (i - 1)
    let mrk = marker (Array.create (arr.[arr.Length - 1] + 1) false) (arr.Length - 1)
    (mrk,
        (fun n ->
            if n >= mrk.Length then false
            else mrk.[n]))
    
let (mapping, isAbundantCheck) = (makeMapping abundants)
    
let checkSummability n =
    let rec checkSummabilityRecurse n i =
        match n with
        | _ when abundants.[i] > (n / 2) -> false
        | _ when isAbundantCheck (n - abundants.[i]) -> true
        | _ -> checkSummabilityRecurse n (i + 1)
            
    checkSummabilityRecurse n 0

let summables =
    seq {
        for n in [0 .. nTop] do
            if not (checkSummability n) then
                yield n
    }

let answer = summables |> Seq.fold (+) 0


#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 23", "https://projecteuler.net/problem=23")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
4179871
"
#endif
