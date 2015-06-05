using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml.Linq;
using System;
using System.Xml;

 
namespace WpfApplication5
{

    public class Directive<T>
    {
        public bool IsTag = false;

        public bool Commented = false;

        public string Key { get; set; }

        public T Value { get; set; }
    }

    public class VHost
    {

        public bool Active { get; set; }

        public string HostName { get; set; }
        public string Port { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public List<string> Content { get; set; }

        public void OpenInBrowser()
        {
            System.Diagnostics.Process.Start("http://" + this.HostName);
        }

        public Dictionary<string, string> Directives { get; set; }

    }

    class VHostManager
    {
        private static VHostManager _instance;

        const string FileName = @"D:\wamp\bin\apache\apache2.4.9\conf\extra\httpd-vhosts.conf";

        private string LastVHost;

        private Dictionary<string, VHost> VHosts = new Dictionary<string, VHost>();

        public bool VHostTagIsOpen = false;
        public bool DirectiveTagIsOpen = false;


        private VHostManager()
        {
            this.ParseBlocks();
        }


        /// <summary>
        /// 
        /// </summary>
        public static VHostManager GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new VHostManager();
                }
                return _instance;
            }
        }

        public Dictionary<string, VHost> GetVHosts()
        {
            return VHosts;
        }

        public VHost GetVHost(string VHostName)
        {
            if (VHosts.ContainsKey(VHostName))
                return VHosts[VHostName];

            return null;
        }

        public void AddVHost(VHost NewVHost)
        {

        }

        private void ParseBlocks()
        {
            

            string Content = File.ReadAllText(FileName);
            MatchCollection Result = Regex.Matches(Content, "<VirtualHost\\s+(?<HostName>.+):(?<Port>.+)>(?<Content>(?:(?=([^<]+))\\4|<(?!VirtualHost\\s+(?:.+):(?:.+)>))*?)<\\/VirtualHost>");

            if (Result.Count <= 0)
                return;

            Dictionary<string, Directive<string>> VHOSTS = new Dictionary<string, Directive<string>>(Result.Count);

            foreach(Match M in Result)
            {
                string VHostContent = M.Groups["Content"].Value.Trim();

                Directive<string> newVHost = new Directive<string>()
                {

                };
            }
        }

        public void ParseFile()
        {
            var lines = File.ReadLines(FileName);
            //return;
            int index = 0;
            foreach (var line in lines)
            {

                if (Regex.IsMatch(line, "<VirtualHost", RegexOptions.IgnoreCase))
                {
                    Match Result = Regex.Match(line, "<VirtualHost\\s+(?<HostName>.+):(?<Port>.+)\\s*>", RegexOptions.IgnoreCase);

                    if (Result.Success)
                    {
                        VHostTagIsOpen = true;

                        VHost NewVHost = new VHost()
                        {
                            HostName = Result.Groups["HostName"].Value,
                            Port = Result.Groups["Port"].Value,
                            StartIndex = index,
                            Content = new List<string>()
                            // HostLine = HostsParser.GetInstance()[Result.Groups["HostName"].Value]
                        };

                        LastVHost = Result.Groups["HostName"].Value;

                        VHosts.Add(Result.Groups["HostName"].Value, NewVHost);
                        continue;
                    }
                }
                else if (Regex.IsMatch(line, "</VirtualHost", RegexOptions.IgnoreCase))
                {
                    VHostTagIsOpen = false;

                    VHost TempStruct = VHosts[LastVHost];
                    TempStruct.EndIndex = index;
                    VHosts[LastVHost] = TempStruct;
                    continue;
                }
                else if (VHostTagIsOpen)
                {
                    VHost TempStruct = VHosts[LastVHost];
                    TempStruct.Content.Add(line);
                    VHosts[LastVHost] = TempStruct;
                }

                index++;
            }
        }

        public void ParseDirective(string line)
        {
            if (Regex.IsMatch(line.TrimStart(), @"</?\w+\s+[^>]*>", RegexOptions.IgnoreCase))
            {
                //Directive<List<Directive>> a = new Directive(
            }
        }
    }
}
