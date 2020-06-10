using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace ChallengeCore.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("Euler Project", "Prob 59", "https://projecteuler.net/problem=59")]
        // ReSharper disable once UnusedMember.Global
        public class Euler59 : IChallenge
        {
            public void Solve()
            {
                List<byte> crypt;

                using (var file = new System.IO.StreamReader("Data Files/cipher1.txt"))
                {
                    // ReSharper disable PossibleNullReferenceException
                    crypt = file.ReadLine().Split(',').Select(byte.Parse).ToList();
                    // ReSharper restore PossibleNullReferenceException
                }

                var key = GetKey(crypt);
                var decrypt = crypt.ZipRepeat(key, (ch, k) => ch ^ k).ToList();
                WriteLine(decrypt.Sum());
            }

            public string RetrieveSampleInput()
            {
                return null;
            }

            public string RetrieveSampleOutput()
            {
                return @"
129448
";
            }

            private static IEnumerable<byte> GetKey(IEnumerable<byte> crypt)
            {
                var lists = new List<byte>[3];
                var sorts = new List<byte>[3];
                var iList = 0;

                for (int i = 0; i < 3; i++)
                {
                    lists[i] = new List<byte>();
                }

                foreach (var val in crypt)
                {
                    lists[iList].Add(val);
                    iList = (iList + 1) % 3;
                }

                for (int i = 0; i < 3; i++)
                {
                    sorts[i] = SortByFrequency(lists[i]);
                }

                return new[] {(byte) (sorts[0][0] ^ 32), (byte) (sorts[1][0] ^ 32), (byte) (sorts[2][0] ^ 32)};
            }

            private static List<byte> SortByFrequency(IEnumerable<byte> bytes)
            {
                var counts = new Dictionary<byte, int>();
                foreach (var val in bytes)
                {
                    if (counts.ContainsKey(val))
                    {
                        counts[val]++;
                    }
                    else
                    {
                        counts[val] = 1;
                    }
                }

                var kvpList = counts.ToList();
                kvpList.Sort((x, y) => y.Value.CompareTo(x.Value));
                return kvpList.Select(kvp => kvp.Key).ToList();
            }
        }
    }
}
