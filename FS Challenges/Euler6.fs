module Euler6
open System
open DAP.EulerProblems.Utility

let ar = [| 1I .. 100I |]
let sq x = x * x

let answer = 
    (sq (Array.reduce (+) ar)) -
        (Array.reduce (+) (Array.map sq ar))

#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 6", "https://projecteuler.net/problem=6")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
25164150
"
#endif
