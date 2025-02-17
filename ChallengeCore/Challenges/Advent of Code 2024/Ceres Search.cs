﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Console;
// ReSharper disable StringLiteralTypo
// ReSharper disable IdentifierTypo

// ReSharper disable once CheckNamespace
namespace ChallengeCore.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("Advent of Code 2024", "Ceres Search (4)", "https://adventofcode.com/2024/day/4")]
        // ReSharper disable once UnusedMember.Global
        public class CeresSearch : IChallenge
        {
            private static Regex _regexXmas;
            private static Regex _regexRxmas;

            public void Solve()
            {
                // Get data
                var lines = GetLines();
                var width = lines[0].Length;
                var height = lines.Count;

                // Part 1
                _regexXmas = new Regex("XMAS");
                _regexRxmas = new Regex("SAMX");

                var sum = lines.Sum(XmasCount);

                // Horizontal search

                // Vertical search
                for (var iCol = 0; iCol < width; iCol++)
                {
                    var nextString = new string(Enumerable.Range(0, height).Select(i => lines[i][iCol]).ToArray());
                    sum += XmasCount(nextString);
                }

                // Diagonal search on diagonals that intersect the left column
                // TODO: diagonals that don't start in column 0
                for (var iRowDown = 0; iRowDown < height - 3; iRowDown++)
                {
                    var iRowUp = height - iRowDown - 1;
                    var maxCol = Math.Min(width, iRowUp + 1);
                    // Leaning down...
                    var nextString = new string(Enumerable.Range(0, maxCol).Select(i => lines[iRowDown + i][i]).ToArray());
                    sum += XmasCount(nextString);
                    // Leaning up...
                    nextString = new string(Enumerable.Range(0, maxCol).Select(i => lines[iRowUp - i][i]).ToArray());
                    sum += XmasCount(nextString);
                }

                // Diagonal search on diagonals that intersect the top or bottom (excluding the first since it was included
                // in the above search)
                for (var iCol = 1; iCol < width - 3; iCol++)
                {
                    // From the top
                    var nextString = new string(Enumerable.Range(0, width - iCol).Select(i => lines[i][iCol + i]).ToArray());
                    sum += XmasCount(nextString);

                    // From the bottom
                    nextString = new string(Enumerable.Range(0, width - iCol)
                        .Select(i => lines[height - i - 1][iCol + i]).ToArray());
                    sum += XmasCount(nextString);
                }

                WriteLine(sum);

                // Part 2
                // Okay - dumping the sophisticated rgexp stuff and just doing it manually.
                // Probably should have done the first part this way (manually) also but I was too enamored with rgexp.

                // ReSharper disable once InconsistentNaming
                const byte sumMS = (byte)'M' + (byte)'S';
                sum = 0;

                for (var iRow = 1; iRow < height - 1; iRow++)
                {
                    for (var iCol = 1; iCol < width - 1; iCol++)
                    {
                        if (lines[iRow][iCol] == 'A')
                        {
                            var ul = lines[iRow - 1][iCol - 1];
                            var ur = lines[iRow - 1][iCol + 1];
                            var ll = lines[iRow + 1][iCol - 1];
                            var lr = lines[iRow + 1][iCol + 1];
                            if ((byte)ul + (byte)lr == sumMS && (byte)ur + (byte)ll == sumMS)
                            {
                                sum++;
                            }
                        }
                    }
                }
                WriteLine(sum);
            }

            // Probably faster, though a little more verbose, to do a manual search rather than regexp but who cares
            // in Advent of Code as long as it's not noticeable?
            private static int XmasCount(string s) => _regexXmas.Matches(s).Count + _regexRxmas.Matches(s).Count;

            public string RetrieveSampleInput()
            {
                return @"
SAMSXXMAXAMXSMXMMMSMSSSSMASMMAMMSMMMMMMMMSMSMSSXMAMXXXMSXSXSMSSMAMMXMSMMXSASMAXASMSMXMASMMMMSAMXXMAXXMMXAMAMXMSMMMMMMMMMMXSSMASMXXXXXMASXXAX
SAASMMASAXMAXMAMXAMSAMXMXASASMXAAAAAMSSMMAAAAXAAMASXSAMXMMAMXMAMMXSASAAAAMMAMMMMXAAMMSAMXSAMMASMSAMMAMXXXSXMAXAAASXAAASAMXMASASMSSSMMAMXMASX
SMMSXSAXMSMSXSASMSMMMSMSMMSAMMMSSSMSMAAASMSMSMSMMAAMSXMAAMAMASMXSASXSSMMXSAMXSAAMXMMAMASAMXSXXMAMXMASMMSMMAMMXXXMXSSSMSASXSMMMMXAXAMAASAMXMX
XMASAMASXXAAAMASAAXAXAAAAXMMAXXMAMAAMSSMMXAAAAMXMXXAMXSXMSXSAMXXMASAMMMSAMXXASASMSXSSXXMASASXMSXMAMAAAASAXAASXSMSAMXXASAMASXMASMMSMMSXSMMASM
SASMAMSMMMMMSMAMXMMMSMMMSXMSSMMMAMMMXAMMMSMSMMMAMMMSMMMSMXAMMSMAMMMMMAAMAMSMAXAMAAMXASAXXMMSAMAMXAMSSMMSSMSMMAAMMASAMXMMXXMAMAMAXXAAMMXAXAXM
ASMSMMMAAMMAMMAMMSSMAXAMXXXAAAMMASAXMASMASAMAXSAMSXMAAAAMMSMAAAMXMASMMSXAMXXMMMMMMSMAAMSXSMSXMAXSAMXXMXMMXXAMMMMSSMXSXAMSMSSMSSMMSMMSSSXMSSS
MAXAMAMMMSMAMMASAAASASMSXSMSMMMSAXMXSMMMXMASAMSMAMMSSMSMSAXMSSXMAXSXXSMXXSXAMXMXMAMMXMAAAMMMAMAMSAMSMMAXMAXAXAAAXAXSMXSMAAAMMAAAAXXAXMXAXAAA
XMSMSAXMXXMSXSASMSXMXSAMAXXAASAMAXSAMASMSMXMAMXMMMAMAXMAMXSAMAMSSMMMXAAMXMMAXAMMMSSMMXSMXMAXMMSXSAMAXXMSMMSMSSSSMMSMAAMSSMMXMSSMXMMMXMSSMMSM
XMAMAAMMMMXXXMAMAMAASMAMXMASXMASAXMASAAAASMSMMXMASXSXMMAMMAMXAXXMASAMMMMMMXMXAXMAMAAMAXXASMSXMXXXXMXSMXMXAAAMMAAAXAMMMMAXXXXMAXMASASAAAMAXAA
SSSMSMMAAXASMMSMXMMMXSAMAXXMAMAMMSSMMMSAMXXAASXMAMXXMAMAXSMSSMSAXMAXXMXAXSASXSSMMMSMMMMXXXXAMMMMSMSMSAAXMSMSMMSSMSXSSSMMSMSMMASMMSASASMSAMXA
AAAMAASXMMMSAAXMAAXXAXMSMSASMSSXMAMXMXMXXMMMMMAMAMAMXSSXXAAXXASXMXAMSMMMXMAXAAAXMAMASMSMSXAMAXAAAAAASXMMXAXAXXAXXXMXAAAASAXAMAMAXMAMXMMMAMXM
MMMMMXMAAXASMMMXXMMMMMMAMXAAAAXAMXSXSMMAXAAXXSASAMSAXXAMSMMMMMMMSSXMAAASMMSMMSMMMMSAMAAAAMSXMSMSSSMMMSSXXXSMSMMSMMMSMSMMSSSXMXSAMMMMXMXSAMAX
XMXAXMASMMASMMMMMASMSASXSMXMXMSXMXMXAAXXMMXSMMASAXMAMMAMMAAMXAAAAAASMSMXAAXAXMASAXMXMSMSMAMAMSMAMAXXAMXSAMAMAAXAAAAAXXXXXASXMASAXMSMMMAMAXXX
AASMSMMAMXAMAMAASXMAMMMAXAXMXXMASAMSSMMMAXMMMMXMXMASAMXMMSMSXSSMMSMMAMXSMMSSMSAMXMAMMAAMXXXAMXMASAMMXMAXMSSSSSSSSMSSSMMSMXMXMASXMMAAMMSXMMSA
SMMAAXSXAMSSMMSXSAMXMSXMMSMAAMSAMXXAAAASXSMAAXAASXAXASMSAMXSXAXAXXMASXMASMAMMMXSSMMASXMMASMMSXSASMXSAMMSXMAAAAXXMAMAAXAASAMXMASAMSSSMAMAMAXA
XXMMMMMASAXAXXMXXXMMAMAXAXAXMXMSSXMSXSMSAXMAMXASMMMSAMAMXMASMMXSMXXSXMXSXMASAMMAMAXMXAAMXMAAMAMMSXAMAMASAMMMMMMSMXSSMMSXSXSAMXSXMAAAMASXMSSS
SASXSXSAMXSXMASXSSMSASXMMSSMXAXMMMXMAMAMXMXMSXMAXXXMAMXSAMMMAXSXMXSMSAXXASAXAMMASMMXMSMMMMMMMMMASMMSSMASAMMSAAAXAAXAMXMASMSASMMAMMSMMMSAAMAM
SAMASXMASASMMASMAAAXXMAAXMAASXSAXMAMAMXAAMAAMAMXAMMSXMXAXMAMSMAASXAAAXXSAMMSSXSMSAAXAXAMMASXSSMASAAAAMMXXMAXMXXMMMXAMSAMXAMXMXMASAAAAASMMMAM
MAMAMAXAMASAXAMMSMMMSSSMMMMMXAMMAMASAMXSXSASXSMSASAAASAMXSASMXSAMSMMMSAMAMAAAMXAMXMMMMAMSASAAAMASMMMMSASMSSSMSMSAMXMMMASMXMAXSMASMSMMXXMAMAM
XSMAMXMSMMSAMXSAMAMSAAASXSSMSMXSXSASASAXAAAXAAASMMMSXSAMASXSXAXMASXAXMASMMMSSSSXMASXXXXMMASMSSMMMMSXAMASAAXAAAASMMAAMSAMAMSXMXMAXXAMSSSSSSSS
XXSSSMMAAMXMAXMMMAMMMSMMXXAAXXXXMMASAMXSMMAMSMMMXSAMAXAMAXAMMSSMMXMXXSAMXAAAAAMASAXXMASXSXXMAXMMAAXAMXXMMMSMMMXMASMXMMASMMXAXMMSSMMSAAMAXAXX
XMAXXAXSMMASXSXSXSXXMAMMXSMMMSMSAMAMXMAMXSXMXMAXXMASMXSMSSXSAAAMXMASXMXSXMMMSASMMMMXMASAMXMMMSMSMMXSXMAXXAAXXMSMMMAASXXMXXXMMXAMAXXMMXMMMMMM
AMXMSSMMXSMMMAAXAXXXSAMXAXAAAAAXMMXSASXAAXAMSMSMSMMMXAMAXAAAMMSMSMAXASAMXSXXMAMXAAAXMAMAMAMXAAXXASXXASXMMMSMMMSAAMMMSASMXSASAMXSMMSMMSMXSXAA
MMXMAAXAASAXMMMMXMAXMAMMXMSMSXXXMSXMAXXMXSAMAAAAXAAAXXMAMMSMMSMXMMMSXMAXAAAXMSMXMXSXMASMSASMSSXSAMMXXMSAAAXASASXMXSASAMXAMAMXSAMMAAAMAMAMMMX
MSMMSSMMMMAMXAAXMSMXSAMXMMMMMMMSMAAMMMSMASAXMSMMSXMSSMSXSAXXMAMMXAXXXMXMSSMSAMASXMMMSXSASASAXAMMSMXMSAXSMXSMMXSAMAMASMSSSMSMAMASMSSSMAMASXSS
XAAMAMXXSXAASXMSAAAXSAMXSAAAAASASXXMAAXMAXMMAAAXSMXXAAAAMXSMSSSMSSSMSSSMXAXMAMAMAAAASAMXMAMMMXMMXSAAMAMASAMXSXSAMXMMMMMAAAXMXMSMAMXXXXSAAXAA
SSXMAMMASXMXSAAAMMSMXAMASMSMSSSMMSASMSSMSSSSSXSASAMXMXMMMAAXAXAAXMAMXAAMSMMMXMAMXMMMMXXAMXMASMMXAXMMMMSAMSXMSASXMMXMAAMSMMMXMMMMXMASMXSMMMSM
AAXXSSMMSASMXMSMXXAXXXMXMAMXMAXXXSXSAMXAMAAAMXMMXASXMAXASMSMSSMMMSXMMSMMAXAXMMASAMXMSMSMSASMMASMMSMXSAMXSMSAMAMMXAAMSXXMXSSSMASXAMXMMASAXAMX
SSSXAAMASAMMAXMAASMSAXSMMAMAMMMMXXMMMMMAMMMMMXAMMAMASMMASAAAAXXAMXMMMMASXMMSASASAMSXAAAASXXAXSASAAMAMAXXAAXMAXMMSSSMXMAMAAAAAAMXXSASMASMMXSA
AAXMSMMMMMMSASMMMMASMMSASMSMXMASXMXAAAMAMAAAASASXSSMASMXMMMMMSSMSAAAASMMSAASMMAMAXXSMSMXMMSMMMMXMAXXSSMSMSMXSAAXAAXAXSXMSMSMMMSXXMASMASAMASA
MASAXAMXMMAMXXAMXMMMXASMMAMAAXXSAAMSSSXMSSSSMSASAMXMAXMXSMMXXXAASXSSXSXAMMMMAMXSMMMXMXXAXMASASXMSSXXAMASXXMAXSMMMSMMMSMAXAAMXAXAAMAMMXMAMXXA
XAAXMAMXSSSSMMMMMXMXMMSMSAMSMSAMMSMMAMMMAMXAAMXMXMSMMSSMAAASMMMMMAXXXXMMXSASAMXAAAXAMXMMMSASXXAXAASMAMAMMSAMXAXXXMXMASMAMMMMMXSMSMXMSASMMSSS
MMXXSAMXXAAXAASAMMXAAXMASXXXAAMSXAMSAMXMASMMMMSMXXSAAAASXMMAAAAXMSMSMMMAXSAXAMXMXMSMSSSMAMASMSMMMSMSMMASAMAXSXMASMAMSSMASAMXSMMXAXMXSMSXAAAX
AAAXAMXMMMMMSMXASASMSMMAMMMMSMXXAMXSAMXSAMMAAAAMSMSMMXAMXMAXSMSMMAAAMAMXXMSMSSSMSAAMAAAMMMXMAAMXSXMSMSXSXXSMMMMAAMXMAMMAMASASASMMMMAMXSMMMMM
MMXSASAMAXAAXXSSMMMMAMMXSXXAAXMMXMASAMAMAXSXMSMSAAXMXMSXMASAXMAXSMMMXSSXSAMXMAAAMSMMXSMMXSAMSMXXAXAXAXASMMMAXAMSSSXMXSMSMMMASAMAXASMMMSASXSM
AMAMMSMSSMMMSAMAAXXSAMSMXAMMMMAAMSXMAMXSXMMMAXMMMSMSXXMASAMXAXMAXMASAAMXMXMAMMMMMMXMXXXAASMMMMSAMMMMMMMMAAXXMMXAAMXMXAMMASMMMMMSSMXAXASXMAAS
SMXSAMXAAXAAAAMSMMASASAASMSSMMSSMAASMSXMMMAAMSAMXXASMASAMXXMXASXXSAMAMXAXASXXAXAXMASMSMMXSXMAASMXAXAAAASXMSMSMMMMMAMXMASAMXAAXMXAXSMMMSAMSMS
XAMMMMMSMMMSMSMMAXMSMMAMMAXMAXAAMSMMASXSXSASXMMMMAAXSMMASXMSMXMAAMASMXSMSAXMASXMSXMAAAASAMASXMXMASMXSSXXAAAXXXXAASAMAXAMAMXMASMSMMAAAASMMAAM
MSMSAXMMMSMXAMASMMXMXSMMMXMXSMMXMAAMAMAXAXSXMAAMXMSMXMSXMXAAXAMMMMAMMAAAMMMXAXXMAXAMMMXMSSMMAXMASXAXMXMSMSMSAMMSMSASMSMSXASXXSAAAMSSMXMAMMSM
MMAMASMAAAAMAMAMSAAXMXAAXXMAMASXSSSMSSMMMMMXXXMSXMMAAXMMMMSMSMSSXMASMMMMSMSMMMAMASXXSMSAMXAMXMAXAMMMMAMAAAXAAMXMAMXMXAXXMMMMXMXMMMAXMASMXMAM
MMSMAXXAMMMMXMAXMSXXASXMSSMXSAMXAAXAXSXAXASMXSXMASXSMXAAAAAAAXAMXMAXAAXXSAAASMSMAMAMAMMMMSXSSSXSMXSAAAMMSMSSXMAMAMSMSXMMAAASXSASMMSSSXSXASAS
MAAMXMMMSSSMASMXMASXMMASAAMAMAMMMMMMMXSSMMXAMSASAMMXMASXSSMSMMMSAMMSSMMAMXMSMAMMXMSAAMMSAMXAASAAXAMMSMSAMXXMXMASMMAAAMAASMSAAMAMSAXAMASMMSAS
MSSSXMASAAASASAXMASMAMSMXSMAXAMXXXAAXMXMASMSMSAMXMXMXXMAAXAMXMXMXSAAXXMASMXXMAMXMMMSXSAMASMMSMMMMMXAAAMXMXAXAMMMMSMSMSMMMXMMMMMMMMMAMAMXXMMM
AAXXXMMMMXMMMMXMMSMXSMAASXMSSMASMSXXSAXSMMAMAMMMSSSSMSMXMXXMAAASAMMSMXMSAMSAMXSAAAAXMMMSAAAXAXXXASMMMSMMSMMSXSAAAAXAXAMXMXMMXASAAASAMXSMMSAS
MMSMMXSAMASAXMSXMXAAXXMXAXXMAXMXAMXSAMXSXMAMMMMAMAAAAMAAXAMSMSMMSXXMAXSAMXXAMXMMSSSSMXAMXSSMMAXMASMXMXXMAAMSASMSSSSXSMSAAMXAXAXMSASAMXAAAXAM
XAAAMASASASASAMAXMMMSXXMXMSSMXMMSMMMAXXXASAMXAMASMXMMMSMSAAAAXXAMMSMSXMAMMSAMXSAXMAMMAMSAXMXXMMSAMXASMSSXSMMAMAMAXMMMMSMSSSSSXSAMAMXMSMMMMXM
MSMXMASAMXSXSASXMXSXSXXASXAAXXXAMAASMMMSAMXMSXSASMXSAMXXXXSMSMMSMASXXASXMAXASAMXSMAMXMSAMXAXMXAXAXSMXAXMAXAMSMSMSXXAAASXXAAASAAXMMSMMMXAXSAS
MASAMXSMSMMASMMAMASXMAXSAMSSMMMSXSMSAAAMAMAAAAMAMXAMXMAMSXXAMXAAMMMAXMMAMSSXMASXSMXSAMAMSMSMMMSXMMSMMSMSMSAMXAXAXAMMMXMMMMSMMXMXMAAXAAXSXSAS
SAXMSMMAAAMMMMSAMAMAASXMMXAXAAAMAMMSXMXSASXMMMMMMSMMMSAAXXAMXMSSMXMSMSAXMAMXXAMXMAMMASAMXAMXMAAASASMMXAMASMMMSMAMASMMSMSAXAMSSXMMSSSMSSMAMAM
MXSXMAXSSSMAAXMAMSXXMMAAXMMSSMXSAMASMMXSASXSMSASXAXAASMSMSSSSXAMXMAMASMXSASMMSSMMAMSXSXSXMMAMMSSMASMAMMMXMMAMAMASXMAAAASXSAMAMAMXMXMXMAMMMSM
XSAXMXMAAXXMSSSXMAASMSXMMAAMAMAMAMAMAXAXMSMAAXAXAMMMXSAAASAAAMAXSMAXAMSXSAXXMXAASXMMAXASXASXSMMAMSMMASXMMSXMSXSASMSMMMMMMMAMASAMAMAMASAMXAXA
MXAAXAMMSMMSAAMAMXMXAASXMMXSAMASAMMSMMMSASAMSMSMXXAMAMXMMMMMMXSASXAMMSXXMMMMXSSMMSAMXMMMSXMXSXMAMAASASASMMMXMAMASAXAXXSMMSMMXSXXXXXSASAXMSSM
AMXMXXSAMAMMMAMXMSSMMMMMASMSXSASASAMXAXMAMSAXAXAMXXMAXMSXAXASAXASMSSXAXAMXAXAMXMAMXAAAXXMMXXMMSMSSXMASMXAXXAMXMAMMMMMMMMAAAXMMMXSAMAMSAMXAAM
SASXSSMMXSXAXAXAMSAMMASMSMAMAMAMAMAXMMMMMMMMMMMSSMSSMSAMSMMXAAMAMAASAMSMAXSMMMAMMSSSXXXMSAXXSAAAAMAMXMMSMMSMSMMASXMSAXAMSSSMAAAAMXMAMMMAMSSM
XXMXMXASAMSSSXSMXSAMXAXMXMAMSMSMSMSXMMXAXAXAAMXMAMXAAMAMAMSAMXMMMMMMMXAXSMXAASXSAAMXMMSMMMXMASMSMMXSXMAAXAAXAAMASAASMSXXAAMXSMMXMAXXSXXSMAXA
SMMSSSXMAXAXSXAXASXMMSSSXMSAMAAAAAXAXSSSSMSSMSASAMMMMMAMAMXXXXXXXMAMMXMAMASXMMAMMXMAAAAAAAASAMXXXMMMASXMMSSSSSMMSMMSMXMSMAMAXAXASMXSAMXAMMMA
MAAAAXAMSMMMXXMAXSAMAMMMAXAAMSMSMSMMMMAAAMAAMSASASMSXSSXMXSXSAMASMSSXAXAMMMAMMSMMSAMSSSXMSMSASXXAAAMAMASAAAAAXAXXMAXXXXXAMMMSMMXAAXMAXXXSSSX
SMMSMSSMMAAMXAXMASXMASAMMMSSMXXXXAMAAMSMSMMSMMMSXMAAAMAASMMAAMSAMAAAXMMXXXMXMMMAAMAMMXXMMXMSAMASXSXSASAMMSMXMSMMAMXMMMSMSMAMAXSMMSMSSXMSAAMS
XXAXXMMAXMASXMSSXMMSXSAAMXAXMMSXSSSMSXXAXMAXAXXSAMXMXMXMMAMMMXMAMMXMASMMMSXXSASMMSSMXMMXSAMMAMAXAMXAXMASMAXSXSXSXMAAAMAAAMXMSAXAAXAMAASAMXMX
XMAXASMSMSMXMAAMMAXSXSXMAMMMXAMXMXAAXMMMMSASAMXXAMMSASAXXAMXXMASMXMXAAXSASMMSAMSAAMAXAMASXMSXMASMSASASAMMMXSAMMSASXSSSMSMSMSAMSMMMSSMMMMXAAM
AMXSMMAMAASAMMMSSSMMAMXMAAMSMXSASMMMMAXSAMXSMXXMAMXXASMMSSSXMASASAAMSSXSSMXAMXMMMXSMSSMASMXXMXMAMAAXMMASMMAMSMAMAMXMAXAAMAXAMMMXXAAMAXXXXMXX
XXXAXMAMSMSMSMSAAAAMMMASMXAAAXSMSAMXSXMMMSMMMXMMMSSMMMXAXMAXAMXASXSAMXMMASMSSXXXXXAMAAMXMASMSAXMASMMXMXMAMXMXSSMSMMMAMXMSXSMXMXSMMXSSMMMAMMS
AXMMMMSXXXMMAMMMSSSMASXSAASMSXMASAMXXXMMASAAMAMSAAXAAAMMSXAXMSMAMAXXXMASAMXAAXMASMMMSSMMMMMASMSXMAMMXMMSXMXMMAMAMXXMMSMMMAMXSXAXAXMAXAASASAA
SMSXMXMASXXMASXXMMXMASAMXMMAMMXAMAMSMAAMXSMMXAXXMMSSMSAASMMSXSMMMMMXMMAMMSMMXSAMXAXMAXMASAMXMAMMMAMSAMAMAMAXMAMMMSMXAXAASMSAMMMMSMXSMMMSASXM
MAAASAMSMMASXSMXXMAMAMXSXMMXMAXXSXXAAAXSASXSSMSXXXXXMMMXSAMAMXAXXMSAMMXXAAAMASAMSMMMAXMASMMAMXMASAXSAXSXXMAMXSMAAAAMMXSMSAMMSAMXAMXMSMMMMMAX
MAMMMMMMAXMSASXSMMSMSMMMASXMASXMAMXASAXMASAXAAAXXMXMXXAAMAMAAXSMMASASXMMSMSMMSXMASXMXSXXMASMMMSASMXMMMMAXMSSMAXMXXSASAXXMXMASAMMMMAMASXAMSSM
MMMSAXASXMXMAMMSXAASAAASAMAAXAXAAMSXMASMXMASMMMMMMAMAXMSMMMXSAAXAMMMMMAAMAAMXSXSMASMAMAMAMAAXXXMXMMMMAXSMMAAMMXSMMXAXASXAXMASXMASMXSASXSMAAM
MAASMMMMAMSMSMAXMMMSSSMMXSMSMMMSMMAMXXAMAMXMAAXAASASXSMMASXAMMMMSXAAASMMSSXSAMMMMAXMASAMAAMMMMAMMAAAMMXMAXXMXXAMAMMAMSAMXMMXSXMASAXMAMAXMXSM
SMXXMAXXMSAAAMXMXSAMXXASASXXAAMAMAMAMMMSAMAMSMSSXSMXAAASAMMXMASXMSSMMSXXXXXMASAAMASMMSMMXSXMASAMXSSSSSMSAMXXMMMSAMMSMAAMAAAAXMXAXMSMSMMMMAMA
XMSMSASAMXMSMXSAMMAMXSXMAMASXMMMXAXMXAXMMMMXMXXMASAMSMMMSXMXSMSAAMMMMMMXMMXMASXXSAMXXXMMMMMSAMAXXAMXAAXMAMSMMMASASAXSMMSSSMMSAMMAAAAXAMAMMSM
SXAAXMAXXAMXAAMAMSMMXXXMAMMMASAMSSSMMSMXSAMAXMXMAMMMXAXAXMSMMXSMMMAAAMAAMAMMXSXMMMSMMAXXAAXMAXSMSASXMMMSSMMAAMMSAMXMAXMAMXAAAMASMSMSMMSMXXAM
XSMSMSSSMMAMMMSMMXAXMSMSAXAXXMAMAAAXAMXASASASXXMMSXMSSMMXXAXXAMAMSSSMMSMMSASAMXXXASAMXMSMSSSMSMAMXSAXXAAMASXMSMMAMSAMXMASMMMMSMXAXXAMXAMXMAS
MMSMAMAAAXSXAXAXAXXMMAMMSXSSSSXMMSMMMSMMSAMXMAAXMAXAAXAAMSMSMSSSMAXMAXAMXMMSAMASAASMMSAMAMAXMAMMMMSAMMSMSAMAAXXXAMXXAXMMMMXXXSAMXMSSMSASXSMM
XSAMAMSSMMMMSSSSSMMASASAXMAMXAAXXMAAXSAMMMMSMSSMSAXMXSMSMAAXXAAMMMMSMSASAMXSAMASMAMAAMAMXMAMSASXAASAMAMXMXXMMMXSASXMAXXAAMMSXSAMXMXMXMXMAAXS
MSMMXMMXMASAXAAAAAAMSASASXSSSMMMSSMMXMASXAAXMAAXMMSXAMXAMMSSMMXMXMASXSASMSXSMMXSXMMMMSSMAMSMSMSMMMSAMXSSMSMSAAAAXMMMASMSMSAAAXMMXSAMSSXMXMMS
ASXSSSMMSASMMMMSSMMMXMMAMAAAASAXMAMXMXAMMMMSMSSMAMXAMXMSSXAAAAMSMMASXMASXSMMMSMMASAAXAMMXMAMXAMXMASAMAMXAAXSAMMMAAXMASXMASMMSMSAXMASAXXMMMAX
MMMMAAAMMMSMAXAMXMXSXMXXMMMMMSXSSXSMXSASXSXSAAXMAAXMXAAMAMSSMMMAAMMSXMXMAXAAAAAXAMAMMMSMASASMSMSMXSSMXSMSMMMMSMXMAXMMMAMXMAXXAMASXXMMMXXAMSX
SASMSMMMAXXXMSMMMMAXMMMMMMAMXSXMMMMAMSXMASAMMMSSSSSMASXXAMMXASMSSMAMXSMMSMSMSSSMSSXMASAMAMAXAMAMXXMXXXAAMXSMAAXXSMSMSMXMASAMMSMMMMAMXAXSMSXS
SASXXMXSXMXMMAMAAMMXSAAAAMMSASASAAMSMMAMXMMMMXXAAAXMAMASMMXMASAAXMAXXSXMAAAAAAXAXAASXSMMMSSMMMAMSASAMSMMMASMSSSXSAAAAMMSMSASXAXAXMXMMSMAXAMX
MAMAXSASMSAXMAMXXAXAMMMSXSAMASASMSSMAMSSXSMAMMMMMMMMMMMAMXSMAMMMMSMSMXASMMMMMSAMXXMMXXXXMAXAASAMXMMAXXAAMAMXMXMAMSMSMSAAAXAMMMSMSAMXMAAMMSSS
XSAMXMASAMASMXSMSSMMMSAAXMXMAMAMXMAMAMXAAMSAMAXXAAMAAMXMXMMMASASMSAAASMMSMXMXMAMSAXXXMMSMXSSMSAXXXXXMXSAMSSMMAMXMMMMMMXMXMSMMAAAAXAMSXSXMMAM
AMAMXMSMMMSMXAAASMAMAMMSMMMMXMAMASXMMSMMMMSMXSASMSSMXSAXAASMMMASAMSMMXMASMXMASAMXAMXAAMAMXAAASAMMXSAAXAXAAAASAXSMXAAAMXSXXMAMSSSMXSAMMMAMMAM
XXAMMXXMAXAAMMMMMASMXSAMASAAAXXSAXAAAAXAMAXMAXMAXMAMASASXSMASMXMXMXMAXMAMSAMMSAMMSSSMSAMXMXMAMASAAXMMMMMMSSMMXXAXSSSMSASMMSAMXAAAXMXSAXAMSAM
MSSSSMXSSSMXMASXSSXMXMASAMMSMMAMXXSMSSSSMMSMMSSXMSSMMMMAMAMAMAMMMMSMMXSMXSXSXMAMAAAAAAXXSAMXXSXMMSMXAAXAXMAMAXMMMMMAAMXMAMAMXXXMXMXMMMSMMSSX
AXAAMAXAMAMXMMMAXMASMMAMXMXAMASXSMMAXAMAAXMAMAMAMXMAXXXMSSMMSAAMAAAAXAMXMXMXMSSMMSSMMMMMMSASMMMSAMXSSXMAXSAMMSAXAXMSMMMMMMSSSSMSSSMSAAAMMMXS
SMMSMSMXMXXMMAMMMMMMMMSSMSSSSMXAMXMXMMMXSMMSMMSXMAMXMXMXMAAXSXSSMXSSMMSAAAAAMMAAMAMXXXAAAXSAAAAXXXXXAAASAMXSAMMSMSXAXSXXMAMAAXAAAAASMSMSAMSA
XSMAAASAMMMMSASXMASXXAXAAXAAMXMSMXSXMASAMAAXMXMMSMSXSAAASMMMMXMAMMAMAASXSASXXMAMMSSMSXSSSSMSSMSSMSAMMSMXASMMAMXAAMAMXMAXMAMMMMMMMMMMXAMMASXX
SMSMSMSXXAAMMASMSASAMXSMSMSMXSMAMXMASAMXSMMSMAMAAAAASASASMMAMXMAXMASMMXMMXMASAAMAXMASAMXMAAAXMXMAAAMAAXMXMASAMSMSMAXAMAXSXMXMAXSXSXSMSSSSMMS
AAAXAMXMMSMSXASAMASXAAAAXAXMAXMASASAMASXSXMAMMSSMMMMMXMMXXXAMXMSXSAMAXXAMAXAMMMMXSMAMAMASAMXSXAXMSMMXSXSXMXMXXSAXMASXXAXMASMSSXMASAAAMAMXAMX
MSMSMSAMXMAMMMMAMAMXSSSMMMMMSSSMSAMMSAMMSASXSMAXXSMSSMMXSSSMXSAMAMAMAMMSMMMMMMAXXMMASAMMMXSSSMMSAAXSAMAAXMSSMSMMMMMSMMSXSAMXAAMMSMSMMMMMSXMS
XMAMASXMAMAMMXSSMASAMMAMMMAMAAMAMXMAMASASAMXXMASAXAMAAXXAAAAMMAMMSSMASXMAMXXAXXXXMSASMSAMXSAMXMAMMSMAMAMXXAAMMAAAAAMMAAAXMSMMMMMAAMXXSXAMMMA
MMAMMSAMMXXMXAMASXMAXSAMXSMSMSMSMSMMSAMAMSAXMSAMXMSSSMMMMSMMMMSMXAAMAMXSMMXMMXMMAXMASMXMASMAXXMMSMXMXMSMSMSSMSSMSMSXMMSSSMMMXAAXMAMAMMMMSAXM
XSXSASMMSSXMMMMAMSSMXMASAMASMMAAAMAXMXSAXXMXXMXMXMAXAAMAXXMASXMAMMMMASXMXASXSAAXXMSXMAXXXXMMMSMMMXMMSXXASAAAXXAAMAXMMXAMXAASMSMSMXMAMSAMXXAX
XAAMASXAASAMASMMSAXMASXMAMAMAMSMSMSSSXMMMMSMSMSMXMASXXMSMMMMSASXMSASAXAAMXMAMXXAAXMASMMMXMAAMXMASMMASXMAMMMSMSMMMAMAMMASXMMSAXAXXMMAXMASMAXM
SMXMXMMMSSXMASAMMMXXASMSAMASXMAXXAAAXXXAMXAAMAAMSXMMAMXAXASMSAMAASAMMMMMMAMSMAMSXMSAMAXSASMMMAXMSAMASXMAMAXAMXXMMMSXMSAMXMMMMMSMMMSMXSAMXAAA
XMAMAXSXMMMSXSMMMXSMXMMSASXSMSSSMMMSMSSXMAMSMMMSMASMMSSXSXMAMXMXMMMMAAXASXSAMXMMAXMASAMMAMXASXXXSXMAMAMASMSSSXSXAAAXXMMXMAAAXAXAXMAMAMXSASMS
XXAMMSMASAAXXMASMAXXAMASAMAMXXAMXXMAAMAMXXXAMXMMXAMAAMMMMXMSMSSSSSMMSMSAXXSMSXMSXMSXMASMAMXMASAMXAMSSSSXSAAAXAASMMSSMAMASXSSMSXXMMAXXSSMAMAA
MSMSMAMAMMXSMXXAMXSSMSAMAMMMMSMMMSMMMMAXAMASMSMMMSSMMSAAXSXAAAXAXAAMAMMMMASXSMXMAMSMXAMMAMMXAMXMSXMAAMMMSMMXMMMMXMAAXAMMXAAXAXSSMSXSAMXMAMSM
AAAAXAAMXSMAXSMMSAMMAMASAMAAAAAAAAAXSSMMXSAMAMAMAMAMAMMXMXXMSMMMMMMXAMAAMXMAMAAMMXMAMASXXSAMSMAXXXMMMMSAMAMSMMAXSMMSMXXXSSMMSMMMAAAMMMXAXMXS
SMSMSXSMASXSMSAXMASMAMASASXMSSSMSMSMAAXSAMXSXSAMXSAMMSMSMSMMAMAXSXMMMMSMMXMXMSSMMAXAMXAXMXMMXMXMXAMXXAMASXMASMMSMAXXMSAAAAXAAAAMMMMMXAXSXMAX
AXAXAXMMMSAMASXMSMMXMSMXXMMAMAAAAMMMXSAMXSAMMSASMSAXAAMAAAASMSSSMAAAAAXMSAMAAXAASMSSSXSXSAXSXAAAXMSAMXMXMASAMXMAMMMSAMMMXXMMSSMSMMSSMMMMAMAM
SAMMMXSAMMMMXMMMMMAAXAAMSSMAMXMASXSAMXAMAMASASXMASAMSSSMSMMXXAMAXXMXMSSMSASMSMSMMAAAXAXAMXSXASMSMMAXXXASMXMASASXMSAMXMXMMXSXXMAAMXXMAAAMAMAX
XAXMAAXASXXMMXSXAMSSSXMXAASMXAXMXMMMMSAMXSAMMSSXXMAMAAXXMXSSMSSSMSSXMXAXSASAMXXXXMMMMMMSMSMMMMMXAMAMXMXXAXSMMMMMAMMXAMAAXMMMAMSMSMXMSMSSMSMS
SXMMMMSAMMSXAAAXMXAAMASAXMMSSSSSXSXMAMAMXMMSAMASXSSMMSMXSAMAAAAXAAAAMSMMMMMASAMXMXSASAAAMAAAMAXSSMAMXSMSAMXAAXAMXMXSAMXMMAMAMXAMAAAMAAXXAAXA
XAXXAAMAMAASMMSAMMMAMMMSAAXXXMAXAMSMXSXXXMAMASMAXSAMXAAAMAXMMMSMMMSSMAMAMAMSMMSSMMSASMSMSSSMMMMAMMSMMAASMXXSXXMSAAXXMSASMSSMXSASXSSSMSMMSMSM
SXMMSXSAMSMAXAXMXAMMMAAXSMMMAAAMXMAXMAXMAMASXMXMMXSXSMSMSSMSAXXAAXAMMASXSSXMAMAMMAMAMXAAXAAXASAMMAMASMMMXSMMXSAMMXMAXSAXAAAMMMXMXAAAAAAAAXAX
SAMAXXMXXASMMSSMSSSXSMMXXXASXMXMXMAXSAMMAMMSAMAASAMXMAXXAMMSAMMXAMXXMXXAAXMXAMASXSMSMMMSMSMMMSASMAMAMXMAMXAAAMXMAXMMMMAMSMMXSMASMMMMXMMSXSXS
SAMAMSSXSASXAMAXAAMAXMMMASMSXMASAMAMMAMSSMAMASXMMAXXMASMMSAXAMSASMSXMMMMMMXSXMMAAXAXAAAAXMXMXMAMMSMSMSMAMSMMSXXMAMMAXMAMXAXAXSASAASAMAXXASMS
SAMMSMAAMAMXMXAMMSMMMSAMXAMXASASASXXMXMAXMAXMMAXSAMSMMMAXMASAMXAMAMXAAXAAXAAXMMXMMXSMMXXMMASAMAMMXAXAMSAMXASXXSSXXSSSSSSXMMSMMSSMMSAMXMSAMAS
SMMMAMMMMMMSMXSMAAAAAXXXXMASXMAMXMAMMMMMSSXSSSMMMMSXAASXMAMXAXMMMSMSSMSSMSSMMXMAXXXMASXSMSASXSMSMMAMXMMMSSMMMAXAAMAAXMASAXAXAMASAXSAMSAMAMAM
XXMMMSXMAMASAAMMXSSMSSMSXSASMSMMSSMMAAAMXMXMXAXXAXSMSMMSAXXSMMMSAMXXXXMAMAMXAASMXMAMMSAAAMASAMXAXMXMSMSMAAXAMMMMSSMAMMXSMMXSXMASMMMAMSSSSMSS
MXSAMXXXMXMMMMSAXAXAXXAAAMAMAAMMAAASMSSMASMMSMMAMXMAXXMASMMAMAXAASMMSAMXMSSSMMXAASXMAMMMMMSMMMSMSSMXMAAMMSSSSXXXAAMXSMMMAMXMMMASXMSSXMASAAXM
AAMXXMMSXSAAXMMMMAXXMMSMSMSMSMSMSMMMXAAXAMMAXAAXSSMSMMXMAXSAMXSMXMXAAAASAMMMAAMSMMAMXSXSXXXMXMAAAXAAMXMXXAAMXMMMSSMASAASAMAMAMAMAMAMAMXMMXMM
SMXXAAXAAMXMMXAMMXMMSAMAXAMMAMXXXMXMMXMMMSMASMSMAAAMAXSXMXSXMAAXMASXXSMMASASMMMMASXMXSAMXMMMASMXMSSMSAASMMSMSSMXAAMAMMMSMSASXMMSMMASAMXXXMAS
XXAMSXMMSMSMXSAXMAAMASMAMMMXMASXMASMXSMMASMMMAAMSMMSXMXAMAXMMMSMMMASXMXSSMASMMSXMASXAMXMAAASASXAMXMAXMSMAAAMXAMMSMMMSSMMMSMSXXMAMSASASMSXMAS
XMMXMAXAAAAAXSXMSXSAMXMXMSXMASAMXAMAAAASASAMXSXMXAASMSSMMAMAAAAMAMXSXMASMMXMAAMAMAMMMMAXXXMAASMAMAMXMSMMMMMMMMMAXSAXAAAAXSMSAXMAMMASAMAMMMMM
XAXASAMMSSMSMMMMMMXMXMASMMASXMXMMSSMMSMMASMMMMAMSMMSAXXXAMASXSMSMSMMMMMMXMAMMMXAMAMASXMSSMXMXMASXAMXMAXXMSASAXMASASMXSMMMSASAMSSMMAMAMSMASAS
MMSAXSAMAMXMAMSAMAASAMXSASAMSMAMAMAXMAXMXMXAAMXMAXXMXMSSSSMMAAAXAAAAXAASXMASXSSMSAXAMAMAXMASAMMXSSSSSMMMXSASXXMXXMXMXMASXMMMAMAMAMXSAMASXSAS
MAMXMXMAXXASAMSASXMSXMASMMMMAXXMASAMSMMSMSSSSSMMXSXAXXXAASAMSMMMSMSMXASAMXMAAXAXSAMSSMMSSSMSXSMAAXAAXMASMMAMAMSSSXAMASAXMSSSSMASXMAXASAMXMXM
MXSXMAAMMSMSAMMAMMAMXMAXXAAMMMXSMMMXXAASXMAAMAASAMMSMMMMMMXMXXMMMAAXMSMMXAMMMSMMSAMXAAXMAAASAMMMMMMMMSMSAMAMMMAAASMSAMXMAXXXASASMSSSXMASMMSM
SAMASAXSMAMMXMMAMMMMSMMXSXXSAXMAMXXMMMMSAXMMSSMMASAAAXAAAMSMMAMAMMMMXMASMXXSAMXASAMSXMMMSMMMAMMMMMXSASASMMAMAMMSMAMMXSAASMMSMMXSAAAMASAMAAAS
MAXMMMMMXAXMAMXASASXSASMSAASAMSAMSASAXASXMXXAAAMAMMSXSXMMXAASXMASAXSASXMASMMMXMXXAMMASAMAXXSXMAAAMMMAXXSMSSSMSAMXXXAAXASAAAAXAAMMMSMXMMSMSMS
SAMSASASMSMSAXSAMASAMAMAMMMMMMMMMSASAXAMXMASXXMMSSXXASXMSSSXXMSASXASAXXAXXSAAMSSXSMMXMASMMXMXSSSSSSMSMXMAXXAXAMXAAMMSSSXMMSSMMXSAXAAAMAMAAXM
MAASAMASAMAMAMMXMXMXMAMXMXAMXAAAXMMMXAAAASMMSAMAXAMMXMASAAMAAMMASXMMSMSSSXSMXMAAAXXSMSAMXSAMMXXXAAXAASXMAMMSMSXSXAXAAXMAMMAMMSASXSMSMXAMSXSA
SSMMAMAMMMXMSMMASAAMMXMSSXMSMMSXXAAAASAMXXAASXMMSMXAAXXMMSMMXMMAMMXMAMAAAAMXSSMMXMAXXAMXAXAXAXMMMMMXMXMASMXAAMAMMMMMSMSAMMASASXSXSAAXSXMAAMA
AXMMAMXXMXSAMAXAMMXMMXMMSAXAAXAXXSMXXAMXMSMMSXSXAXMSMSMMAXMSMAMMSXXSSMMSMAMAMAXAAMMMMMXMMSSMMMXAAXMMXASXAXSMSMSMAMAAAMAMXXMMMSAMAMSMMAMMSSMA
SMMSASXAMAMAMSMSSXSASMAASXMAMSAMXMAMXXMAMAMMMXMXSXMAXAAMASAASAMSXMAXAMXMXMMXSAMSMXAAXXMXMAXXXAMSMSAMMXMMMXXMAAXASMMXSASMSSSMXMMMAMAMSMXAAAXA
AAAAAMXXMASAMAXMAMSAXMMMMMXSMAMMAMAMASXSSXSASXSAMASASMSMSMMMSXSXAXSSMMAMASAAAAXXAASXMAXXMASMXSXAASAMMMMAXMASMSMMMMMAMXAAAAAMXXAMASAMXXMMSSMS
MSMMMMXMSXSASXSMSMMSMSSSXAAXMASMXXSMAMSAMXSASAMASMSXXAXXAASXXXXXXMAAXMXSAMMSSXMXMAXAASMXSAMXMAMMXSAMMAMMMSAMXMAMAAMXMAMMMSMMSMXXXXXAMXXXXMAM
XXXAAXMAAASAMASAXMAMMXAAXMMSSMSAXMMMMSAMMAMMMMMMMXMAMSMMMAMAMASXMSXMMMXMASAMMMMXXAMMMMAMXMSAAAXMASAMSXMSAMXSXMASMSXMMMMMMAMAMASMSSMSMMMAAMAM
SMSSXMAMXMMMMMMAMMSSSMMMSXAAAMXMXASAMXMXMAXAAXAMXAMXMAAAXMAMMAXXAXAXAXMXAMMMAAXAMXSAMXMMAXXXSMSMXMAMAMXMASXAXMASXXXSAAAXAAMAMSMAAAAAAMSMMMAM
XAAAAXSSMMAXMMMSMAXAXAAAXMMXSMAXXMMASMXAXSSSSSMSSSSMSSSMSMSSMXSMSMMSMXSMXSAMSMSMSASASXMSXSMMXXAMSSMMMAAAXXMASXMSAMASMSMSSXSAXAMMMMMMXMAASXXS
MSMSMMXAASAMXAAAMXSSSMMMSXAAMMMSSXSAMXSXMXAMXMAMAAAAAAMAMXAAMAXAAAXAAXSAMMXXAAMMMASAMAASASMASMSMAAASXSXSMMAAAAMMXMAMXMAMMASMSMXXAXASXMMSXAAA
XAAAMXSSMMXMSMXSSMXXXAAAXMMMSAASAAMMMASXMSAMXMAMMMMMMSMAMMSSMAMSXSSSSXSASMSSMSMAMXMMMMMMAXMXSAXMMSMMAMAAAXSSSSMASMSMMSSMMMXXAXMSSMMMAXXAXMAX
XMMMMAMAXMAMASMMAMSMMSMSSMSASMMXMXMAMASAMXMMASMSMMASAXXMXXXXAMMMAMAMAMSMMMAAAAMMSSMAMXXMMMXAMAMXMAMMAMSMMMMAAXMXSAXMMAAASAMMXXXAAASMMMSAMXMA
XSMXMXXAMXXSASMSAMASXAXMAXMXSXXMSMSXSXSMMAXXASAAASAMMSMSAMXMASAMXMAMAAXSSMSMMMAMAASASMMAAXMMMMMASAMXXMAXAASMSMSAMXMMMMSMMAMXAAMMMMMAMXMMAAMA
XSASMSMSXXXMAXXMASAXMASMXSXAMMSAAAMAMAMXSSSMAMMMMMAXXAAXMAMSAMASXSSSSMSASAXMSXSMSXSXSASXMMMXAXXXSASAXSASMMSAMAMXSSXMAXAXMMMSMSXAXMSMMASMSAMX
MMAMAAAAMSXMSMXSAMAMXMAMAMMAMXMMMSMAMAMXAAXMXMXSSSSMSMSMMSMMASMMMSAMAXMAMMMMMAAAXASXSMMASMMMSSMASAMMMMMMMAMAMSMSXMASXSSXSAAAAAMMSAAASXMXMASA
AMAMSXMASMAAXAAMXMMAASXMASXSMXMASXMXSXSMMXMXAXXXAAXAAAXXAAASAMMAMXAMSAMXMXAAMXMMMAMXMASAXXAAASMAMXMXAAAAMXMXMAXMASAMAAAASMSMSAAAAMSMMAXXMASX
SSXMXASMXMMMMSMXXMASMSXSSSXASXSXSAMXSMXXXXASASMMMMMSMSSMSSMMSSSXXSAMXMASMSSXSAXXMXMMSMMMSSMSSMMSSXSXSSSSSXMASXXSAMSSMMMMMXXAMXMASXMXSSMMMXSA";
            }

            public string RetrieveSampleOutput()
            {
                return @"
2524
1873
";
            }
        }
    }
}
