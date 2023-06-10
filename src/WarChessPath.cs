using System.Collections.Generic;

namespace WarChess {
	public class WarChessPath {
		private LinkedList<WarChessCell> _cells = new LinkedList<WarChessCell>();
		public WarChessCell endCell { get; private set; } = null;
		public int size { get; private set; } = 0;

		public WarChessPath Clone() {
			var ret = new WarChessPath();
			foreach (var item in _cells) {
				ret._cells.AddLast(item);
			}
			ret.endCell = endCell;
			ret.size = size;
			return ret;
		}
		public bool Contains(WarChessCell cell) {
			return _cells.Contains(cell);
		}
		public void Add(WarChessCell cell) {
			endCell = cell;
			_cells.AddLast(cell);
			++size;
		}

		public void Clear() {
			size = 0;
			endCell = null;
			_cells.Clear();
		}

		public LinkedList<WarChessCell> GetCells() {
			return _cells;
		}
	}
}