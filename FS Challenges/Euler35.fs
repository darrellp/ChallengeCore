module Euler35
open System
open DAP.EulerProblems.Utility

let allRotations (n:int) =
    if n < 10 then [ n ]
    else
        let s = n.ToString()
        let cch = s.Length
        let rots = [ for i in 0 .. (cch - 1) -> Int32.Parse (rotate s i cch) ]
        if List.head rots = List.head (List.tail rots) then
            [ List.head rots ]
        else
            rots
    
let isIconic (arr:list<int>) =
    let n = Seq.head arr
    List.forall (fun nTest -> (isPrime nTest) && nTest >= n) arr
    
let countPrimes (n:int) =
    let rec checkPrimes (arr:list<int>) =
        if arr.Length = 0 then true
        else (isPrime (List.head arr)) && (checkPrimes (List.tail arr))
        
    let impossible (n:int) =
        if n = 2 || n = 5 then false
        else
            let s = n.ToString()
            s.Contains "0" ||
            s.Contains "2" ||
            s.Contains "4" ||
            s.Contains "6" ||
            s.Contains "8" ||
            s.Contains "5"
    
    if impossible n then
        0
    else
        let rotates = allRotations n
        if isIconic rotates then
            rotates.Length
        else
            0

let answer = primes |> truncateIf (fun p -> p > 1000000) |> Seq.map (fun n -> countPrimes n) |> Seq.fold (+) 0

#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 35", "https://projecteuler.net/problem=35")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
55
"
#endif
