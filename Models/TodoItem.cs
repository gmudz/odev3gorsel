namespace odev3gorsel.Models
{
    public class TodoItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string TaskName { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty; // Detay alanı
        public DateTime Date { get; set; } = DateTime.Now; // Tarih
        public TimeSpan Time { get; set; } = DateTime.Now.TimeOfDay; // Saat
        public bool IsCompleted { get; set; } = false; // Tamamlanma durumu
    }
}