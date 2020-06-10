module Euler15
open System
open DAP.EulerProblems.Utility

let answer = binomial 40 20

#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 15", "https://projecteuler.net/problem=15")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
137846528820
"
#endif
