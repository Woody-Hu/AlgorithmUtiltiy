using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RBTree
{
    public class RBTreeNode<T>
        where T:IComparable
    {
        T m_thisValue;

        bool m_bIfIsRemoved;

        bool m_bIfIsRed;

        RBTreeNode<T> m_leftNode;

        RBTreeNode<T> m_rightNode;

        RBTreeNode<T> m_parent;

        public T ThisValue
        {
            get
            {
                return m_thisValue;
            }

            internal set
            {
                m_thisValue = value;
            }
        }

        public bool IfIsRed
        {
            get
            {
                return m_bIfIsRed;
            }

            internal set
            {
                m_bIfIsRed = value;
            }
        }

        public RBTreeNode<T> LeftNode
        {
            get
            {
                return m_leftNode;
            }

            internal set
            {
                m_leftNode = value;
            }
        }

        public RBTreeNode<T> RightNode
        {
            get
            {
                return m_rightNode;
            }

            internal set
            {
                m_rightNode = value;
            }
        }

        public RBTreeNode<T> Parent
        {
            get
            {
                return m_parent;
            }

            internal set
            {
                m_parent = value;
            }
        }

        public bool IfIsRemoved
        {
            get
            {
                return m_bIfIsRemoved;
            }

            internal set
            {
                m_bIfIsRemoved = value;
            }
        }

        public RBTreeNode(T inputValue,bool ifIsRed = true)
        {
            ThisValue = inputValue;
            m_bIfIsRed = ifIsRed;
            m_bIfIsRemoved = false;
        }

        internal void LeftRoate(RBTreeClass<T> useTree)
        {
            var thisRightValue = this.RightNode;

            var thisRigthLeft = this.RightNode.LeftNode;

            this.RightNode = thisRigthLeft;

            if (null != this.RightNode)
            {
                this.RightNode.Parent = this;
            }

            thisRightValue.Parent = this.Parent;

            if (this.Parent.LeftNode == this)
            {
                this.Parent.LeftNode = thisRigthLeft;
            }
            else
            {
                this.Parent.RightNode = thisRigthLeft;
            }

            thisRightValue.LeftNode = this;

            this.Parent = thisRightValue;

            if (this == useTree.RootNode)
            {
                useTree.RootNode = this.Parent;
            }
        }

        internal void RightRoate(RBTreeClass<T> useTree)
        {
            var thisLeftValue = this.LeftNode;

            var thisLeftRightValue = thisLeftValue.RightNode;

            this.LeftNode = thisLeftRightValue;

            if (null != this.LeftNode)
            {
                this.LeftNode.Parent = this;
            }

            thisLeftValue.Parent = this.Parent;

            if (this.Parent.LeftNode == this)
            {
                this.Parent.LeftNode = thisLeftValue;
            }
            else
            {
                this.Parent.RightNode = thisLeftValue;
            }

            thisLeftValue.RightNode = this;

            this.Parent = thisLeftValue;

            if (this == useTree.RootNode)
            {
                useTree.RootNode = this.Parent;
            }

        }
        
    }
}
