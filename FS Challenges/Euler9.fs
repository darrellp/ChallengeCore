module Euler9
open System
open DAP.EulerProblems.Utility

let answer = (20*20-5*5)*(2*20*5)*(20*20+5*5)

#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 9", "https://projecteuler.net/problem=9")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
31875000
"
#endif
