using System;
using System.Text.RegularExpressions;
using RegexStringLibrary;
using static System.Console;
using static RegexStringLibrary.Stex;

namespace ChallengeCore.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("Advent of Code 2016", "Radioisotope Thermoelectric Generators (11)", "https://adventofcode.com/2016/day/11")]
        public class RadioisotopeThermoelectricGenerators : IChallenge
        {
            public void Solve() 
            {
                var input = ReadAll();
                var specs = input.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                var search = (Word.Rep(1).Named("generator") + " generator").Capture()
                    .OrAnyOf(Word.Rep(1).Named("chip") + "-compatible");
                var regex = new Regex(search);

                for (var iFloor = 0; iFloor < 4; iFloor++)
                {
                    WriteLine($"Floor {iFloor + 1}");
                    var matches = regex.Matches(specs[iFloor]);

                    foreach (Match match in matches)
                    {
                        WriteLine(match.Groups["generator"].Success
                            ? $"generator: {match.Groups["generator"].Value}"
                            : $"chip: {match.Groups["chip"].Value}");
                    }
                }
            }

            public string RetrieveSampleInput()
            {
                return @"
The first floor contains a strontium generator, a strontium-compatible microchip, a plutonium generator, and a plutonium-compatible microchip.
The second floor contains a thulium generator, a ruthenium generator, a ruthenium-compatible microchip, a curium generator, and a curium-compatible microchip.
The third floor contains a thulium-compatible microchip.
The fourth floor contains nothing relevant.
";
            }

            public string RetrieveSampleOutput()
            {
                return null;
            }
        }
    }
}
