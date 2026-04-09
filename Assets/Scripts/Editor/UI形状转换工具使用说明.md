# UI形状转换工具使用说明

## 功能介绍
这个工具可以自动将Unity中的方形按钮转换为圆形或三角形按钮，无需手动操作。

## 使用方法

### 方法1：自动转换所有按钮（推荐）

1. 在Unity编辑器中，打开菜单：`TreePlanQAQ > UI Tools > Convert Buttons to Shapes`
2. 点击窗口中的 **"自动设置所有UI形状"** 按钮
3. 工具会自动识别按钮名称并转换：
   - 包含"pause"、"reset"、"暂停"、"重置"的按钮 → 圆形
   - 包含"up"、"increase"、"上"、"增加"的按钮 → 正三角形 ▲
   - 包含"down"、"decrease"、"下"、"减少"的按钮 → 倒三角形 ▼

### 方法2：手动转换选中的按钮

1. 在Hierarchy中选择一个或多个按钮
2. 打开菜单：`TreePlanQAQ > UI Tools > Convert Buttons to Shapes`
3. 点击对应的转换按钮：
   - "将选中的按钮转换为圆形"
   - "将选中的按钮转换为正三角形 ▲"
   - "将选中的按钮转换为倒三角形 ▼"

### 方法3：右键菜单快速转换

1. 在Hierarchy中右键点击按钮
2. 选择 `GameObject > UI > Convert to...`
3. 选择要转换的形状：
   - Convert to Circle Button
   - Convert to Triangle Button ▲
   - Convert to Triangle Button ▼

## 按钮命名规范

为了让自动转换功能正常工作，建议按以下规范命名按钮：

### 圆形按钮
- PauseButton（暂停按钮）
- ResetButton（重置按钮）
- CircleButton（通用圆形按钮）

### 正三角形按钮（向上/增加）
- TempUpButton（温度增加）
- HumidityUpButton（湿度增加）
- IncreaseButton（通用增加按钮）

### 倒三角形按钮（向下/减少）
- TempDownButton（温度减少）
- HumidityDownButton（湿度减少）
- DecreaseButton（通用减少按钮）

## 转换效果

### 圆形按钮
- 使用Unity内置的Knob sprite
- 自动调整为正方形（保持圆形）
- 默认大小：80x80

### 三角形按钮
- 使用TriangleImage组件（代码生成）
- 正三角形：浅红色 (255, 100, 100)
- 倒三角形：浅蓝色 (100, 150, 255)
- 默认大小：50x50

## 转换后的调整

转换完成后，你可以在Inspector中调整：

### 圆形按钮
- Image组件的Color属性（改变颜色）
- RectTransform的Size（改变大小）

### 三角形按钮
- TriangleImage组件的Triangle Color（改变颜色）
- TriangleImage组件的Point Up（切换方向）
- RectTransform的Size（改变大小）

## 常见问题

### Q: 转换后按钮点击没反应？
A: 检查Button组件的Target Graphic是否正确设置。工具会自动设置，但如果有问题可以手动拖入对应的Image或TriangleImage组件。

### Q: 三角形显示不正确？
A: 确保TriangleImage脚本已正确编译。如果有编译错误，请先修复错误再使用转换工具。

### Q: 想要自定义颜色？
A: 转换后，在Inspector中找到Image或TriangleImage组件，修改Color属性即可。

### Q: 可以批量转换多个场景的按钮吗？
A: 目前只支持当前打开的场景。如果需要转换多个场景，请逐个打开场景并运行转换工具。

## 手动微调建议

转换完成后，建议手动调整以下内容以获得最佳效果：

1. **按钮位置**：调整RectTransform的Position
2. **按钮大小**：调整RectTransform的Size
3. **按钮颜色**：调整Image/TriangleImage的Color
4. **按钮间距**：调整父对象的布局
5. **文本位置**：如果按钮有文本子对象，调整其位置

## 完整工作流程示例

假设你要设置温度和湿度控制UI：

1. **创建按钮**（如果还没有）：
   ```
   Canvas
   └── EnvironmentPanel
       ├── TempUpButton
       ├── TempDownButton
       ├── HumidityUpButton
       └── HumidityDownButton
   ```

2. **运行自动转换**：
   - 打开 `TreePlanQAQ > UI Tools > Convert Buttons to Shapes`
   - 点击 "自动设置所有UI形状"

3. **检查结果**：
   - TempUpButton → 正三角形 ▲（浅红色）
   - TempDownButton → 倒三角形 ▼（浅蓝色）
   - HumidityUpButton → 正三角形 ▲（浅红色）
   - HumidityDownButton → 倒三角形 ▼（浅蓝色）

4. **微调**：
   - 调整按钮大小为 50x50
   - 调整按钮间距
   - 在按钮之间添加温度/湿度显示文本

5. **完成**！

## 技术细节

### 圆形按钮实现
使用Unity内置的 `UI/Skin/Knob.psd` sprite，这是一个完美的圆形图片。

### 三角形按钮实现
使用自定义的 `TriangleImage` 组件，通过代码生成三角形网格：
- 继承自 `MaskableGraphic`
- 使用 `OnPopulateMesh` 方法生成三角形顶点
- 支持正三角形和倒三角形
- 支持自定义颜色

## 相关文件

- `UIShapeConverter.cs` - 转换工具主脚本
- `TriangleImage.cs` - 三角形图形组件
- `CircleImage.cs` - 圆形图形组件（可选）
- `EnhancedGrowthUIController.cs` - UI控制器脚本

## 下一步

转换完成后，记得：
1. 保存场景（Ctrl+S）
2. 测试按钮功能
3. 调整颜色和大小以匹配你的UI设计
4. 添加按钮事件监听器（如果还没有）

