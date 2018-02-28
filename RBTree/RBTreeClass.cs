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

            var findedNode = FindLoop(RootNode, inputValue);

            return (null != findedNode && !findedNode.IfIsRemoved);
        }

        private bool InputCheck(T inputValue)
        {
            return null == m_thisRoot || null == inputValue;
        }

        private RBTreeNode<T> FindLoop(RBTreeNode<T> nowNode,T inputValue)
        {
            if (null == nowNode)
            {
                return null;
            }

            var compareValue = inputValue.CompareTo(nowNode.ThisValue);

            if (compareValue == 0)
            {
                return nowNode;
            }
            else if (compareValue > 0)
            {
                return FindLoop(nowNode.RightNode, inputValue);
            }
            else
            {
                return FindLoop(nowNode.LeftNode, inputValue);
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
                RootNode = new RBTreeNode<T>(inputValue);
                return true;
            }

            var findedValue = FindLoop(RootNode, inputValue);

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

            throw new NotImplementedException();
        }

        public bool Delete(T inputValue)
        {
            if (InputCheck(inputValue))
            {
                return false;
            }
            throw new NotImplementedException();
        }
    }
}
