using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication5
{

    public class ConfigNode
    {
        public enum SearchBy
        {
            DirectiveName,
            DirectiveValue
        };

        private String Name;
        private String Value;

        private bool Commented;

        private ConfigNode Parent;
        private List<ConfigNode> Childs = new List<ConfigNode>();

        

        /**
         * Private constructor. {@code ConfigNode} instances should be created via the creation factory methods.
         * 
         * @param name
         *            the node name
         * @param Value
         *            the node Value
         * @param parent
         *            the parent of the node
         */
        private ConfigNode(String Name, String Value, ConfigNode Parent, bool Commented)
        {
            this.Commented = Commented;
            this.Name = Name;
            this.Value = Value;
            this.Parent = Parent;
        }

        /**
         * Creates a root node.
         * 
         * <p>
         * A root node will have a null parent, name, and Value. It is the top level of the configuration tree with child
         * nodes containing actual values.
         * 
         * @return a new root configuration node
         */
        public static ConfigNode CreateRootNode()
        {
            return new ConfigNode(null, null, null, true);
        }

        /**
         * Creates a child node
         * 
         * <p>
         * A child node contains a configuration name and configuration Value as well as a parent node in the tree. If the
         * child node is an apache configuration section it may also have child nodes of its own.
         * 
         * @param name
         *            the configuration name (cannot be null)
         * @param Value
         *            the configuration Value (cannot be null)
         * @param parent
         *            the child nodes parent (cannot be null)
         * @return a new child configuration node
         * @throws NullPointerException
         *             if name, Value, or parent is null
         */
        public static ConfigNode CreateChildNode(String name, String Value, ConfigNode parent, bool commented)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name: null");
            }
            if (Value == null)
            {
                throw new ArgumentNullException("content: null");
            }
            if (parent == null)
            {
                throw new ArgumentNullException("parent: null");
            }

            ConfigNode child = new ConfigNode(name, Value, parent, commented);
            parent.AddChild(child);

            return child;
        }

        /**
         * 
         * @return the configuration name; null if this is a root node
         */
        public String GetName()
        {
            return Name;
        }

        /**
         * 
         * @return the configuration Value; null if this is a root node
         */
        public String GetContent()
        {
            return Value;
        }

        /**
         * 
         * @return The nodes parent; null if this is a root node
         */
        public ConfigNode getParent()
        {
            return Parent;
        }

        /**
         * 
         * @return a list of child configuration nodes
         */
        public List<ConfigNode> GetChildren()
        {
            return Childs;
        }

        /**
         * 
         * @return a list of child configuration nodes
         */
        public List<ConfigNode> GetChildren(string SearchFor, SearchBy SearchBy)
        {
            switch (SearchBy)
            {
                case SearchBy.DirectiveName:
                    return Childs.FindAll(item => item.GetName() == SearchFor);
            }

            return null;
        }

        public bool HasChildren()
        {
            return Childs.Count > 0;
        }

        /**
         * 
         * @return true if this is a root node; false otherwise
         */
        public virtual bool isRootNode()
        {
            return Parent == null;
        }

        public override String ToString()
        {
            return BuildOutput();
        }

        public string BuildOutput(int TabsLevel = 0)
        {
            StringBuilder Out = new StringBuilder();

            string Tabs = TabsLevel == 0 ? String.Empty : "\t".PadLeft(TabsLevel);
            string Hash = Commented ? "#" : String.Empty;

            if (HasChildren())
            {
                Out.AppendLine(String.Format("{0}{1}<{2} {3}>", Hash, Tabs, Name, Value));

                foreach (ConfigNode child in GetChildren())
                {
                    Out.AppendLine(String.Format("{0}{1}{2}", Hash, Tabs, child.BuildOutput(TabsLevel + 1)));
                }

                Out.AppendFormat("{0}</{1}>", Tabs, Name);
            }
            else
            {
                Out.Append(String.Format("{0}{1}{2} {3}", Hash, Tabs, GetName(), GetContent()));
            }

            return Out.ToString();
        }

        private void AddChild(ConfigNode child)
        {
            Childs.Add(child);
        }
    }
}
