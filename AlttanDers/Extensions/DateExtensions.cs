namespace KampusKodu.Extensions
{
    public static class DateExtensions
    {
        public static string ToTimeAgo(this DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;

            if (timeSpan.TotalMinutes < 1)
                return "Az önce";

            if (timeSpan.TotalMinutes < 60)
                return $"{Math.Floor(timeSpan.TotalMinutes)} dakika önce";

            if (timeSpan.TotalHours < 24)
                return $"{Math.Floor(timeSpan.TotalHours)} saat önce";

            if (timeSpan.TotalDays < 7)
                return $"{Math.Floor(timeSpan.TotalDays)} gün önce";

            // 7 günden eskiyse normal tarih göster
            return dateTime.ToString("dd.MM.yyyy HH:mm");
        }
    }
}