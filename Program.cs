﻿using System;
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

            string key = "IYPJ";
            string cipher = xorBreaker.Xor(text, key);

            var result = xorBreaker.breakXor(cipher, key.Length, 0.057);
            
            Console.WriteLine("in : " + result.timeToBreak + " ms");
            int i = 0;
            foreach (var kp in result.keyPlains)
            {
                Console.WriteLine(++i + ". " + kp.Key + " - " + kp.Value.Ic());
            }
        }
    }
}
