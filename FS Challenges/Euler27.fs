module Euler27
open System
open DAP.EulerProblems.Utility

let countPrimes a b = nonNegs |> Seq.map (fun n -> n*n + a*n + b) |> Seq.findIndex (fun n -> not (isPrime n))

let maxA b =
    nonNegs |>
    Seq.map (fun n -> 1 + n - b) |>
    Seq.truncate (1000 + b - 1) |>
    Seq.map (fun a -> (a, countPrimes a b)) |>
    Seq.fold (fun (t1:int*int*int) (t2:int*int) ->
        let (a1:int, ourb:int, b1:int) = t1
        let (a2:int,b2:int) = t2
        if b1 > b2 then
            (a1, b, b1)
        else
            (a2, b, b2)) (-1, -1, -1)

let answer =
    primes |>
    truncateIf (fun p -> p >= 1000) |>
    Seq.map (fun b -> maxA b) |>
    Seq.fold (fun (t1:int*int*int) (t2:int*int*int) ->
        let (a1:int,b1:int,max1:int) = t1
        let (a2:int,b2:int,max2:int) = t2
        if max1 > max2 then
            (a1, b1, max1)
        else
            (a2, b2, max2)) (-1, -1, -1) |>
    fun (t:int*int*int) ->
        let (a, b, max) = t
        a*b

#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 27", "https://projecteuler.net/problem=27")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
-59231
"
#endif

    
