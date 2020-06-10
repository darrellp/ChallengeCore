module Euler25
open System
open DAP.EulerProblems.Utility

let answer =
    (fibos |>
        Seq.findIndex (
            fun t ->
                String.length(bigIntToNumericString t) >= 1000)) + 1

#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 25", "https://projecteuler.net/problem=25")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
4782
"
#endif
