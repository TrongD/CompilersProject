using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new TCCLParser();
            string name = "";
            while (true)
            {
                Console.Write("Enter a file name: ");
                name = Console.ReadLine();
                Console.WriteLine("Parsing file " + name);
                parser.Parse(name + ".txt");
                Console.WriteLine("Parsing complete");
            }
        }
    }
}
