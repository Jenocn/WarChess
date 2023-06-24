using System.Collections.Generic;

namespace WarChess {
	public class WarChessActionResult {
		public WarChessNode<WarChessCell> tree { get; set; } = null;
		public WarChessSpace space { get; set; } = null;
		public WarChessSprite sprite { get; set; } = null;

		private LinkedList<WarChessCell> _visibleCells = new LinkedList<WarChessCell>();
		private HashSet<WarChessCell> _sideCells = new HashSet<WarChessCell>();

		public void FixData() {
			// 清理被精灵占据的边缘格子
			do {
				if (tree.value == null) { break; }
				if (tree.children.Count == 0) { break; }
				_FixChildren(tree);
			} while (false);

			_GeneratorCellList();
		}

		public LinkedList<WarChessCell> GetVisibleCells() {
			return _visibleCells;
		}
		public HashSet<WarChessCell> GetSideCells() {
			return _sideCells;
		}

		private void _FixChildren(WarChessNode<WarChessCell> node) {
			// 先深度优先清理子节点
			foreach (var item in node.children) {
				_FixChildren(item);
			}
			do {
				if (node.children.Count != 0) { break; }
				if (node.value != null) {
					var findSprite = space.GetSprite(node.value.x, node.value.y);
					if (findSprite == null || findSprite == sprite) { break; }
				}
				// 若没有子节点了，且符合清理条件，则清理自身
				node.RemoveFromParent();
			} while (false);
		}

		private void _GeneratorCellList() {
			_visibleCells.Clear();
			_sideCells.Clear();
			if (tree.value != null) {
				_visibleCells.AddLast(tree.value);
				_GeneratorCellListFromChildren(tree, in _visibleCells, in _sideCells);
			}
		}

		private void _GeneratorCellListFromChildren(WarChessNode<WarChessCell> node, in LinkedList<WarChessCell> visibleList, in HashSet<WarChessCell> sideList) {
			if (node.value == null) { return; }

			foreach (var item in node.children) {
				if (item.value == null) { continue; }
				if (visibleList.Find(item.value) == null) {
					visibleList.AddLast(item.value);
				}
			}
			foreach (var item in node.children) {
				_GeneratorCellListFromChildren(item, in visibleList, in sideList);
			}
			if (node.children.Count == 0) {
				sideList.Add(node.value);
			}
		}
	}
}
