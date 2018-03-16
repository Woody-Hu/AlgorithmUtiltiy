using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LRUCache
{
    /// <summary>
    /// 缓存节点
    /// </summary>
    /// <typeparam name="X">Key</typeparam>
    /// <typeparam name="Y">Value</typeparam>
    internal class LRULindNode<X,Y>
    {
        /// <summary>
        /// 此节点的Key值
        /// </summary>
        private X m_thisKey;

        /// <summary>
        /// 此节点的Value值
        /// </summary>
        private Y m_thisValue;

        /// <summary>
        /// 前向节点
        /// </summary>
        private LRULindNode<X, Y> m_preNode = null;

        /// <summary>
        /// 后向节点
        /// </summary>
        private LRULindNode<X, Y> m_postNode = null;

        /// <summary>
        /// 此节点的Key值
        /// </summary>
        internal X ThisKey
        {
            get
            {
                return m_thisKey;
            }

            set
            {
                m_thisKey = value;
            }
        }

        /// <summary>
        /// 此节点的Value值
        /// </summary>
        internal Y ThisValue
        {
            get
            {
                return m_thisValue;
            }

            set
            {
                m_thisValue = value;
            }
        }

        /// <summary>
        /// 前向节点
        /// </summary>
        internal LRULindNode<X, Y> PreNode
        {
            get
            {
                return m_preNode;
            }

            set
            {
                m_preNode = value;
            }
        }

        /// <summary>
        /// 后向节点
        /// </summary>
        internal LRULindNode<X, Y> PostNode
        {
            get
            {
                return m_postNode;
            }

            set
            {
                m_postNode = value;
            }
        }
    }
}
