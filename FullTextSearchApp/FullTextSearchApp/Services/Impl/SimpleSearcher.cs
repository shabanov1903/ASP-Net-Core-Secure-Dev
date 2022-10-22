using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullTextSearchApp.Services.Impl
{
    public class SimpleSearcher
    {
        public IEnumerable<string> Search(string word, string item)
        {
            int pos = 0;
            while (true)
            {
                pos = item.IndexOf(word, pos);
                if (pos >= 0)
                {
                    yield return PrettyMatchV2(item, pos);
                }
                else
                    break;
                pos++;
            }
        }

        public IEnumerable<string> SearchV3(string word, IEnumerable<string> data)
        {
            foreach (var item in data)
            {
                int pos = 0;
                while (true)
                {
                    pos = item.IndexOf(word, pos);
                    if (pos >= 0)
                    {
                        yield return PrettyMatchV2(item, pos);
                    }
                    else
                        break;
                    pos++;
                }
            }
        }

        public string PrettyMatchV2(string text, int pos)
        {
            var start = Math.Max(0, pos - 50);
            int end = Math.Min(start + 100, text.Length - 1);
            return $"{(start == 0 ? "" : "...")}{text.Substring(start, end - start)}{(end == text.Length - 1 ? "" : "...")}";
        }
    }
}
