using System;

namespace Jenkins.Api.Client
{
	public sealed class HealthReport
	{
		public string Description { get; set; }
		public int Score { get; set; }
		public Uri IconUrl { get; set; }
	}
}