module Euler30
open System
open DAP.EulerProblems.Utility

// No seven digit number can have the property since the largest such a number could be is 7 * 9^5 < 9^6 < 10^6
// which is less than any seven digit so we only have to look at numbers smaller than a million.
let rec getDigits n = if n < 10 then [n] else List.append (getDigits (n / 10)) [n % 10]
let sumFifthPowers n = getDigits n |> List.map (fun n -> n*n*n*n*n) |> List.fold (+) 0
let meetsConditions n = (sumFifthPowers n) = n

let answer = seq {2..1000000} |> Seq.filter meetsConditions |> Seq.fold (+) 0

#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 30", "https://projecteuler.net/problem=30")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
443839
"
#endif

