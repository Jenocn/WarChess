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
				_SearchActionSideCells(ret, space, sprite, sprite.GetBaseActionPoint(), 0);
			}
			return ret;
		}

		/// <summary>
		/// 根据树形结构的数据生成列表数据
		/// </summary>
		public static LinkedList<WarChessCell> GeneratorCellList(WarChessNode<WarChessCell> node) {
			var ret = new LinkedList<WarChessCell>();
			if (node.value != null) {
				ret.AddLast(node.value);
				_GeneratorCellListFromChildren(node, in ret);
			}
			return ret;
		}

		/// <summary>
		/// 在所有子节点中获得从根节点到目标坐标的最短路径的节点集合
		/// </summary>
		public static LinkedList<WarChessCell> GeneratorWay(WarChessNode<WarChessCell> root, int x, int y) {
			var tempList = new LinkedList<WarChessCell>();
			var minLayerNode = _FindMinLayerNode(root, x, y);
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

		/// <summary>
		/// 在所有子节点中获得从根节点到目标节点的最短路径的节点集合
		/// </summary>
		public static LinkedList<WarChessCell> GeneratorWay(WarChessNode<WarChessCell> root, WarChessCell cell) {
			return GeneratorWay(root, cell.x, cell.y);
		}

		private static WarChessNode<WarChessCell> _FindMinLayerNode(WarChessNode<WarChessCell> root, int x, int y) {
			WarChessNode<WarChessCell> ret = null;
			foreach (var item in root.children) {
				if ((item.value.x == x) && (item.value.y == y)) {
					ret = item;
					break;
				}
			}
			if (ret == null) {
				// 如果当前层级下没有找到对应的cell则继续往深层找
				foreach (var item in root.children) {
					var temp = _FindMinLayerNode(item, x, y);
					if (temp == null) { continue; }
					if (ret == null || ret.layer > temp.layer) {
						ret = temp;
					}
				}
			}
			return ret;
		}

		private static void _GeneratorCellListFromChildren(WarChessNode<WarChessCell> node, in LinkedList<WarChessCell> li) {
			if (node.value == null) { return; }

			foreach (var item in node.children) {
				if (item.value == null) { continue; }
				if (li.Find(item.value) == null) {
					li.AddLast(item.value);
				}
			}
			foreach (var item in node.children) {
				_GeneratorCellListFromChildren(item, in li);
			}
		}

		private static void _SearchActionSideCells(WarChessNode<WarChessCell> node, WarChessSpace space, WarChessSprite sprite, float ap, float sumCostAP) {
			_SearchActionCell(node.value.top, space, node, sprite, ap, sumCostAP);
			_SearchActionCell(node.value.bottom, space, node, sprite, ap, sumCostAP);
			_SearchActionCell(node.value.left, space, node, sprite, ap, sumCostAP);
			_SearchActionCell(node.value.right, space, node, sprite, ap, sumCostAP);
		}

		private static void _SearchActionCell(WarChessCell cell, WarChessSpace space, WarChessNode<WarChessCell> parent, WarChessSprite sprite, float ap, float sumCostAP) {
			if (cell == null) { return; }
			// 当前Sprite是否可以通行这个地形
			if (!sprite.TryGetTerrainActionCost(cell.terrain, out var cost)) { return; }
			// AP是否足够
			if (ap < cost) { return; }
			// 这个单元格内的其他Sprite是否为我方或友方阵营
			var destSpr = space.GetSprite(cell.x, cell.y);
			if ((destSpr != null) && !sprite.IsFriendlyCamp(destSpr)) { return; }

			var tempParent = parent.parent;
			float newSumCostAP = sumCostAP + cost;
			while (tempParent != null) {
				// 不重复
				if (tempParent.value == cell) {
					return;
				}
				foreach (var child in tempParent.children) {
					if (child.value == cell) {
						// 已有消耗更少的单元格包含了当前格子
						if (child.customData < newSumCostAP) {
							return;
						}
					}
				}
				tempParent = tempParent.parent;
			}
			var newNode = parent.AddChild(cell);
			newNode.customData = newSumCostAP;
			_SearchActionSideCells(newNode, space, sprite, ap - cost, newSumCostAP);
		}
	}
}