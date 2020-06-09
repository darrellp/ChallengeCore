using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace ChallengeCore.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("ProgChallenges", "Shoemaker's Problem",
			"https://onlinejudge.org/index.php?option=onlinejudge&Itemid=8&page=show_problem&problem=967")]
		public class Shoemaker : IChallenge
		{
			public void Solve()
			{
				var ret = new StringBuilder();
				var cCases = GetVal();

				for (var iCase = 0; iCase < cCases; iCase++)
				{
					var caseCur = new ShoemakerCase();
					caseCur.Solve(ret);
				}

				Write(ret.ToString());
			}

			public string RetrieveSampleInput()
			{
				return @"
1

4
3 4
1 1000
2 2
5 5
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
2 1 3 4
";
			}

			class ShoemakerCase
			{
				struct Job
				{
					private readonly int _fine;
					private readonly int _days;
					// ReSharper disable once InconsistentNaming
					public int IJob { get; private set; }

					public int Fine
					{
						get { return _fine; }
					}

					public int Days
					{
						get { return _days; }
					}


					public Job(int iJob, int fine, int days) : this()
					{
						_fine = fine;
						_days = days;
						IJob = iJob + 1;
					}
				}

				private readonly List<Job> _jobs;

				public ShoemakerCase()
				{
					ReadLine();
					var cJobs = GetVal();
					_jobs = new List<Job>(cJobs);

					for (var iJob = 0; iJob < cJobs; iJob++)
					{
						var jobInfo = GetVals();
						_jobs.Add(new Job(iJob, jobInfo[1], jobInfo[0]));
					}
				}

				public void Solve(StringBuilder output)
				{
					var fFirst = true;

					_jobs.Sort((j1, j2) => (j1.Days * j2.Fine).CompareTo(j2.Days * j1.Fine));

					foreach (var t in _jobs)
					{
						output.Append((fFirst ? "" : " ") + t.IJob);
						fFirst = false;
					}
					output.Append(Environment.NewLine);
				}
			}
		}
	}
}
