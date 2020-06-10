module Euler20
open System
open DAP.EulerProblems.Utility

let answer = bigDigitSum (bigFact 100I)

#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 20", "https://projecteuler.net/problem=20")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
648
"
#endif
