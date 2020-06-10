module Euler2
open System
open DAP.EulerProblems.Utility

let answer =
    fibos |>
    Seq.filter(fun x -> x % 2I = 0I) |>
    truncateIf (fun x -> x > 4000000I) |>
    Seq.fold (+) 0I
 
#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 2", "https://projecteuler.net/problem=2")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
4613732
"
#endif
 