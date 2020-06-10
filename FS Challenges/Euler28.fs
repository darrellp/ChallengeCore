module Euler28
open System
open DAP.EulerProblems.Utility
    
let answer = (fun n -> -3 + 2 * (6 + 13 * n + 15 * (pow n 2) + 8 * (pow n 3)) / 3) 500

#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 28", "https://projecteuler.net/problem=28")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
669171001
"
#endif
