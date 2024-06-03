namespace SMSLive247.UI
{
    public class Member
    {
        public Member(string email, string apiKey)
        {
            Email = email;
            ApiKey = apiKey;
        }

        public void UpdateSmsBalance(double newSmsBalance)
        {
            SmsBalance = newSmsBalance;
        }

        public string GetNameInitials()
        {
            var splittedName = FullName.Split(' ');
            return $"{splittedName[0].Substring(0)}{splittedName[1].Substring(0)}";
        }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string ApiKey { get; private set; }
        public double SmsBalance { get; private set; } = 0.00;
    }
}
