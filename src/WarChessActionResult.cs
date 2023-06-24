using System.Collections.Generic;

namespace WarChess {
	public class WarChessActionResult {
		public WarChessNode<WarChessCell> tree { get; set; } = null;
		public WarChessSpace space { get; set; } = null;
		public WarChessSprite sprite { get; set; } = null;

		private LinkedList<WarChessCell> _visibleCells = new LinkedList<WarChessCell>();

		public void FixData() {
			_visibleCells.Clear();
			if (tree.value != null) {
				_visibleCells.AddLast(tree.value);
				_GeneratorCellListFromChildren(tree, in _visibleCells);
			}
		}

		public LinkedList<WarChessCell> GetVisibleCells() {
			return _visibleCells;
		}

		public LinkedList<WarChessCell> GetWay(int x, int y) {
			foreach (var item in _visibleCells) {
				if (item.x == x && item.y == y) {
					return GetWay(item);
				}
			}
			return new LinkedList<WarChessCell>();
		}
		public LinkedList<WarChessCell> GetWay(WarChessCell cell) {
			var tempList = new LinkedList<WarChessCell>();
			var minLayerNode = _FindMinLayerNode(tree, cell);
			if (minLayerNode == null) {
				return tempList;
			}
			var node = minLayerNode;
			while (node != null) {
				tempList.AddFirst(node.value);
				node = node.parent;
			}
			return tempList;
		}

		private WarChessNode<WarChessCell> _FindMinLayerNode(WarChessNode<WarChessCell> root, WarChessCell cell) {
			WarChessNode<WarChessCell> ret = null;
			foreach (var item in root.children) {
				if (item.value == cell) {
					ret = item;
					break;
				}
			}
			if (ret == null) {
				// 如果当前层级下没有找到对应的cell则继续往深层找
				foreach (var item in root.children) {
					var temp = _FindMinLayerNode(item, cell);
					if (temp == null) { continue; }
					if (ret == null || ret.layer > temp.layer) {
						ret = temp;
					}
				}
			}
			return ret;
		}

		private void _GeneratorCellListFromChildren(WarChessNode<WarChessCell> node, in LinkedList<WarChessCell> visibleList) {
			if (node.value == null) { return; }

			foreach (var item in node.children) {
				if (item.value == null) { continue; }
				if (visibleList.Find(item.value) == null) {
					visibleList.AddLast(item.value);
				}
			}
			foreach (var item in node.children) {
				_GeneratorCellListFromChildren(item, in visibleList);
			}
		}
	}
}
