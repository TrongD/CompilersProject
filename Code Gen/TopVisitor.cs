using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTBuilder
{
    class GlobalVar
    {
        public static string PATH;
    }
    public class NodeVisitor : IReflectiveVisitor
    {
        public static int ldlocCount = 0;
        public static int stlocCount = 0;
        public static Dictionary<string, int> storeLocs = new Dictionary<string, int>();
        public virtual void Visit(dynamic node)
        {
            this.VisitNode(node);
        }
        public virtual void VisitNode(AbstractNode node)
        {
            AbstractNode child = node.Child;
            while (child != null)
            {
                child.Accept(this);
                node.TypeRef = child.TypeRef;
                child = child.Sib;
            };
        }
        public virtual void VisitChildren(AbstractNode node)
        {
            AbstractNode child = node.Child;
            while (child != null)
            {
                child.Accept(this);
                node.TypeRef = child.TypeRef;
                child = child.Sib;
            };
        }
    }
    class TopVisitor : NodeVisitor
    {
        public override void Visit(dynamic node)
        {
            this.VisitNode(node);
        }
        public void VisitNode(CompilationUnit node)
        {
            VisitChildren(node);
        }
        public void VisitNode(ClassDeclaration node)
        {
            Identifier name = (Identifier)node.Child.Sib;
            AbstractNode body = name.Sib;
            ClassTypeDescriptor typedescriptor = (ClassTypeDescriptor)node.AttributesRef.TypeRef;
            using (FileStream fs = File.Open(GlobalVar.PATH, FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    string s = ".class";
                    if (typedescriptor.IsPublic)
                        s += " public ";
                    if (typedescriptor.IsPrivate)
                        s += " private ";
                    if (typedescriptor.IsStatic)
                        s += " static ";
                    sw.WriteLine(s + name.Name + "\n{");   
                }
            }
            body.Accept(this);
            using (FileStream fs = File.Open(GlobalVar.PATH, FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("\n}");
                }
            }
        }
        public void VisitNode(MethodDeclaration node)
        {
            Identifier name = (Identifier)node.Child.Sib.Sib.Child;
            AbstractNode body = node.Child.Sib.Sib.Sib;
            MethodAttributes attr = (MethodAttributes)node.AttributesRef;
            List<string> mods = attr.Mods;
            AbstractNode param = null;
            if (((SignatureTypeDescriptor)attr.Signature).Parameters != null)
                param = ((SignatureTypeDescriptor)attr.Signature).Parameters.Child;
            using (FileStream fs = File.Open(GlobalVar.PATH, FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    string s = ".method ";
                    if(mods.Contains("PUBLIC"))
                        s += "public ";
                    if (mods.Contains("PRIVATE"))
                        s += "private ";
                    if (mods.Contains("STATIC"))
                        s += "static ";
                    s += attr.ReturnType.type + " ";
                    s += name.Name;
                    s += " (";
                    if (param != null)
                    {
                        while(param != null)
                        {
                            AbstractNode type = param.Child;
                            s += type.TypeRef.type;
                            param = param.Sib;
                            if (param != null)
                                s += ", ";
                        }
                    }
                    s += ")";
                    sw.WriteLine(s + "\n{");
                    sw.WriteLine(".entrypoint");
                    sw.WriteLine(".maxstack 100");
                }
            }
            MethodBodyVisitor visitor = new MethodBodyVisitor();
            body.Accept(visitor);
            using (FileStream fs = File.Open(GlobalVar.PATH, FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("\nret\n}");
                }
            }

        }
    }
    class MethodBodyVisitor : NodeVisitor
    {
        public override void Visit(dynamic node)
        {
            this.VisitNode(node);
        }
        public void VisitNode(ConstantValue node)
        {
            using (FileStream fs = File.Open(GlobalVar.PATH, FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("ldc.i4 " + node.Value);
                }
            }
        }
        public void VisitNode(StringValue node)
        {
            using (FileStream fs = File.Open(GlobalVar.PATH, FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("ldstr \"" + node.Value+"\"");
                }
            }
        }
        public void VisitNode(LocalVariableDeclarationStatement node)
        {
            string type = node.Child.TypeRef.type;
            Identifier dcls = (Identifier) node.Child.Sib.Child;
            using (FileStream fs = File.Open(GlobalVar.PATH, FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(".locals init (");
                    while (dcls != null)
                    {
                        storeLocs.Add(dcls.Name, stlocCount++);
                        sw.Write(type);
                        dcls = (Identifier)dcls.Sib;
                        if (dcls != null)
                            sw.Write(", ");
                    }
                    sw.WriteLine(", " + type + ", " + type + ")");

                }
            }
            
        }
        public void VisitNode(MethodCall node)
        {
            Identifier functionCall = (Identifier)node.Child.Child;
            AbstractNode list = node.Child.Sib;
            string s = "";
            if (node.TypeRef.GetType() == typeof(ASTBuilder.StringTypeDescriptor))
            {
                list.Accept(this);
                if (functionCall.Name == "WriteLine")
                    s += "call void [mscorlib]System.Console::WriteLine(string)";
                else if (functionCall.Name == "Write")
                    s += "call void [mscorlib]System.Console::Write(string)";
                using (FileStream fs = File.Open(GlobalVar.PATH, FileMode.Append))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(s);
                    }
                }
            }
            else if(node.TypeRef.GetType() == typeof(ASTBuilder.IntegerTypeDescriptor))
            {
                using (FileStream fs = File.Open(GlobalVar.PATH, FileMode.Append))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine("ldstr \"{0}\"");
                    }
                }
                list.Accept(this);
                using (FileStream fs = File.Open(GlobalVar.PATH, FileMode.Append))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        if(list.GetType().IsSubclassOf(typeof(ASTBuilder.ArithmeticExpression)))
                        {
                            sw.WriteLine("stloc " + (stlocCount));
                            sw.WriteLine("ldloc " + (stlocCount));
                        }
                            
                        sw.WriteLine("box int32");
                        sw.WriteLine("call void [mscorlib]System.Console::WriteLine(string, object) ");
                    }
                }

            }
        }
        public void VisitNode(ArithmeticExpression node)
        {
            AbstractNode expr1 = node.Child;
            AbstractNode expr2 = expr1.Sib;
            VisitChildren(node);
            using (FileStream fs = File.Open(GlobalVar.PATH, FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    switch (node.GetType().ToString())
                    {
                        case ("ASTBuilder.Plus"): sw.WriteLine("add"); break;
                        case ("ASTBuilder.Minus"): sw.WriteLine("sub"); break;
                        case ("ASTBuilder.Multiply"): sw.WriteLine("mul"); break;
                        case ("ASTBuilder.Division"): sw.WriteLine("div"); break;
                    }
                }
            }
           
        }
        public void VisitNode(BooleanExpression node)
        {

        }
        public void VisitNode(Assign node)
        {
            AbstractNode lhs = node.Child;
            AbstractNode rhs = lhs.Sib;
            rhs.Accept(this);
            LHSVisitor lhsVisitor = new LHSVisitor();
            lhs.Accept(lhsVisitor);
        }
        public void VisitNode(SelectionStatement node)
        {

        }
        public void VisitNode(IterationStatement node)
        {

        }
        public void VisitNode(Identifier node)
        {
            using (FileStream fs = File.Open(GlobalVar.PATH, FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("ldloc " + storeLocs[node.Name]);
                }
            }
        }
    }
    class LHSVisitor : TopVisitor
    {
        public override void Visit(dynamic node)
        {
            this.VisitNode(node);
        }
        public void VisitNode(Identifier node)
        {
            using (FileStream fs = File.Open(GlobalVar.PATH, FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("stloc " + storeLocs[node.Name]);
                }
            }
        }
        public void VisitNode(LocalVariableDeclarationStatement node)
        {

        }
        public void VisitNode(FieldVariableDeclaration node)
        {

        }
    }
}
