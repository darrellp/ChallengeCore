module Euler14
open System
open DAP.EulerProblems.Utility

open System.Collections.Generic

let memoArray = [| for i in 0 .. 2000000 -> 0 |]
memoArray.[1] <- 1

let rec CollatzLength (n:Int64) =
    let memoize (n:Int64) iCol =
        if n < 2000000L then
            memoArray.[int32 n] <- iCol
            
    let memoized n =
        if n < 2000000L then
            memoArray.[int32 n] <> 0
        else
            false
            
    let CollatzNext (n:Int64) = 
        match n with
        | _ when n % 2L = 0L -> n / 2L
        | _ -> 3L * n + 1L
        
    let rec CollatzRecurse (n:Int64) (lst:List<Int64>) =
        if memoized n then
            memoArray.[int32 n]
        else
            lst.Add(n)
            CollatzRecurse (CollatzNext n) lst
        
    let ProcessList n (lst:List<Int64>) =
        let count = ref (n + (Seq.length lst))
        let nValue = !count
        Seq.iter (fun n ->
            memoize n !count
            count := !count - 1) lst
        nValue
                       
    if memoized n then memoArray.[int32 n]
    else
        let lstCollatz = List<Int64>(List.Empty)
        ProcessList (CollatzRecurse n lstCollatz) lstCollatz
    
let answer =
    nonNegs |>
    Seq.truncate 1000000 |>
    skip 500000 |>
    Seq.map (fun n -> (n, CollatzLength (int64 n))) |>
    Seq.fold (fun (n1, l1) (n2, l2) -> 
        if l1 > l2 then (n1, l1)
        else (n2, l2)
        ) (0, 0) |>
        fst

#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 14", "https://projecteuler.net/problem=14")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
837799
"
#endif

