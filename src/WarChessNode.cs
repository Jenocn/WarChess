using System.Collections.Generic;

namespace WarChess {
	public class WarChessNode<T> {
		public T value { get; set; } = default(T);
		public int layer { get; private set; } = 0;
		public WarChessNode<T> parent { get; set; } = null;
		public LinkedList<WarChessNode<T>> children = new LinkedList<WarChessNode<T>>();

		public WarChessNode() { }
		public WarChessNode(T value) {
			this.value = value;
		}
		public WarChessNode<T> AddChild(T child) {
			var ret = new WarChessNode<T>(child);
			children.AddLast(ret);
			ret._SetParent(this);
			return ret;
		}

		public void RemoveFromParent() {
			if (parent != null) {
				parent.children.Remove(this);
			}
		}

		private void _SetParent(WarChessNode<T> node) {
			parent = node;
			layer = node.layer + 1;
		}
	}
}