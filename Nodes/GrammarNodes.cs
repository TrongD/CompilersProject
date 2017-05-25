using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASTBuilder;

namespace ASTBuilder
{
    enum PToken
    {
        INT = 10, VOID = 24, BOOLEAN = 41, STATIC = 4,
        NULL = 9, THIS = 17, PUBLIC = 20, PRIVATE = 33,
        LITERAL = 52
    };
    // Substitute your GrammarNode.cs file for this one
    public class CompilationUnit : AbstractNode
    {
        // just for the compilation unit because it's the top node
        //public override AbstractNode LeftMostSibling => this;
        public override AbstractNode Sib => null;

        public CompilationUnit(AbstractNode classDecl)
        {
            adoptChildren(classDecl);
        }

    }
    /*****************************/
    /*    Ternary Type Nodes     */
    /*****************************/
    public class Ternary : AbstractNode
    {
        public Ternary(AbstractNode first, AbstractNode second, AbstractNode third)
        {
            this.adoptChildren(first);
            this.adoptChildren(second);
            this.adoptChildren(third);
        }
    }
    public class ClassDeclaration : Ternary
    {
        public ClassDeclaration(AbstractNode mods, AbstractNode id, AbstractNode body) : base(mods, id, body) { }
    }
    public class StructDeclaration : Ternary
    {
        public StructDeclaration(AbstractNode mods, AbstractNode id, AbstractNode body) : base(mods, id, body) { }
    }
    public class FieldVariableDeclaration : Ternary
    {
        public FieldVariableDeclaration(AbstractNode mods, AbstractNode type, AbstractNode dcls) : base(mods, type, dcls) { }
    }
    public class SelectionStatement : Ternary
    {
        public SelectionStatement(AbstractNode expr, AbstractNode ifStmt, AbstractNode elseStmt) : base(expr, ifStmt, elseStmt) { }
    }

