module Euler36
open System
open DAP.EulerProblems.Utility

let palInTwo n =
    let rec palInTwoRecurse n powerTwo =
        if n = 0 || (n = 1 && powerTwo = 1) then
            true
        else
            match (n &&& 1) with
            | 0 -> (n < powerTwo) && (palInTwoRecurse (n / 2) (powerTwo >>> 2))
            | 1 -> (n > powerTwo) && (palInTwoRecurse ((n ^^^ powerTwo) / 2) (powerTwo >>> 2))
            | _ -> true
            
    let rec getPower n powCur =
        if powCur > n then
            (powCur >>> 1)
        else
            getPower n (powCur <<< 1)

    let powerOrig = getPower n 1            
    palInTwoRecurse ((n ^^^ powerOrig) / 2) (powerOrig >>> 2)

let palindromiateOdd digMiddle n =
    let s = n.ToString()
    let sMiddle = digMiddle
    Int32.Parse(s + sMiddle + (reverse s))
    
let palindromiate n =
    let s = n.ToString()
    Int32.Parse(s + (reverse s))
    
let oddsUsingMiddleDig n =
    let sDig = n.ToString()
    [1 .. 99] |> Seq.map (palindromiateOdd sDig)
    
let allOdds =
    [0 .. 9] |> Seq.map oddsUsingMiddleDig |> Seq.concat |> Seq.filter isOdd
    
let allEvens = [1 .. 999] |> Seq.map palindromiate |> Seq.filter isOdd

let allOddPalindromes = Seq.append [1..2..9] (Seq.append allOdds allEvens)
    
let answer = allOddPalindromes |> Seq.filter palInTwo |> print |> Seq.fold (+) 0

#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 36", "https://projecteuler.net/problem=36")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
210
"
#endif
