# 圆形按钮和三角形UI设置指南

## 概述
本指南将帮助你创建一个美观的UI界面，包含：
- 圆形的暂停/继续和重置按钮
- 正三角形和倒三角形的温度/湿度调节按钮
- 优化的温度和湿度显示位置

## Unity中的设置步骤

### 1. 准备工作

#### 创建三角形Sprite
1. 在Photoshop、GIMP或在线工具中创建三角形图片：
   - 正三角形（向上）：用于增加按钮
   - 倒三角形（向下）：用于减少按钮
   - 建议尺寸：128x128像素，透明背景
   - 颜色：白色（可以在Unity中调整颜色）

2. 导入到Unity：
   - 将图片拖入 `Assets/Materials/UI/` 文件夹
   - 选中图片，在Inspector中设置：
     - Texture Type: Sprite (2D and UI)
     - Sprite Mode: Single
     - 点击Apply

#### 创建圆形按钮Sprite（可选）
如果想要自定义圆形按钮：
1. 创建圆形图片（128x128像素）
2. 导入并设置为Sprite
3. 或者使用Unity内置的圆形Sprite

### 2. 修改Canvas结构

打开你的主场景，找到Canvas，按以下结构组织UI：

```
Canvas
├── TopPanel (顶部信息面板)
│   ├── StageText (阶段文本)
│   ├── GrowthText (生长百分比)
│   └── GrowthBar (生长进度条)
│
├── EnvironmentPanel (环境控制面板 - 中间偏右)
│   ├── TemperatureGroup (温度组)
│   │   ├── TempUpButton (正三角形)
│   │   ├── TemperatureText (温度显示)
│   │   └── TempDownButton (倒三角形)
│   │
│   ├── HumidityGroup (湿度组)
│   │   ├── HumidityUpButton (正三角形)
│   │   ├── HumidityText (湿度显示)
│   │   └── HumidityDownButton (倒三角形)
│   │
│   └── SunlightText (光照显示)
│
└── ControlPanel (控制按钮面板 - 底部)
    ├── PauseButton (圆形暂停按钮)
    └── ResetButton (圆形重置按钮)
```

### 3. 创建环境控制面板

#### 3.1 创建EnvironmentPanel
1. 右键Canvas -> UI -> Panel
2. 重命名为 "EnvironmentPanel"
3. 设置RectTransform：
   - Anchor: Middle Right
   - Pos X: -150
   - Pos Y: 0
   - Width: 250
   - Height: 400
4. 设置Image：
   - Color: 半透明背景 (R:0, G:0, B:0, A:150)
   - 或者禁用Image组件（如果不需要背景）

#### 3.2 创建温度控制组
1. 右键EnvironmentPanel -> Create Empty
2. 重命名为 "TemperatureGroup"
3. 设置RectTransform：
   - Anchor: Top Center
   - Pos X: 0
   - Pos Y: -80
   - Width: 200
   - Height: 120

##### 创建温度增加按钮（正三角形）
1. 右键TemperatureGroup -> UI -> Button
2. 重命名为 "TempUpButton"
3. 设置RectTransform：
   - Anchor: Top Center
   - Pos X: 0
   - Pos Y: 0
   - Width: 60
   - Height: 60
4. 设置Button组件：
   - 删除子对象Text（不需要文字）
5. 设置Image组件：
   - Source Image: 拖入正三角形Sprite
   - Color: 浅蓝色或你喜欢的颜色
   - Image Type: Simple
   - Preserve Aspect: 勾选
6. 添加圆角效果（可选）：
   - 如果想要圆形背景，可以添加一个子对象Image作为背景

##### 创建温度显示文本
1. 右键TemperatureGroup -> UI -> Text
2. 重命名为 "TemperatureText"
3. 设置RectTransform：
   - Anchor: Middle Center
   - Pos X: 0
   - Pos Y: 0
   - Width: 150
   - Height: 40
4. 设置Text组件：
   - Text: "25.0°C"
   - Font Size: 28
   - Alignment: Center & Middle
   - Color: 白色
   - Font Style: Bold

##### 创建温度减少按钮（倒三角形）
1. 右键TemperatureGroup -> UI -> Button
2. 重命名为 "TempDownButton"
3. 设置RectTransform：
   - Anchor: Bottom Center
   - Pos X: 0
   - Pos Y: 0
   - Width: 60
   - Height: 60
4. 设置Button和Image（同上，使用倒三角形Sprite）

#### 3.3 创建湿度控制组
按照温度控制组的相同步骤创建湿度控制组：
1. 创建 "HumidityGroup"
2. 设置位置：
   - Pos Y: -220 (在温度组下方)
3. 创建 HumidityUpButton、HumidityText、HumidityDownButton
4. HumidityText显示格式："60.0%"

#### 3.4 创建光照显示
1. 右键EnvironmentPanel -> UI -> Text
2. 重命名为 "SunlightText"
3. 设置RectTransform：
   - Anchor: Bottom Center
   - Pos Y: 40
   - Width: 200
   - Height: 40
4. 设置Text：
   - Text: "光照: 5000 lux"
   - Font Size: 24
   - Alignment: Center

### 4. 创建圆形控制按钮

#### 4.1 创建ControlPanel
1. 右键Canvas -> Create Empty
2. 重命名为 "ControlPanel"
3. 设置RectTransform：
   - Anchor: Bottom Center
   - Pos Y: 80
   - Width: 400
   - Height: 100

#### 4.2 创建暂停按钮（圆形）
1. 右键ControlPanel -> UI -> Button
2. 重命名为 "PauseButton"
3. 设置RectTransform：
   - Anchor: Middle Center
   - Pos X: -100
   - Pos Y: 0
   - Width: 80
   - Height: 80
