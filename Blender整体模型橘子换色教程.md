# Blender整体模型橘子换色教程

## 你的情况
- 整棵树是一个完整对象（树干、叶子、橘子连在一起）
- 有复杂的细分网格
- 可能使用了顶点颜色或纹理贴图

---

## 🎯 方法一：通过材质节点修改（最简单）

### 步骤1：检查当前材质设置
1. 选中橘子树模型
2. 切换到 **Shading** 工作区（顶部标签）
3. 在下方 **Shader Editor** 查看节点树

### 步骤2：找到颜色来源
查看 Principled BSDF 的 Base Color 连接了什么：

#### 情况A：连接了 Color Attribute（顶点颜色）
如果看到 **Color Attribute** 节点：
```
Color Attribute → Principled BSDF (Base Color)
```

**解决方案：添加颜色调整节点**

1. 按 `Shift + A` 添加节点
2. 选择 **Color → RGB Curves** 或 **Color → Hue Saturation Value**
3. 插入到 Color Attribute 和 Principled BSDF 之间：
```
Color Attribute → Hue Saturation Value → Principled BSDF
```

4. 调整参数：
   - **Hue（色相）**: 调整整体颜色倾向
     - 向左：偏红/黄
     - 向右：偏绿/蓝
   - **Saturation（饱和度）**: 增加或减少颜色鲜艳度
   - **Value（明度）**: 调整亮度

#### 情况B：连接了 Image Texture（纹理贴图）
如果看到 **Image Texture** 节点：
```
Image Texture → Principled BSDF (Base Color)
```

**解决方案：使用 ColorRamp 或 Mix RGB**

1. 按 `Shift + A` → **Converter → ColorRamp**
2. 插入到 Image Texture 和 Principled BSDF 之间
3. 调整 ColorRamp 的色标：
   - 左侧色标：暗部颜色（深橙色）
   - 右侧色标：亮部颜色（浅橙色）

或者使用 Mix RGB：
1. 按 `Shift + A` → **Color → Mix RGB**
2. 设置混合模式为 "Multiply" 或 "Overlay"
3. Color2 设置为你想要的橘子颜色
4. 调整 Fac 值控制混合强度（0.5-0.8）

---

## 🎯 方法二：通过顶点组分离材质（精确控制）

如果你想只改橘子，不影响叶子和树干：

### 步骤1：创建顶点组
1. 选中模型，按 `Tab` 进入编辑模式
2. 按 `Alt + A` 取消全选
3. 按 `C` 进入圆形选择模式（或按 `B` 框选）
4. 滚动鼠标滚轮调整选择圈大小
5. 在橘子上拖动鼠标选择所有橘子的顶点
6. 选完后按 `Esc` 或右键退出选择模式

### 步骤2：分配顶点组
1. 在右侧属性面板找到 **Object Data Properties**（绿色三角形图标）
2. 展开 **Vertex Groups**
3. 点击 "+" 添加新组，命名为 "Oranges"
4. 点击 "Assign" 按钮
5. 按 `Tab` 退出编辑模式

### 步骤3：使用顶点组控制颜色
在 Shading 工作区：

1. 按 `Shift + A` → **Input → Attribute**
2. 在 Attribute 节点的 Name 字段输入 "Oranges"
3. 将 Fac 输出连接到 **Mix RGB** 或 **ColorRamp** 的 Fac 输入
4. 构建节点树：

```
原始颜色 ─┐
          ├→ Mix RGB → Principled BSDF
橘子新颜色 ─┘     ↑
                  │
Attribute(Oranges) Fac
```

这样只有顶点组 "Oranges" 的部分会应用新颜色。

---

## 🎯 方法三：使用材质槽（最灵活）

### 步骤1：添加新材质槽
1. 选中模型
2. 在右侧 **Material Properties**（球体图标）
3. 点击材质槽列表下方的 "+" 添加新槽
4. 点击 "New" 创建新材质，命名 "Orange_New"
5. 设置你想要的橘子颜色

### 步骤2：分配材质到橘子
1. 按 `Tab` 进入编辑模式
2. 选择所有橘子的面（参考方法二步骤1）
3. 在材质槽列表选择 "Orange_New"
4. 点击 "Assign" 按钮
5. 按 `Tab` 退出编辑模式

---

## 🎨 推荐的橘子颜色方案

### 成熟橘子（橙色）
```
Base Color: RGB(1.0, 0.5, 0.0) 或 Hex #FF8000
Roughness: 0.4
Subsurface: 0.15
```

### 未成熟橘子（绿色）
```
Base Color: RGB(0.5, 0.8, 0.3) 或 Hex #80CC4D
Roughness: 0.3
```

### 过熟橘子（深橙/红）
```
Base Color: RGB(0.9, 0.3, 0.0) 或 Hex #E64D00
Roughness: 0.5
```

### 柠檬色
```
Base Color: RGB(1.0, 0.9, 0.0) 或 Hex #FFE600
Roughness: 0.35
```

---

## 🔍 快速诊断：我该用哪个方法？

### 检查步骤：
1. 选中模型 → Shading 工作区
2. 查看 Shader Editor 中的节点

**如果看到：**
- ✅ **Color Attribute** → 用方法一的情况A（添加 HSV 节点）
- ✅ **Image Texture** → 用方法一的情况B（添加 ColorRamp）
- ✅ **只有 Principled BSDF** → 直接改 Base Color
- ✅ **想精确控制** → 用方法二（顶点组）或方法三（材质槽）

---

## 💡 实用技巧

### 技巧1：实时预览颜色变化
- 按 `Z` 键 → 选择 "Material Preview" 或 "Rendered"
- 调整节点参数时立即看到效果

### 技巧2：快速选择橘子
在编辑模式下：
- 按 `L` 键（鼠标悬停在橘子上）选择连接的几何体
- 如果橘子是独立的球体，这个方法最快

### 技巧3：批量调整多个橘子
如果橘子是复制的实例：
- 修改一个，其他自动更新
- 检查方法：选中橘子 → 看右上角是否显示 "Instance"

### 技巧4：保留原始纹理细节
使用 Mix RGB 节点时：
- 混合模式选 "Overlay" 或 "Soft Light"
- 保留原始的明暗变化和纹理细节
- 只改变颜色倾向

---

## ⚠️ 常见问题

### Q: 改了颜色但看不到变化？
A: 
1. 检查视图模式（按 `Z` 切换到 Material Preview）
2. 确认选中了正确的材质槽
3. 检查节点是否正确连接

### Q: 叶子和树干也变色了？
A: 使用方法二（顶点组）或方法三（材质槽）精确控制

### Q: 颜色太亮或太暗？
A: 
- 调整 Principled BSDF 的 Roughness（粗糙度）
- 添加 Brightness/Contrast 节点微调
- 检查场景光照设置

### Q: 想要渐变色橘子？
A: 
1. 使用 ColorRamp 节点
2. 添加多个色标创建渐变
3. 用 Texture Coordinate → Object 控制渐变方向

---

## 🚀 快速操作流程（最常用）

**30秒快速换色：**

1. 选中模型 → `Shift + F3`（Shading 工作区）
2. 找到 Principled BSDF 节点
3. 点击 Base Color 色块 → 选择新颜色
4. 按 `Z` → Material Preview 查看效果
5. 完成！

**如果上面不行（有 Color Attribute）：**

1. `Shift + A` → Color → Hue Saturation Value
2. 插入到 Color Attribute 和 Principled BSDF 之间
3. 调整 Hue 滑块改变颜色
4. 完成！
