namespace DrovaTranditionChineseHelper
{
    internal class Program
    {
        private const string NewFolder = @"D:\Steam\steamapps\common\Drova - Forsaken Kin\Drova_Data\StreamingAssets\Localization\zh_TW";
        private const string OldFolder = @"D:\Steam\steamapps\common\Drova - Forsaken Kin\Drova_Data\StreamingAssets\Localization\zh_TW_old";

        static void Main(string[] args)
        {
            // 取得新舊檔案清單
            string[] oldFiles = Directory.GetFiles(OldFolder, "*.loc", SearchOption.AllDirectories);
            string[] newFiles = Directory.GetFiles(NewFolder, "*.loc", SearchOption.AllDirectories);

            foreach (var newFile in newFiles)
            {
                // 依檔名對應舊檔
                string expectedOldName = Path.GetFileName(newFile).Replace("_en", "_zh_TW");
                var oldFile = oldFiles.FirstOrDefault(f => Path.GetFileName(f) == expectedOldName);
                if (oldFile == null)
                    continue;

                // 讀取舊檔並轉成字典
                Dictionary<string, string> oldDict = LoadDictionary(oldFile);

                // 讀取新檔案內容
                string[] lines = File.ReadAllLines(newFile);
                string outputFile = newFile.Replace("_en", "_zh_TW");

                File.Delete(newFile);
                using (StreamWriter writer = new StreamWriter(outputFile, false))
                {
                    foreach (var line in lines)
                    {
                        // 空行直接輸出
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            writer.WriteLine(line);
                            continue;
                        }

                        // 以 " {" 為分割，僅拆成兩段
                        var parts = line.Split(new string[] { " {" }, 2, StringSplitOptions.None);
                        if (parts.Length < 2)
                        {
                            writer.WriteLine(line);
                            continue;
                        }

                        string key = parts[0];
                        string value = "{" + parts[1];

                        // 若舊字典有對應 key 則採用舊內容
                        writer.WriteLine(key + " " + (oldDict.ContainsKey(key) ? oldDict[key] : value));
                    }
                }
            }
        }

        // 讀取檔案並轉換為 key-value 字典
        private static Dictionary<string, string> LoadDictionary(string file)
        {
            return File.ReadAllLines(file)
                       .Where(l => !string.IsNullOrWhiteSpace(l))
                       .Select(l => l.Split(new string[] { " {" }, 2, StringSplitOptions.None))
                       .Where(parts => parts.Length == 2)
                       .ToDictionary(parts => parts[0], parts => "{" + parts[1]);
        }
    }
}
