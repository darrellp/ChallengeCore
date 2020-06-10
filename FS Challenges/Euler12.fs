module Euler12
open System
open DAP.EulerProblems.Utility

let answer = nonNegs |> Seq.map (fun n -> n * (n + 1) / 2) |> Seq.find (fun n -> (Seq.length (allFactors n)) > 500)

#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 12", "https://projecteuler.net/problem=12")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
76576500
"
#endif
