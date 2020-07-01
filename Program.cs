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
            string text = File.ReadAllText("D/file_074.txt");


            var result = xorBreaker.breakXor(text, 4, 0.06);
            
            foreach (var kp in result)
            {
                Console.WriteLine( kp.Key + " - " + kp.Value.Ic());
            }
        }
    }
}
