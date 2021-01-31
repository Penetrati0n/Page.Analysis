using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using NLog;

namespace Page.Analysis
{
    public static class PageAnalysis
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();
        private static char[] Separators { get; } = { '\n', '\r', '\t', ' ', '~', '!', '@', '#', '$', '%', '^', '&', '*',
            '(', ')', '_', '+', '=', '-', '№', '[', ']', '{', '}', ';', ':', '\'', '"', '/', '?', '.', ',', '<', '>', '|', '\\' };

        public static IEnumerable<KeyValuePair<string, int>> GetStatistic(string urlAdress)
        {
            Dictionary<string, int> result = new Dictionary<string, int>(); 
            string fileName = "default.pa";
            DownloadHtmlPage(urlAdress, fileName);

            if(!File.Exists(fileName))
            {
                result.Add("null", -1);
                return result;
            }

            using (StreamReader reader = new StreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().ToLower();

                    string[] words = line.Split(Separators, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var t in words)
                    {
                        if (!IsWord(t))continue;

                        if (!result.ContainsKey(t))
                        {
                            result.Add(t, 1);
                        }
                        else
                        {
                            result[t]++;
                        }
                    }
                }
            }

            if (File.Exists(fileName)) File.Delete(fileName);

            return result.OrderBy(o => o.Value);
        }
        public static string GetStringStatistic(string urlAdress)
        {
            StringBuilder report = new StringBuilder();
            var stats = GetStatistic(urlAdress);
            
            foreach(var pair in stats)
            {
                report.Append($"{pair.Key} : {pair.Value}\n");
            }

            report.Remove(report.Length - 1, 1);

            return report.ToString();
        }
        public static void PrintStatisticInConsole(string urlAdress)
        {
            Console.WriteLine("————————————————————");
            Console.WriteLine($"{"Start", 13}");
            Console.WriteLine("————————————————————");
            Console.OutputEncoding = GetEncodingFromPage(urlAdress);
            var stats = GetStatistic(urlAdress);

            foreach(var word in stats)
            {
                Console.WriteLine($"{word.Key} : {word.Value}");
            }

            Console.WriteLine("————————————————————");
            Console.WriteLine($"{"End", 11}");
            Console.WriteLine("————————————————————");


        }
        public static Encoding GetEncodingFromPage(string urlAdress)
        {
            string charSet = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAdress);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    charSet = response.CharacterSet;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "returned UTF-8");
            }

            return charSet == null ? Encoding.Default : Encoding.GetEncoding(charSet);
        }

        private static void DownloadHtmlPage(string urlAdress, string fileName)
        {
            Encoding encoding = GetEncodingFromPage(urlAdress);
            
            WebClient client = new WebClient() { Encoding = encoding };
            
            try
            {
                client.DownloadFile(urlAdress, fileName);
            }
            catch(Exception ex)
            {
                Log.Fatal(ex);
            }
        }
        private static bool IsWord(string word)
        {
            foreach(var t in word)
            {
                if(!char.IsLetter(t))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
