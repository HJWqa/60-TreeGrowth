# Blender 手搓叶子详细教程

> 纯建模，不用纹理，从零开始手工制作写实叶子

## 目标效果

- 椭圆形叶片
- 中央主叶脉凸起
- 侧叶脉细节
- 叶片边缘自然弯曲
- 叶柄（叶茎）

---

## 第一步：创建基础叶片形状（5分钟）

### 1. 创建Plane

```
Shift + A → Mesh → Plane
S → 0.15（缩放到15厘米）
保存：Ctrl + S
```

### 2. 变成椭圆形

```
Tab（进入编辑模式）
S → Y → 1.5（Y轴拉长，变椭圆）
```

### 3. 细分叶片

```
A（全选）
右键 → Subdivide
左下角设置：Number of Cuts: 5
现在叶片有6x6的网格
```

### 4. 塑造叶尖

```
1（切换到顶点模式）
选中最顶端的顶点
G → Y → 0.02（向上拉一点，形成尖端）
S → 0.7（缩小，让尖端更尖）
```

### 5. 塑造叶基（底部）

```
选中最底端的顶点
G → Y → -0.01（向下拉一点）
S → 0.8（稍微缩小）
```

---

## 第二步：添加主叶脉（10分钟）

### 方法A：挤出法（推荐）

**1. 选择中央顶点线**

```
Alt + 左键点击中央的边
会选中整条中央线
```

**2. 挤出叶脉**

```
E（挤出）
Z（限制在Z轴）
0.005（向上挤出0.5厘米）
Enter
```

**3. 加粗叶脉**

```
保持选中状态
S → Shift + Z（排除Z轴）
1.2（稍微加粗）
```

### 方法B：使用修改器（简单但效果好）

**1. 退出编辑模式**

```
Tab（回到物体模式）
```

**2. 添加Solidify修改器**

```
右侧 → Modifier Properties（扳手图标）
Add Modifier → Solidify
设置：
- Thickness: 0.002（厚度2毫米）
- Offset: 1.0（向上）
```

**3. 添加Subdivision Surface**

```
Add Modifier → Subdivision Surface
设置：
- Levels Viewport: 2
- Render: 3
```

---

## 第三步：添加侧叶脉（15分钟）

### 1. 进入编辑模式

```
Tab
1（顶点模式）
```

### 2. 启用比例编辑

```
O（启用比例编辑）
顶部会显示圆圈图标
```

### 3. 创建叶脉凹陷

**选择侧边的顶点：**

```
选中叶片左侧或右侧的一排顶点
（从中央向外数第2-3排）
```

**向下压：**

```
G → Z → -0.002（向下移动2毫米）
滚动鼠标滚轮调整影响范围
Enter
```

**重复另一侧：**

```
选中右侧对应的顶点
G → Z → -0.002
```

### 4. 添加更多细节（可选）

```
随机选择一些顶点
G → Z → 轻微上下移动
制造不规则的表面
```

---

## 第四步：叶片边缘处理（10分钟）

### 1. 选择边缘顶点

```
Alt + Shift + 左键点击边缘
选中整圈边缘顶点
```

### 2. 向下弯曲边缘

```
G → Z → -0.003（向下3毫米）
制造叶片边缘下垂效果
```

### 3. 添加轻微波浪（可选）

```
保持边缘选中
O（启用比例编辑）
随机选择几个边缘顶点
G → Z → 轻微上下移动
制造自然的波浪边缘
```

---

## 第五步：整体弯曲叶片（5分钟）

### 方法A：手动弯曲

```
Tab（编辑模式）
O（比例编辑）
选中叶尖的顶点
G → Z → 0.01（向上弯）
或
G → Z → -0.01（向下弯）
```

### 方法B：使用Simple Deform修改器

```
Tab（物体模式）
Add Modifier → Simple Deform
设置：
- Mode: Bend（弯曲）
- Angle: 15度（轻微弯曲）
- Axis: Y
```

---

## 第六步：添加叶柄（5分钟）

### 1. 创建圆柱体

```
Shift + A → Mesh → Cylinder
设置：
- Vertices: 8（8个顶点够用）
- Radius: 0.005（5毫米粗）
- Depth: 0.05（5厘米长）
```