4. 设置Image组件：
   - Source Image: UI/Skin/Knob (Unity内置圆形)
   - 或使用自定义圆形Sprite
   - Color: 绿色 (R:100, G:200, B:100)
   - Image Type: Simple
5. 修改子对象Text：
   - Text: "暂停"
   - Font Size: 20
   - Color: 白色
   - Alignment: Center & Middle

#### 4.3 创建重置按钮（圆形）
1. 复制PauseButton（Ctrl+D）
2. 重命名为 "ResetButton"
3. 设置RectTransform：
   - Pos X: 100 (在暂停按钮右侧)
4. 设置Image：
   - Color: 橙色 (R:255, G:150, B:50)
5. 修改Text：
   - Text: "重置"

### 5. 添加脚本组件

1. 选中Canvas
2. Add Component -> Enhanced Growth UI Controller
3. 设置脚本参数：

#### UI引用
- Stage Text: 拖入StageText
- Growth Text: 拖入GrowthText
- Growth Bar: 拖入GrowthBar

#### 环境显示
- Temperature Text: 拖入TemperatureText
- Humidity Text: 拖入HumidityText
- Sunlight Text: 拖入SunlightText

#### 温度控制
- Temperature Up Button: 拖入TempUpButton
- Temperature Down Button: 拖入TempDownButton

#### 湿度控制
- Humidity Up Button: 拖入HumidityUpButton
- Humidity Down Button: 拖入HumidityDownButton

#### 圆形控制按钮
- Pause Button: 拖入PauseButton
- Reset Button: 拖入ResetButton
- Pause Button Text: 拖入PauseButton的子Text

#### 调节参数
- Temperature Step: 1 (每次调节1度)
- Humidity Step: 5 (每次调节5%)

#### 目标
- Tree Controller: 拖入场景中的OrangeTreeController
- Environment Manager: 会自动查找

### 6. 美化建议

#### 添加按钮悬停效果
1. 选中任意按钮
2. 在Button组件中设置：
   - Normal Color: 原色
   - Highlighted Color: 稍亮的颜色
   - Pressed Color: 稍暗的颜色
   - Disabled Color: 灰色
   - Color Multiplier: 1.2
   - Fade Duration: 0.1

#### 添加阴影效果
1. 选中按钮
2. Add Component -> Shadow
3. 设置：
   - Effect Color: 黑色半透明
   - Effect Distance: (2, -2)
   - Use Graphic Alpha: 勾选

#### 添加按钮动画
1. 选中按钮
2. Add Component -> Animator
3. 创建简单的缩放动画：
   - 按下时缩小到0.95
   - 释放时恢复到1.0

#### 添加背景装饰
1. 在EnvironmentPanel中添加标题文本
2. 添加图标（温度计、水滴等）
3. 使用渐变背景

### 7. 快速创建三角形的方法（无需外部工具）

如果没有图片编辑工具，可以使用代码生成三角形：

1. 创建一个新脚本 `TriangleImage.cs`：
```csharp
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class TriangleImage : MaskableGraphic
{
    public bool pointUp = true;
    
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;
        
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;
        
        if (pointUp)
        {
            // 正三角形
            vertex.position = new Vector3(0, height / 2);
            vh.AddVert(vertex);
            vertex.position = new Vector3(-width / 2, -height / 2);
            vh.AddVert(vertex);
            vertex.position = new Vector3(width / 2, -height / 2);
            vh.AddVert(vertex);
        }
        else
        {
            // 倒三角形
            vertex.position = new Vector3(0, -height / 2);
            vh.AddVert(vertex);
            vertex.position = new Vector3(width / 2, height / 2);
            vh.AddVert(vertex);
            vertex.position = new Vector3(-width / 2, height / 2);
            vh.AddVert(vertex);
        }
        
        vh.AddTriangle(0, 1, 2);
    }
}
```

2. 使用方法：
   - 创建空GameObject作为按钮的子对象
   - Add Component -> Triangle Image
   - 设置Point Up为true（正三角）或false（倒三角）
   - 调整颜色

### 8. 测试

1. 保存场景
2. 点击Play
3. 测试功能：
   - 点击正三角形，温度/湿度应该增加
   - 点击倒三角形，温度/湿度应该减少
   - 点击圆形暂停按钮，生长应该暂停
   - 点击圆形重置按钮，树应该重置

## 常见问题

### 按钮不是圆形
- 确保使用了圆形的Sprite（如UI/Skin/Knob）
- 或者在Image组件中设置Image Type为Filled，Fill Method为Radial 360

### 三角形显示不正确
- 检查Sprite的导入设置
- 确保Preserve Aspect已勾选
- 或使用代码生成的TriangleImage组件

### 按钮点击没反应
- 检查Canvas是否有EventSystem
- 确保按钮的Interactable已勾选
- 检查脚本中的按钮引用是否正确拖入

### 温度/湿度调节无效
- 检查EnvironmentManager是否存在
- 确保EnvironmentManager有SetTemperature和SetHumidity方法
- 查看Console是否有错误信息

## 进阶优化

### 添加长按连续调节
修改脚本，使用EventTrigger实现长按效果：
- 按住按钮时持续调节数值
- 松开时停止

### 添加数值输入框
允许用户直接输入温度和湿度值：
- 使用InputField组件
- 添加数值验证

### 添加预设按钮
创建快速设置按钮：
- "理想环境"：设置最佳温湿度
- "极端环境"：测试树的适应能力

