using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDTree
{
    /// <summary>
    /// 临时中间件封装
    /// </summary>
    internal class MidValuePacker : IDimensionValueBean
    {
        #region 私有字段
        /// <summary>
        /// 字段：多维值
        /// </summary>
        private List<double> lstThisValue;

        /// <summary>
        /// 字段：维度数
        /// </summary>
        private int thisSumDimension; 
        #endregion

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="inputValues">输入的double列表</param>
        internal MidValuePacker(List<double> inputValues)
        {
            if (null == inputValues || 0 == inputValues.Count)
            {
                throw new ArgumentException();
            }

            LstThisValue = inputValues;
            thisSumDimension = LstThisValue.Count;

        }

        /// <summary>
        ///多维值
        /// </summary>
        public List<double> LstThisValue
        {
            get
            {
                return lstThisValue;
            }

            private set
            {
                lstThisValue = value;
            }
        }

        /// <summary>
        /// 接口方法：获取维度总数
        /// </summary>
        /// <returns></returns>
        public int GetSumDimensionNumber()
        {
            return thisSumDimension;
        }

        /// <summary>
        /// 接口方法：获取对应维度的值
        /// </summary>
        /// <param name="inputIndex"></param>
        /// <returns></returns>
        public double GetValueAtDimensionIndex(int inputIndex)
        {
            return LstThisValue[inputIndex];
        }


    }
}
