using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDTree
{
    /// <summary>
    /// 静态方法
    /// </summary>
    internal static class UtilityMethod
    {
        /// <summary>
        /// 双精度容差
        /// </summary>
        private static double tolerance = 1e-6;

        /// <summary>
        /// 比较两个双精度浮点数
        /// </summary>
        /// <param name="inputOne">输入1</param>
        /// <param name="inputTwo">输入2</param>
        /// <returns>比较结果枚举</returns>
        internal static CompareResultEnum CompareDoulbe(double inputOne,double inputTwo)
        {
            if (Math.Abs(inputOne - inputTwo) <= tolerance)
            {
                return CompareResultEnum.Equal;
            }
            else if (inputOne < inputTwo)
            {
                return CompareResultEnum.Less;
            }
            else
            {
                return CompareResultEnum.Large;
            }
        }

        /// <summary>
        /// 比较两个多维数据封装是否相同
        /// </summary>
        /// <param name="inputOne">输入1</param>
        /// <param name="inputTwo">输入2</param>
        /// <param name="ifCheck">是否进行输入检查</param>
        /// <returns>是否相同</returns>
        /// <exception cref="ArgumentException">输入为Null或维度不相同或维度为0</exception>
        internal static bool IfDDimensionIsEqula
            (IDimensionValueBean inputOne,IDimensionValueBean inputTwo,bool ifCheck = true)
        {
            if (ifCheck)
            {
                inputCheck(inputOne, inputTwo);
            }

            bool returnValue = true;

            int sumNumber = inputOne.GetSumDimensionNumber();

            //检查各维度
            for (int tempIndex = 0; tempIndex < sumNumber; tempIndex++)
            {
                //当出现不等时
                if (CompareResultEnum.Equal != CompareDoulbe
                    (inputOne.GetValueAtDimensionIndex(tempIndex),
                    inputTwo.GetValueAtDimensionIndex(tempIndex)))
                {
                    returnValue = false;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// 输入检查
        /// </summary>
        /// <param name="inputOne">输入1</param>
        /// <param name="inputTwo">输入2</param>
        /// <exception cref="ArgumentException">输入为Null或维度不相同或维度为0</exception>
        private static void inputCheck(IDimensionValueBean inputOne, IDimensionValueBean inputTwo)
        {
            //null值检查 维度相同检查
            if (null == inputOne || null == inputTwo
                || inputOne.GetSumDimensionNumber() != inputTwo.GetSumDimensionNumber()
                || 0 >= inputOne.GetSumDimensionNumber()
                )
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// 计算两个多维数据之间的欧几里得距离
        /// </summary>
        /// <param name="inputOne">输入1</param>
        /// <param name="inputTwo">输入2</param>
        /// <returns>欧几里得距离</returns>
        /// <exception cref="ArgumentException">输入为Null或维度不相同或维度为0</exception>
        internal static double CalculateDistanceByEuclidean
            (IDimensionValueBean inputOne, IDimensionValueBean inputTwo, bool ifCheck = true)
        {

            if (ifCheck)
            {
                inputCheck(inputOne, inputTwo);
            }

            double returnValue = 0.0d;

            int tempSize = inputOne.GetSumDimensionNumber();

            //维度距离累计
            for (int tempIndex = 0; tempIndex < tempSize; tempIndex++)
            {
                returnValue = returnValue +  Math.Pow
                    ((inputOne.GetValueAtDimensionIndex(tempIndex) - 
                    inputTwo.GetValueAtDimensionIndex(tempIndex)),2.0D);
            }

            //开根号
            returnValue = Math.Pow(returnValue, 0.5D);

            return returnValue;
        }
    }
}
