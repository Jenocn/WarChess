using System.Collections.Generic;

namespace WarChess {
	/// <summary>
	/// 精灵
	/// </summary>
	public class WarChessSprite {
		public int x { get; private set; } = 0;
		public int y { get; private set; } = 0;

		// 基础行动力
		private float _baseActionPoint = 0;
		// 地形行动力消耗
		private Dictionary<int, float> _terrainActionCostDict = new Dictionary<int, float>();

		public void SetPosition(int x, int y) {
			this.x = x;
			this.y = y;
		}

		public float GetBaseActionPoint() {
			return _baseActionPoint;
		}

		public void SetBaseActionPoint(float v) {
			_baseActionPoint = v;
		}

		/// <summary>
		/// 尝试获取地形行动力比例
		/// </summary>
		/// <param name="terrain">地形key</param>
		/// <param name="cost">消耗</param>
		/// <returns>是否可以通行</returns>
		public bool TryGetTerrainActionCost(int terrain, out float cost) {
			return _terrainActionCostDict.TryGetValue(terrain, out cost);
		}

		public void SetTerrainActionCost(int terrain, float v) {
			if (_terrainActionCostDict.ContainsKey(terrain)) {
				_terrainActionCostDict[terrain] = v;
			} else {
				_terrainActionCostDict.Add(terrain, v);
			}
		}
		public void RemoveTerrainActionCost(int terrain) {
			_terrainActionCostDict.Remove(terrain);
		}
	}
}