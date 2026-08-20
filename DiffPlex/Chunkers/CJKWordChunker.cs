using System.Collections.Generic;

namespace DiffPlex.Chunkers
{

    /// <summary>
    /// Support CJK word (characters)
    /// </summary>
    public class CJKWordChunker : IChunker
    {
        /// <summary>
        /// Gets the default singleton instance of the chunker.
        /// </summary>
        public static CJKWordChunker Instance { get; } = new CJKWordChunker();

        IReadOnlyList<string> IChunker.Chunk(string text)
        {
            var result = WordChunker.Instance.Chunk(text);
            var retVal = new List<string>();

            //  Further breakdown CJK characters
            foreach (var word in result)
            {
                int iStart = 0;
                for (int i = 0; i < word.Length; i++)
                {
                    var ch = word[i];
                    if (ch >= 0x4E00 && ch <= 0x9FFF)   // CJK
                    {
                        var len = i - iStart;
                        if (len > 0)
                        {   //  Some non-Cjk
                            retVal.Add(word.Substring(iStart, len));
                        }
                        retVal.Add($"{ch}");
                        iStart = i + 1;
                    }
                }
                if (word.Length > iStart)
                {
                    retVal.Add(word.Substring(iStart));
                }
            }
            return retVal;
        }
    }
}
