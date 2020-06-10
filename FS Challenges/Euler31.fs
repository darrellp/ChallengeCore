module Euler31
open System
open DAP.EulerProblems.Utility

// 1p, 2p, 5p, 10p, 20p, 50p, £1 (100p) and £2 (200p).

let answer = seq [1; 2; 5; 10; 20; 50; 100; 200] |> sumCount 200

#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 31", "https://projecteuler.net/problem=31")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
73682
"
#endif
