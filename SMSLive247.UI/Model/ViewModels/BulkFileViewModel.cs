using Microsoft.AspNetCore.Components.Forms;

namespace SMSLive247.UI
{
    public class BulkFileViewModel
    {

        private IBrowserFile UploadedFile { get; set; }
        private string FileName { get; set; }
        private string? BulkNumbers { get; set; }
    }
}
