using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDTree
{
    /// <summary>
    /// KD树节点封装
    /// </summary>
    internal class KDTreeNode
    {
        #region 私有字段
        /// <summary>
        /// 父节点
        /// </summary>
        private KDTreeNode parent = null;

        /// <summary>
        /// 左子节点
        /// </summary>
        private KDTreeNode leftChild = null;

        /// <summary>
        /// 右子节点
        /// </summary>
        private KDTreeNode rightChild = null;

        /// <summary>
        /// 此节点的数值
        /// </summary>
        private IDimensionValueBean thisDataValue = null;

        /// <summary>
        /// 此节点的总维度数
        /// </summary>
        int sumDiminsionNumber;

        /// <summary>
        /// 此节点使用的维度索引
        /// </summary>
        int nowDimisionIndex;

        /// <summary>
        /// 是否删除标志
        /// </summary>
        private bool isDeleteTag = false;
        #endregion

        #region 公开属性
        /// <summary>
        /// 父节点
        /// </summary>
        internal KDTreeNode Parent
        {
            get
            {
                return parent;
            }

            set
            {
                parent = value;
            }
        }

        /// <summary>
        /// 左子节点
        /// </summary>
        internal KDTreeNode LeftChild
        {
            get
            {
                return leftChild;
            }

            set
            {
                leftChild = value;
            }
        }

        /// <summary>
        /// 右子节点
        /// </summary>
        internal KDTreeNode RightChild
        {
            get
            {
                return rightChild;
            }

            set
            {
                rightChild = value;
            }
        }

        /// <summary>
        /// 此节点的数值
        /// </summary>
        internal IDimensionValueBean ThisDataValue
        {
            get
            {
                return thisDataValue;
            }

            set
            {
                thisDataValue = value;
            }
        }

        /// <summary>
        /// 此节点的总维度数
        /// </summary>
        internal int SumDiminsionNumber
        {
            get
            {
                return sumDiminsionNumber;
            }

            private set
            {
                sumDiminsionNumber = value;
            }
        }

        /// <summary>
        /// 此节点使用的维度索引
        /// </summary>
        internal int NowDimisionIndex
        {
            get
            {
                return nowDimisionIndex;
            }

            private set
            {
                nowDimisionIndex = value;
            }
        }

        /// <summary>
        /// 节点是否删除标志
        /// </summary>
        internal bool IsDeleteTag
        {
            get
            {
                return isDeleteTag;
            }

            set
            {
                isDeleteTag = value;
            }
        }
        #endregion

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="inputValue">核心数值</param>
        /// <param name="useIndex">使用的维度索引</param>
        internal KDTreeNode(IDimensionValueBean inputValue,int useIndex)
        {
            this.thisDataValue = inputValue;
            this.sumDiminsionNumber = this.thisDataValue.GetSumDimensionNumber();
            this.nowDimisionIndex = useIndex;
        }

        /// <summary>
        /// 获得子节点
        /// </summary>
        /// <param name="ifLeftChild">是否是左子节点</param>
        /// <returns>相应方向的子节点</returns>
        internal KDTreeNode GetChildNode(bool ifLeftChild)
        {
            if (ifLeftChild)
            {
                return LeftChild;
            }
            else
            {
                return rightChild;
            }
        }

        /// <summary>
        /// 设置相应的子节点
        /// </summary>
        /// <param name="ifLeftChild">是否是左子节点</param>
        /// <param name="inputTreeNode">输入的节点</param>
        internal void SetChildNode(bool ifLeftChild,KDTreeNode inputTreeNode)
        {
            //获得当前的相应子节点
            KDTreeNode nowNode = GetChildNode(ifLeftChild);

            //插入位置子节点声明
            KDTreeNode tempLeftNode = null;
            KDTreeNode tempRightNode = null;

            //若当前位置有子节点
            if (null != nowNode)
            {
                //临时变量赋值
                tempLeftNode = nowNode.LeftChild;
                tempRightNode = nowNode.RightChild;
                //清空目前节点与树的连接关系
                nowNode.Parent = null;
                nowNode.LeftChild = null;
                nowNode.RightChild = null;
            }

            //将输入节点的父节点设为自身
            inputTreeNode.Parent = this;

            //将输入节点与父节点连接
            if (ifLeftChild)
            {
                this.LeftChild = inputTreeNode;
            }
            else
            {
                this.RightChild = inputTreeNode;
            }

            //将输入节点与子节点连接
            inputTreeNode.LeftChild = tempLeftNode;
            inputTreeNode.RightChild = tempRightNode;
            
        }

        /// <summary>
        /// 比较输入多维数据是否与此节点数据相同
        /// </summary>
        /// <param name="inputValue">输入的多维数据</param>
        /// <returns>是否相同</returns>
        internal bool IfEqula(IDimensionValueBean inputValue)
        {
            return UtilityMethod.IfDDimensionIsEqula(this.thisDataValue, inputValue);
        }

        /// <summary>
        /// 比较输入的多维节点是否与此节点数据相同
        /// </summary>
        /// <param name="inputValue">输入的多维节点</param>
        /// <returns>是否相同</returns>
        internal bool IfEqula(KDTreeNode inputValue)
        {
            return IfEqula(inputValue.thisDataValue);
        }

    }
}
