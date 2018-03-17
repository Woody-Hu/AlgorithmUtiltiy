using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LRUCache
{
    /// <summary>
    /// LRU管理器
    /// </summary>
    /// <typeparam name="X">使用的键值泛型</typeparam>
    /// <typeparam name="Y">使用的值值泛型</typeparam>
    public class LRUCacheManger<X, Y> : IEnumerable<KeyValuePair<X, Y>>
    {
        #region 私有字段
        /// <summary>
        /// 哈希键值-节点结构
        /// </summary>
        private Dictionary<X, LRULindNode<X, Y>> m_useCache;
        /// <summary>
        /// 使用的字典缓存（迭代用）
        /// </summary>
        private Dictionary<X, Y> m_useBackDic = null;
        /// <summary>
        /// 当前数据
        /// </summary>
        private int m_count;
        /// <summary>
        /// 当前容量
        /// </summary>
        private int m_capacity;
        /// <summary>
        /// 当前的首节点
        /// </summary>
        LRULindNode<X, Y> m_headNode = null;
        /// <summary>
        /// 当前的尾节点
        /// </summary>
        LRULindNode<X, Y> m_tailNode = null;
        #endregion
        /// <summary>
        /// 构造LRU缓存
        /// </summary>
        /// <param name="inputCapacity">容量</param>
        /// <exception cref="ArgumentException">容量小于等于1</exception>
        public LRUCacheManger(int inputCapacity)
        {
            if (inputCapacity <= 1)
            {
                throw new ArgumentException();
            }
            m_useCache = new Dictionary<X, LRULindNode<X, Y>>();
            m_count = 0;
            m_capacity = inputCapacity;
            m_headNode = new EndNode<X, Y>();
            m_tailNode = new EndNode<X, Y>();
            m_headNode.PostNode = m_tailNode;
            m_headNode.PreNode = null;
            m_tailNode.PostNode = null;
            m_tailNode.PreNode = m_headNode;
        }
        /// <summary>
        /// 是否包含键值
        /// </summary>
        /// <param name="inputKey">输入的键值</param>
        /// <returns>是/否</returns>
        public bool IfContainsKey(X inputKey)
        {
            return m_useCache.ContainsKey(inputKey);
        }
        /// <summary>
        /// 尝试获取
        /// </summary>
        /// <param name="inputKey">输入的Key值</param>
        /// <param name="findValue">找到的值</param>
        /// <returns>是否成功</returns>
        public bool TryGet(X inputKey, out Y findValue)
        {
            findValue = default(Y);
            if (!IfContainsKey(inputKey))
            {
                return false;
            }
            else
            {
                findValue = Get(inputKey);
                return true;
            }
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="inputKey">输入的键值</param>
        /// <returns>对应值</returns>
        public Y Get(X inputKey)
        {
            if (!IfContainsKey(inputKey))
            {
                throw new ArgumentException();
            }
            LRULindNode<X, Y> useNode = m_useCache[inputKey];
            MoveToHead(useNode);
            return useNode.ThisValue;
        }
        /// <summary>
        /// 设置储存值(重复键值则复写）
        /// </summary>
        /// <param name="inputKey">输入的Key</param>
        /// <param name="inputValue">输入的Value</param>
        public void Set(X inputKey, Y inputValue)
        {
            //若存在则复写
            if (IfContainsKey(inputKey))
            {
                LRULindNode<X, Y> useNode = m_useCache[inputKey];
                MoveToHead(useNode);
                useNode.ThisValue = inputValue;
            }
            //不存在则添加
            else
            {
                LRULindNode<X, Y> addedNode = new LRULindNode<X, Y>();
                addedNode.ThisKey = inputKey;
                addedNode.ThisValue = inputValue;
                AddNode(addedNode);
                //添加到字典
                m_useCache.Add(inputKey, addedNode);
                //增加数量
                m_count++;
                if (m_count > m_capacity)
                {
                    var nodeNeedRemove = Poptail();
                    RemoveNodeInManger(nodeNeedRemove);
                }
            }
            //清空迭代缓存
            m_useBackDic = null;
        }
        /// <summary>
        /// 移除节点
        /// </summary>
        /// <param name="inputKey"></param>
        public void Remove(X inputKey)
        {
            if (!IfContainsKey(inputKey))
            {
                return;
            }
            else
            {
                var nodeNeedRemove = m_useCache[inputKey];
                RemoveNodeInManger(nodeNeedRemove);
            }
        }
        /// <summary>
        /// 迭代接口
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<X, Y>> GetEnumerator()
        {
            return PrepareEnumeratorDic().GetEnumerator();
        }
        /// <summary>
        /// 迭代接口
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return PrepareEnumeratorDic().GetEnumerator();
        }

        #region 私有方法组
        /// <summary>
        /// 获取迭代用字典
        /// </summary>
        /// <returns></returns>
        private Dictionary<X, Y> PrepareEnumeratorDic()
        {
            if (null == m_useBackDic)
            {
                m_useBackDic = (from n in m_useCache select new KeyValuePair<X, Y>(n.Key, n.Value.ThisValue)).ToDictionary(k => k.Key, k => k.Value);
            }
            return m_useBackDic;
        }
        /// <summary>
        /// 从缓存系统中移除节点
        /// </summary>
        /// <param name="nodeNeedRemove"></param>
        private void RemoveNodeInManger(LRULindNode<X, Y> nodeNeedRemove)
        {
            RemoveNodeInLink(nodeNeedRemove);
            m_useCache.Remove(nodeNeedRemove.ThisKey);
            m_count--;
        }
        /// <summary>
        /// 新添一个节点到顶端
        /// </summary>
        /// <param name="inputNode"></param>
        private void AddNode(LRULindNode<X, Y> inputNode)
        {
            var nowHeadNode = m_headNode.PostNode;
            m_headNode.PostNode = inputNode;
            inputNode.PreNode = m_headNode;
            nowHeadNode.PreNode = inputNode;
            inputNode.PostNode = nowHeadNode;
        }
        /// <summary>
        /// 移动一个节点到顶端
        /// </summary>
        /// <param name="inputNode"></param>
        private void MoveToHead(LRULindNode<X, Y> inputNode)
        {
            //移除节点
            RemoveNodeInLink(inputNode);
            //新增节点
            AddNode(inputNode);
        }
        /// <summary>
        /// 从链表中移除节点
        /// </summary>
        /// <param name="inputNode"></param>
        private void RemoveNodeInLink(LRULindNode<X, Y> inputNode)
        {
            var nowPreNode = inputNode.PreNode;
            var nowPostNode = inputNode.PostNode;
            inputNode.PreNode = null;
            inputNode.PostNode = null;
            nowPreNode.PostNode = nowPostNode;
            nowPostNode.PreNode = nowPreNode;
        }
        /// <summary>
        /// 弹出尾节点
        /// </summary>
        /// <returns>尾节点</returns>
        private LRULindNode<X, Y> Poptail()
        {
            LRULindNode<X, Y> useNode = m_tailNode.PreNode;
            return useNode;
        }
        #endregion
    }
}
