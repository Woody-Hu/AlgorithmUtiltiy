using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDTree
{
    /// <summary>
    /// KD树
    /// </summary>
    public class KDTree
    {
        #region 私有字段
        /// <summary>
        /// 总节点数
        /// </summary>
        private int nodeCount = -1;

        /// <summary>
        /// 当前使用的维度索引
        /// </summary>
        private int nowUseIndex = 0;

        /// <summary>
        /// 跟节点
        /// </summary>
        private KDTreeNode rootNode = null;

        /// <summary>
        /// 总维度数
        /// </summary>
        private int thisDimensionNumber;

        /// <summary>
        /// 在递归中是否进行输入检查
        /// </summary>
        private bool ifUseRecursionCheck = false;

        /// <summary>
        /// 在维度数值相同的时采取的策略
        /// （true：视为小于;false:视为大于）
        /// </summary>
        private bool ifValueEqualAsLess = true;

        /// <summary>
        /// 在插入时当存在同值节点时是否替代节点值（若为否则会不添加）
        /// </summary>
        private bool ifEqualValueReplace = true;
        #endregion

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="inputSumDimensionNumber"></param>
        public KDTree(int inputSumDimensionNumber)
        {
            //输入维度检查
            if (0 >= inputSumDimensionNumber)
            {
                throw new ArgumentException();
            }
            thisDimensionNumber = inputSumDimensionNumber;
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="inputValue">输入的值</param>
        public KDTree(IDimensionValueBean inputValue)
        {
            InsertNoneBlance(inputValue);
        }

        /// <summary>
        /// 寻找一个节点
        /// </summary>
        /// <param name="inputValue">输入的多维封装</param>
        /// <returns>找到的节点</returns>
        public IDimensionValueBean Search(IDimensionValueBean inputValue)
        {
            KDTreeNode tempNode = SearchNode(inputValue);
            if (null == tempNode)
            {
                return null;
            }
            else
            {
                return tempNode.ThisDataValue;
            }
        }

        /// <summary>
        /// 节点插入(不进行平衡调整）
        /// </summary>
        /// <param name="inputValue">输入的多维数据值</param>
        /// <returns>插入结果</returns>
        public bool InsertNoneBlance(IDimensionValueBean inputValue)
        {
            //若根节点为Null
            if (null == rootNode)
            {
                inputCheck(inputValue, false);
                int tempIndex = GetUseDimensionIndex();
                KDTreeNode tempTreeNode = new KDTreeNode(inputValue, tempIndex);
                rootNode = tempTreeNode;
                nodeCount++;
                return true;
            }
            else
            {
                inputCheck(inputValue);
                return insertLoopMethod(inputValue);
            }
        }

        /// <summary>
        /// 节点删除(不进行平衡调整)
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public bool DeleteNoneBlace(IDimensionValueBean inputValue)
        {
            //寻找节点
            KDTreeNode findedNode = SearchNode(inputValue);
            //若没找到或目前节点已删除
            if (null == findedNode || findedNode.IsDeleteTag == true)
            {
                return false;
            }
            //正常情况仅调整标签
            else
            {
                findedNode.IsDeleteTag = true;
                nodeCount--;
                return true;
            }
        }

        #region 私有方法

        /// <summary>
        /// 寻找树中相应节点
        /// </summary>
        /// <param name="inputValue">输入的多维数据</param>
        /// <returns>找到的节点</returns>
        private KDTreeNode SearchNode(IDimensionValueBean inputValue)
        {
            //输入检查
            inputCheck(inputValue);
            return searchRecursion(rootNode, inputValue);
        }

        /// <summary>
        /// 添加节点的递归方法
        /// </summary>
        /// <param name="inputValue">输入的值</param>
        /// <returns></returns>
        private bool insertLoopMethod(IDimensionValueBean inputValue)
        {
            //当前递归节点
            KDTreeNode currentNode = rootNode;
            //当前节点的父节点
            KDTreeNode parentNode = null;
            //临时节点
            KDTreeNode tempNode = null;
            //父节点的索引
            int tempDimensionIndex = -1;

            //当前节点应当使用的索引
            int tempNowIndex = -1;

            //表征节点前进方向是否是向左节点
            bool ifIsLeftTag = true;

            double tempParentValue = 0.0d;
            double tempCurrentValue = 0.0d;

            CompareResultEnum tempCompareResult;

            //递归寻找添加位点
            while (true)
            {
                //当前递归层级节点变为父节点
                parentNode = currentNode;
                //获得父节点的索引
                tempDimensionIndex = parentNode.NowDimisionIndex;
                //获得父节点的值
                tempParentValue = parentNode.ThisDataValue.
                    GetValueAtDimensionIndex(tempDimensionIndex);
                //获取此节点的值
                tempCurrentValue = inputValue.
                    GetValueAtDimensionIndex(tempDimensionIndex);
                //比较节点值
                tempCompareResult = UtilityMethod.
                    CompareDoulbe(tempCurrentValue, tempParentValue);
                //按相等策略调整
                tempCompareResult = equalStrategy(tempCompareResult);

                //小于:左子树方向
                if (tempCompareResult == CompareResultEnum.Less)
                {
                    ifIsLeftTag = true;
                }
                //其它右子树方向
                else
                {
                    ifIsLeftTag = false;
                }

                //获取父节点相应位置处的子节点
                currentNode = parentNode.GetChildNode(ifIsLeftTag);
                //获得子节点的索引
                tempNowIndex = GetUseDimensionInde(tempDimensionIndex);
                if (null == currentNode)
                {
                    tempNode = new KDTreeNode(inputValue, tempNowIndex);
                    parentNode.SetChildNode(ifIsLeftTag, tempNode);
                    break;
                }

                //若多维值相同
                if (UtilityMethod.IfDDimensionIsEqula(currentNode.ThisDataValue,inputValue))
                {
                    //若需要替换或当前节点是已删除状态
                    if (ifEqualValueReplace || currentNode.IsDeleteTag)
                    {
                        currentNode.ThisDataValue = inputValue;
                        currentNode.IsDeleteTag = false;
                    }
                    break;
                }
            }
            nodeCount++;
            return true;
        }

        /// <summary>
        /// 输入检查
        /// </summary>
        /// <param name="inputValue"></param>
        private void inputCheck(IDimensionValueBean inputValue,bool ifCheckDimensionNumber = true)
        {
            //若需检查维度数
            if (ifCheckDimensionNumber)
            {  
                if (null == inputValue
               || inputValue.GetSumDimensionNumber() != nodeCount
               || 0 >= inputValue.GetSumDimensionNumber())
                {
                    throw new ArgumentException();
                }
            }
            else
            {
                if (null == inputValue
                    || 0 >= inputValue.GetSumDimensionNumber())
                {
                    throw new ArgumentException();
                }
            }
           
        }

        /// <summary>
        /// 递归寻找方法
        /// </summary>
        /// <param name="inputNode">输入的节点</param>
        /// <param name="inputValue">输入的值</param>
        /// <returns>找到的值若为Null则未找到</returns>
        private KDTreeNode searchRecursion(KDTreeNode inputNode, IDimensionValueBean inputValue)
        {
            if (null == inputNode)
            {
                return null;
            }
            //若两节点相同
            else if
                (UtilityMethod.IfDDimensionIsEqula
                (inputNode.ThisDataValue, inputValue, ifUseRecursionCheck))
            {
                //若节点已删除
                if (inputNode.IsDeleteTag)
                {
                    return null;
                }
                else
                {
                    return inputNode;
                }
            }
            else
            {
                //获取维度索引
                int dimensionIndex = inputNode.NowDimisionIndex;
                //获取多维数据封装
                IDimensionValueBean nodeValue = inputNode.ThisDataValue;
                //获得当前节点维度的值
                double tempDimensionValue = nodeValue.GetValueAtDimensionIndex(dimensionIndex);
                //获得输入数值在节点维度的值
                double tempInputValue = inputValue.GetValueAtDimensionIndex(dimensionIndex);

                //比较两值
                CompareResultEnum compareResult = UtilityMethod.CompareDoulbe(tempInputValue, tempDimensionValue);
                //比较结果调整
                compareResult = equalStrategy(compareResult);

                //次轮迭代使用的节点
                KDTreeNode nextRoundNode = null;

                //若小于去左节点
                if (compareResult == CompareResultEnum.Less)
                {
                    nextRoundNode = inputNode.LeftChild;
                }
                //其它去右节点
                else
                {
                    nextRoundNode = inputNode.RightChild;
                }
                //递归执行
                return searchRecursion(nextRoundNode, inputValue);
            }
        }

        /// <summary>
        /// 相等策略调整
        /// </summary>
        /// <param name="inputResult">输入的比较结果</param>
        /// <returns>调整后的结果</returns>
        private CompareResultEnum equalStrategy(CompareResultEnum inputResult)
        {
            if (CompareResultEnum.Equal != inputResult)
            {
                return inputResult;
            }

            //相等视为小于
            if (true == ifValueEqualAsLess)
            {
                return CompareResultEnum.Less;
            }
            //相等视为大于
            else
            {
                return CompareResultEnum.Large;
            }
        } 

        /// <summary>
        /// 获取一个需要使用的索引
        /// </summary>
        /// <returns></returns>
        private int GetUseDimensionIndex(List<int> lstHasUsedIndex = null)
        {
            int tempReturnValue = nowUseIndex;

            nowUseIndex = ++nowUseIndex % (nodeCount - 1);

            return tempReturnValue;
        }

        /// <summary>
        /// 获取输入索引的下一层索引
        /// </summary>
        /// <param name="input">输入的索引</param>
        /// <returns>下层索引</returns>
        private int GetUseDimensionInde(int input)
        {
            int tempValue = input;
            tempValue = (tempValue + 1)% (nodeCount - 1);

            return tempValue;
        }

        #endregion
    }
}
