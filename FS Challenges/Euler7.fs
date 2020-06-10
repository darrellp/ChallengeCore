#light

module Euler7
open System
open DAP.EulerProblems.Utility

let answer = Seq.item 10000 primes

#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 7", "https://projecteuler.net/problem=7")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
104743
"
#endif
