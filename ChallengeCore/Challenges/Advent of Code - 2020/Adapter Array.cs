using System;
using System.Linq;
using static System.Console;

namespace ChallengeCore.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("Advent of Code 2020", "Adapter Array (10)", "https://adventofcode.com/2020/day/10")]
        public class AdapterArray : IChallenge
        {
            public void Solve()
            {
                var values = GetLines().Select(int.Parse).ToArray();
                var c1 = 0;
                var c3 = 1;         // Count the final jump to the device

                // Part 1
                Array.Sort(values);
                for (int i = -1; i < values.Length - 1; i++)
                {
                    // i == -1 is special case to handle from the wall of 0 to the first adapter
                    var diff = values[i + 1] - (i < 0 ? 0 : values[i]);
                    
                    if (diff == 1)
                    {
                        c1++;
                    }
                    else if (diff == 3)
                    {
                        c3++;
                    }
                }
                WriteLine(c1 * c3);

                // Part 2
                // Matrix multiplication is the secret here
                var dim = values.Length + 2;
                var orig = new long[dim, dim];

                // Notable indices in the matrix - 0 is the wall charger, 1 is the first adapter,
                // values.Length is the last adapter, values.Length + 1 is the device
                if (values[0] == 1)
                {
                    orig[0, 1] = 1;
                }

                if (values[0] == 2 || values[1] == 2)
                {
                    orig[0, 2] = 1;
                }

                if (values[0] == 3 || values[1] == 3 || values[2] == 3)
                {
                    orig[0, 3] = 1;
                }

                orig[values.Length, values.Length + 1] = 1;

                for (int i = 1; i <= values.Length; i++)
                {
                    // Index in values array is 1 less than index in matrix - see comments above
                    int iValues = i - 1;

                    var adapterValue = values[iValues];
                    for (var di = 1; di <= 3; di++)
                    {
                        if (iValues + di >= values.Length)
                        {
                            continue;
                        }

                        var d = values[iValues + di] - adapterValue;

                        if (d == 1 || d == 2 || d == 3)
                        {
                            orig[i, i + di] = 1;
                        }
                    }
                }
                var pow = MatrixMultiply(orig, orig);

                var cPaths = 0l;
                for (int iPow = 0; iPow < values.Length; iPow++)
                {
                    pow = MatrixMultiply(pow, orig);
                    cPaths += pow[0, values.Length + 1];
                }

                WriteLine(cPaths);
            }

            private static long[,] MatrixMultiply(long[,] mtx1, long[,] mtx2)
            {
                var dim = mtx1.GetLength(0);
                var result = new long[dim, dim];

                for (var iRowResult = 0; iRowResult < dim; iRowResult++)
                {
                    for (var iColResult = 0; iColResult < dim; iColResult++)
                    {
                        var sum = 0l;

                        for (var k = 0; k < dim; k++)
                        {
                            sum += mtx1[iRowResult, k] * mtx2[k, iColResult];
                        }

                        result[iRowResult, iColResult] = sum;
                    }
                }

                return result;
            }

            public string RetrieveSampleInput()
            {
                return @"
2
49
78
116
143
42
142
87
132
86
67
44
136
82
125
1
108
123
46
37
137
148
106
121
10
64
165
17
102
156
22
117
31
38
24
69
131
144
162
63
171
153
90
9
107
79
7
55
138
34
52
77
152
3
158
100
45
129
130
135
23
93
96
103
124
95
8
62
39
118
164
172
75
122
20
145
14
112
61
43
141
30
85
101
151
29
113
94
68
58
76
97
28
111
128
21
11
163
161
4
168
157
27
72";
            }

            public string RetrieveSampleOutput()
            {
                return @"
2450
32396521357312
";
            }
        }
    }
}
