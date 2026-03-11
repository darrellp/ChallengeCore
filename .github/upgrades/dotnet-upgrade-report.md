# .NET 10.0 Upgrade Report

## Project target framework modifications

| Project name                     | Old Target Framework | New Target Framework | Commits              |
|:---------------------------------|:--------------------:|:--------------------:|:---------------------|
| ChallengeCore\ChallengeCore.csproj | netcoreapp3.1        | net10.0-windows      | d643c937, c3ee2ccc   |

## Assembly reference changes

| Project name                       | Change                                                        | Commits    |
|:-----------------------------------|:--------------------------------------------------------------|:-----------|
| ChallengeCore\ChallengeCore.csproj | Removed NumberTheoryBig.dll, NumberTheoryLong.dll, Priority Queue.dll | 5a0c03f6   |
| ChallengeCore\ChallengeCore.csproj | Added NumberTheory.dll (consolidated replacement), Priority Queue.dll | c3ee2ccc   |

## Code changes

### ChallengeCore\ChallengeCore.csproj

- Target framework changed from `netcoreapp3.1` to `net10.0-windows`
- Replaced assembly references for `NumberTheoryBig.dll` and `NumberTheoryLong.dll` with consolidated `NumberTheory.dll` (uses generic numeric parameters)
- Re-added `Priority Queue.dll` assembly reference

### Euler66.cs
- Changed `using NumberTheoryBig` to `using NumberTheory`
- Converted `i.IntegerSqrt()` extension method to static call `NumberTheory.Utilities.IntegerSqrt(i)`
- Fully qualified `NumberTheory.Pells.SolvePells(...)` to avoid ambiguity

### SquareSums.cs
- Changed `using NumberTheoryBig` to `using NumberTheory`

### euler44.cs
- Added `using NumberTheory`
- Converted `disc.IntegerSqrt()` extension method to static call `NumberTheory.Utilities.IntegerSqrt(disc)`

### Euler58.cs
- Added `using NumberTheory` for `Primes.IsPrime` calls

### euler49.cs
- Added `using NumberTheory` for `Primes.IsPrime` calls

### Summation of Four Primes.cs
- Added `using NumberTheory`
- Converted `n.IsPrime()` extension method to static call `Primes.IsPrime(n)`

### SmithNumbers.cs
- Added `using NumberTheory`
- Converted `l.IsPrime()` extension method to static call `Primes.IsPrime(l)`

### Factovisors.cs
- Added `using NumberTheory` for `Factoring.Factor` calls

### Shuttle Search.cs
- Added `using NumberTheory` for `ChineseRemainder.CRT` calls

### Timing Is Everything.cs
- Added `using NumberTheory` for `ChineseRemainder.CRT` calls

### Signals and Noise.cs
- Fixed MoreLinq `MaxBy`/`MinBy` ambiguity with built-in LINQ methods (added in .NET 6+) by fully qualifying calls as `MoreLinq.MoreEnumerable.MaxBy(...)` and `MoreLinq.MoreEnumerable.MinBy(...)`

### Black Box.cs
- Replaced `SimplePriorityQueue<T>` (not available in current Priority Queue.dll) with `BinaryPriorityQueue<T>` using a comparison function constructor

## All commits

| Commit ID | Description                                                                                     |
|:----------|:------------------------------------------------------------------------------------------------|
| c7ab9fa6  | Commit upgrade plan                                                                             |
| c9d9714d  | Removed using NumberTheoryLong in Factovisors.cs                                                |
| 3d80b610  | Removed using NumberTheoryLong in Timing Is Everything.cs                                       |
| d643c937  | Update ChallengeCore.csproj to .NET 10 and refactor packages                                    |
| 934b327d  | Removed using NumberTheoryLong in Euler58.cs                                                    |
| 5a0c03f6  | Remove external assembly references from ChallengeCore.csproj                                   |
| f1e2eceb  | Removed using NumberTheoryLong in SmithNumbers.cs                                               |
| 99d872e2  | Removed using NumberTheoryLong in euler44.cs                                                    |
| c3ee2ccc  | Fixed all build errors: assembly refs, namespace updates, API migration, MoreLinq disambiguation |
| 00a7cf61  | Removed using NumberTheoryLong in euler49.cs                                                    |
| ee9f7ab6  | Removed using NumberTheoryLong in Shuttle Search.cs                                             |
| dbd39f21  | Replaced BinaryPriorityQueue with SimplePriorityQueue in Black Box.cs                           |
| a6ed622a  | Removed using NumberTheoryLong in Summation of Four Primes.cs                                   |

## Next steps

- Upgrade the external `Priority Queue.dll` to .NET 10 (as noted by the user — to be handled separately)
- Consider replacing the `morelinq` package with built-in LINQ methods where possible, since .NET 6+ added `MaxBy`, `MinBy`, `DistinctBy`, and other methods that overlap with MoreLinq
