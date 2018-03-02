using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDTree
{
    /// <summary>
    /// 多维数据接口
    /// </summary>
    public interface IDimensionValueBean
    {
        /// <summary>
        /// 获得总维度数量
        /// </summary>
        /// <returns></returns>
        int GetSumDimensionNumber();

        /// <summary>
        /// 利用维度索引获得相应的数据描述
        /// </summary>
        /// <param name="inputIndex">输入的维度索引</param>
        /// <returns>获得的双精度数据描述</returns>
        double GetValueAtDimensionIndex(int inputIndex);

    }
}
