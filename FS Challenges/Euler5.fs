module Euler5
open System
open DAP.EulerProblems.Utility

let answer = Seq.fold bigLCM 1I [1I .. 20I]

#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 5", "https://projecteuler.net/problem=5")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
232792560
"
#endif
