using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Diagnostics;
using NLog;
using static System.Reflection.Assembly;

namespace Page.Analysis
{
    class Program
    {
        static void Main(string[] args)
        {
            Start();
        }

        private static void Start()
        {
            Console.Title = "Page Analysis";
            Console.OutputEncoding = Encoding.Default;
            Console.WriteLine("Welcome, user!");
            Console.Write("Please, enter page URL adress: ");
            string url = Console.ReadLine();

            while (!CheckUrl(ref url))
            {
                Console.WriteLine("Invalid URl!!!");
                Console.Write("Please, enter page URL adress: ");
                url = Console.ReadLine();
            }
            Console.WriteLine("Good, valid URl!!!");

            PageAnalysis.PrintStatisticInConsole(url, true);
            
            static bool CheckUrl(ref string url)
        {
            if (new Regex(RegexPattern.Url_With_Http).IsMatch(url))
            {
                return true;
            }
            else if(new Regex(RegexPattern.Url_WithOut_Http).IsMatch(url))
            {
                url = "http://" + url;
                return true;
            }
            else
            {
                return false;
            }
        }
        }
    }
}
