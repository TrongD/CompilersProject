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
        public virtual void Visit(dynamic node)
        {
            this.VisitNode(node);
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
        public void VisitChildren(AbstractNode node)
        {
            AbstractNode child = node.Child;
            while (child != null)
            {
                child.Accept(this);
                node.TypeRef = child.TypeRef;
                child = child.Sib;
            };
        }
        public void VisitNode(AbstractNode node)
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
        public void VisitNode(SelectionStatement node)
        {
            VisitChildren(node);
            BooleanCheck(node.Child);
        }
        public void VisitNode(IterationStatement node)
        {
            VisitChildren(node);
            BooleanCheck(node.Child);
        }
        public void VisitNode(ConstantValue node)
        {
            node.TypeRef = table.lookup("INT").TypeRef;
        }
        bool IsDataObject(Attributes attr)
        {
            if (attr.TypeRef.GetType() == typeof(ASTBuilder.IntegerTypeDescriptor) 
                || attr.TypeRef.GetType() == typeof(ASTBuilder.BooleanTypeDescriptor)
                || attr.TypeRef.GetType() == typeof(ASTBuilder.JavaInternalTypeDescriptor))
                return true;
            return false;
        }
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
        public override void VisitNode(Identifier node)
        {
            SemanticsVisitor visitor = new SemanticsVisitor();
            node.Accept(visitor);
            if (!isAssignable(node.AttributesRef)){
                node.TypeRef = new ErrorTypeDescriptor();
                node.AttributesRef = null;
            }
        }
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
            body.Accept(this);
            ((ClassTypeDescriptor)attr.TypeRef).Names = table.decrNestLevel();
            SetCurrentClass(null);
        }
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
            if (param != null)
            {
                param.Accept(typeVisitor);
                attr.Signature = new SignatureTypeDescriptor(param.Child);
            }
            name.AttributesRef = attr;
            MethodAttributes oldCurrentMethod = GetCurrentMethod();
            SetCurrentMethod(attr);
            if(param != null)
                param.Accept(this);
            body.Accept(this);
            SetCurrentMethod(oldCurrentMethod);
            attr.Locals = table.decrNestLevel();
        }
    }
    public class TypeVisitor : TopDeclVisitor
    {
        public override void Visit(dynamic node)
        {
            this.VisitNode(node);
        }
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
    