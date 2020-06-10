module Euler17
open System
open DAP.EulerProblems.Utility

let answer =
    let specialCases n =
        match n with
        | 10 -> 3
        | 11 -> 6
        | 12 -> 6
        | 13 -> 8
        | 14 -> 8
        | 15 -> 7
        | 16 -> 7
        | 17 -> 9
        | 18 -> 8
        | 19 -> 8
        | _ -> -1
        
    let tenMults n =
        match ((n / 10) % 10) with
        | 2 -> 6
        | 3 -> 6
        | 4 -> 5
        | 5 -> 5
        | 6 -> 5
        | 7 -> 7
        | 8 -> 6
        | 9 -> 6
        | _ -> printfn "%s" "Oopsy Daisy!"; 0
        
    let unitDigitCount n =
        match (n % 10) with
        | 0 -> 0
        | 1 -> 3
        | 2 -> 3
        | 3 -> 5
        | 4 -> 4
        | 5 -> 4
        | 6 -> 3
        | 7 -> 5
        | 8 -> 5
        | 9 -> 4
        | _ -> printfn "%s" "Whoops!"; 0
        
    let hundredDigitCount n =
        if n < 100 then
            0
        elif (n % 100) = 0 then
            (unitDigitCount (n / 100)) + 7
        else
            (unitDigitCount (n / 100)) + 10
        
    let underHundredCount n =
        let np = n % 100
        let countSpecial = specialCases np
        if countSpecial > 0 then
            countSpecial
        elif np < 10 then
            unitDigitCount np
        else
            (unitDigitCount np) + (tenMults np)
            
    let countLetters n =
        (underHundredCount n) + (hundredDigitCount n)
            
    11 + (seq { 1 .. 999 } |> Seq.map (fun n -> countLetters n) |> Seq.fold (+) 0)

#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 17", "https://projecteuler.net/problem=17")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
21124
"
#endif
