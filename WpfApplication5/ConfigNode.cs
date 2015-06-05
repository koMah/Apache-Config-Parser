using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication5
{

public class ConfigNode
{

	private String name;
	private String content;
	private List<ConfigNode> children = new List<ConfigNode>();

	private ConfigNode parent;

	/**
	 * Private constructor. {@code ConfigNode} instances should be created via the creation factory methods.
	 * 
	 * @param name
	 *            the node name
	 * @param content
	 *            the node content
	 * @param parent
	 *            the parent of the node
	 */
	private ConfigNode(String name, String content, ConfigNode parent) {
		this.name = name;
		this.content = content;
		this.parent = parent;
	}

	/**
	 * Creates a root node.
	 * 
	 * <p>
	 * A root node will have a null parent, name, and content. It is the top level of the configuration tree with child
	 * nodes containing actual values.
	 * 
	 * @return a new root configuration node
	 */
	public static ConfigNode createRootNode() {
		return new ConfigNode(null, null, null);
	}

	/**
	 * Creates a child node
	 * 
	 * <p>
	 * A child node contains a configuration name and configuration content as well as a parent node in the tree. If the
	 * child node is an apache configuration section it may also have child nodes of its own.
	 * 
	 * @param name
	 *            the configuration name (cannot be null)
	 * @param content
	 *            the configuration content (cannot be null)
	 * @param parent
	 *            the child nodes parent (cannot be null)
	 * @return a new child configuration node
	 * @throws NullPointerException
	 *             if name, content, or parent is null
	 */
	public static ConfigNode createChildNode(String name, String content, ConfigNode parent) {
		if (name == null) {
			throw new ArgumentNullException("name: null");
		}
		if (content == null) {
			throw new ArgumentNullException("content: null");
		}
		if (parent == null) {
			throw new ArgumentNullException("parent: null");
		}

		ConfigNode child = new ConfigNode(name, content, parent);
		parent.AddChild(child);

		return child;
	}

	/**
	 * 
	 * @return the configuration name; null if this is a root node
	 */
	public String getName() {
		return name;
	}

	/**
	 * 
	 * @return the configuration content; null if this is a root node
	 */
	public String getContent() {
		return content;
	}

	/**
	 * 
	 * @return The nodes parent; null if this is a root node
	 */
	public ConfigNode getParent() {
		return parent;
	}

	/**
	 * 
	 * @return a list of child configuration nodes
	 */
	public List<ConfigNode> getChildren() {
		return children;
	}

	/**
	 * 
	 * @return true if this is a root node; false otherwise
	 */
	public virtual bool isRootNode() {
		return parent == null;
	}

	public override String ToString() {
		return "ConfigNode {name=" + name + ", content=" + content + ", childNodeCount=" + children.Count().ToString() + "}";
	}

	private void AddChild(ConfigNode child) {
		children.Add(child);
	}
}
}
