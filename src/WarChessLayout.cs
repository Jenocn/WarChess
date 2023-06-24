using System.Collections.Generic;

namespace WarChess {
	/// <summary>
	/// 布局，单元格排列
	/// </summary>
	public class WarChessLayout {
		// x, y
		public LinkedList<System.Tuple<int, int>> points { get; } = new LinkedList<System.Tuple<int, int>>();

		public void Add(int x, int y) {
			if (!Contains(x, y)) {
				points.AddLast(new System.Tuple<int, int>(x, y));
			}
		}

		public void Remove(int x, int y) {
			var ite = points.First;
			while (ite != null) {
				var item = ite.Value;
				if (_IsEqual(item, x, y)) {
					points.Remove(ite);
					break;
				}
				ite = ite.Next;
			}
		}

		public bool Contains(int x, int y) {
			foreach (var item in points) {
				if (_IsEqual(item, x, y)) {
					return true;
				}
			}
			return false;
		}

		private bool _IsEqual(System.Tuple<int, int> a, int x, int y) {
			return a.Item1 == x && a.Item2 == y;
		}
	}
}