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
            Console.Write("Enter FileName here:    ");
            name = Console.ReadLine();
            Console.WriteLine("Parsing file " + name);
            parser.Parse(name);
            Console.WriteLine("Parsing complete");
            Console.Read();

        }
    }
}
