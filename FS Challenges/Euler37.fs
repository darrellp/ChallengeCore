module Euler37
open System
open DAP.EulerProblems.Utility
    
let digitCheck (s:string) =
    let sBack = s.Substring(1)
    not (
        s.Contains "0" ||
        sBack.Contains "2" ||
        s.Contains "4" ||
        s.Contains "6" ||
        s.Contains "8" ||
        sBack.Contains "5")
        
let primeString (s:string) =
    isPrime (System.Int32.Parse(s))

let dropFirst (s:string) = s.Substring(1)
let dropLast (s:string)  = s.Substring(0, s.Length - 1)
let truncable (s:string) =
    let rec tLeft (s:string) =
        match s with
        | "" -> true
        | _ -> (primeString s) && tLeft (dropFirst s)
        
    let rec tRight (s:string) =
        match s with
        | "" -> true
        | _ -> (primeString s) && tRight (dropLast s)
    
    tLeft (dropFirst s) && tRight (dropLast s)

    
let truncatablePrime (n:int) =
    let s = n.ToString()
    if digitCheck s then
        truncable s
    else
        false

let answer = primes |> skip 4 |> Seq.filter (fun n -> truncatablePrime n) |> print |> Seq.truncate 11 |> Seq.fold (+) 0

#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 37", "https://projecteuler.net/problem=37")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
748317
"
#endif
