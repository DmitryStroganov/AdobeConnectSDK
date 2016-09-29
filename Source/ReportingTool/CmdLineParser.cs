/* Command Line Parser (C) Richard Lopes 
 * http://www.codeproject.com/KB/recipes/command_line.aspx
 */

using System;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace ReportingTool
{
    internal class CmdLineParams
    {
        // Fields
        private StringDictionary Parameters = new StringDictionary();

        // Methods
        public CmdLineParams(string[] Args)
        {
            Regex regex = new Regex("^-{1,2}|^/|=|:", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex regex2 = new Regex("^['\"]?(.*?)['\"]?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            string key = null;
            foreach (string str2 in Args)
            {
                string[] strArray = regex.Split(str2, 3);
                switch (strArray.Length)
                {
                    case 1:
                        if (key != null)
                        {
                            if (!this.Parameters.ContainsKey(key))
                            {
                                strArray[0] = regex2.Replace(strArray[0], "$1");
                                this.Parameters.Add(key, strArray[0]);
                            }
                            key = null;
                        }
                        break;

                    case 2:
                        if ((key != null) && !this.Parameters.ContainsKey(key))
                        {
                            this.Parameters.Add(key, "true");
                        }
                        key = strArray[1];
                        break;

                    case 3:
                        if ((key != null) && !this.Parameters.ContainsKey(key))
                        {
                            this.Parameters.Add(key, "true");
                        }
                        key = strArray[1];
                        if (!this.Parameters.ContainsKey(key))
                        {
                            strArray[2] = regex2.Replace(strArray[2], "$1");
                            this.Parameters.Add(key, strArray[2]);
                        }
                        key = null;
                        break;
                }
            }
            if ((key != null) && !this.Parameters.ContainsKey(key))
            {
                this.Parameters.Add(key, "true");
            }
        }

        // Properties
        public string this[string Param]
        {
            get
            {
                return this.Parameters[Param];
            }
        }
    }
}

