using System.Collections.Generic;

namespace WarChess {
	/// <summary>
	/// 控制器
	/// 计算地形的影响和行动结果
	/// </summary>
	public static class WarChessController {
		/// <summary>
		/// 获得精灵的可行动单元格
		/// </summary>
		public static WarChessNode<WarChessCell> CreateActionCellTree(WarChessSpace space, WarChessSprite sprite) {
			WarChessNode<WarChessCell> ret = null;
			var cell = space.GetCell(sprite.x, sprite.y);
			if (cell != null) {
				// tree
				ret = new WarChessNode<WarChessCell>(cell);
				_SearchActionSideCells(ret, space, sprite);
			}
			return ret;
		}

		/// <summary>
		/// 生成列表数据
		/// </summary>
		public static LinkedList<WarChessCell> GeneratorCellList(WarChessNode<WarChessCell> node) {
			var ret = new LinkedList<WarChessCell>();
			if (node.value == null) {
				return ret;
			}

			var pool = new Queue<WarChessNode<WarChessCell>>();
			pool.Enqueue(node);

			while (pool.Count > 0) {
				var tempNode = pool.Dequeue();
				ret.AddLast(tempNode.value);
				foreach (var item in tempNode.children) {
					pool.Enqueue(item);
				}
			}

			return ret;
		}

		/// <summary>
		/// 在所有子节点中获得从根节点到目标坐标的最小行动力(AP最少)的节点集合
		/// </summary>
		public static LinkedList<WarChessCell> GeneratorWay(WarChessNode<WarChessCell> root, int x, int y) {
			var tempList = new LinkedList<WarChessCell>();
			var node = _FindMinAPNode(root, x, y);
			while (node != null) {
				tempList.AddFirst(node.value);
				node = node.parent;
			}
			return tempList;
		}

		/// <summary>
		/// 在所有子节点中获得从根节点到目标节点的最小行动力(AP最少)的节点集合
		/// </summary>
		public static LinkedList<WarChessCell> GeneratorWay(WarChessNode<WarChessCell> root, WarChessCell cell) {
			return GeneratorWay(root, cell.x, cell.y);
		}

		private static WarChessNode<WarChessCell> _FindMinAPNode(WarChessNode<WarChessCell> root, int x, int y) {
			WarChessNode<WarChessCell> ret = null;
			foreach (var item in root.children) {
				// 先在子节点中找
				if (item.value.x == x && item.value.y == y) {
					if (ret == null || ret.customData > item.customData) {
						ret = item;
					}
				}
				// 继续深入查找
				var node = _FindMinAPNode(item, x, y);
				if (node != null) {
					if (ret == null || ret.customData > node.customData) {
						ret = node;
					}
				}
			}
			return ret;
		}

		private static void _SearchActionSideCells(WarChessNode<WarChessCell> root, WarChessSpace space, WarChessSprite sprite) {
			var pool = new Queue<WarChessNode<WarChessCell>>();
			var close = new Dictionary<WarChessCell, WarChessNode<WarChessCell>>();

			pool.Enqueue(root);
			close.Add(root.value, root);
			var tempArr = new WarChessCell[4];
			while (pool.Count > 0) {
				var node = pool.Dequeue();

				tempArr[0] = node.value.top;
				tempArr[1] = node.value.bottom;
				tempArr[2] = node.value.left;
				tempArr[3] = node.value.right;

				for (int i = 0; i < tempArr.Length; ++i) {
					var value = tempArr[i];
					if (_CheckActionCell(value, space, node, sprite, out var sumCostAP)) {
						if (close.TryGetValue(value, out var f)) {
							if (sumCostAP >= f.customData) {
								continue;
							} else {
								close.Remove(value);
							}
						}
						var newNode = node.AddChild(tempArr[i]);
						newNode.customData = sumCostAP;
						pool.Enqueue(newNode);
						close.Add(value, newNode);
					}
				}
			}
		}

		private static bool _CheckActionCell(WarChessCell cell, WarChessSpace space, WarChessNode<WarChessCell> parent, WarChessSprite sprite, out float sumCostAP) {
			sumCostAP = 0;
			if (cell == null) { return false; }
			// 当前Sprite是否可以通行这个地形
			if (!sprite.TryGetTerrainActionCost(cell.terrain, out var cost)) { return false; }
			// AP是否足够
			if (parent.customData + cost > sprite.GetBaseActionPoint()) { return false; }
			// 这个单元格内的其他Sprite是否为我方或友方阵营
			var destSpr = space.GetSprite(cell.x, cell.y);
			if ((destSpr != null) && !sprite.IsFriendlyCamp(destSpr)) { return false; }
			sumCostAP = parent.customData + cost;
			return true;
		}
	}
}