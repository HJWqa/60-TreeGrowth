# 产量UI设置说明

## 快速创建

### 方法1：使用编辑器工具（推荐）

1. **打开Unity编辑器**
2. **点击菜单**：`橘子树 → UI设置 → 创建产量系统UI`
3. **选择创建选项**：
   - `创建产量显示UI` - 只创建左上角的产量显示
   - `创建收获消息面板` - 只创建收获提示面板
   - `创建全部UI` - 一键创建所有UI

### 方法2：手动创建

如果编辑器工具不可用，可以手动创建。

## UI组件说明

### 1. 产量显示UI（YieldDisplay）

**位置**：左上角

**结构**：
```
YieldDisplay (GameObject + YieldDisplayUI)
├── Background (Image) - 半透明黑色背景
└── YieldText (Text) - "预期产量: 50kg"
```

**手动创建步骤**：

1. **创建父对象**
   - 在Canvas下创建空GameObject，命名为 `YieldDisplay`
   - 添加 `YieldDisplayUI` 组件

2. **设置RectTransform**
   - Anchor: 左上角 (0, 1)
   - Pivot: (0, 1)
   - Position: (20, -20)
   - Size: (200, 50)

3. **创建背景**
   - 添加子对象 `Background`
   - 添加 `Image` 组件
   - 颜色：黑色，Alpha = 0.7
   - 拉伸填充父对象

4. **创建文本**
   - 添加子对象 `YieldText`
   - 添加 `Text` 组件
   - 文本：`预期产量: 50kg`
   - 字体大小：20
   - 颜色：白色
   - 对齐：居中
   - 拉伸填充父对象

5. **配置YieldDisplayUI组件**
   - Yield Text: 拖入 `YieldText`
   - Display Format: `预期产量: {0}kg`
   - Normal Color: 白色
   - Warning Color: 黄色
   - Danger Color: 红色
   - Warning Threshold: 30
   - Danger Threshold: 10

### 2. 收获消息面板（HarvestMessagePanel）

**位置**：屏幕中央

**结构**：
```
HarvestMessagePanel (GameObject + HarvestMessageUI)
├── Overlay (Image) - 半透明黑色遮罩
├── Background (Image) - 绿色背景
├── TitleText (Text) - "🎉 收获成功！"
├── MessageText (Text) - "恭喜您！您已收获 XX 公斤的橘子！"
└── CloseButton (Button)
    └── Text - "确定"
```

**手动创建步骤**：

1. **创建面板**
   - 在Canvas下创建空GameObject，命名为 `HarvestMessagePanel`
   - 添加 `HarvestMessageUI` 组件

2. **设置RectTransform**
   - Anchor: 中心 (0.5, 0.5)
   - Pivot: (0.5, 0.5)
   - Position: (0, 0)
   - Size: (500, 250)

3. **创建遮罩层**
   - 添加子对象 `Overlay`
   - 添加 `Image` 组件
   - 颜色：黑色，Alpha = 0.5
   - 拉伸到全屏（Offset Min: -1000, Offset Max: 1000）

4. **创建背景**
   - 添加子对象 `Background`
   - 添加 `Image` 组件
   - 颜色：绿色 (0.2, 0.6, 0.2, 0.95)
   - 拉伸填充父对象

5. **创建标题文本**
   - 添加子对象 `TitleText`
   - 添加 `Text` 组件
   - 文本：`🎉 收获成功！`
   - 字体大小：32
   - 字体样式：粗体
   - 颜色：白色
   - 对齐：居中
   - Anchor: 上部 (0, 0.6) 到 (1, 0.9)

6. **创建消息文本**
   - 添加子对象 `MessageText`
   - 添加 `Text` 组件
   - 文本：`恭喜您！您已收获 50 公斤的橘子！`
   - 字体大小：24
   - 颜色：白色
   - 对齐：居中
   - Anchor: 中部 (0.1, 0.4) 到 (0.9, 0.6)

