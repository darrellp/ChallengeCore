module Euler16
open System
open DAP.EulerProblems.Utility

let pow m n : bigint = 
    let rec recMultiply m n prod =
        match n with
        | x when x = 0I -> prod
        | _ -> recMultiply m (n - 1I) (prod * m)
    recMultiply m n 1I
    
let answer = bigDigitSum (pow 2I 1000I)

#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 16", "https://projecteuler.net/problem=16")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
1366
"
#endif


    