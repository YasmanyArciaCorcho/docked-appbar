using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.String
{
    public class CmdStringRead : ICmdStringRead
    {
        Dictionary<string, string> _data;

        public CmdStringRead(string[] args)
        {
            Initialize(args);
        }

        public string GetValue(string paramenterName)
            => _data[paramenterName];

        public bool HasKey(string parameterName)
            => _data.ContainsKey(parameterName);

        private void Initialize(string[] args)
        {
            _data = new Dictionary<string, string>();

            foreach (var arg in args)
            {
                var (outkey, outvalue) = GetArgValue(arg);
                _data.Add(outkey,outvalue);
            }
        }

        private (string outkey, string outvalue) GetArgValue(string arg)
        {
            string key = "";
            string value = "";
            int i = 0;
            for (; i < arg.Length; i++)
            {
                if (arg[i].Equals('='))
                {
                    i++;
                    break;
                }
                key += arg[i];
            }
            for (; i < arg.Length; i++)
            {
                value += arg[i];
            }
            return (outkey: key, outvalue: value);
        }
    }
}
