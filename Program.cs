using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleExpression
{
    class Program
    {
        
        class BinaryTreeNode<T>
        {
            public uint Priority = 0;
            public T Value;
            public bool isLeaf = false;
            public BinaryTreeNode<T> LeftChild;
            public BinaryTreeNode<T> RightChild;
        }
        class BinaryTree<T>
        {
            public BinaryTreeNode<T> Root;
            public BinaryTreeNode<T> PointerNode;
            public BinaryTree(BinaryTreeNode<T> Root)
            {
                this.Root = Root;
            }
            public void addNode(BinaryTreeNode<T> thisNode, BinaryTreeNode<T> Node)
            {
                if (thisNode.Priority > Node.Priority)
                {
                    if (thisNode.LeftChild == null)
                    {
                        thisNode.LeftChild = Node;
                        return;
                    }
                    if (thisNode.RightChild == null)
                    {
                        thisNode.RightChild = Node;
                        return;
                    }
                    if (!Node.isLeaf)
                    {
                        Node.LeftChild = thisNode.RightChild;
                        thisNode.RightChild = Node;
                        return;
                    }
                    if (!thisNode.LeftChild.isLeaf)
                    {
                        addNode(thisNode.LeftChild, Node);
                    }
                    if (!thisNode.RightChild.isLeaf)
                    {
                        addNode(thisNode.RightChild, Node);
                    }
                }
                else
                {
                    Node.LeftChild = thisNode;
                    Root = Node;
                    return;
                }
            }
            //后序遍历，进入队列
            public void EnqueueNode(BinaryTreeNode<T> thisNode, Queue<T> queue)
            {
                if (thisNode.LeftChild != null) EnqueueNode(thisNode.LeftChild, queue);
                if (thisNode.RightChild != null) EnqueueNode(thisNode.RightChild, queue);
                queue.Enqueue(thisNode.Value);
            }
        }
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.Write("Command > ");
                    string line = Console.ReadLine();
                    if (line == "exit") break;
                    string symbol = "";
                    BinaryTree<string> binaryTree = new BinaryTree<string>(new BinaryTreeNode<string>());
                    bool flag_isFirstSymbol = true;
                    for(int i = 0; i < line.Length; i++)
                    {
                        if (line.ElementAt(i) <= '9' && line.ElementAt(i) >= '0')
                        {
                            symbol += line.ElementAt(i);
                        }
                        else
                        {
                            if (symbol != "")
                            {
                                BinaryTreeNode<string> node = new BinaryTreeNode<string>();
                                node.Value = symbol;
                                node.isLeaf = true;
                                if (flag_isFirstSymbol)
                                {
                                    binaryTree = new BinaryTree<string>(node);
                                    flag_isFirstSymbol = false;
                                }
                                else
                                {
                                    binaryTree.addNode(binaryTree.Root, node);
                                }
                                symbol = "";
                            }
                            BinaryTreeNode<string> node1 = new BinaryTreeNode<string>();
                            node1.Value = line.ElementAt(i).ToString();
                            if (node1.Value == "+" || node1.Value == "-") node1.Priority = 2;
                            if (node1.Value == "*" || node1.Value == "/") node1.Priority = 1;
                            binaryTree.addNode(binaryTree.Root, node1);
                        }
                    }
                    if (symbol != "")
                    {
                        BinaryTreeNode<string> node = new BinaryTreeNode<string>();
                        node.Value = symbol;
                        node.isLeaf = true;
                        if (flag_isFirstSymbol)
                        {
                            binaryTree = new BinaryTree<string>(node);
                            flag_isFirstSymbol = false;
                        }
                        else
                        {
                            binaryTree.addNode(binaryTree.Root, node);
                        }
                        symbol = "";
                    }
                    //二叉树元素按后序遍历进入队列
                    Queue<string> queue_letter = new Queue<string>();
                    binaryTree.EnqueueNode(binaryTree.Root, queue_letter);
                    //出队入栈，并计算
                    Stack<string> stack_letter = new Stack<string>();
                    string[] symbolC = new string[4] { "+", "-", "*", "/" };
                    while (queue_letter.Count > 0)
                    {
                        string letter = queue_letter.Dequeue();
                        if (symbolC.Contains(letter))
                        {
                            if (stack_letter.Count > 0)
                            {
                                int a;

                                switch (letter)
                                {
                                    case "+":
                                        a = int.Parse(stack_letter.Pop());
                                        a += int.Parse(stack_letter.Pop());
                                        stack_letter.Push(a.ToString());
                                        break;
                                    case "-":
                                        a = - int.Parse(stack_letter.Pop()) + int.Parse(stack_letter.Pop());
                                        stack_letter.Push(a.ToString());
                                        break;
                                    case "*":
                                        a = int.Parse(stack_letter.Pop());
                                        a *= int.Parse(stack_letter.Pop());
                                        stack_letter.Push(a.ToString());
                                        break;
                                    case "/":
                                        a = int.Parse(stack_letter.Pop());
                                        int b = int.Parse(stack_letter.Pop());
                                        a = b / a;
                                        stack_letter.Push(a.ToString());
                                        break;
                                }
                            }
                        }
                        else
                        {
                            stack_letter.Push(letter);
                        }
                    }
                    if (stack_letter.Count > 0)
                    {
                        Console.WriteLine("result = " + stack_letter.Pop());
                    }
                }catch(Exception e)
                {
                    Console.Write(e.ToString() + "\n");
                }
            }
        }
    }
}
