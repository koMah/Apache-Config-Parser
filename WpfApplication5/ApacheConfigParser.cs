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
	        private static String directiveRegex = "([^\\s]+)\\s*(.+)";
	        private static String sectionOpenRegex = "<([^/\\s>]+)\\s*([^>]+)?>";
	        private static String sectionCloseRegex = "</([^\\s>]+)\\s*>";

            private static Regex commentMatcher = new Regex(commentRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
            private static Regex directiveMatcher = new Regex(directiveRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
            private static Regex sectionOpenMatcher = new Regex(sectionOpenRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
            private static Regex sectionCloseMatcher = new Regex(sectionCloseRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

            public ApacheConfigParser()
            {
            }

            public ConfigNode Parse(string path){

                if (path == null)
                {
			        throw new ArgumentNullException("path: null");
		        }

                string[] lines = File.ReadAllLines(path);

		        ConfigNode currentNode = ConfigNode.createRootNode();

                foreach(string line in lines)
                {
                    if (commentMatcher.IsMatch(line))
                    {
                        continue;
                    }
                    else if (sectionOpenMatcher.IsMatch(line))
                    {
                        Match Result = sectionOpenMatcher.Match(line);

                        if (Result.Success)
                        {
                            String name = Result.Groups[1].Value;
                            String content = Result.Groups[2].Value;
                            ConfigNode sectionNode = ConfigNode.createChildNode(name, content, currentNode);
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
                            String name = Result.Groups[1].Value;
                            String content = Result.Groups[2].Value;
                            ConfigNode.createChildNode(name, content, currentNode);
                        }
                    } // TODO: Should an exception be thrown for unknown lines?
                }

		        return currentNode;
	        }
    }
}
