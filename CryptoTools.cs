using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace xortool
{
    /// <summary>
    /// Set of tools to gathering information about given text.
    /// 
    /// </summary>
    public static class CryptoTools
    {
        /// <summary>
        /// Format the message to remove accentuate characters.
        /// </summary>
        /// <param name="textToFormat"></param>
        /// <returns>formated message</returns>
        private static string FormatMessage(string textToFormat)
        {
            string text = textToFormat.ToLower();
            string msg = text.Replace(" ", "");
            /*foreach(char ch in "()<>\"«»[]{},?.!:;0123456789'-=_+*")
            {
                msg = msg.Replace(char.ToString(ch), "");
            }*/
            //éèëêàôûâêœ
            msg = msg.Replace("é", "e");
            msg = msg.Replace("è", "e");
            msg = msg.Replace("ë", "e");
            msg = msg.Replace("ê", "e");
            msg = msg.Replace("à", "a");
            msg = msg.Replace("ä", "a");
            msg = msg.Replace("â", "a");
            msg = msg.Replace("œ", "oe");
            return msg;
        }
        /// <summary>
        /// Will calculates the Index of coincidence of the given text
        /// (Extension)
        /// </summary>
        /// <param name="text">Text to calculate its index</param>
        /// <returns>Index of the text</returns>
        public static double Ic(this string text)
        {
            string msg = FormatMessage(text);
            int n = msg.Length;
            string alpha = "abcdefghijklmnopqrstuvwxyz";

            double ic = 0.0;
            foreach (char c in alpha)
            {
                int numberAlpha = msg.Count(ct => ct == c);

                double num = numberAlpha * (numberAlpha - 1);
                double den = n * (n - 1);

                ic += num / den;

            }
            return ic;
        }


        /// <summary>
        /// Will determine if a string contains printable char or not
        /// (extension)
        /// </summary>
        /// <param name="text"></param>
        /// <returns>true if string contains printable char and false otherwise</returns>
        public static bool IsPrintable(this string text)
        {
            var nonRenderingCategories = new UnicodeCategory[] {
                    UnicodeCategory.Control,
                    UnicodeCategory.OtherNotAssigned,
                    UnicodeCategory.Surrogate };

            bool pritable = true;

            foreach (char c in text)
            {
                var isPrintable = char.IsWhiteSpace(c) || !nonRenderingCategories.Contains(char.GetUnicodeCategory(c));
                if (!isPrintable) return false;
            }

            return pritable;
        }

        /// <summary>
        /// Will generate all permutation of the given list with a specific length
        /// </summary>
        /// <typeparam name="T">Type of object to be permuted</typeparam>
        /// <param name="list">Object will be permuted</param>
        /// <param name="length">Size of each permutation</param>
        /// <returns>Permutations</returns>
        public static IEnumerable<IEnumerable<T>> GetPermutationsWithRept<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetPermutationsWithRept(list, length - 1)
                .SelectMany(t => list,
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }
        
        /// <summary>
        /// Divides a text into chunk
        /// </summary>
        /// <param name="text">text you want to split</param>
        /// <param name="chunkSize">size of a chunk</param>
        /// <returns>Divided text</returns>
        public static IEnumerable<string> DivideText(string text, int chunkSize)
        {
            if (text == null || text == string.Empty || chunkSize <= 0) throw new Exception("text or chunkSize wrong");

            return Enumerable.Range(0, text.Length / chunkSize).Select(i => text.Substring(i * chunkSize, chunkSize));
        }

        /// <summary>
        /// Will transposed each block.
        /// Example :
        /// AAAA BBBB CCCC DDDD
        /// will produce :
        /// ABCD ABCD ABCD ABCD
        /// </summary>
        /// <param name="blocks">Block you want to transposed</param>
        /// <returns>Transposed blocks</returns>
        public static List<string> Transposed(List<string> blocks)
        {
            List<string> transposed = new List<string>();

            int blockSize = blocks[0].Length;

            for (int i = 0; i < blockSize; i++)
            {
                string tmp = "";
                for (int j = 0; j < blocks.Count; j++)
                {
                    tmp += blocks[j][i];

                }

                transposed.Add(tmp);
            }

            return transposed;
        }


    }
}
