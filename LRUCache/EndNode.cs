using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LRUCache
{
    /// <summary>
    /// 端点节点多态
    /// </summary>
    /// <typeparam name="X"></typeparam>
    /// <typeparam name="Y"></typeparam>
    internal class EndNode<X, Y> : LRULindNode<X,Y>
    {
    }
}