    /****************************/
    /*   Binary Type Nodes      */
    /****************************/
    public class Binary : AbstractNode
    {
        public Binary(AbstractNode name, AbstractNode pList)
        {
            this.adoptChildren(name);
            this.adoptChildren(pList);
        }
    }
    public class MethodDeclarator : Binary
    {
        public MethodDeclarator(AbstractNode name, AbstractNode paramList) : base(name, paramList) { }
    }
    public class Parameter : Binary
    {
        public Parameter(AbstractNode type, AbstractNode name) : base(type, name) { }
    }
    public class LocalVariableDeclarationStatement : Binary
    {
        public LocalVariableDeclarationStatement(AbstractNode type, AbstractNode dcls) : base(type, dcls) { }
    }
    public class IterationStatement : Binary
    {
        public IterationStatement(AbstractNode expr, AbstractNode stmt) : base(expr, stmt) { }
    }
    public class MethodCall : Binary
    {
        public MethodCall(AbstractNode methodRef, AbstractNode list) : base(methodRef, list) { }
    }
    public class BooleanExpression : Binary
    {
        public BooleanExpression(AbstractNode expr1, AbstractNode expr2) : base(expr1, expr2) { }
    }
    public class ArithmeticExpression : Binary
    {
        public ArithmeticExpression(AbstractNode expr1, AbstractNode expr2) : base(expr1, expr2) { }
    }
    public class Assign : Binary
    {
        public Assign(AbstractNode expr1, AbstractNode expr2) : base(expr1, expr2) { }
    }
    public class ShortCircuitOR : BooleanExpression
    {
        public ShortCircuitOR(AbstractNode expr1, AbstractNode expr2) : base(expr1, expr2) { }
    }
    public class ShortCircuitAND : BooleanExpression
    {
        public ShortCircuitAND(AbstractNode expr1, AbstractNode expr2) : base(expr1, expr2) { }
    }
    public class Pipe : BooleanExpression
    {
        public Pipe(AbstractNode expr1, AbstractNode expr2) : base(expr1, expr2) { }
    }
    public class Hat : BooleanExpression
    {
        public Hat(AbstractNode expr1, AbstractNode expr2) : base(expr1, expr2) { }
    }
    public class And : BooleanExpression
    {
        public And(AbstractNode expr1, AbstractNode expr2) : base(expr1, expr2) { }
    }
    public class Equals : BooleanExpression
    {
        public Equals(AbstractNode expr1, AbstractNode expr2) : base(expr1, expr2) { }
    }
    public class NotEquals : BooleanExpression
    {
        public NotEquals(AbstractNode expr1, AbstractNode expr2) : base(expr1, expr2) { }
    }
    public class GreaterThan : BooleanExpression
    {
        public GreaterThan(AbstractNode expr1, AbstractNode expr2) : base(expr1, expr2) { }
    }
    public class LessThan : BooleanExpression
    {
        public LessThan(AbstractNode expr1, AbstractNode expr2) : base(expr1, expr2) { }
    }
    public class LessThanOrEquals : BooleanExpression
    {
        public LessThanOrEquals(AbstractNode expr1, AbstractNode expr2) : base(expr1, expr2) { }
    }
    public class GreaterThanOrEquals : BooleanExpression
    {
        public GreaterThanOrEquals(AbstractNode expr1, AbstractNode expr2) : base(expr1, expr2) { }
    }
    public class Plus : ArithmeticExpression
    {
        public Plus(AbstractNode expr1, AbstractNode expr2) : base(expr1, expr2) { }
    }
    public class Minus : ArithmeticExpression
    {
        public Minus(AbstractNode expr1, AbstractNode expr2) : base(expr1, expr2) { }
    }
    public class Multiply : ArithmeticExpression
    {
        public Multiply(AbstractNode expr1, AbstractNode expr2) : base(expr1, expr2) { }
    }
    public class Division : ArithmeticExpression
    {
        public Division(AbstractNode expr1, AbstractNode expr2) : base(expr1, expr2) { }
    }
    public class Remainder : ArithmeticExpression
    {
        public Remainder(AbstractNode expr1, AbstractNode expr2) : base(expr1, expr2) { }
    }
    /****************************/
    /*    Unary Type Nodes      */
    /****************************/
    public class Unary : AbstractNode
    {
        public Unary(AbstractNode name)
        {
            this.adoptChildren(name);
        }
    }
    public class FieldDeclarations : Unary
    {
        public FieldDeclarations(AbstractNode child) : base(child) { }
    }
    public class FieldVariableDeclarators : Unary
    {
        public FieldVariableDeclarators(AbstractNode child) : base(child) { }
    }
    public class ParameterList : Unary
    {
        public ParameterList(AbstractNode param) : base(param) { }
    }
    public class QualifiedName : Unary
    {
        public QualifiedName(AbstractNode id) : base(id) { }
    }
    public class LocalVariableDeclarationAndStatement : Unary
    {
        public LocalVariableDeclarationAndStatement(AbstractNode dcl) : base(dcl) { }
    }
    public class LocalVariableDeclarators : Unary
    {
        public LocalVariableDeclarators(AbstractNode dcl) : base(dcl) { }
    }
    /****************************/
    /*  Unique/Base Type Nodes  */
    /****************************/
    public class MethodDeclaration : AbstractNode
    {
        public MethodDeclaration(AbstractNode mods, AbstractNode type, AbstractNode dcl, AbstractNode body)
        {
            this.adoptChildren(mods);
            this.adoptChildren(type);
            this.adoptChildren(dcl);
            this.adoptChildren(body);
        }
    }
    public class ModifierList : AbstractNode
    {
        public List<string> list = new List<string>();
        public ModifierList(int t)
        {
            if (t != (int)PToken.PUBLIC || t != (int)PToken.PRIVATE || t != (int)PToken.STATIC)
                Add(t);
        }
        public void Add(int t)
        {
            if (t == (int)PToken.PUBLIC)
                list.Add("PUBLIC");
            else if (t == (int)PToken.PRIVATE)
                list.Add("PRIVATE");
            else if (t == (int)PToken.STATIC)
                list.Add("STATIC");
        }
        public string PrintModList()
        {
            string s = "";
            foreach (string attr in list)
                s += attr + " ";
            return s;
        }
    }
    public class Identifier : AbstractNode
    {
        private string id;
        public Identifier(string id)
        {
            this.id = id;
        }
        public string Name
        {
            get
            {
                return id;
            }
        }
    }
    public class ConstantValue : AbstractNode
    {
        private string value;
        public ConstantValue(string value)
        {
            this.value = value;
        }
        public string Value
        {
            get
            {
                return value;
            }
        }
    }
    public class PrimitiveType : AbstractNode
    {
        private string type;
        public PrimitiveType(int t)
        {
            if (t == (int)PToken.INT)
                type = "INT";
            else if (t == (int)PToken.BOOLEAN)
                type = "BOOLEAN";
            else if (t == (int)PToken.VOID)
                type = "VOID";
        }
        public string Type { get { return type; } }
    }
    public class SpecialName : AbstractNode
    {
        private string type;
        public SpecialName(int t)
        {
            if (t == (int)PToken.THIS)
                type = "THIS";
            else if (t == (int)PToken.NULL)
                type = "NULL";
        }
        public string SpecialType { get { return type; } }
    }


}

