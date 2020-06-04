using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine
{
    [Serializable]
    public class FileTree<T>
    {
        private int _groupCounter = 0;
        private FolderNode _root;
        public FolderNode Root
        {
            get
            {
                if (_root == null)
                {
                    _root = new FolderNode(null, "Root");
                }
                return _root;
            }
            private set { _root = value; }
        }

        public bool MultiSelection = false;

        private List<Node> _selected;
        public List<Node> Selected
        {
            get
            {
                if (_selected == null)
                {
                    _selected = new List<Node>();
                }
                return _selected;
            }
            private set { _selected = value; }
        }

        private bool _sorted;

        public FileTree(bool sorted)
        {
            _sorted = sorted;
        }


        public void Select(Node node)
        {
            MultiSelection = false;
            foreach(Node n in Selected)
            {
                n.Selected = false;
            }
            node.Selected = true;
            Selected.Clear();
            Selected.Add(node);
        }

        public void Deselect()
        {
            foreach (Node n in Selected)
            {
                n.Selected = false;
            }
            Selected.Clear();
        }

        public void AddToSelection(Node node)
        {
            MultiSelection = true;
            node.Selected = true;
            Selected.Add(node);
        }

        public void AddElement(T element, string name)
        {
            if (Selected.Count == 1 && Selected[0] is FolderNode)
            {
                ((FolderNode)Selected.First()).Values.Add(new FileNode((FolderNode)Selected.First(), element, name));
                ((FolderNode)Selected.First()).Values.Sort();
            }
            else
            {
                Root.Values.Add(new FileNode(Root, element, name));
                Root.Values.Sort();
            }
        }

        public void AddGroup()
        {
            if (Selected.Count == 1 && Selected.First() is FolderNode)
            {
                ((FolderNode)Selected.First()).Values.Add(new FolderNode((FolderNode)Selected.First(), String.Format("group_{0}", _groupCounter)));
            }
            else if (Selected.Count == 1 && Selected.First() is FileNode && Selected.First().Parent != null)
            {
                ((FolderNode)Selected.First().Parent).Values.Add(new FolderNode(Selected.First().Parent, String.Format("group_{0}", _groupCounter)));
            }
            else
            {
                ((FolderNode)Root).Values.Add(new FolderNode(Root, String.Format("group_{0}", _groupCounter)));
            }
            _groupCounter++;
        }

        public void AddGroup(string group)
        {
            if (Selected.Count == 1 && Selected.First() is FolderNode)
            {
                ((FolderNode)Selected.First()).Values.Add(new FolderNode((FolderNode)Selected.First(), group));
            }
            else if (Selected.Count == 1 && Selected.First() is FileNode && Selected.First().Parent != null)
            {
                ((FolderNode)Selected.First().Parent).Values.Add(new FolderNode(Selected.First().Parent, group));
            }
            else
            {
                ((FolderNode)Root).Values.Add(new FolderNode(Root, group));
            }
            _groupCounter++;
        }

        public void GroupSelected()
        {
            FolderNode parent = null;
            int min = int.MaxValue;
            foreach (Node n in Selected)
            {
                if (n.Depth < min)
                {
                    min = n.Depth;
                    parent = n.Parent;
                }
            }

            FolderNode group;
            if (parent == null)
            {
                group = new FolderNode(Root, String.Format("group_{0}", _groupCounter));
                Root.Values.Add(group);
            }
            else
            {
                group = new FolderNode(parent, String.Format("group_{0}", _groupCounter));
                parent.Values.Add(group);
            }

            foreach (Node n in Selected)
            {
                if (n.Depth == min)
                {
                    if (n.Parent != null)
                    {
                        n.Parent.Values.Remove(n);
                    }
                    group.Values.Add(n);
                    n.Parent = group;
                }
                n.Depth++;
            }
            _groupCounter++;
        }

        public void SelectBetween(Node start, Node end)
        {
            Deselect();
            if (start.Parent != null)
            {
                MultiSelection = true;
                bool started_start = false;
                bool started_end = false;
                foreach (Node n in start.Parent.Values)
                {
                    if (n.Equals(start) && !started_start)
                    {
                        started_start = true;
                    }
                    if (n.Equals(end) && !started_end)
                    {
                        started_end = true;
                    }
                    if (started_start || started_end)
                    {
                        n.Selected = true;
                        Selected.Add(n);
                    }
                    if ((started_start || started_end) && n is FolderNode && ((FolderNode)n).Values.Count > 0)
                    {
                        if (started_start)
                        {
                            SelectBetween(((FolderNode)n).Values[0], end);
                        }
                        else
                        {
                            SelectBetween(start, ((FolderNode)n).Values[0]);
                        }
                        
                    }
                    if ((n.Equals(end) && started_start) || (n.Equals(start) && started_end))
                    {
                        started_start = false;
                        started_end = false;
                    }
                }
            }
        }

        public List<T> RemoveSelected()
        {
            List<T> removed = new List<T>();
           
            foreach(Node n in Selected)
            {
                if (n is FolderNode)
                {
                    removed = RemoveSelected(removed, (FolderNode)n);
                }
                if (n is FileNode)
                {
                    removed.Add(((FileNode)n).Value);
                }
                if (n.Parent != null)
                {
                    n.Parent.Values.Remove(n);
                }

            }
            Selected.Clear();
            return removed;
        }

        private List<T> RemoveSelected(List<T> removed, FolderNode node)
        {
            foreach (Node n in node.Values)
            {
                if (n is FileNode)
                {
                    removed.Add(((FileNode)n).Value);
                    if (n.Parent != null)
                    {
                        n.Parent.Values.Remove(n);
                    }
                }
                else
                {
                    removed = RemoveSelected(removed, (FolderNode)n);
                }
            }

            node.Values.Clear();
            return removed;
        }

        public void RenameFile(T file, string name)
        {
            FileNode node = SearchFile(file);
            if (node != null)
                node.Name = name;
        }

        public FileNode SearchFile(T node)
        {
            return SearchFile(Root, node);
        }

        private FileNode SearchFile(FolderNode root, T node)
        {
            foreach (Node n in root.Values)
            {
                if (n is FileNode)
                {
                    if (((FileNode)n).Value.Equals(node))
                        return (FileNode)n;
                }
                else
                {
                    SearchFile((FolderNode)n, node);
                }
            }
            return null;
        }

        public void SortFolders()
        {
            if (_sorted)
                SortFolders(Root);
        }

        private void SortFolders(FolderNode root)
        {
            root.Values.Sort();
            foreach (Node n in root.Values)
            {
                if (n is FolderNode)
                {
                    SortFolders((FolderNode)n);
                }
            }
        }





        #region Nodes
        [Serializable]
        public abstract class Node: IComparable<Node>
        {
            public string Name { get; set; }
            public bool Selected { get; set; }
            public bool Editing { get; set; }
            internal int Depth { get; set; }
            public FolderNode Parent { get; internal set; }
            public int CompareTo(Node obj)
            {
                return Name.CompareTo(obj.Name);
            }
        }


        [Serializable]
        public class FolderNode : Node
        {


            public bool Expanded { get; set; }

            private List<Node> _values;
            public List<Node> Values
            {
                get
                {
                    if (_values == null)
                    {
                        _values = new List<Node>();
                    }
                    return _values;
                }
            }

            public FolderNode(FolderNode parent, string name)
            {
                this.Name = name;
                this.Parent = parent;
                if (parent == null)
                {
                    Depth = 0;
                }
                else
                {
                    Depth = parent.Depth + 1;
                }
            }
        }


        [Serializable]
        public class FileNode: Node
        {
            public T Value { get; private set; }

            public FileNode(FolderNode parent, T value, string name)
            {
                this.Value = value;
                this.Parent = parent;
                this.Name = name;
                Depth = parent.Depth + 1;

            }
        }

        #endregion

    }
}
