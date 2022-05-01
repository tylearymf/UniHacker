using System;
using System.Collections.Generic;
using System.Linq;

namespace UniHacker
{
    /// <summary>
    /// Original by Matthew Watson from https://stackoverflow.com/questions/37500629/find-byte-sequence-within-a-byte-array
    /// </summary>
    public sealed class BoyerMooreSearcher
    {
        readonly byte[] needle;
        readonly int[] charTable;
        readonly int[] offsetTable;

        public BoyerMooreSearcher(byte[] needle)
        {
            this.needle = needle;
            this.charTable = makeByteTable(needle);
            this.offsetTable = makeOffsetTable(needle);
        }

        public IEnumerable<int> Search(byte[] haystack)
        {
            if (needle.Length == 0)
                yield break;

            for (int i = needle.Length - 1; i < haystack.Length;)
            {
                int j;

                for (j = needle.Length - 1; needle[j] == haystack[i]; --i, --j)
                {
                    if (j != 0)
                        continue;

                    yield return i;
                    i += needle.Length - 1;
                    break;
                }

                i += Math.Max(offsetTable[needle.Length - 1 - j], charTable[haystack[i]]);
            }
        }

        public static List<int> FindPattern(List<byte[]> needles, byte[] haystack)
        {
            var indexes = new List<int>(needles.Count);
            foreach (var needle in needles)
            {
                var array = FindPattern(needle, haystack).ToArray();
                if (array.Length != 1)
                    break;

                indexes.Add(array[0]);
            }

            return indexes;
        }

        public static IEnumerable<int> FindPattern(byte[] needle, byte[] haystack)
        {
            return new BoyerMooreSearcher(needle).Search(haystack).ToArray();
        }

        static int[] makeByteTable(byte[] needle)
        {
            int[] table = new int[256];

            for (int i = 0; i < table.Length; ++i)
                table[i] = needle.Length;

            for (int i = 0; i < needle.Length - 1; ++i)
                table[needle[i]] = needle.Length - 1 - i;

            return table;
        }

        static int[] makeOffsetTable(byte[] needle)
        {
            int[] table = new int[needle.Length];
            int lastPrefixPosition = needle.Length;

            for (int i = needle.Length - 1; i >= 0; --i)
            {
                if (isPrefix(needle, i + 1))
                    lastPrefixPosition = i + 1;

                table[needle.Length - 1 - i] = lastPrefixPosition - i + needle.Length - 1;
            }

            for (int i = 0; i < needle.Length - 1; ++i)
            {
                int slen = suffixLength(needle, i);
                table[slen] = needle.Length - 1 - i + slen;
            }

            return table;
        }

        static bool isPrefix(byte[] needle, int p)
        {
            for (int i = p, j = 0; i < needle.Length; ++i, ++j)
                if (needle[i] != needle[j])
                    return false;

            return true;
        }

        static int suffixLength(byte[] needle, int p)
        {
            int len = 0;

            for (int i = p, j = needle.Length - 1; i >= 0 && needle[i] == needle[j]; --i, --j)
                ++len;

            return len;
        }
    }
}
