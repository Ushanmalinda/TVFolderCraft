using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static TVFolderCraft.HistoryPage;

namespace TVFolderCraft
{
    public static class HistoryManager
    {
        //get location to manage history 
        private static string historyFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TVFolderCraft", "history.txt");

        static HistoryManager()
        {
            // ensure directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(historyFilePath));
        }

        // for GeneratePage (manual creation)
        public static void AddManualHistoryItem(string seriesName, int year, string outputPath)
        {
            var historyEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}|{seriesName}|{year}|{outputPath}|Manual";
            File.AppendAllText(historyFilePath, historyEntry + Environment.NewLine); // create and add hostory
        }

        // for InternetPage (API-based creation)
        public static void AddInternetHistoryItem(string seriesName, string outputPath)
        {
            var historyEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}|{seriesName}||{outputPath}|Internet";
            File.AppendAllText(historyFilePath, historyEntry + Environment.NewLine); // create and add hostory
        }

        //get history item info to list
        public static List<HistoryItem> GetHistoryItems()
        {
            var items = new List<HistoryItem>();
            //check hostory file have or not
            if (File.Exists(historyFilePath))
            {
                var lines = File.ReadAllLines(historyFilePath);
                foreach (var line in lines.Reverse()) // show newest first
                {
                    var parts = line.Split('|');
                    if (parts.Length >= 4) // updated to handle both formats
                    {
                        if (DateTime.TryParse(parts[0], out DateTime date))
                        {
                            string title;
                            if (!string.IsNullOrEmpty(parts[2])) // has year (manual creation)
                            {
                                title = $"{parts[1]} ({parts[2]})";
                            }
                            else // internet creation
                            {
                                title = parts[1];
                            }

                            //default as mannuwal
                            items.Add(new HistoryItem
                            {
                                Date = FormatDate(date),
                                Title = title,
                                Path = parts[3],
                                Source = parts.Length > 4 ? parts[4] : "Manual" 
                            });
                        }
                    }
                }
            }

            return items;
        }

        // clear history text
        public static void ClearHistory()
        {
            try
            {
                if (File.Exists(historyFilePath))
                {
                    File.Delete(historyFilePath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to clear history: {ex.Message}");
            }
        }

        // method to check if history exists
        public static bool HasHistory()
        {
            return File.Exists(historyFilePath) && new FileInfo(historyFilePath).Length > 0;
        }

        // formart date time to save history
        private static string FormatDate(DateTime date)
        {
            var now = DateTime.Now;
            if (date.Date == now.Date)
                return $"Today, {date:h:mm tt}";
            else if (date.Date == now.Date.AddDays(-1))
                return $"Yesterday, {date:h:mm tt}";
            else
                return date.ToString("MMM dd, yyyy, h:mm tt");
        }
    }
}