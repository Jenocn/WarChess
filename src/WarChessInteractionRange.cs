using System.Collections.Generic;

namespace WarChess {
	/// <summary>
	/// 攻击、治疗等指令的交互范围
	/// </summary>
	public class WarChessInteractionRange {
		// 交互点布局
		public WarChessLayout interaction { get; private set; } = null;
		// 触发点布局
		public WarChessLayout trigger { get; private set; } = null;
	}
}