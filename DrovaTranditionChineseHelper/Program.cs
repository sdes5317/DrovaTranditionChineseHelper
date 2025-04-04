using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace DrovaTranditionChineseHelper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1. 配置新舊翻譯檔資料夾
            var newVersionFolder = @"D:\Steam\steamapps\common\Drova - Forsaken Kin\Drova_Data\StreamingAssets\Localization\zh_TW";
            var oldVersionFolder = @"D:\Steam\steamapps\common\Drova - Forsaken Kin\Drova_Data\StreamingAssets\Localization\zh_TW_old";

            // 2. 從資料夾取出清單
            var oldFiles = Directory.GetFiles(oldVersionFolder, "*.loc", SearchOption.AllDirectories);
            var newFiles = Directory.GetFiles(newVersionFolder, "*.loc", SearchOption.AllDirectories);

            // 3. 逐筆更新翻譯檔
            foreach (var newFileName in newFiles)
            {
                var old = oldFiles
                    .Where(oldFileName => Path.GetFileName(oldFileName) == Path.GetFileName(newFileName).Replace("_en", "_zh_TW"))
                    .FirstOrDefault();
                if (old is null) continue;
                var oldDictionary = LoadFileToDictionary(old);

                var newFileTemp = File.ReadAllLines(newFileName);
                File.Delete(newFileName);
                using (var sw=new StreamWriter(newFileName.Replace("_en", "_zh_TW"), false))
                {
                    foreach (var line in newFileTemp)
                    {
                        if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line)) sw.WriteLine(line);
                        else
                        {
                            var split = line.Split(" {");
                            var lineTemp = new { Key = split[0], Value = "{" + split[1] };

                            if (oldDictionary.ContainsKey(lineTemp.Key))
                            {
                                sw.WriteLine(lineTemp.Key + " " + oldDictionary[lineTemp.Key]);
                            }
                            else
                            {
                                sw.WriteLine(lineTemp.Key + " " + lineTemp.Value);
                            }
                        }
                    }
                }
            }
        }

        private static Dictionary<string, string> LoadFileToDictionary(string? old)
        {
            var t = File.ReadAllLines(old)
                .Where(line => !string.IsNullOrEmpty(line) && !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Split(" {"));
               return t.Where(line => line.Count() == 2)
                .ToDictionary(line => line[0], line => "{" + line[1]);
        }
    }
}
