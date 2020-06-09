using System;

namespace ChallengeCore
{
	public class ChallengeAttribute : Attribute
	{
		public string Contest { get; }
		public string Name { get; }
		public Uri URI { get; }

		public ChallengeAttribute(string contest, string name, string uriString = null)
		{
			Contest = contest;
			Name = name;
			URI = uriString == null ? null : new Uri(uriString);
		}
	}
}