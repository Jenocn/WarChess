namespace WarChess {
	/// <summary>
	/// 控制器
	/// 计算地形的影响和行动结果
	/// </summary>
	public static class WarChessController {
		/// <summary>
		/// 获得精灵的可行动单元格
		/// </summary>
		public static WarChessActionResult GetActionCells(WarChessSpace space, WarChessSprite sprite) {
			var ret = new WarChessActionResult();
			var cell = space.GetCell(sprite.x, sprite.y);
			if (cell != null) {
				// tree
				ret.tree = new WarChessNode<WarChessCell>(cell);
				_SearchActionSideCells(ret.tree, space, sprite, sprite.GetBaseActionPoint());
				ret.GeneratorData();
			}
			return ret;
		}

		private static void _SearchActionSideCells(WarChessNode<WarChessCell> node, WarChessSpace space, WarChessSprite sprite, float ap) {
			_SearchActionCell(node.value.top, space, node, sprite, ap);
			_SearchActionCell(node.value.bottom, space, node, sprite, ap);
			_SearchActionCell(node.value.left, space, node, sprite, ap);
			_SearchActionCell(node.value.right, space, node, sprite, ap);
		}

		private static bool _SearchActionCell(WarChessCell cell, WarChessSpace space, WarChessNode<WarChessCell> parent, WarChessSprite sprite, float ap) {
			if (cell == null) { return false; }
			// 回头路不可
			if ((parent.parent != null) && (parent.parent.value == cell)) { return false; }
			// 当前Sprite是否可以通行这个地形
			if (!sprite.TryGetTerrainActionCost(cell.terrain, out var cost)) { return false; }
			// AP是否足够
			if (ap < cost) { return false; }
			// 这个单元格内的其他Sprite是否为我方或友方阵营
			var destSpr = space.GetSprite(cell.x, cell.y);
			if ((destSpr != null) && !sprite.IsFriendlyCamp(destSpr)) { return false; }
			_SearchActionSideCells(parent.AddChild(cell), space, sprite, ap - cost);
			return true;
		}
	}
}