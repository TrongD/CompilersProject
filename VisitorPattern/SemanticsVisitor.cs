using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTBuilder
{
    public class SemanticsVisitor : IReflectiveVisitor
    {
        // This method is the key to implenting the Reflective Visitor pattern
        // in C#, using the 'dynamic' type specification to accept any node type.
        // The subsequent call to VisitNode does dynamic lookup to find the
        // appropriate Version.
        protected static SymbolTable table = new SymbolTable();
        public virtual void Visit(dynamic node)
        {
            this.VisitNode(node);
        }
        /*OBSOLETE CODE NOT USED*/
        protected static ClassAttributes currentClass = null;
        protected static MethodAttributes currentMethod = null;

        protected void SetCurrentClass(ClassAttributes c)
        {
            currentClass = c;
        }
        protected ClassAttributes GetCurrentClass()
        {
            return currentClass;
        }
        protected void SetCurrentMethod(MethodAttributes m)
        {
            currentMethod = m;
        }
        protected MethodAttributes GetCurrentMethod()
        {
            return currentMethod;
        }
       
        // Call this method to begin the semantic checking process
        public void CheckSemantics(AbstractNode node)
        {
            if (node == null)
            {
                return;
            }
            TopDeclVisitor visitor = new TopDeclVisitor();
            node.Accept(visitor);
        }

        public virtual void VisitNode(AbstractNode node)
        {
            AbstractNode child = node.Child;
            TopDeclVisitor visitor = new TopDeclVisitor();
            while (child != null)
            {
                child.Accept(visitor);
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
        //Starting Node of an AST
        public void VisitNode(CompilationUnit node)
        {
            table.incrNestLevel();
            table.BuildTree();
            AbstractNode child = node.Child;
            TopDeclVisitor visitor = new TopDeclVisitor();
            while (child != null)
            {
                child.Accept(visitor);
                node.TypeRef = child.TypeRef;
                child = child.Sib;
            };
        }
        //Visit Identifier and find name in symbol table
        public virtual void VisitNode(Identifier node)
        {
            node.TypeRef = new ErrorTypeDescriptor();
            node.AttributesRef = null;
            Attributes attr = table.lookup(node.Name);
            if(attr == null)
            {
                Console.WriteLine(node.Name + " is not declared.");
            }
            else
            {
                    node.AttributesRef = attr;
                    node.TypeRef = attr.TypeRef;
            }
        }
        //Visit Assign and type check to make sure rhs can be assigned to lhs
        public virtual void VisitNode(Assign node)
        {
            LHSSemanticVisitor lhsVisitor = new LHSSemanticVisitor();
            AbstractNode targetName = node.Child;
            AbstractNode valueExpr = targetName.Sib;
            targetName.Accept(lhsVisitor);
            valueExpr.Accept(this);
            if(Assignable(targetName.TypeRef, valueExpr.TypeRef))
            {
                node.TypeRef = targetName.TypeRef;
            }
            else
            {
                Console.WriteLine("Right hand side expression is not assignable to left hand side");
                node.TypeRef = new ErrorTypeDescriptor();
            }
        }
        //check if lhs and rhs are the same and make node a boolean type
        public void VisitNode(BooleanExpression node)
        {
            AbstractNode expr1 = node.Child;
            AbstractNode expr2 = expr1.Sib;
            expr1.Accept(this);
            expr2.Accept(this);
            if (Assignable(expr1.TypeRef,expr2.TypeRef))
            {
                node.TypeRef = table.lookup("BOOLEAN").TypeRef;
            }
            else
            {
                Console.WriteLine("Right hand side expression is not assignable to left hand side");
                node.TypeRef = new ErrorTypeDescriptor();
            }
        }
        //check lhs and rhs are the same and assign node type based on lhs or rhs type
        public void VisitNode(ArithmeticExpression node)
        {
            AbstractNode expr1 = node.Child;
            AbstractNode expr2 = expr1.Sib;
            expr1.Accept(this);
            expr2.Accept(this);
            if (Assignable(expr1.TypeRef, expr2.TypeRef))
            {
                node.TypeRef = expr1.TypeRef;
            }
            else
            {
                Console.WriteLine("Right hand side expression is not assignable to left hand side");
                node.TypeRef = new ErrorTypeDescriptor();
            }
        }
        //if statement - check if children are correct and then do boolean check
        public void VisitNode(SelectionStatement node)
        {
            VisitChildren(node);
            BooleanCheck(node.Child);
        }
        //loop statement - check if children are correct and then do boolean check
        public void VisitNode(IterationStatement node)
        {
            VisitChildren(node);
            BooleanCheck(node.Child);
        }
        //make node Int type
        public void VisitNode(ConstantValue node)
        {
            node.TypeRef = table.lookup("INT").TypeRef;
        }
        //make node string type
        public void VisitNode(StringValue node)
        {
            node.TypeRef = table.lookup("String").TypeRef;
        }
        //check if two nodes based on types are assignable
        bool Assignable(TypeDescriptor attr1, TypeDescriptor attr2)
        {
            if (!(attr1.GetType() == typeof(ASTBuilder.IntegerTypeDescriptor))
                && !(attr1.GetType() == typeof(ASTBuilder.BooleanTypeDescriptor))
                && !(attr1.GetType() == typeof(ASTBuilder.JavaInternalTypeDescriptor)))
                return false;
            if (!(attr2.GetType() == typeof(ASTBuilder.IntegerTypeDescriptor))
                && !(attr2.GetType() == typeof(ASTBuilder.BooleanTypeDescriptor))
                && !(attr2.GetType() == typeof(ASTBuilder.JavaInternalTypeDescriptor)))
                return false;
            if (attr1.GetType() != attr2.GetType())
                return false;
            return true;
        }
        //check if type is a boolean or error type
        void BooleanCheck(AbstractNode node)
        {
            if ((node.TypeRef.GetType() != typeof(ASTBuilder.BooleanTypeDescriptor))
                && !(node.TypeRef.GetType() != typeof(ASTBuilder.ErrorTypeDescriptor)))
                Console.WriteLine("Require Boolean Type at " + node.ClassName());
        }

    }
    public class LHSSemanticVisitor : SemanticsVisitor
    {
        public override void Visit(dynamic node)
        {
            this.VisitNode(node);
        }
        //check if Identifier is declared in Symbol table and assignable
        public override void VisitNode(Identifier node)
        {
            SemanticsVisitor visitor = new SemanticsVisitor();
            node.Accept(visitor);
            if (!isAssignable(node.AttributesRef)){
                node.TypeRef = new ErrorTypeDescriptor();
                node.AttributesRef = null;
            }
        }
        //check if all Qualified Name names are in Symbol table 
        public void VisitNode(QualifiedName node)
        {
            AbstractNode name = node.Child;
            while (name != null)
            {
                name.Accept(this);
                node.TypeRef = name.TypeRef;
                name = name.Sib;
            }
        }
        bool isAssignable(Attributes attr)
        {
            if (attr.TypeRef.GetType() == typeof(ASTBuilder.IntegerTypeDescriptor) 
                || attr.TypeRef.GetType() == typeof(ASTBuilder.BooleanTypeDescriptor)
                || attr.TypeRef.GetType() == typeof(ASTBuilder.JavaInternalTypeDescriptor))
                return true;
            return false;

        }
    }

    public class TopDeclVisitor : SemanticsVisitor
    {
        public override void Visit(dynamic node)
        {
            this.VisitNode(node);
        }
        //check if field declaration (static int x) is declared in Symbol table and add if not
        public void VisitNode(FieldVariableDeclaration node)
        {
            AbstractNode mods = node.Child;
            AbstractNode type = mods.Sib;
            AbstractNode dcls = type.Sib.Child;
            TypeVisitor typeVisitor = new TypeVisitor();
            dcls.Accept(typeVisitor);
            while (dcls != null)
            {
                string name = ((Identifier)dcls).Name;
                try
                {
                    VariableAttributes attr = new VariableAttributes(((ModifierList)mods).list, type.TypeRef);
                    dcls.TypeRef = type.TypeRef;
                    table.enter(name, attr);
                    dcls.AttributesRef = attr;
                }
                catch (FoundKeyException e)
                {
                    Console.WriteLine(e.Message);
                    node.TypeRef = new ErrorTypeDescriptor();
                    node.AttributesRef = null;
                }
                dcls = dcls.Sib;
                Console.Write(((Identifier)dcls).Name + "   ");
                node.TypeRef.PrintType();
            }
        }
        //check parameter and declare in Symbol table
        public void VisitNode(Parameter node)
        {
            AbstractNode typeName =node.Child;
            AbstractNode idName = typeName.Sib;
            TypeVisitor typeVisitor = new TypeVisitor();
            typeName.Accept(typeVisitor);
            string name = ((Identifier)idName).Name;
            try
            {
                VariableAttributes attr = new VariableAttributes(null, typeName.TypeRef);
                idName.TypeRef = typeName.TypeRef;
                table.enter(name, attr);
                idName.AttributesRef = attr;
            }
            catch (FoundKeyException e)
            {
                Console.WriteLine(e.Message);
                node.TypeRef = new ErrorTypeDescriptor();
                node.AttributesRef = null;
            }
            idName = idName.Sib;
        }
        //check if variable (int x) is declared in symbol table and add if not
        public void VisitNode(LocalVariableDeclarationStatement node)
        {
            AbstractNode typeName = node.Child;
            AbstractNode idName = typeName.Sib.Child;
            TypeVisitor typeVisitor = new TypeVisitor();
            typeName.Accept(typeVisitor);
            while (idName != null)
            {
                string name = ((Identifier)idName).Name;
                try
                {
                    VariableAttributes attr = new VariableAttributes(null, typeName.TypeRef);
                    idName.TypeRef = typeName.TypeRef;
                    table.enter(name, attr);
                    idName.AttributesRef = attr;
                }
                catch (FoundKeyException e)
                {
                    Console.WriteLine(e.Message);
                    node.TypeRef = new ErrorTypeDescriptor();
                    node.AttributesRef = null;
                }
                idName = idName.Sib;
            }
        }
        //create class type descriptor, increase scope, and check body 
        public void VisitNode(ClassDeclaration node)
        {
            AbstractNode mods = node.Child;
            AbstractNode name = mods.Sib;
            AbstractNode body = name.Sib;
            ClassTypeDescriptor typeRef = new ClassTypeDescriptor();
            ClassAttributes attr = new ClassAttributes();
            attr.TypeRef = typeRef;
            table.enter(((Identifier)name).Name, attr);
            SetCurrentClass(attr);
            foreach(string t in ((ModifierList)mods).list)
            {
                switch (t)
                {
                    case "PUBLIC": typeRef.IsPublic = true; break;
                    case "PRIVATE": typeRef.IsPrivate = true; break;
                    case "STATIC": typeRef.IsStatic = true; break;
                }  
            }
            if(typeRef.IsPublic && typeRef.IsPrivate)
            {
                Console.WriteLine("Cannot call Private and Public on same class.");
                attr.TypeRef = new ErrorTypeDescriptor();
            }
            table.incrNestLevel();
            node.AttributesRef = attr;
            body.Accept(this);
            Console.WriteLine("Symbol table after Class " + ((Identifier)name).Name);
            table.PrintTable();
            ((ClassTypeDescriptor)attr.TypeRef).Names = table.decrNestLevel();
            SetCurrentClass(null);
        }
        //define method attribute, increase scope, and check body 
        public void VisitNode(MethodDeclaration node)
        {
            AbstractNode mods = node.Child;
            AbstractNode type = mods.Sib;
            AbstractNode name = type.Sib.Child;
            AbstractNode body = type.Sib.Sib;
            AbstractNode param = name.Sib;
            TypeVisitor typeVisitor = new TypeVisitor();
            type.Accept(typeVisitor);
            MethodAttributes attr = new MethodAttributes();
            attr.ReturnType = type.TypeRef;
            attr.TypeRef = type.TypeRef;
            attr.Mods = ((ModifierList)mods).list;
            attr.IsDefinedIn = GetCurrentClass();                
            table.enter(((Identifier)name).Name, attr);
            table.incrNestLevel();
            attr.Signature = new SignatureTypeDescriptor();
            if (param != null)
            {
                param.Accept(typeVisitor);
                ((SignatureTypeDescriptor)attr.Signature).Parameters = param;
            }
            name.AttributesRef = attr;
            node.AttributesRef = attr;
            MethodAttributes oldCurrentMethod = GetCurrentMethod();
            SetCurrentMethod(attr);
            if(param != null)
                param.Accept(this);
            body.Accept(this);
            SetCurrentMethod(oldCurrentMethod);
            Console.WriteLine("Symbol table after Method " + ((Identifier)name).Name);
            table.PrintTable();
            attr.Locals = table.decrNestLevel();
        }
    }
    public class TypeVisitor : TopDeclVisitor
    {
        public override void Visit(dynamic node)
        {
            this.VisitNode(node);
        }
        //check if identifier is in symbol table and add if not
        public override void VisitNode(Identifier node)
        {
            string name = node.Name;
            Attributes attr = table.lookup(name);
            if(attr != null && attr.GetType().ToString() == "ASTBuilder.TypeAttributes")
            {
                node.TypeRef = attr.TypeRef;
                node.AttributesRef = attr;
            }
            else
            {
                Console.WriteLine(name + " is not a type name");
                node.TypeRef = new ErrorTypeDescriptor();
                node.AttributesRef = null;
            }
        }
        //check if allnames are in symbol table and add identifier if not
        public void VisitNode(QualifiedName node)
        {
            AbstractNode name = node.Child;
            while(name!=null)
            {
                name.Accept(this);
                node.TypeRef = name.TypeRef;
                name = name.Sib;
            }  
        }
        //check if primitive type is in symbol table and get type from tis attribute
        public void VisitNode(PrimitiveType node)
        {
            Attributes attr = table.lookup(node.Type);
            if(attr == null)
            {
                Console.WriteLine(node.Type + " is not declared in Table.");
                node.TypeRef = new ErrorTypeDescriptor();
                node.AttributesRef = null;
            }
            else
            {
                node.TypeRef = attr.TypeRef;
                node.AttributesRef = new TypeAttributes(node.TypeRef);
            }
        }
    }
}
    