### 2. 旋转和定位

```
R → X → 90（旋转90度，横向）
G → Y → -0.08（移动到叶片底部）
```

### 3. 连接到叶片

```
选中叶柄
Shift + 选中叶片
Ctrl + J（合并）
```

### 4. 平滑叶柄

```
Tab（编辑模式）
选中叶柄部分的顶点
右键 → Shade Smooth
```

---

## 第七步：添加材质（10分钟）

### 1. 切换到Shading工作区

```
顶部标签 → Shading
```

### 2. 创建材质

```
Material Properties → New
命名：LeafMaterial
```

### 3. 设置Principled BSDF

**基础颜色：**
```
Base Color: RGB(0.15, 0.50, 0.15) - 深绿色
```

**表面属性：**
```
Metallic: 0
Specular: 0.3（轻微反光）
Roughness: 0.4（有点光泽）
Sheen: 0.2（柔和光泽）
```

**次表面散射（重要！）：**
```
Subsurface: 0.3（透光效果）
Subsurface Radius: RGB(0.5, 0.8, 0.5)
Subsurface Color: RGB(0.6, 0.9, 0.6) - 浅绿色
```

**透明度（可选）：**
```
Transmission: 0.1（轻微透光）
```

### 4. 叶柄材质

```
选中叶柄部分
Material Properties → + → New
Base Color: RGB(0.3, 0.4, 0.2) - 黄绿色
Roughness: 0.6
```

---

## 第八步：添加程序化纹理（可选，增加细节）

### 1. 添加Noise纹理（叶片颜色变化）

```
Shift + A → Texture → Noise Texture
设置：
- Scale: 20
- Detail: 5
- Roughness: 0.5
```

### 2. 混合到Base Color

```
Shift + A → Color → Mix
连接：
- Noise Texture (Fac) → Mix (Factor): 0.2
- 原Base Color → Mix (Color1)
- 稍浅的绿色 → Mix (Color2)
- Mix (Result) → Principled BSDF (Base Color)
```

### 3. 添加Bump（表面凹凸）

```
Shift + A → Texture → Noise Texture
设置：
- Scale: 100（很细的纹理）
- Detail: 10

Shift + A → Vector → Bump
连接：
- Noise Texture (Fac) → Bump (Height)
- Bump (Normal) → Principled BSDF (Normal)
设置：
- Strength: 0.05（轻微凹凸）
```

---

## 第九步：最终调整（5分钟）

### 1. 平滑着色

```
Tab（物体模式）
右键 → Shade Smooth
```

### 2. 添加边缘锐化（可选）

```
Tab（编辑模式）
3（面选择模式）
选中叶脉部分的面
Shift + E（边缘锐化）
向上拖动鼠标
```

### 3. 查看效果

```
Z → Material Preview（材质预览）
或
Z → Rendered（渲染预览）
```

### 4. 添加光照

```
Shift + A → Light → Sun
R → X → 45（旋转45度）
调整强度：右侧 Light Properties → Strength: 3
```

---

## 第十步：创建变体（10分钟）

### 1. 复制叶子

```
Shift + D（复制）
Enter
```

### 2. 随机变化

**大小变化：**
```
S → 0.8-1.2（随机缩放）
```

**旋转变化：**
```
R → Z → 随机角度
R → X → 5-15度（轻微倾斜）
```

**弯曲变化：**
```
Tab（编辑模式）
O（比例编辑）
选中叶尖
G → Z → 随机上下移动
```

**颜色变化：**
```
Material Properties
复制材质
调整 Base Color 的绿色深浅
```

### 3. 创建5-6个变体

```
重复上述步骤
创建不同大小、角度、弯曲的叶子
```

---

## 第十一步：导出到Unity（5分钟）

### 1. 准备导出

```
选中所有叶子变体
Ctrl + A → All Transforms（应用所有变换）
```

### 2. 导出FBX

```
File → Export → FBX (.fbx)
设置：
- ☑ Selected Objects
- Scale: 1.00
- Forward: -Z Forward
- Up: Y Up
- ☑ Apply Modifiers
```

### 3. 保存位置

```
Unity项目/Assets/Models/Leaves/OrangeLeaf.fbx
```

