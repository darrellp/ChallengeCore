module Euler10
open System
open DAP.EulerProblems.Utility

let answer = Seq.fold (+) 0I (Seq.map (fun (x:int) -> new bigint(x)) (truncateIf (fun p -> (p >= 2000000)) primes))

#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 10", "https://projecteuler.net/problem=10")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
142913828922
"
#endif
