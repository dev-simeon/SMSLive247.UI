using Microsoft.AspNetCore.Components.Forms;

namespace SMSLive247.OpenApi
{
    public partial class FileParameter
    {
        public FileParameter(IBrowserFile file)
            : this(file.OpenReadStream(), file.Name, file.ContentType) { }
    }

}
