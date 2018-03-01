using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RBTree
{
    public class RBTreeClass<T>
        where T : IComparable
    {
        private RBTreeNode<T> m_thisRoot = null;

        internal RBTreeNode<T> RootNode
        {
            get
            {
                return m_thisRoot;
            }

            set
            {
                m_thisRoot = value;
            }
        }

        public bool Find(T inputValue)
        {
            if (InputCheck(inputValue))
            {
                return false;
            }

            RBTreeNode<T> lastParent = null;

            var findedNode = FindLoop(RootNode, inputValue,ref lastParent);

            return (null != findedNode && !findedNode.IfIsRemoved);
        }

        private bool InputCheck(T inputValue)
        {
            return null == m_thisRoot || null == inputValue;
        }

        private RBTreeNode<T> FindLoop(RBTreeNode<T> nowNode,T inputValue,ref RBTreeNode<T> lastFindParentNode)
        {
            if (null == nowNode)
            {
                return null;
            }

            if (ChangeColor(nowNode))
            {
                RoateNode(nowNode);
            }

            var compareValue = inputValue.CompareTo(nowNode.ThisValue);

            if (compareValue == 0)
            {
                return nowNode;
            }
            else if (compareValue > 0)
            {
                lastFindParentNode = nowNode;
                return FindLoop(nowNode.RightNode, inputValue,ref lastFindParentNode);
            }
            else
            {
                lastFindParentNode = nowNode;
                return FindLoop(nowNode.LeftNode, inputValue, ref lastFindParentNode);
            }
        }

        public bool Insert(T inputValue)
        {
            if (null == inputValue)
            {
                return false;
            }

            if (null == RootNode)
            {
                RootNode = new RBTreeNode<T>(inputValue,false);
                return true;
            }

            RBTreeNode<T> lastParent = null;

            var findedValue = FindLoop(RootNode, inputValue, ref lastParent);

            if (null != findedValue)
            {
                if (findedValue.IfIsRemoved)
                {
                    findedValue.ThisValue = inputValue;
                    findedValue.IfIsRemoved = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (null != lastParent)
            {
                RBTreeNode<T> tempNode = new RBTreeNode<T>(inputValue);

                if (inputValue.CompareTo(lastParent.ThisValue) > 0)
                {
                    tempNode.Parent = lastParent;
                    lastParent.RightNode = tempNode;
                }
                else if (inputValue.CompareTo(lastParent.ThisValue) < 0)
                {
                    tempNode.Parent = lastParent;
                    lastParent.LeftNode = tempNode;
                }
                RoateNode(tempNode);

                return true;
            }

            return false;
        }

        private void RoateNode(RBTreeNode<T> inputNode)
        {
            var parentNode = inputNode.Parent;

            var parentparentNode = parentNode.Parent;

            if (!parentNode.IfIsRed)
            {
                return;
            }

            if ((inputNode == parentNode.LeftNode && parentNode == parentparentNode.LeftNode) 
                || 
                (inputNode == parentNode.RightNode && parentNode == parentparentNode.RightNode))
            {
                parentparentNode.ChangeColor();
                parentNode.ChangeColor();

                if (parentNode == parentparentNode.LeftNode)
                {
                    parentparentNode.RightRoate(this);
                }
                else
                {
                    parentparentNode.LeftRoate(this);
                }

              
            }
            else
            {
                parentparentNode.ChangeColor();
                inputNode.ChangeColor();

                if (inputNode == parentNode.LeftNode)
                {
                    parentNode.RightRoate(this);
                }
                else
                {
                    parentNode.LeftRoate(this);
                }

                if (inputNode == parentparentNode.LeftNode)
                {
                    parentparentNode.RightRoate(this);
                }
                else
                {
                    parentparentNode.LeftRoate(this);
                }
            }

            ChangeColor(parentparentNode.Parent);
        }

        private bool ChangeColor(RBTreeNode<T> inputNode)
        {

            if (!inputNode.IfIsRed && null != inputNode.LeftNode && null != inputNode.RightNode && inputNode.LeftNode.IfIsRed && inputNode.RightNode.IfIsRed)
            {
                inputNode.LeftNode.ChangeColor();
                inputNode.RightNode.ChangeColor();

                if (inputNode != RootNode)
                {
                    inputNode.ChangeColor();
                    return true;
                }
                else
                {
      
                    return false;
                }
               
            }

            return false;


        }

        public bool Delete(T inputValue)
        {
            if (InputCheck(inputValue))
            {
                return false;
            }

            RBTreeNode<T> lastParent = null;

            var findedValue = FindLoop(RootNode, inputValue, ref lastParent);

            if (null == findedValue)
            {
                return false;
            }
            else
            {
                if (findedValue.IfIsRemoved )
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            
        }
    }
}
