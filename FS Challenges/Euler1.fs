module Euler1
open System
open DAP.EulerProblems.Utility

let answer =
    [3 .. 999] |>
    Seq.filter(fun x -> x % 3 = 0 || x % 5 = 0) |>
    Seq.fold (+) 0

#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 1", "https://projecteuler.net/problem=1")>]
type TestChallenge() =
    interface IChallenge with
        member this.Solve() =
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
233168
"
#endif