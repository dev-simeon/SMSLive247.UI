namespace Gibs.UI.Components.Forms.Input
{
    public class FormInputDateRangeValue
    {
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddYears(1);

    }
}
