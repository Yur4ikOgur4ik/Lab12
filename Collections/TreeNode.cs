using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    public class TreeNode<T>
    {
        public T? Data { get; set; }
        public TreeNode<T> Left { get; set; }
        public TreeNode<T> Right { get; set; }
        public int Height { get; set; }

        public TreeNode()
        {
            Data = default(T);
            Left = null;
            Right = null;
            Height = 1;
        }

        public TreeNode(T data)
        {
            Data = data;
            Left = null;
            Right = null;
            Height = 1;
        }
    }
}
