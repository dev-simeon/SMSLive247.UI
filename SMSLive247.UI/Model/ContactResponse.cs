using System.Text.Json.Serialization;

namespace SMSLive247.UI
{
    public partial class ContactResponse
    {
        [Newtonsoft.Json.JsonIgnore]
        public bool Checked { get; set; } = false;
    }
}
