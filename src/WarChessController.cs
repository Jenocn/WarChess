using System.Collections.Generic;

namespace WarChess {
	public class WarChessActionResult {
		public LinkedList<WarChessPath> pathList = new LinkedList<WarChessPath>();
		public LinkedList<WarChessCell> visibleCells = new LinkedList<WarChessCell>();
	}

	/// <summary>
	/// 控制器
	/// 计算地形的影响和行动结果
	/// </summary>
	public class WarChessController {
		public WarChessActionResult GetActionCells(WarChessSpace space, WarChessSprite sprite) {
			var ret = new WarChessActionResult();
			var cell = space.GetCell(sprite.x, sprite.y);
			_SearchActionCells(space, cell, sprite, sprite.GetBaseActionPoint(), null, in ret);
			return ret;
		}

		private void _SearchActionCells(WarChessSpace space, WarChessCell cell, WarChessSprite sprite, float curAP, WarChessPath curPath, in WarChessActionResult ret) {
			if (curPath == null) {
				curPath = new WarChessPath();
				curPath.Add(cell);
			}

			_SearchActionCellCur(space, cell.top, sprite, curAP, curPath, in ret);
			_SearchActionCellCur(space, cell.bottom, sprite, curAP, curPath, in ret);
			_SearchActionCellCur(space, cell.left, sprite, curAP, curPath, in ret);
			_SearchActionCellCur(space, cell.right, sprite, curAP, curPath, in ret);
		}

		private void _SearchActionCellCur(WarChessSpace space, WarChessCell cur, WarChessSprite sprite, float curAP, WarChessPath curPath, in WarChessActionResult ret) {
			if (cur == null) { return; }
			if (!sprite.TryGetTerrainActionCost(cur.terrain, out var cost)) { return; }
			if (curAP < cost) { return; }
			if (curPath.Contains(cur)) { return; }
			var destSpr = space.GetSprite(cur.x, cur.y);
			if ((destSpr != null) && !sprite.IsFriendlyCamp(destSpr)) { return; }
			var newPath = curPath.Clone();
			newPath.Add(cur);
			_SaveActionCellToResult(cur, newPath, in ret);
			_SearchActionCells(space, cur, sprite, curAP - cost, newPath, in ret);
		}

		private void _SaveActionCellToResult(WarChessCell cell, WarChessPath curPath, in WarChessActionResult ret) {
			bool bAdd = true;
			foreach (var item in ret.pathList) {
				if (item.endCell == cell) {
					if (item.size > curPath.size) {
						bAdd = true;
						ret.pathList.Remove(item);
						break;
					} else {
						bAdd = false;
						break;
					}
				}
			}
			if (bAdd) {
				ret.pathList.AddLast(curPath.Clone());
			}

			if (!ret.visibleCells.Contains(cell)) {
				ret.visibleCells.AddLast(cell);
			}
		}
	}
}