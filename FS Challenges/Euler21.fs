module Euler21
open System
open DAP.EulerProblems.Utility

let answer =
    let DivisorSum n =
        (allFactors n |> Seq.fold (+) 0) - n
        
    let amicable n =
        let d = DivisorSum n
        n <> d && DivisorSum d = n
    
    nonNegs |> Seq.truncate 10000 |> Seq.filter amicable |> Seq.fold (+) 0

#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 21", "https://projecteuler.net/problem=21")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
31626
"
#endif
