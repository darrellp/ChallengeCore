module Euler4
open System
open DAP.EulerProblems.Utility

let makepal (n:int) =
    let s = n.ToString()
    Int32.Parse(s + reverse(s))

let findProperFactor n small high =
    Seq.exists
        (fun f1 ->
            let f2 = n / f1
            f1 >= small && f1 <= high && f2 >= small && f2 <= high)
        (allFactors n)
        
let properlyComposite n =
    findProperFactor n 100 1000

let palindromiate n =
    let s = n.ToString()
    Int32.Parse(s + (reverse s))
    
let answer =
    (Seq.map palindromiate [997 .. -1 .. 1]) |>
    Seq.find properlyComposite

let answerAlternate =
    let isPalindrome (n : int) =
        let array = n.ToString().ToCharArray()
        let maxIndex = (array.GetUpperBound(0) / 2) + 1
        let rec compareIndices index =
            if index >= maxIndex then
                true
            else
                if array.[index] = array.[array.GetUpperBound(0) - index] then
                    compareIndices (index + 1)
                else
                    false
        compareIndices 0

    let palindromes =
        [ for a in 999 .. -1 .. 100 do
            for b in 999 .. -1 .. 100 do
                if isPalindrome (a * b) then yield (a * b)]

    palindromes |> List.fold (fun x y -> Math.Max(x, y)) 0
    
#if CHALLENGE_RUNNER
open FS_Challenges
    
[<Challenge("Euler Project", "Prob 4", "https://projecteuler.net/problem=4")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)
    
        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
906609
"
#endif
    