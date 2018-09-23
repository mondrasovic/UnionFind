using System;
using System.Collections.Generic;

namespace UnionFind
{
	internal class Node<T> where T : System.IComparable<T>
	{
		public T Data { get; set; }
		public Node<T> Parent { get; set; }
		public int Rank { get; set; }

		public Node(T data)
		{
			Data = data;
			Parent = this;
			Rank = 0;
		}
	}

	public class DisjointSet<T> where T: System.IComparable<T>
    {
		private Dictionary<T, Node<T>> nodes;

        public DisjointSet()
        {
			nodes = new Dictionary<T, Node<T>>();
        }

		public bool MakeSet()
		{
			return false;
		}
    }
}
