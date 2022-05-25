using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataStructures.KMP
{
    public class KMPSearch
    {
        public bool Contains(string pattern, string text)
        {
            int M = pattern.Length;
            int N = text.Length;

            int[] lps = new int[M];
            int j = 0;

            ComputeLPSArray(pattern, M, lps);

            int i = 0;
            while (i < N)
            {
                if (pattern[j] == text[i])
                {
                    j++;
                    i++;
                }
                if (j == M)
                {
                    j = lps[j - 1];
                    return true;
                }
                else if (i < N && pattern[j] != text[i])
                {
                    if (j != 0)
                        j = lps[j - 1];
                    else
                        i = i + 1;
                }
            }
            return false;
        }

        private void ComputeLPSArray(string pat, int M, int[] lps)
        {
            int len = 0;
            int i = 1;
            lps[0] = 0;

            while (i < M)
            {
                if (pat[i] == pat[len])
                {
                    len++;
                    lps[i] = len;
                    i++;
                }
                else
                {
                    if (len != 0)
                    {
                        len = lps[len - 1];
                    }
                    else
                    {
                        lps[i] = len;
                        i++;
                    }
                }
            }
        }
    }
}
