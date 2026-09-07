namespace SMSLive247.Blazor2.Components
{
    public enum ColorStyle { Primary, Secondary, Success, Warning, Info, Danger, Light, Dark }

    public static class ColorStyleExtensions
    {
        public static string ToTailwindCss(this ColorStyle colorStyle)
        {
            return colorStyle switch
            {
                ColorStyle.Primary      => "bg-brand-500  text-white",
                ColorStyle.Secondary    => "bg-slate-100  text-slate-700",
                ColorStyle.Success      => "bg-green-500  text-white",
                ColorStyle.Danger       => "bg-red-500    text-white",
                ColorStyle.Warning      => "bg-amber-500  text-white",
                ColorStyle.Info         => "bg-blue-500   text-white",
                ColorStyle.Light        => "bg-gray-100   text-gray-800",
                ColorStyle.Dark         => "bg-gray-800   text-gray-100",
                _ => throw new ArgumentOutOfRangeException(nameof(colorStyle), colorStyle, null)
            };
        }
    }
}
