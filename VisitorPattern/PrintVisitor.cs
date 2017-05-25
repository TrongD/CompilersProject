using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTBuilder
{
    class PrintVisitor : IReflectiveVisitor
    {
        // This method is the key to implenting the Reflective Visitor pattern
        // in C#, using the 'dynamic' type specification to accept any node type.
        // The subsequent call to VisitNode does dynamic lookup to find the
        // appropriate Version.

        public void Visit(dynamic node)
        {
            this.VisitNode(node);
        }

        // Call this method to begin the tree printing process
        public void PrintTree(AbstractNode node, string prefix = "")
        {
            if (node == null) { 
                return;
            }
            Console.Write(prefix);
            node.Accept(this);
            if (node.TypeRef != null && node.TypeRef.GetType() != typeof(ASTBuilder.ErrorTypeDescriptor))
            {
                node.TypeRef.PrintType();
            }
            else
                Console.WriteLine();
                
            VisitChildren(node, prefix + "   ");
       }
        public void VisitChildren(AbstractNode node, String prefix)
        {
            AbstractNode child = node.Child;
            while (child != null) {             
                PrintTree(child, prefix);
                child = child.Sib;
            };
        }

        public void VisitNode(AbstractNode node)
        {
            Console.Write("<" + node.ClassName() + ">");
           
            
        }

        public void VisitNode(ModifierList node)
        {
            Console.Write("<" + node.ClassName() + ">: ");
            // Add code here to print Modifier info
            var stringEnums = node.PrintModList();
            Console.Write(string.Join(", ", stringEnums));
        }

        public void VisitNode(Identifier node)
        {
            Console.Write("<" + node.ClassName() + ">: ");
            Console.Write(node.Name);

        }
        public void VisitNode(ConstantValue node)
        {
            Console.Write("<" + node.ClassName() + ">: ");
            Console.Write(node.Value);
        }
        public void VisitNode(PrimitiveType node)
        {
            Console.Write("<" + node.ClassName() + ">: ");
            Console.Write(node.Type);
        }

        public void VisitNode(SpecialName node)
        {
            Console.Write("<" + node.ClassName() + ">: ");
            Console.Write(node.SpecialType);
        }
    }
}
