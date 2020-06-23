using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace xortool
{
    public class XorBreaker
    {
        /// <summary>
        /// Will Xor the text by the given key
        /// </summary>
        /// <param name="text">Text to encrypt</param>
        /// <param name="key">Key to encrypt the text</param>
        /// <returns>ciphertext</returns>
        public string Xor(string text, string key)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                char cipherChar = (char)(text[i] ^ key[i % key.Length]);
                stringBuilder.Append(cipherChar);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Will break a cipher code by using transposition and Coincidence index
        /// IC : ~0.06 = english ; ~0.07 = French
        /// </summary>
        /// <param name="cipher">the text uncrypted</param>
        /// <param name="sizeChunk">size of you key</param>
        /// <param name="ic">threshold</param>
        /// <returns>Tuple (string, string) of key and plaintext</returns>
        public (Dictionary<string, string> keyPlains, long timeToBreak) breakXor(string cipher, int sizeChunk, double ic = 0.07)
        {
            //To calculate the time to break
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var blocks = CryptoTools.DivideText(cipher, sizeChunk).ToList();
            var trans = CryptoTools.Transposed(blocks);
            Dictionary<string, string> keyPlains = new Dictionary<string, string>();



            //will contains the potential key used to encrypt the message
            string keys = "";

            foreach (var block in trans)
            {
                Dictionary<string, int> blockKeys = new Dictionary<string, int>();
                foreach (char ch in "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
                {

                    string text = Xor(block, char.ToString(ch));

                    if (text.IsPrintable() && text.Ic() >= ic - 0.01)//If the block is printable and its index is more than ic-0.01 we add the key into blockKeys
                    {
                        var c = char.ToString(ch);
                        if (blockKeys.ContainsKey(c))
                        {
                            blockKeys[c] += 1;
                        }
                        else
                        {
                            blockKeys.Add(c, 1);
                        }

                    }
                }
                //We order the letter to make the permutation process easier.
                keys +=  string.Join("", blockKeys.OrderBy(c => c.Value).Select(a => a.Key).ToArray());

            }
            keys = string.Join("", keys.GroupBy(c => c).Select(c => char.ToString(c.Key)).ToArray());

            Console.WriteLine("Keys : {0} -  {1}", keys, keys.Length);

            if (keys.Length == 0)
            {
                sw.Stop();
                return (keyPlains, sw.ElapsedMilliseconds);
            }

            //For each permutation of keys
            foreach (var elem in CryptoTools.GetPermutationsWithRept(keys.ToList(), sizeChunk))
            {
                //Merge char eachothers
                string key = string.Join("", elem.ToArray());

                string plain = Xor(cipher, key);
                var index = plain.Ic();

                if (index >= ic)
                {
                    keyPlains.Add(key, plain);
                }
            }
            sw.Stop();
            return (keyPlains, sw.ElapsedMilliseconds);
        }
    }
}
