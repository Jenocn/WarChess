## WarChess战棋系统

- 战棋系统，是在一个m*n的类似棋盘的地图上进行的，棋盘中每一个单元格代表一种地形，敌我双方角色在棋盘上行动和博弈。
- 本系统中实现了棋盘和地形的创建，角色的创建，角色的移动范围、最佳移动路径、交互范围的功能。
- 使用者只需调用相关方法，即可轻松得到需要的各种数据，将战棋核心逻辑与特色玩法独立开，也方便开发和维护。


## 使用方法
### 棋盘
```csharp
// 创建棋盘空间
var space = new WarChessSpace();
// 创建10x10的棋盘，默认地形为‘1’
space.Init(10, 10, 1);
```
### 精灵
```csharp
// 创建精灵
var player = new Sprite();
// 设置坐标
player.SetPosition(1, 1);
// 设置行动力
player.SetActionPoint(2);
// 设置在‘1’这种地形上需要消耗的行动力为‘1’
player.SetTerrainActionCost(1, 1);
// 设置阵营类型
player.SetCamp(1);
// 添加到棋盘空间中去
space.AddSprite(player);
```

### 布局
```csharp
// 创建一个攻击范围的布局
var layout = new WarChessLayout();
// 攻击范围为上下左右的1格
layout.Add(0, 1);
layout.Add(1, 0);
layout.Add(0, -1);
layout.Add(-1, 0);
```

### 操作
```csharp
// 创建精灵的行动区域树
var tree = WarChessController.CreateActionCellTree(space, sprite);
// 获取行动区域节点列表
var list = WarChessController.GeneratorCellList(tree);
// 获取精灵移动到指定坐标的最佳路径
var way = WarChessController.GeneratorWay(tree, 2, 2);
// 获取精灵的攻击范围
var range = WarChessController.GeneratorInteractionCells(list, space, sprite, layout, false);
```


## 接下来要做的
- 编辑器