using Common.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setup
{
    public class CmdParameters
    {
        public string Action { get; private set; }
        public string InstallationPath { get; set; }
        public string ChromiumExePath { get; set; }
        public string PreInstallUrl { get; set; }
        public string StartWithWindows { get; set; }
        public string TrakingId { get; set; }

        public CmdParameters(string[] args)
        {
            if (args.Length > 0)
            {
                try
                {
                    Action = args[0].Substring(2, args[0].Length - 2);
                    if (args.Length > 1)
                    {
                        InstallationPath = GetArgValue(args[1]);
                        ChromiumExePath = GetArgValue(args[2]);
                        PreInstallUrl = GetArgValue(args[3]);
                        StartWithWindows = GetArgValue(args[4]);
                        TrakingId = GetArgValue(args[5]);
                    }
                }
                catch (Exception e)
                {
                    StaticLogger.Logger.Error(e);
                }
            }
        }

        private string GetArgValue(string fullArg)
        {
            string result = "";
            int i = 0;
            for (; i < fullArg.Length; i++)
            {
                if (fullArg[i].Equals('='))
                {
                    i++;
                    break;
                }
            }
            for (; i < fullArg.Length; i++)
            {
                result += fullArg[i];
            }
            return result;
        }
    }
}