7. **创建关闭按钮**
   - 添加子对象 `CloseButton`
   - 添加 `Button` 和 `Image` 组件
   - 颜色：浅灰色
   - Anchor: 下部 (0.3, 0.1) 到 (0.7, 0.3)
   - 添加子对象 `Text`，文本为 `确定`

8. **配置HarvestMessageUI组件**
   - Message Panel: 拖入 `HarvestMessagePanel` 自身
   - Message Text: 拖入 `MessageText`
   - Close Button: 拖入 `CloseButton`
   - Auto Hide Duration: 5
   - Auto Hide: 勾选

9. **初始隐藏**
   - 取消勾选 `HarvestMessagePanel` 的 Active
   - 游戏开始时面板会隐藏，收获时自动显示

## 功能测试

### 测试产量显示

1. **运行游戏**
2. **观察左上角**：应该显示 `预期产量: 50kg`（白色）
3. **点击开始生长**
4. **让环境超出范围**（停止生长）
5. **观察产量减少**：
   - 停止1次 → `预期产量: 49kg`
   - 停止20次 → `预期产量: 30kg`（变黄色）
   - 停止40次 → `预期产量: 10kg`（变红色）

### 测试收获消息

1. **运行游戏**
2. **点击开始生长**
3. **保持环境适宜**
4. **等待30秒**（生长到100%）
5. **观察屏幕中央**：应该弹出收获消息面板
6. **检查消息内容**：`恭喜您！您已收获 XX 公斤的橘子！`
7. **等待5秒**：面板自动隐藏
8. **或点击"确定"按钮**：立即隐藏

## 自定义设置

### 修改产量显示位置

在 `YieldDisplay` 的 RectTransform 中修改：
- 左上角：Anchor (0, 1), Position (20, -20)
- 右上角：Anchor (1, 1), Position (-20, -20)
- 左下角：Anchor (0, 0), Position (20, 20)
- 右下角：Anchor (1, 0), Position (-20, 20)

### 修改颜色阈值

在 `YieldDisplayUI` 组件中修改：
- Warning Threshold: 警告阈值（默认30kg）
- Danger Threshold: 危险阈值（默认10kg）

### 修改自动隐藏时间

在 `HarvestMessageUI` 组件中修改：
- Auto Hide Duration: 自动隐藏时间（默认5秒）
- Auto Hide: 是否自动隐藏（默认勾选）

### 修改面板样式

**背景颜色**：
- 修改 `Background` 的 Image 颜色
- 默认：绿色 (0.2, 0.6, 0.2, 0.95)
- 可改为：蓝色、金色等

**文本样式**：
- 修改 `TitleText` 和 `MessageText` 的字体、大小、颜色
- 可以使用自定义字体

**按钮样式**：
- 修改 `CloseButton` 的 Image 颜色和大小
- 可以添加按钮图片

## 常见问题

### Q1: 产量显示不更新？
**A**: 检查 `YieldDisplayUI` 组件的 `Yield Text` 是否正确连接到文本对象。

### Q2: 收获消息不显示？
**A**: 检查：
1. `HarvestMessageUI` 组件的引用是否正确连接
2. `HarvestMessagePanel` 初始状态是否为隐藏（未勾选Active）
3. 是否真的达到了100%生长

### Q3: 产量没有减少？
**A**: 检查：
1. `OrangeTreeController` 是否正确更新
2. 环境是否真的从适宜变为不适宜
3. 查看Console日志，应该有 "⚠️ 生长停止！产量减少..." 的提示

### Q4: 面板显示后不自动隐藏？
**A**: 检查 `HarvestMessageUI` 组件的 `Auto Hide` 是否勾选。

### Q5: 颜色不变化？
**A**: 检查 `YieldDisplayUI` 组件的颜色设置和阈值设置。

## 相关文件

- `CreateYieldUI.cs` - 编辑器工具（自动创建UI）
- `YieldDisplayUI.cs` - 产量显示组件
- `HarvestMessageUI.cs` - 收获消息组件
- `OrangeTreeController.cs` - 树木控制器（产量逻辑）
- `产量系统说明.md` - 产量系统完整说明

## 更新日期

2026-04-09
