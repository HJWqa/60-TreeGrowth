# 最终UI效果说明

## 按钮样式

### 温度/湿度调节按钮
- **大小**：120x120 像素（圆形）
- **背景**：纯色填充圆形（无边框）
  - 增加按钮：浅红色 (255, 102, 102)
  - 减少按钮：浅蓝色 (102, 153, 255)
- **图标**：白色三角形（50x50像素）
  - 增加：▲ 正三角形
  - 减少：▼ 倒三角形

### 暂停/重置按钮
- **大小**：120x120 像素（圆形）
- **背景**：纯色填充圆形（无边框）
  - 暂停：绿色 (102, 204, 102)
  - 重置：橙色 (255, 153, 51)
- **文字**：在圆形中间

## 视觉效果

```
温度控制：
   🔴
   ⏶   ← 120x120 红色圆形 + 白色正三角
 25.0°C
   🔵
   ⏷   ← 120x120 蓝色圆形 + 白色倒三角

湿度控制：
   🔴
   ⏶   ← 120x120 红色圆形 + 白色正三角
  60%
   🔵
   ⏷   ← 120x120 蓝色圆形 + 白色倒三角
```

## 使用转换工具

1. 打开：`TreePlanQAQ > UI Tools > Convert Buttons to Shapes`
2. 点击：`🎯 自动设置所有UI形状（推荐）`
3. 完成！

## 按钮结构

```
Button (120x120)
├── CircleImage组件 ← 纯色圆形背景
│   ├── filled = true (填充模式)
│   ├── segments = 36 (圆滑)
│   └── circleColor = 红色/蓝色
├── Button组件
└── TriangleIcon (子对象, 50x50)
    └── TriangleImage组件 ← 白色三角形
        ├── pointUp = true/false
        └── triangleColor = 白色
```

## 颜色代码

### RGB值
- 红色圆形：(255, 102, 102)
- 蓝色圆形：(102, 153, 255)
- 绿色圆形：(102, 204, 102)
- 橙色圆形：(255, 153, 51)
- 白色三角：(255, 255, 255)

### Unity Color值
```csharp
// 增加按钮（红色）
new Color(1f, 0.4f, 0.4f)

// 减少按钮（蓝色）
new Color(0.4f, 0.6f, 1f)

// 暂停按钮（绿色）
new Color(0.4f, 0.8f, 0.4f)

// 重置按钮（橙色）
new Color(1f, 0.6f, 0.2f)

// 三角形（白色）
Color.white
```

## 手动调整

### 改变圆形大小
1. 选中按钮
2. RectTransform → Size: (120, 120)
3. 保持正方形以维持圆形

### 改变三角形大小
1. 选中 TriangleIcon 子对象
2. RectTransform → Size: (50, 50)
3. 建议范围：30-60像素

### 改变颜色
1. 选中按钮
2. CircleImage组件 → Circle Color
3. 选择你喜欢的颜色

### 改变三角形颜色
1. 选中 TriangleIcon
2. TriangleImage组件 → Triangle Color
3. 建议使用白色以获得最佳对比度

## 布局建议

### 垂直排列（推荐）
```
间距：20-30像素

[  ⏶  ] ← 增加按钮
  ↓ 20px
25.0°C  ← 数值显示
  ↓ 20px
[  ⏷  ] ← 减少按钮
```

### 位置设置
```
TempUpButton:
  Pos Y: 80

TempText:
  Pos Y: 0

TempDownButton:
  Pos Y: -80
```

## 完整示例

### 创建温度控制组
1. 创建空对象 "TempGroup"
2. 添加子对象：
   - TempUpButton (Button)
   - TempText (Text)
   - TempDownButton (Button)
3. 运行转换工具
4. 调整位置（见上方位置设置）

### 创建湿度控制组
1. 复制 TempGroup
2. 重命名为 "HumidityGroup"
3. 重命名子对象：
   - HumidityUpButton
   - HumidityText
   - HumidityDownButton
4. 按钮会自动识别并转换

## 优化建议

### 添加悬停效果
Button组件设置：
- Normal Color: 原色
- Highlighted Color: 稍亮 (Color Multiplier: 1.2)
- Pressed Color: 稍暗 (Color Multiplier: 0.8)

### 添加阴影
1. 选中按钮
2. Add Component → Shadow
3. Effect Distance: (3, -3)
4. Effect Color: 黑色半透明 (0, 0, 0, 128)

### 添加动画
1. 按下时缩小到 0.95
2. 释放时恢复到 1.0
3. 使用 DOTween 或 Animator

## 性能说明

- CircleImage 使用代码生成网格
- 36个分段足够圆滑
- 不需要额外的图片资源
- 运行时性能良好

