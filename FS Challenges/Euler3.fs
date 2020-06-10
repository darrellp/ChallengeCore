module Euler3
open System
open DAP.EulerProblems.Utility

let answer =
    let factorization = factorBig 600851475143I
    Seq.item 0 factorization


#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 3", "https://projecteuler.net/problem=3")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
6857
"
#endif
