using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News360App
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            { 
                if (args.Length == 0)
                {
                    DoInteractive();
                }
                else
                {
                    DoFile(args[0]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
}

        private static void DoFile(string path)
        {
            var text = File.ReadAllText(path);
            var equation = new Equation(text);
            File.WriteAllText(path + ".out", equation.Canonicalize());
        }

        private static void DoInteractive()
        {
            
            do
            {
                Console.WriteLine("Enter Equation");

                var line = Console.ReadLine();
                var equation = new Equation(line);
                Console.WriteLine(equation.Canonicalize());

            } while (true);
        }
    }
}
