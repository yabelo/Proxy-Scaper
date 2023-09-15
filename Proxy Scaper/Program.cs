using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Unit4.CollectionsLib;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using System.Collections;
using File = System.IO.File;

namespace Proxy_Scraper {
    internal class Program {
        static void Main(string[] args) {

            WelcomeMessage.WelcomeMessage.ShowMessage();

            ScrapeProxyScrape();
        }

        public static void ScrapeProxyScrape() {

            Console.WriteLine("Choose Proxy Type:");
            int answer = 0;
            while (true) {
                Console.Write(" 1 | http\n 2 | socks4\n 3 | socks5\n ");
                answer = int.Parse(Console.ReadLine());

                if (answer >= 1 && answer <= 3) break;
            }


            string[] https = { "https://api.proxyscrape.com/v2/?request=displayproxies&protocol=http&timeout=10000&country=all&ssl=all&anonymity=all", "https://api.proxylist.to/http?key=PROXY-70C74142-LIST-DE9F9256-TO", "https://www.proxy-list.download/api/v1/get?type=http" };
            string[] socks4s = { "https://api.proxyscrape.com/v2/?request=displayproxies&protocol=socks4&timeout=10000&country=all&ssl=all&anonymity=all", "https://api.proxylist.to/socks4?key=PROXY-70C74142-LIST-DE9F9256-TO", "https://www.proxy-list.download/api/v1/get?type=socks4" };
            string[] socks5s = { "https://api.proxyscrape.com/v2/?request=displayproxies&protocol=socks5&timeout=10000&country=all&ssl=all&anonymity=all", "https://api.proxylist.to/socks5?key=PROXY-70C74142-LIST-DE9F9256-TO", "https://www.proxy-list.download/api/v1/get?type=socks5" };

            switch (answer) {
                case 1:
                    useApis(https);
                    break;
                case 2:
                    useApis(socks4s);
                    break;
                case 3:
                    useApis(socks5s);
                    break;
            }  

        }

        public static void useApis(string[] apis) {
            for (int i = 0; i < apis.Length; i++) {
                HttpClient client = new HttpClient();

                var response = client.GetAsync(apis[i]);

                response.Wait();
                if (response.IsCompleted) {
                    var result = response.Result;

                    if (result.IsSuccessStatusCode) {
                        var msg = result.Content.ReadAsStringAsync().Result;
                        string[] strings = msg.Split('\n');

                        try {
                            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            string fileName = "scrapedProxy.txt";
                            string filePath = Path.Combine(desktopPath, fileName);

                            // Check if the file exists, if not, create it with the proxy information
                            if (!File.Exists(filePath)) {
                                File.WriteAllLines(filePath, strings);
                            } else {
                                // Open the file in append mode and write the new proxy information to it
                                using (StreamWriter writer = File.AppendText(filePath)) {
                                    foreach (string proxy in strings) {
                                        writer.WriteLine(proxy);
                                    }
                                }
                            }
                        } catch (Exception ex) {
                            Console.WriteLine($"Error saving file: {ex.Message}");
                        }
                    }
                }
            }

            Console.WriteLine("Done.");
        }

    }
}
