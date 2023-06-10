using System.Collections.Generic;

namespace WarChess {
	/// <summary>
	/// 棋盘空间
	/// </summary>
	public class WarChessSpace {
		private List<WarChessCell> _cellList = null;

		public int row { get; private set; } = 0;
		public int col { get; private set; } = 0;

		public void Init(string src) {
			// todo...
		}
		public void Init(int col, int row) {
			this.col = col;
			this.row = row;
			_cellList = new List<WarChessCell>(col * row);

			int index = 0;
			for (int i = 0; i < row; ++i) {
				for (int j = 0; j < col; ++j) {
					var item = new WarChessCell();
					item.Init(index++, j, i, col, row);
					_cellList.Add(item);
				}
			}

			foreach (var item in _cellList) {
				var left = GetCell(item.x - 1, item.y);
				var right = GetCell(item.x + 1, item.y);
				var top = GetCell(item.x, item.y + 1);
				var bottom = GetCell(item.x, item.y - 1);
				item.SetNerghbor(left, right, top, bottom);
			}
		}

		public WarChessCell GetCell(int x, int y) {
			if (x < 0 || x >= col || y < 0 || y > row) {
				return null;
			}
			return GetCell(y * col + x);
		}
		public WarChessCell GetCell(int index) {
			if (index >= 0 && index < _cellList.Count) {
				return _cellList[index];
			}
			return null;
		}
	}
}