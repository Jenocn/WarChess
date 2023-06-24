using System.Collections.Generic;

namespace WarChess {
	public class WarChessWay {
		private Queue<WarChessCell> _cells = new Queue<WarChessCell>();
		public WarChessCell endCell => _cells.Peek();
		public int size => _cells.Count;

		public bool Contains(WarChessCell cell) {
			return _cells.Contains(cell);
		}
		public void Add(WarChessCell cell) {
			_cells.Enqueue(cell);
		}

		public void Clear() {
			_cells.Clear();
		}

		public Queue<WarChessCell> GetCells() {
			return _cells;
		}
	}
}