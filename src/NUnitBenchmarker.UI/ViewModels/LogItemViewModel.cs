namespace NUnitBenchmarker.UI.ViewModels
{
	public class LogItemViewModel
	{
		public LogItemViewModel(string dateTime, string level, string message)
		{
			DateTime = dateTime;
			Level = level;
			Message = message;
		}

		public string DateTime { get; set; }
		public string Level { get; set; }
		public string Message { get; set; }
	}
}