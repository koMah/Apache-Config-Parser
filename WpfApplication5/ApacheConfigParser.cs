using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WpfApplication5
{
    public class ApacheConfigParser
    {

        	private static String commentRegex = "#.*";
            private static String directiveRegex = "\\s*(#)*\\s*([^\\s]+)\\s*(.+)";
            private static String sectionOpenRegex = "\\s*(#)*\\s*<([^/\\s>]+)\\s*([^>]+)?>";
	        private static String sectionCloseRegex = "</([^\\s>]+)\\s*>";

            private static RegexOptions RXPOptions = RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled;

            private static Regex commentMatcher = new Regex(commentRegex, RXPOptions);
            private static Regex directiveMatcher = new Regex(directiveRegex, RXPOptions);
            private static Regex sectionOpenMatcher = new Regex(sectionOpenRegex, RXPOptions);
            private static Regex sectionCloseMatcher = new Regex(sectionCloseRegex, RXPOptions);

            public ApacheConfigParser()
            {
            }

            public ConfigNode Parse(string path){

                if (path == null)
                {
			        throw new ArgumentNullException("path: null");
		        }

                string[] lines = File.ReadAllLines(path);

		        ConfigNode currentNode = ConfigNode.CreateRootNode();

                foreach(string line in lines)
                {
                    /*if (commentMatcher.IsMatch(line))
                    {
                        continue;
                    }
                    else*/

                    if (sectionOpenMatcher.IsMatch(line))
                    {
                        Match Result = sectionOpenMatcher.Match(line);

                        if (Result.Success)
                        {
                            
                            bool commented = Result.Groups[1].Value != String.Empty;
                            String name = Result.Groups[2].Value;
                            String content = Result.Groups[3].Value;
                            ConfigNode sectionNode = ConfigNode.CreateChildNode(name, content, currentNode, commented);
                            currentNode = sectionNode;
                        }
                    }
                    else if (sectionCloseMatcher.IsMatch(line))
                    {
                        currentNode = currentNode.getParent();
                    }
                    else if (directiveMatcher.IsMatch(line))
                    {
                        Match Result = directiveMatcher.Match(line);

                        if (Result.Success)
                        {
                            Console.WriteLine(Result.Groups[1].Value);
                            bool commented = Result.Groups[1].Value != String.Empty;
                            String name = Result.Groups[2].Value;
                            String content = Result.Groups[3].Value;
                            ConfigNode.CreateChildNode(name, content, currentNode, commented);
                        }
                    } // TODO: Should an exception be thrown for unknown lines?
                }

		        return currentNode;
	        }
    }
}
