
namespace WarChess {
	/// <summary>
	/// 单元格
	/// </summary>
	public class WarChessCell {
		public int index { get; private set; } = 0;
		public int x { get; private set; } = 0;
		public int y { get; private set; } = 0;
		public int col { get; private set; } = 0;
		public int row { get; private set; } = 0;
		public int terrain { get; private set; } = 0;

		public WarChessCell left { get; private set; } = null;
		public WarChessCell right { get; private set; } = null;
		public WarChessCell top { get; private set; } = null;
		public WarChessCell bottom { get; private set; } = null;

		public void Init(int index, int x, int y, int col, int row) {
			this.index = index;
			this.x = x;
			this.y = y;
			this.col = col;
			this.row = row;
		}
		public void SetNerghbor(WarChessCell left, WarChessCell right, WarChessCell top, WarChessCell bottom) {
			this.left = left;
			this.right = right;
			this.top = top;
			this.bottom = bottom;
		}
		public void SetTerrain(int value) {
			terrain = value;
		}
	}
}