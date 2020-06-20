using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
 

namespace xortool
{
    class Program
    {

        static void Main(string[] args)
        {
            //if (args.Length == 0 || args.Length > 1) return;
            XorBreaker xorBreaker = new XorBreaker();
            string text = File.ReadAllText("test.txt");
            Console.WriteLine(text.Ic());

            string key = "ZGHT";
            string cipher = xorBreaker.Xor(text, key);

            var result = xorBreaker.breakXor(cipher, key.Length, 0.059);

            Console.WriteLine("Key : " + result.key + " - in : " + result.timeToBreak + " ms");
        }
    }
}
