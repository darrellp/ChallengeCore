using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ChallengeCore.Challenges;

namespace ChallengeCore
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		// Put here so we can update broken links more easily
		private static Dictionary<string, string> mpChallengeToWebSite = new Dictionary<string, string>
		{
			{"Advent of Code 2020: Encoding Error (9)", "https://adventofcode.com/2020/day/9"},
			{"ProgChallenges: Ant On a Chessboard", "https://onlinejudge.org/external/101/10161.pdf"},
			{"ProgChallenges: Archeologist's Dilemma", "https://onlinejudge.org/external/7/701.pdf"},
			{"ProgChallenges: Australian Voting", "https://onlinejudge.org/external/101/10142.pdf"},
			{"ProgChallenges: Bicoloring", "https://onlinejudge.org/external/100/10004.pdf"},
			{"ProgChallenges: Bigger Square Please", "https://onlinejudge.org/external/102/10270.pdf"},
			{"ProgChallenges: Check the Check", "https://onlinejudge.org/external/101/10196.pdf"},
			{"ProgChallenges: Crypt Kicker", "https://onlinejudge.org/external/8/843.pdf"},
			{"ProgChallenges: Dog and Gopher", "https://onlinejudge.org/external/100/10010.pdf" },
			{"ProgChallenges: Edit Step Ladder", "https://onlinejudge.org/external/100/10029.pdf"},
			{"ProgChallenges: Erdos Numbers", "https://onlinejudge.org/external/100/10044.pdf"},
			{"ProgChallenges: Factovisors", "https://onlinejudge.org/external/101/10139.pdf"},
			{"ProgChallenges: File Fragmentation", "https://onlinejudge.org/external/101/10132.pdf"},
			{"ProgChallenges: Graphical Editor", "https://onlinejudge.org/external/102/10267.pdf"},
			{"ProgChallenges: Hartals", "https://onlinejudge.org/external/100/10050.pdf"},
			{"ProgChallenges: How Many Fibs?", "https://onlinejudge.org/external/101/10183.pdf"},
			{"ProgChallenges: Interpreter", "https://onlinejudge.org/external/100/10033.pdf"},
			{"ProgChallenges: Is Bigger Smarter?", "https://onlinejudge.org/external/101/10131.pdf"},
			{"ProgChallenges: Jolly Jumpers", "https://onlinejudge.org/external/100/10038.pdf"},
			{"ProgChallenges: Knights of the Round Table", "https://onlinejudge.org/external/13/1364.pdf"},
			{"ProgChallenges: LC Display", "https://onlinejudge.org/external/7/706.pdf"},
			{"ProgChallenges: MineSweeper", "https://onlinejudge.org/external/101/10189.pdf"},
			{"ProgChallenges: Ones", "https://onlinejudge.org/external/101/p10127.pdf"},
			{"ProgChallenges: Pairsumonious Numbers", "https://onlinejudge.org/external/102/10202.pdf"},
			{"ProgChallenges: Playing With Wheels", "https://onlinejudge.org/external/100/10067.pdf"},
			{"ProgChallenges: Poker Hands", "https://onlinejudge.org/external/103/10315.pdf"},
			{"ProgChallenges: Primary Arithmetic", "https://onlinejudge.org/external/100/10035.pdf"},
			{"ProgChallenges: Reverse and Add", "https://onlinejudge.org/external/100/10018.pdf"},
			{"ProgChallenges: Rope Crisis in Ropeland!", "https://onlinejudge.org/external/101/10180.pdf"},
			{"ProgChallenges: Shoemaker's Problem", "https://onlinejudge.org/external/100/10026.pdf"},
			{"ProgChallenges: Smith Numbers", "https://onlinejudge.org/external/100/10042.pdf"},
			{"ProgChallenges: Stack 'em Up", "https://onlinejudge.org/external/102/10205.pdf"},
			{"ProgChallenges: Stern-Brocot", "https://onlinejudge.org/external/100/10077.pdf"},
			{"ProgChallenges: Summation of Four Primes", "https://onlinejudge.org/external/101/10168.pdf"},
			{"ProgChallenges: The 3n + 1 Problem", "https://onlinejudge.org/external/1/100.pdf"},
			{"ProgChallenges: The Monocycle", "https://onlinejudge.org/external/100/10047.pdf"},
			{"ProgChallenges: The Trip", "https://onlinejudge.org/external/101/10137.pdf"},
			{"ProgChallenges: Unidirectional TSP", "https://onlinejudge.org/external/1/116.pdf"},
			{"ProgChallenges: Vito's Family", "https://onlinejudge.org/external/100/10041.pdf"},
			{"ProgChallenges: WERTYU", "https://onlinejudge.org/external/100/10082.pdf"},
			{"ProgChallenges: Where's Waldorf", "https://onlinejudge.org/external/100/10010.pdf"},
		};
		
		#region DllImports
		// If the Target of the build changes then the output directory will change.  We reference this directory in the post-build
		// action of CPP Challenges to copy it's dll into the solution directory which means that post-build command must be edited
		// to reflect the new target directory.
        [DllImport("CPP Challenges.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.LPStr)]
        private static extern string GatherChallengeInfo();
		#endregion

		#region Private Variables
		private bool _originalInput = true;
		private bool _changingSelection;
		private Thread _challengeThread;
		#endregion

		#region Constructor
		public MainWindow()
		{
			InitializeComponent();
			SetupChallenges();
		}
		#endregion

		#region Initialize Challenge list
		private void SetupChallenges()
		{
			var challenges = (typeof(ChallengeClass)).
				GetNestedTypes().
				Select(MethodTest).
				Where(t => t != null)
				.ToList();

            challenges.AddRange(ParseCppChallengeInfo(GatherChallengeInfo()));
            challenges.AddRange(GatherFSChallengeInfo());

			var contests = new Dictionary<string, List<ChallengeInfo>>();

			foreach (var test in challenges)
			{
				if (test.Uri is null)
				{
					var key = $"{test.Contest}: {test.Name}";
					if (mpChallengeToWebSite.TryGetValue(key, out string uri))
					{
						test.Uri = new Uri(uri);
					}
				}
				if (contests.ContainsKey(test.Contest))
				{
					contests[test.Contest].Add(test);
				}
				else
				{
					contests[test.Contest] = new List<ChallengeInfo> { test };
				}
			}
			foreach (var info in contests)
			{
				var infoList = info.Value;
				var contest = info.Key;
				infoList.Sort((info1, info2) => string.Compare(info1.Name, info2.Name, StringComparison.Ordinal));
				var item = new TreeViewItem
				{
					Header = contest,
					ItemsSource = infoList
				};
				tvChallenges.Items.Add(item);
			}
		}

		private IEnumerable<ChallengeInfo> GatherFSChallengeInfo()
		{
			return Assembly.GetAssembly(typeof(FS_Challenges))
                .GetTypes()
                .Select(FsMethodTest)
                .Where(t => t != null)
				.ToList();
		}

		private static IEnumerable<ChallengeInfo> ParseCppChallengeInfo(string infoString)
		{
			var ret = new List<ChallengeInfo>();
			var readPointer = 0;
			var index = 0;

			while (readPointer < infoString.Length)
			{
				ret.Add(ParseOneCppChallenge(infoString, index++, ref readPointer));
			}
			return ret;
		}

		private static ChallengeInfo ParseOneCppChallenge(string infoString, int challengeIndex, ref int readPointer)
		{
			var index = infoString.IndexOf('$', readPointer);
			var contest = infoString.Substring(readPointer, index - readPointer);
			readPointer = index + 1;
			index = infoString.IndexOf('<', readPointer);
			var name = infoString.Substring(readPointer, index - readPointer);
			readPointer = index + 1;
			index = infoString.IndexOf('>', readPointer);
			var uri = infoString.Substring(readPointer, index - readPointer);
			readPointer = index + 1;
			index = infoString.IndexOf('$', readPointer);
			var input = ChallengeClass.CppStringToCs(infoString.Substring(readPointer, index - readPointer));
			readPointer = index + 1;
			index = infoString.IndexOf('$', readPointer);
			var output = ChallengeClass.CppStringToCs(infoString.Substring(readPointer, index - readPointer));
			readPointer = index + 1;
			IChallenge newChallenge = new CppChallenge(challengeIndex, input, output);
			return new ChallengeInfo(name, contest, newChallenge, new Uri(uri));
		}

		private static ChallengeInfo FsMethodTest(Type member)
		{
			return member.
				GetCustomAttributes(true).
				OfType<FS_Challenges.ChallengeAttribute>().
				Select(t => new ChallengeInfo(
					t.Name,
					t.Contest,
					new FsChallenge((FS_Challenges.IChallenge)Activator.CreateInstance(member)),
					t.URI)).
				FirstOrDefault();
		}

		private static ChallengeInfo MethodTest(Type member)
		{
			return member.
				GetCustomAttributes(true).
				OfType<ChallengeAttribute>().
				Select(t => new ChallengeInfo(
					t.Name,
					t.Contest,
					(IChallenge)Activator.CreateInstance(member),
					t.URI)).
				FirstOrDefault();
		}
		#endregion

		#region Challenge solving
		private string GetChallengeData(IChallenge challenge)
		{
			var challengeDataString = challenge.RetrieveSampleInput();
			string challengeData = null;

			if (challengeDataString != null)
			{
				challengeData = challengeDataString.Substring(Environment.NewLine.Length);
			}
			return challengeData;
		}

		private void SolveChallengeAsync(IChallenge challenge, string challengeData, Stopwatch sw)
		{
			var challengeTask = Task.Factory.StartNew(delegate
			{
				string strRet;
				var isError = false;

				_challengeThread = Thread.CurrentThread;
				using (var str = challengeData == null ? null : new StringReader(challengeData))
				{
					try
					{
						if (str != null)
						{
							Console.SetIn(str);
						}
						var swrit = new StringWriter();
						Console.SetOut(swrit);
						sw.Start();
						challenge.Solve();
						sw.Stop();
						strRet = swrit.ToString();
					}
					catch (Exception ex)
					{
						sw.Stop();
						isError = true;
						strRet = "Error: " + ex.Message;
					}
				}
				return new Tuple<string, bool>(strRet, isError);
			});

			challengeTask.ContinueWith(_ =>
			{
				Dispatcher.Invoke(
					delegate
					{
						try
						{
							ChallengeSolved(challengeTask.Result.Item1, challengeTask.Result.Item2, challenge, sw);
						}
						catch (AggregateException)
						{
							sw.Stop();
							ChallengeSolved("Challenge Terminated", true, challenge, sw);
						}
					});
			});
		}

		private void ChallengeSolved(string result, bool isError, IChallenge challenge, Stopwatch sw)
		{
			var strResultString = challenge.RetrieveSampleOutput();
			var isResult = strResultString != null && _originalInput;

			var isSuccess = isResult && result == strResultString.Substring(Environment.NewLine.Length);
			var colorOutput = Colors.Black;
			svOutput.ScrollToTop();

			// If there was an error or if there was a result we were supposed to meet and we didn't meet it
			if (isError || (isResult && !isSuccess))
			{
				colorOutput = Colors.Red;
			}
			// If there was a result we were supposed to meet and we nailed it
			else if (isResult)
			{
				colorOutput = Colors.Green;
			}
			tbOutput.Foreground = new SolidColorBrush(colorOutput);
			tbOutput.Text = result + Environment.NewLine + sw.ElapsedMilliseconds + " ms.";
			EnableUI();
		}

		private void EnableUI()
		{
			btnCancel.IsEnabled = false;
			tvChallenges.IsEnabled = true;
			btnRun.IsEnabled = true;
			btnUri.IsEnabled = true;
			_challengeThread = null;
		}

		private void DisableUI()
		{
			btnCancel.IsEnabled = true;
			tvChallenges.IsEnabled = false;
			btnRun.IsEnabled = false;
			btnUri.IsEnabled = false;
		}
		#endregion

		#region Event handlers
		private void RunChallenges(object sender, RoutedEventArgs e)
		{
			// Check to see if we're located on a valid challenge...
			var challengeInfo = tvChallenges.SelectedItem as ChallengeInfo;
			if (challengeInfo == null)
			{
				return;
			}

			// Yes - get the challenge and the input data if any
			var challenge = challengeInfo.Challenge;
			var challengeData = tbxInput.Text;
			var sw = new Stopwatch();

			// Disable all the UI other than Cancel until this is done...
			DisableUI();
			SolveChallengeAsync(challenge, challengeData, sw);
		}

		private void CancelChallenge(object sender, RoutedEventArgs e)
		{
			if (_challengeThread != null)
			{
                _challengeThread.Abort();
				EnableUI();
			}
		}

		private void tbxInput_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (!_changingSelection)
			{
				_originalInput = false;
			}
		}

		private void Challenges_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			_originalInput = true;
			_changingSelection = true;
			var challengeInfo = tvChallenges.SelectedItem as ChallengeInfo;
			if (challengeInfo == null)
			{
				return;
			}
			var challengeData = GetChallengeData(challengeInfo.Challenge);
			tbxInput.Text = challengeData;
			btnUri.IsEnabled = challengeInfo.Uri != null;
			_changingSelection = false;
		}

		private void VisitURI(object sender, RoutedEventArgs e)
		{
			var challengeInfo = tvChallenges.SelectedItem as ChallengeInfo;
			if (challengeInfo?.Uri == null)
			{
				return;
			}

            var page = challengeInfo.Uri.ToString();
            var psi = new ProcessStartInfo {FileName = page, UseShellExecute = true};
			Process.Start(psi);
		}
		#endregion
	}
}

