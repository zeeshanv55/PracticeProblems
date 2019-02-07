using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.Bst.Easy.Basic
{
    class Program
    {
        class Node
        {
            public int Key { get; set; }

            public string Data { get; set; }

            public Node LeftChild { get; set; }

            public Node RightChild { get; set; }

            public Node(int key, string data)
            {
                this.Key = key;
                this.Data = data;
            }
        }

        class BinarySearchTree
        {
            public Node Root { get; set; }

            public int Count { get; set; }

            public void Add(int key, string data = null)
            {
                if (this.Root == null)
                {
                    this.Root = new Node(key, data);
                }
                else
                {
                    this.AddToSubtree(key, data, this.Root);
                }

                this.Count++;
            }

            public bool Exists(int key, Node root = null)
            {
                if (root == null)
                {
                    root = this.Root;
                }

                if (root.Key == key)
                {
                    return true;
                }
                else if (key < root.Key)
                {
                    if (root.LeftChild != null)
                    {
                        return Exists(key, root.LeftChild);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (root.RightChild != null)
                    {
                        return Exists(key, root.RightChild);
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            public bool Remove(int key)
            {
                if (this.Root == null)
                {
                    return false;
                }
                else
                {
                    if (RemoveFromSubtree(key, this.Root, null, false))
                    {
                        this.Count--;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            public void TraverseInOrder()
            {
                TraverseInOrder(this.Root);
            }

            public void TraversePreOrder()
            {
                TraversePreOrder(this.Root);
            }

            public void TraversePostOrder()
            {
                TraversePostOrder(this.Root);
            }

            private void AddToSubtree(int key, string data, Node root)
            {
                if (key <= root.Key)
                {
                    if (root.LeftChild == null)
                    {
                        root.LeftChild = new Node(key, data);
                    }
                    else
                    {
                        this.AddToSubtree(key, data, root.LeftChild);
                    }
                }
                else
                {
                    if (root.RightChild == null)
                    {
                        root.RightChild = new Node(key, data);
                    }
                    else
                    {
                        this.AddToSubtree(key, data, root.RightChild);
                    }
                }
            }

            private bool RemoveFromSubtree(int key, Node root, Node parent, bool isLeftChild)
            {
                if (root.Key == key)
                {
                    if (root.LeftChild == null && root.RightChild == null)
                    {
                        if (parent != null)
                        {
                            if (isLeftChild)
                            {
                                parent.LeftChild = null;
                            }
                            else
                            {
                                parent.RightChild = null;
                            }
                        }
                        else
                        {
                            this.Root = null;
                        }

                        return true;
                    }

                    if (root.LeftChild == null && root.RightChild != null)
                    {
                        if (parent != null)
                        {
                            if (isLeftChild)
                            {
                                parent.LeftChild = root.RightChild;
                            }
                            else
                            {
                                parent.RightChild = root.RightChild;
                            }
                        }
                        else
                        {
                            this.Root = root.RightChild;
                        }

                        return true;
                    }

                    if (root.LeftChild != null && root.RightChild == null)
                    {
                        if (parent != null)
                        {
                            if (isLeftChild)
                            {
                                parent.LeftChild = root.LeftChild;
                            }
                            else
                            {
                                parent.RightChild = root.LeftChild;
                            }
                        }
                        else
                        {
                            this.Root = root.LeftChild;
                        }

                        return true;
                    }

                    isLeftChild = true;
                    parent = root;
                    var largestYet = root.LeftChild;
                    while (largestYet.RightChild != null)
                    {
                        isLeftChild = false;
                        parent = largestYet;
                        largestYet = largestYet.RightChild;
                    }

                    root.Key = largestYet.Key;
                    root.Data = largestYet.Data;
                    if (isLeftChild)
                    {
                        parent.LeftChild = largestYet.LeftChild;
                    }
                    else
                    {
                        parent.RightChild = largestYet.LeftChild;
                    }

                    return true;
                }
                else if (key < root.Key)
                {
                    if (root.LeftChild != null)
                    {
                        return RemoveFromSubtree(key, root.LeftChild, root, true);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (root.RightChild != null)
                    {
                        return RemoveFromSubtree(key, root.RightChild, root, false);
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            private void TraverseInOrder(Node root)
            {
                if (root == null)
                {
                    return;
                }
                else
                {
                    TraverseInOrder(root.LeftChild);
                    Console.Write(root.Key + " ");
                    TraverseInOrder(root.RightChild);
                }
            }

            private void TraversePreOrder(Node root)
            {
                if (root == null)
                {
                    return;
                }
                else
                {
                    Console.Write(root.Key + " ");
                    TraversePreOrder(root.LeftChild);
                    TraversePreOrder(root.RightChild);
                }
            }

            private void TraversePostOrder(Node root)
            {
                if (root == null)
                {
                    return;
                }
                else
                {
                    TraversePostOrder(root.LeftChild);
                    TraversePostOrder(root.RightChild);
                    Console.Write(root.Key + " ");
                }
            }
        }

        static void Main(string[] args)
        {
            var tree = new BinarySearchTree();

            tree.Add(10);
            tree.Add(5);
            tree.Add(7);
            tree.Add(6);
            tree.Add(15);
            tree.Add(4);
            tree.Add(11);
            tree.Add(17);

            tree.TraverseInOrder();
            Console.WriteLine();
            tree.TraversePreOrder();
            Console.WriteLine();
            tree.TraversePostOrder();
            Console.WriteLine();

            Console.WriteLine(tree.Exists(1));
            Console.WriteLine(tree.Exists(2));
            Console.WriteLine(tree.Exists(3));
            Console.WriteLine(tree.Exists(4));
            Console.WriteLine(tree.Exists(5));
            Console.WriteLine(tree.Exists(6));
            Console.WriteLine(tree.Exists(7));
            Console.WriteLine(tree.Exists(8));
            Console.WriteLine(tree.Exists(9));
            Console.WriteLine(tree.Exists(10));
            Console.WriteLine(tree.Exists(11));
            Console.WriteLine(tree.Exists(15));
            Console.WriteLine(tree.Exists(17));
            Console.WriteLine(tree.Exists(20));
            Console.WriteLine(tree.Exists(100));
            Console.WriteLine();

            Console.WriteLine(tree.Remove(1));
            tree.TraverseInOrder();
            Console.WriteLine();

            Console.WriteLine(tree.Remove(2));
            tree.TraverseInOrder();
            Console.WriteLine();

            Console.WriteLine(tree.Remove(3));
            tree.TraverseInOrder();
            Console.WriteLine();

            Console.WriteLine(tree.Remove(10));
            tree.TraverseInOrder();
            Console.WriteLine();

            Console.WriteLine(tree.Remove(4));
            tree.TraverseInOrder();
            Console.WriteLine();

            Console.WriteLine(tree.Remove(5));
            tree.TraverseInOrder();
            Console.WriteLine();

            Console.WriteLine(tree.Remove(15));
            tree.TraverseInOrder();
            Console.WriteLine();

            Console.WriteLine(tree.Remove(11));
            tree.TraverseInOrder();
            Console.WriteLine();

            Console.WriteLine(tree.Remove(7));
            tree.TraverseInOrder();
            Console.WriteLine();

            Console.WriteLine(tree.Remove(6));
            tree.TraverseInOrder();
            Console.WriteLine();

            Console.WriteLine(tree.Remove(17));
            tree.TraverseInOrder();
            Console.WriteLine();

            Console.ReadKey();
        }
    }
}