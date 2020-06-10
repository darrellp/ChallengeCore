module Euler77
open System
open DAP.EulerProblems.Utility

let answer = nonNegs |> skip 3 |> Seq.find (fun n -> (sumCount n primes) > 5000)

#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 77", "https://projecteuler.net/problem=77")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
71
"
#endif
