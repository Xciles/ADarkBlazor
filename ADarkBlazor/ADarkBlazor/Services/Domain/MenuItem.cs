using ADarkBlazor.Services.Domain.Enums;

namespace ADarkBlazor.Services.Domain
{
    public class MenuItem
    {
        public string HRef { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public EMenuType Type { get; set; }
    }
}
