using System;
using System.Collections;

namespace ASTBuilder
{

    /// <summary>
    /// All AST nodes are subclasses of this node.  This node knows how to
    /// link itself with other siblings and adopt children.
    /// Each node gets a node number to help identify it distinctly in an AST.
    /// </summary>
    public abstract class AbstractNode : ReflectiveVisitable
    {
        private static int nodeNums = 0;
        private int nodeNum;
        private AbstractNode mysib;
        private AbstractNode parent;
        private AbstractNode child;
        private AbstractNode firstSib;
        private Type type;
        private static int tabs = 0;
        private string tab = "";

        public AbstractNode()
        {
            parent = null;
            mysib = null;
            firstSib = this;
            child = null;
            nodeNum = ++nodeNums;
        }

        /// <summary>
        /// Join the end of this sibling's list with the supplied sibling's list </summary>
        public virtual AbstractNode makeSibling(AbstractNode sib)
        {
            if (sib == null)
            {
                throw new Exception("Call to makeSibling supplied null-valued parameter");
            }
            AbstractNode appendAt = this;
            while (appendAt.mysib != null)
            {
                appendAt = appendAt.mysib;
            }
            appendAt.mysib = sib.firstSib;


            AbstractNode ans = sib.firstSib;
            ans.firstSib = appendAt.firstSib;
            while (ans.mysib != null)
            {
                ans = ans.mysib;
                ans.firstSib = appendAt.firstSib;
            }
            return (ans);
        }

        /// <summary>
        /// Adopt the supplied node and all of its siblings under this node </summary>
        public virtual AbstractNode adoptChildren(AbstractNode n)
        {
            if (n != null)
            {
                if (this.child == null)
                {
                    this.child = n.firstSib;
                }
                else
                {
                    this.child.makeSibling(n);
                }
            }
            for (AbstractNode c = this.child; c != null; c = c.mysib)
            {
                c.parent = this;
            }
            return this;
        }

        public virtual AbstractNode orphan()
        {
            mysib = parent = null;
            firstSib = this;
            return this;
        }

        public virtual AbstractNode abandonChildren()
        {
            child = null;
            return this;
        }

        private AbstractNode Parent
        {
            set
            {
                this.parent = value;
            }
            get
            {
                return (parent);
            }
        }


        public virtual AbstractNode Sib
        {
            get
            {
                return (mysib);
            }
        }

        public virtual AbstractNode Child
        {
            get
            {
                return (child);
            }
        }

        public virtual AbstractNode First
        {
            get
            {
                return (firstSib);
            }
        }

        public virtual Type NodeType
        {
            get
            {
                return type;
            }
            set
            {
                this.type = value;
            }
        }

        public virtual string Name
        {
            get
            {
                return "";
            }
        }

        public override string ToString()
        {
            return ("" + Name);
        }

        public virtual string dump()
        {
            Type t = NodeType;
            string tString = (t != null) ? ("<" + t.ToString() + "> ") : "";

            return "" + NodeNum + ": " + tString + whatAmI() + "  \"" + ToString() + "\"";
        }


        public virtual int NodeNum
        {
            get
            {
                return nodeNum;
            }
        }

        private static string trimClass(string cl)
        {
            int dollar = cl.LastIndexOf('$');
            int dot = cl.LastIndexOf('.');
            int trimAt = (dollar > dot) ? dollar : dot;
            if (trimAt >= 0)
            {
                cl = cl.Substring(trimAt + 1);
            }
            return cl;
        }

        private static Type objectClass = (new object()).GetType();
        private static void debugMsg(string s)
        {
        }
        //  private static System.Collections.IEnumerator interfaces(Type c)
        //  {
        //  Type iClass = c;
        //  ArrayList v = new ArrayList();
        //  while (iClass != objectClass)
        //  {
        //debugMsg("Looking for interface  match in " + iClass.Name);
        //Type[] interfaces = iClass.Interfaces;
        //	 for (int i = 0; i < interfaces.Length; i++)
        //	 {
        //	  debugMsg("   trying interface " + interfaces[i]);
        //		  v.Add(interfaces[i]);
        //		Type[] superInterfaces = interfaces[i].Interfaces;
        //		for (int j = 0; j < superInterfaces.Length; ++j)
        //		{
        //	  debugMsg("   trying super interface " + superInterfaces[j]);
        //			  v.Add(superInterfaces[j]);
        //		}

        //	 }
        // iClass = iClass.BaseType;
        //  }
        //  return v.elements();
        //  }

        /// <summary>
        /// Reflectively indicate the class of "this" node </summary>
        public virtual string whatAmI()
        {
            string ans = trimClass(this.GetType().ToString());
            return ans;  /* temporary until remainder is fixed */
            //ISet s = new HashSet();
            //System.Collections.IEnumerator e = interfaces(this.GetType());
            //while (e.MoveNext())
            //{
            //    Type c = (Type)e.Current;
            //    string str = trimClass(c.ToString());
            //    if (!(str.Equals("DontPrintMe") || str.Equals("ReflectiveVisitable")))
            //    {
            //        s.Add(trimClass(c.ToString()));
            //    }
            //}
            //return ans + s.ToString();
        }

        //private void internWalk(int level, Visitable v)
        //{
        //v.pre(level, this);
        //for (AbstractNode c = child; c != null; c = c.mysib)
        //{
        //c.internWalk(level + 1, v);
        //}
        //v.post(level, this);
        //}

        /// <summary>
        /// Reflective visitor pattern </summary>

        public void Visit(AbstractNode v)
        {
            Console.WriteLine("Visiting ( " + v.dump() + " )");
        }
        public void accept(ReflectiveVisitable v)
        {
            v.Visit(this);
            ++tabs;
            tab = "";
            for (int i = 0; i < tabs; i++)
                tab += "   ";
            AbstractNode children = Child;
            while (children != null)
            {
                Console.Write(tab);
                children.accept(v);
                children = children.mysib;
                --tabs;
            }
        }
        /// <summary>
        /// Obsolete, do not use! </summary>
        //public virtual void walkTree(Visitable v)
        //{
        //internWalk(0, v);
        //}
    }

}