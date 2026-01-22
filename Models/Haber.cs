namespace odev3gorsel.Models
{
    public class Haber
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string PubDate { get; set; } = string.Empty;
        public string Thumbnail { get; set; } = "dotnet_bot.png";
    }
}