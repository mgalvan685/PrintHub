namespace PrintHub.Helpers
{
    public class CalculationsHelper
    {
        public double PrintTimeHours(string printTime) => TimeSpan.Parse(printTime).TotalHours;

    }
}
