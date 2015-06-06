using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApplication5
{
    [Flags]
    public enum ApacheParserOptions
    {
        None = 0,
        ParseIncludedFiles = 1,
        ParseWithComments = 2
    }

    public class ApacheConfigParser
    {
        private static String CommentPattern = "#.*";
        private static String DirectivePattern = "([^\\s]+)\\s*(.+)";
       // private static String SectionOpenPattern = "^(?:\\s*#?\\s*)?<([^/\\s>]+)\\s*([^>]+)?>\\s*$";
        private static String SectionOpenPattern = "^(?:[ \\t]*#?[ \\t]*)<([^/\\s>]+)\\s*([^>]+)?>\\s*$";
        private static String SectionClosePattern = "</([^\\s>]+)\\s*>";

        private static Regex CommentRegex = new Regex(CommentPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex DirectiveRegex = new Regex(DirectivePattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex SectionOpenRegex = new Regex(SectionOpenPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex SectionCloseRegex = new Regex(SectionClosePattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

        public ConfigNode RootNode;

        public ApacheConfigParser()
        {
        }

        public ApacheConfigParser(string ConfPath, ApacheParserOptions Options = ApacheParserOptions.None)
        {
            Parse(ConfPath, Options);
        }

        public ConfigNode Parse(string ConfPath, ApacheParserOptions Options = ApacheParserOptions.None)
        {
            
            string ApacheRootConfDir = Directory.GetParent(Path.GetDirectoryName(ConfPath)).FullName;

            if (ConfPath == null)
            {
			    throw new ArgumentNullException("path: null");
		    }

            if(!File.Exists(ConfPath))
            {
                throw new FileNotFoundException( String.Format( "{0} file not found" ) );
            }

            bool ParseComments = Options.HasFlag(ApacheParserOptions.ParseWithComments);
            bool ParseIncludes = Options.HasFlag(ApacheParserOptions.ParseIncludedFiles);

            string[] ConfLines = File.ReadAllLines(ConfPath);
            string[] DirectivesList =  Properties.Resources.SingleDirectives.Split('\n');
            string[] SectionDirectivesList = Properties.Resources.SectionDirectives.Split('\n');

		    ConfigNode CurrentNode = ConfigNode.CreateRootNode();

            foreach(string ConfLine in ConfLines)
            {
                bool IsComment = ConfLine.Trim().StartsWith("#");

                if (SectionOpenRegex.IsMatch(ConfLine))
                {
                    Match Result = SectionOpenRegex.Match(ConfLine);

                    if (Result.Success)
                    {
                        String DirectiveName = Result.Groups[1].Value.Replace("#", String.Empty);
                        if(SectionDirectivesList.Contains(DirectiveName))
                        {
                            Console.WriteLine(DirectiveName);
                            String DirectiveContent = Result.Groups[2].Value;
                            ConfigNode SectionNode = ConfigNode.CreateChildNode(DirectiveName, DirectiveContent, CurrentNode, IsComment);
                            CurrentNode = SectionNode;
                        }
                    }
                }
                else if (SectionCloseRegex.IsMatch(ConfLine))
                {
                    Match Result = SectionCloseRegex.Match(ConfLine);
                    if (Result.Success)
                    {
                        String DirectiveName = Result.Groups[1].Value.Replace("#", String.Empty);
                        if (SectionDirectivesList.Contains(DirectiveName) && CurrentNode != null)
                        {
                            CurrentNode = CurrentNode.getParent();
                        }
                    }
                }
                else if (DirectiveRegex.IsMatch(ConfLine))
                {
                    Match Result = DirectiveRegex.Match(ConfLine);

                    if (Result.Success)
                    {
                        String DirectiveName = Result.Groups[1].Value.Replace("#", String.Empty);
                        if (DirectivesList.Contains(DirectiveName))
                        {
                            
                            String DirectiveContent = Result.Groups[2].Value;
                            ConfigNode.CreateChildNode(DirectiveName, DirectiveContent, CurrentNode, IsComment);
                        } 

                            // TODO: Manage comments
                            // Console.WriteLine(Result.Groups[0].Value);

                    }
                }
            }

		    return CurrentNode;
	    }
    }
}