---

## 性能优化版本

如果觉得太复杂，可以简化：

### 简化版叶子（低多边形）

**步骤1：基础形状**
```
Plane → S → 0.15
S → Y → 1.5（椭圆）
Subdivide: 2次（不要太多）
```

**步骤2：简单弯曲**
```
选中叶尖顶点
G → Z → 0.01（轻微向上）
```

**步骤3：材质**
```
Base Color: 深绿色
Roughness: 0.4
Subsurface: 0.3
完成！
```

---

## 快速检查清单

建模完成前：
- [ ] 叶片是椭圆形
- [ ] 有主叶脉凸起
- [ ] 边缘自然弯曲
- [ ] 有叶柄
- [ ] 已平滑着色
- [ ] 材质已设置
- [ ] 次表面散射已启用

导出前：
- [ ] 所有变换已应用
- [ ] 修改器已应用
- [ ] 原点位置正确
- [ ] 大小合适（10-15厘米）

---

## 时间分配

| 步骤 | 时间 | 说明 |
|------|------|------|
| 基础形状 | 5分钟 | Plane + 椭圆 + 细分 |
| 主叶脉 | 10分钟 | 挤出或修改器 |
| 侧叶脉 | 15分钟 | 比例编辑 |
| 边缘处理 | 10分钟 | 弯曲边缘 |
| 整体弯曲 | 5分钟 | 自然形态 |
| 叶柄 | 5分钟 | 圆柱体 |
| 材质 | 10分钟 | 颜色+SSS |
| 程序纹理 | 10分钟 | 可选 |
| 最终调整 | 5分钟 | 平滑+光照 |
| 创建变体 | 10分钟 | 5-6个 |
| **总计** | **60-85分钟** | 完整版 |
| **简化版** | **20分钟** | 基础版 |

---

## 常见问题

**Q: 叶脉不明显？**
A: 
- 增加挤出高度（E → Z → 0.01）
- 或增加Solidify厚度
- 添加边缘锐化（Shift + E）

**Q: 叶片太平？**
A: 
- 使用比例编辑（O）随机移动顶点
- 添加Simple Deform修改器
- 手动弯曲叶尖和边缘

**Q: 边缘太锐利？**
A: 
- 右键 → Shade Smooth
- 增加Subdivision Surface级别
- 使用Bevel修改器圆润边缘

**Q: 材质不透光？**
A: 
- 确保启用Subsurface Scattering
- 设置Subsurface: 0.3-0.5
- 添加Transmission: 0.1

**Q: 性能太差？**
A: 
- 减少Subdivide次数（2-3次够用）
- 降低Subdivision Surface级别
- 简化叶脉细节

---

## 进阶技巧

### 1. 添加虫咬痕迹

```
Tab（编辑模式）
选择几个面
X → Delete Faces（删除面）
制造破洞效果
```

### 2. 添加枯萎效果

```
材质：
- Base Color: 黄褐色 RGB(0.6, 0.5, 0.2)
- Roughness: 0.8（更粗糙）
- Subsurface: 0.1（减少透光）

建模：
- 边缘向下弯曲更多
- 添加更多不规则变形
```

### 3. 添加水珠

```
Shift + A → Mesh → UV Sphere
S → 0.003（3毫米）
S → Z → 0.7（压扁）
材质：
- Transmission: 1.0（完全透明）
- Roughness: 0.0（完全光滑）
- IOR: 1.33（水的折射率）
```

---

## 最终效果

完成后你会有：
- ✅ 手工制作的写实叶子
- ✅ 自然的叶脉细节
- ✅ 真实的弯曲形态
- ✅ 透光效果
- ✅ 5-6个变体
- ✅ 可用于Unity的FBX模型

虽然比用纹理慢，但完全是你自己手搓的，成就感满满！

---

## 快速开始（立即动手）

```
1. Shift + A → Plane
2. S → 0.15
3. Tab → S → Y → 1.5
4. 右键 → Subdivide → 5次
5. 选中中央线 → E → Z → 0.005
6. Tab → 右键 → Shade Smooth
7. 材质：绿色 + Subsurface: 0.3
8. 完成！
```

现在就开始吧！记得随时保存（Ctrl + S）！🍃
