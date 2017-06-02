﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTBuilder
{
    public abstract class TypeDescriptor
    {
        public string type;
        public void PrintType()
        {
            Console.WriteLine("    " + this.GetType().ToString());
        }
    }
    public class IntegerTypeDescriptor : TypeDescriptor
    {
        int value;
        public IntegerTypeDescriptor()
        {
            type = "int32";
        }

        public int Value { get { return value; } set { this.value = value; } }
    }
    public class StringTypeDescriptor : TypeDescriptor
    {
        int value;
        public StringTypeDescriptor()
        {
            type = "string";
        }

        public int Value { get { return value; } set { this.value = value; } }
    }
    public class BooleanTypeDescriptor : TypeDescriptor
    {
        bool value;
        public BooleanTypeDescriptor()
        {
            type = "ldc.i4";
        }
        public bool Value { get { return value; } set { this.value = value; } }
    }
    public class VoidTypeDescriptor : TypeDescriptor
    {
        public VoidTypeDescriptor()
        {
            type = "void";
        }
    }
    public class JavaInternalTypeDescriptor : TypeDescriptor
    {

    }
    public class MSCorLibTypeDescriptor : TypeDescriptor
    {

    }
    public class ErrorTypeDescriptor : TypeDescriptor
    {
    }
    public class ArrayTypeDescriptor : TypeDescriptor
    {
        private TypeDescriptor elementType;
        private TypeDescriptor bounds;
        public ArrayTypeDescriptor(TypeDescriptor elementType = null, TypeDescriptor bounds = null)
        {
            this.elementType = elementType;
            this.bounds = bounds;
        }
        public TypeDescriptor ElementType { get { return elementType; } set { this.elementType = value; } }
        public TypeDescriptor Bounds { get { return bounds; } set { this.bounds = value; } }
    }
    public class RecordTypeDescriptor : TypeDescriptor
    {
        private Hashtable fields;
        public RecordTypeDescriptor(Hashtable fields = null)
        {
            this.fields = fields;
        }
        public Hashtable Fields { get { return fields; } set { this.fields = value; } }
    }
    public class SignatureTypeDescriptor : TypeDescriptor
    {
        private AbstractNode firstParam;
        public SignatureTypeDescriptor(AbstractNode firstParam=null)
        {
            this.firstParam = firstParam;
        }
        public AbstractNode Parameters { get { return firstParam; } set { firstParam = value; } }
    }
    public class ClassTypeDescriptor : TypeDescriptor
    {
        private Attributes parent;
        private Hashtable names;
        private bool isPublic;
        private bool isPrivate;
        private bool isStatic;
        public ClassTypeDescriptor(Attributes parent = null, Hashtable names = null, bool isPublic = false, bool isPrivate = false, bool isStatic = false)
        {
            this.parent = parent;
            this.names = names;
            this.isPublic = isPublic;
            this.isPrivate = isPrivate;
            this.isStatic = isStatic;
        }
        public Attributes Parent { get { return parent; } set { this.parent = value; } }
        public Hashtable Names { get { return names; } set { this.names = value; } }
        public bool IsPublic { get { return isPublic; } set { this.isPublic = value; } }
        public bool IsPrivate { get { return isPrivate; } set { this.isPrivate = value; } }
        public bool IsStatic { get { return isStatic; } set { this.isStatic = value; } }

    }
}
