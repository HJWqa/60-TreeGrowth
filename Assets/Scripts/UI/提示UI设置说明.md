# 提示UI设置说明

## 功能说明
这是一个提示性UI界面，显示"温度和湿度对于一个果树来说是很重要的，不信你试试"，底部有"知道了"按钮，点击后进入主场景。

## 新版UI特性
主场景现在包含：
- 圆形的暂停/继续和重置按钮
- 正三角形(▲)和倒三角形(▼)的温度/湿度调节按钮
- 优化的环境参数显示布局

详细设置请参考：
- `圆形按钮和三角形UI设置指南.md` - 完整详细教程
- `快速UI设置步骤.md` - 5分钟快速设置

## Unity中设置步骤

### 1. 创建新场景
1. File -> New Scene -> 选择 Empty
2. 保存场景为 `TipScene.unity`

### 2. 创建UI Canvas
1. 右键 Hierarchy -> UI -> Canvas
2. 设置Canvas：
   - Render Mode: Screen Space - Overlay
   - Canvas Scaler -> UI Scale Mode: Scale With Screen Size
   - Reference Resolution: 1920 x 1080

### 3. 添加CanvasGroup组件
1. 选中Canvas
2. Add Component -> Canvas Group
3. 这个用于淡出效果

### 4. 创建背景Panel
1. 右键Canvas -> UI -> Panel
2. 重命名为 "TipPanel"
3. 设置RectTransform：
   - Anchor: Stretch (全屏)
   - Left/Right/Top/Bottom: 0
4. 设置Image组件：
   - Color: 半透明黑色 (R:0, G:0, B:0, A:200) 或你喜欢的颜色

### 5. 创建提示文本
1. 右键TipPanel -> UI -> Text - TextMeshPro (如果没有TMP，用普通Text)
2. 重命名为 "TipText"
3. 设置RectTransform：
   - Anchor: Middle Center
   - Width: 800, Height: 300
   - Pos Y: 50 (稍微往上)
4. 设置文本：
   - Text: "温度和湿度对于一个果树来说是很重要的，不信你试试"
   - Font Size: 48
   - Alignment: Center & Middle
   - Color: 白色
   - 可以添加Outline效果让文字更清晰

### 6. 创建"知道了"按钮
1. 右键TipPanel -> UI -> Button - TextMeshPro
2. 重命名为 "ConfirmButton"
3. 设置RectTransform：
   - Anchor: Bottom Center
   - Width: 300, Height: 80
   - Pos Y: 100 (距离底部100像素)
4. 设置Button颜色：
   - Normal: 浅色
   - Highlighted: 稍亮
   - Pressed: 稍暗
5. 修改按钮文字（展开Button，选中Text子对象）：
   - Text: "知道了"
   - Font Size: 36
   - Color: 黑色或白色（根据按钮背景）

### 7. 添加TipPanel脚本
1. 选中Canvas
2. Add Component -> 搜索 "TipPanel"
3. 设置脚本参数：
   - Confirm Button: 拖入 ConfirmButton
   - Target Scene Name: "OutdoorsScene" (你的主场景名称)
   - Fade Out Duration: 0.5
   - Canvas Group: 拖入Canvas上的CanvasGroup组件

### 8. 添加到Build Settings
1. File -> Build Settings
2. 点击 "Add Open Scenes" 添加TipScene
3. 确保TipScene在场景列表的第一位（作为启动场景）
4. OutdoorsScene在第二位

### 9. 测试
1. 打开TipScene
2. 点击Play按钮测试
3. 点击"知道了"应该会淡出并加载主场景

## 美化建议

### 添加图标或装饰
- 可以在提示文本上方添加一个温度计或水滴图标
- 右键TipPanel -> UI -> Image，添加装饰图片

### 添加动画效果
- 给TipText添加Animator组件
- 创建简单的缩放或淡入动画

### 改进按钮样式
- 使用自定义Sprite作为按钮背景
- 添加按钮点击音效（需要AudioSource组件）

### 添加渐变背景
- 创建一个渐变色的Sprite
- 替换Panel的Image

## 常见问题

### 按钮点击没反应
- 检查Canvas是否有EventSystem（应该自动创建）
- 检查TipPanel脚本的Confirm Button是否正确拖入

### 场景加载失败
- 检查Target Scene Name是否正确
- 确保目标场景已添加到Build Settings

### 文字显示不全
- 调整TipText的Width和Height
- 检查字体大小是否合适
