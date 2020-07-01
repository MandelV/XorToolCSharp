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
        public IEnumerable<KeyValuePair<string, string>> breakXor(string cipher, int sizeChunk, double ic = 0.07)
        {
            if (cipher == string.Empty || cipher == null || sizeChunk <= 0) yield break;

            var blocks = CryptoTools.DivideText(cipher, sizeChunk).ToList();
            var trans = CryptoTools.Transposed(blocks);

            //will contains the potential key used to encrypt the message
            string keys = "";

            foreach (var block in trans)
            {
                List<char> blockKeys = new List<char>();
                foreach (char ch in "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
                {
                    string text = Xor(block, char.ToString(ch));
   
                    if (!blockKeys.Contains(ch) && text.IsPrintable() && text.Ic() > ic - 0.01)//If the block is printable and its index is more than ic-0.01 we add the key into blockKeys
                    {
                        blockKeys.Add(ch);            
                    }
                }
                //We order the letter to make the permutation process easier.
                keys +=  string.Concat( blockKeys.OrderBy(c => c).Select(a => a).ToArray());
            }
            keys = string.Concat(keys.GroupBy(c => c).Select(c => char.ToString(c.Key)).ToArray());

            //Console.WriteLine("Keys : {0} -  {1}", keys, keys.Length);

            if (keys.Length == 0)
                yield break;
            

            //For each permutation of keys
            foreach (var elem in CryptoTools.GetPermutationsWithRept(keys.ToList(), sizeChunk))
            {
                //Merge char eachothers
                string key = string.Concat(elem.ToArray());

                string plain = Xor(cipher, key);
                var index = plain.Ic();

                if (plain.IsPrintable() && index >= ic)
                {
                    yield return new KeyValuePair<string, string>(key, plain);
       
                }
            }
        }
    }
}
