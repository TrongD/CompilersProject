using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ASTBuilder
{
    internal partial class TCCLParser
    {
        public TCCLParser() : base(null) { }

        public void Parse(string filename)
        {
            GlobalVar.PATH = Directory.GetCurrentDirectory() +"\\"+ filename + ".il";
            this.Scanner = new TCCLScanner(File.OpenRead(filename + ".txt"));
            this.Parse();
            //   PrintTree();
            DoSemantics();
            GenerateCode(filename);
            PrintTree();
        }
        public void Parse(Stream strm)
        {
            this.Scanner = new TCCLScanner(strm);
            this.Parse();
            //   PrintTree();
            DoSemantics();
            PrintTree();
        }
        public void PrintTree()
        {
            PrintVisitor visitor = new PrintVisitor();
            Console.WriteLine("Starting to print AST ");
            visitor.PrintTree(CurrentSemanticValue);
        }

        public void DoSemantics()
        {
            SemanticsVisitor visitor = new SemanticsVisitor();
            Console.WriteLine("Starting semantic checking");
            visitor.CheckSemantics(CurrentSemanticValue);

        }
        public void GenerateCode(string filename)
        {
            TopVisitor visitor = new TopVisitor();
            Console.WriteLine("Starting code generation");
            //Path found in TopVisitor
            if (File.Exists(GlobalVar.PATH))
            {
                File.Delete(GlobalVar.PATH);
            }
            using (FileStream fs = File.Create(GlobalVar.PATH))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("//Generated Code in CIL");
                    sw.WriteLine(".assembly extern mscorlib {} ");
                    sw.WriteLine(".assembly " + filename + " {}");
                }
            }
            visitor.Visit(CurrentSemanticValue);

        }
        //
        // Now the node factory methods
        //
        /*****************************/
        /*    Ternary Type Nodes     */
        /*****************************/
        public static AbstractNode MakeTernary(AbstractNode first, AbstractNode second, AbstractNode third)
        {
            return new Ternary(first, second, third);

        }
        public static AbstractNode MakeClassDeclaration(AbstractNode mods, AbstractNode id, AbstractNode body)
        {
            return new ClassDeclaration(mods, id, body);
        }
        public static AbstractNode MakeStructDeclaration(AbstractNode mods, AbstractNode id, AbstractNode body)
        {
            return new StructDeclaration(mods, id, body);
        }
        public static AbstractNode MakeFieldVariableDeclaration(AbstractNode mods, AbstractNode type, AbstractNode dcls)
        {
            return new FieldVariableDeclaration(mods, type, dcls);
        }
        public static AbstractNode MakeSelectionStatement(AbstractNode expr, AbstractNode ifStmt, AbstractNode elseStmt = null)
        {
            return new SelectionStatement(expr, ifStmt, elseStmt);
        }
        /****************************/
        /*   Binary Type Nodes      */
        /****************************/
        public static AbstractNode MakeBinary(AbstractNode name, AbstractNode pList)
        {
            return new Binary(name, pList);
        }
        public static AbstractNode MakeMethodDeclarator(AbstractNode name, AbstractNode pList = null)
        {
            return new MethodDeclarator(name, pList);
        }
        public static AbstractNode MakeParameter(AbstractNode type, AbstractNode name)
        {
            return new Parameter(type, name);
        }
        public static AbstractNode MakeLocalVariableDeclarationStatement(AbstractNode type, AbstractNode dcls)
        {
            return new LocalVariableDeclarationStatement(type, dcls);
        }
        public static AbstractNode MakeIterationStatement(AbstractNode expr, AbstractNode stmt)
        {
            return new IterationStatement(expr, stmt);
        }
        public static AbstractNode MakeMethodCall(AbstractNode methodRef, AbstractNode list = null)
        {
            return new MethodCall(methodRef, list);
        }
        public static AbstractNode MakeExpression(AbstractNode expr1, AbstractNode expr2, int t)
        {
            switch (t)
            {
                case (int)Token.EQUALS: return new Assign(expr1, expr2);
                case (int)Token.OP_LOR: return new ShortCircuitOR(expr1, expr2);
                case (int)Token.OP_LAND: return new ShortCircuitAND(expr1,expr2);
                case (int)Token.PIPE: return new Pipe(expr1, expr2);
                case (int)Token.HAT: return new Hat(expr1, expr2);
                case (int)Token.AND: return new And(expr1, expr2);
                case (int)Token.OP_EQ: return new Equals(expr1, expr2);
                case (int)Token.OP_NE: return new NotEquals(expr1, expr2);
                case (int)Token.OP_GT: return new GreaterThan(expr1, expr2);
                case (int)Token.OP_LT: return new LessThan(expr1, expr2);
                case (int)Token.OP_LE: return new LessThanOrEquals(expr1, expr2);
                case (int)Token.OP_GE: return new GreaterThanOrEquals(expr1, expr2);
                case (int)Token.PLUSOP: return new Plus(expr1, expr2);
                case (int)Token.MINUSOP: return new Minus(expr1, expr2);
                case (int)Token.ASTERISK: return new Multiply(expr1, expr2);
                case (int)Token.RSLASH: return new Division(expr1, expr2);
                case (int)Token.PERCENT: return new Remainder(expr1, expr2);
                default: return new Binary(expr1, expr2);
            }
        }
        /****************************/
        /*    Unary Type Nodes      */
        /****************************/
        public static AbstractNode MakeUnary(AbstractNode name)
        {
            return new Unary(name);
        }
        public static AbstractNode MakeFieldDeclarations(AbstractNode child)
        {
            return new FieldDeclarations(child);
        }
        public static AbstractNode MakeFieldVariableDeclarators(AbstractNode child)
        {
            return new FieldVariableDeclarators(child);
        }
        public static AbstractNode MakeParameterList(AbstractNode child)
        {
            return new ParameterList(child);
        }
        public static AbstractNode MakeQualifiedName(AbstractNode child)
        {
            return new QualifiedName(child);
        }
        public static AbstractNode MakeLocalVariableDeclarationAndStatement(AbstractNode child)
        {
            return new LocalVariableDeclarationAndStatement(child);
        }
        public static AbstractNode MakeLocalVariableDeclarators(AbstractNode child)
        {
            return new LocalVariableDeclarators(child);
        }
        /****************************/
        /*    Unique Type Nodes     */
        /****************************/
        public static AbstractNode MakeList(int t)
        {
            return new ModifierList(t);
        }
        public static AbstractNode MakeSpecialName(int t)
        {
            return new SpecialName(t);
        }
        public static AbstractNode MakeMethodDeclaration(AbstractNode mods, AbstractNode type, AbstractNode dcl, AbstractNode body)
        {
            return new MethodDeclaration(mods, type, dcl, body);
        }
        public static AbstractNode MakePrimitiveType(int t)
        {
            return new PrimitiveType(t);
        }

    }
}
