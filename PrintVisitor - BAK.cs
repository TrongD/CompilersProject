using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTBuilder
{
    class PrintVisitor : NodeVisitor
    {
        public void PreOrderWalk(AbstractNode node, string prefix = "")
        {
            //string s = @"├│└─";
            if (node == null) { 
                return;
            }

            bool isLastChild = (node.Sib == null);

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(prefix);
            Console.Write(isLastChild ? "└─ " : "├─ ");
            Console.ResetColor();
            node.accept(this);
            PreOrderWalk(node.Child, prefix + (isLastChild ? "   " : "│  "));

            if (!isLastChild)
                PreOrderWalk(node.Sib, prefix);
        }
        public void Visit(AbstractNode node)
        {
            Console.WriteLine("<" + node.ClassName() + ">");
        }

        public void Visit(Modifiers node)
        {
            Console.Write("<" + node.ClassName() + ">: ");
            var stringEnums = node.ModifierTokens.Select(x => x.ToString());
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(string.Join(", ", stringEnums));
            Console.ResetColor();
        }

        public void Visit(Identifier node)
        {
            Console.Write("<" + node.ClassName() + ">: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(node.Name);
            Console.ResetColor();
         //   Console.WriteLine("In Visit for Identifier. Name value is " + node.Name);

        }
        public void Visit(PrimitiveType node)
        {
            Console.Write("<" + node.ClassName() + ">: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(node.Type);
            Console.ResetColor();
        }
        public void Visit(Expression node)
        {
            Console.Write("<" + node.ClassName() + ">: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(node.ExprType);
            Console.ResetColor();
        }

        public void Visit(SpecialName node)
        {
            Console.Write("<" + node.ClassName() + ">: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(node.SpecialType);
            Console.ResetColor();
        }
    }
}
