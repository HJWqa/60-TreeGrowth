# Blender桃子树逆向建模详细操作步骤

## 🎯 核心思路

**逆向建模** = 从成熟的树模型开始，通过删除、缩小、简化，逐步创建更早期的生长阶段

**优势**：
- 保持所有阶段的风格一致
- 比从零开始快得多
- 材质和结构可以复用

---

## 📁 第一步：准备工作（5分钟）

### 1. 备份原始模型
```
重要！先备份你的桃子树模型！
```

1. 找到你的桃子树模型文件（.blend或.fbx）
2. 复制一份，命名为 `PeachTree_Original_BACKUP.blend`
3. 创建工作文件夹：
```
PeachTreeStages/
├── PeachTree_Stage1_Seed.blend
├── PeachTree_Stage2_Sprout.blend
├── PeachTree_Stage3_Seedling.blend
├── PeachTree_Stage4_YoungTree.blend
├── PeachTree_Stage5_MatureTree.blend
├── PeachTree_Stage6_Flowering.blend
├── PeachTree_Stage7_Fruiting.blend
└── PeachTree_Stage8_Harvest.blend
```

### 2. 打开原始模型
1. 启动Blender
2. File → Open → 选择你的桃子树模型
3. 观察模型结构：
   - 按 `Z` → Wireframe（线框模式）查看结构
   - 在右侧Outliner面板查看对象层级
   - 记录下有哪些部分：树干、树枝、叶子、桃子等

### 3. 了解模型组成
在Outliner（右上角）中，你的模型可能包含：
- `Trunk`（树干）
- `Branches`（树枝）
- `Leaves`（叶子）- 可能是粒子系统或单独对象
- `Peaches`（桃子）
- `Flowers`（花朵）- 如果有的话

---

## 🌳 第二步：创建阶段8 - 成熟期（已有）

这就是你现有的模型！

1. File → Save As → `PeachTree_Stage8_Harvest.blend`
2. 确保模型完整：
   - 有成熟的粉红色桃子
   - 有茂密的绿叶
   - 树干和树枝完整

**检查清单**：
- ✓ 树干粗壮
- ✓ 树枝茂密
- ✓ 叶子多
- ✓ 桃子成熟（粉红色）

---

## 🍑 第三步：创建阶段7 - 结果期（15分钟）

### 目标：青色的小桃子，叶子完整

1. **打开阶段8文件**
   - File → Open → `PeachTree_Stage8_Harvest.blend`

2. **另存为阶段7**
   - File → Save As → `PeachTree_Stage7_Fruiting.blend`

3. **修改桃子颜色（变成青色）**
   
   **方法A：如果桃子是单独的对象**
   - 在Outliner中找到桃子对象（可能叫Peach、Fruit等）
   - 按住 `Shift` 点击选择所有桃子
   - 切换到Shading工作区（顶部标签）
   - 找到Principled BSDF节点
   - 修改Base Color：
     - R: 0.5（或输入128）
     - G: 0.75（或输入192）
     - B: 0.5（或输入128）
   - 结果：青绿色的未成熟桃子

   **方法B：如果桃子使用粒子系统**
   - 选择树枝对象
   - 右侧属性面板 → 粒子系统图标（像火花的图标）
   - 找到桃子的粒子系统
   - 展开 Render → Object
   - 选择桃子实例对象
   - 按方法A修改颜色

4. **删除部分桃子（保留70%）**
   - 按 `Tab` 进入编辑模式
   - 按 `Alt + A` 取消全选
   - 按住 `Shift` 随机选择30%的桃子
   - 按 `X` → Delete → Vertices（删除）
   - 按 `Tab` 返回对象模式

5. **缩小桃子尺寸**
   - 选择所有桃子对象
   - 按 `S`（缩放）
   - 输入 `0.7`
   - 按 `Enter` 确认

6. **保存**
   - `Ctrl + S` 保存

**检查清单**：
- ✓ 桃子变成青绿色
- ✓ 桃子数量减少
- ✓ 桃子比阶段8小
- ✓ 叶子保持不变

---

## 🌸 第四步：创建阶段6 - 开花期（20分钟）

### 目标：白色/粉色花朵，没有桃子

1. **打开阶段7文件**
   - File → Open → `PeachTree_Stage7_Fruiting.blend`

2. **另存为阶段6**
   - File → Save As → `PeachTree_Stage6_Flowering.blend`

3. **删除所有桃子**
   - 在Outliner中找到所有桃子对象
   - 按住 `Shift` 选择所有桃子
   - 按 `X` → Delete（删除）
   - 或者：如果是粒子系统，直接删除桃子粒子系统

4. **创建花朵**

   **简单方法（推荐）**：
   - 按 `Shift + A` → Mesh → UV Sphere
   - 按 `S` 缩放到很小：输入 `0.03`
   - 按 `Tab` 进入编辑模式
   - 按 `A` 全选
   - 按 `S` → `Z` → `0.3`（压扁成花朵形状）
   - 按 `Tab` 返回对象模式
   - 切换到Shading工作区
   - 设置材质颜色：
     - Base Color: 粉白色
     - R: 1.0, G: 0.9, B: 0.95
   - 命名为 `Flower`

   **详细方法（更真实）**：
   - 创建5片花瓣：
     - `Shift + A` → Mesh → Plane
     - 按 `S` 缩放到 `0.02`
     - 按 `S` → `Y` → `2`（拉长）
     - 进入编辑模式，选择顶部边，按 `S` 缩小（形成尖端）
     - 复制5次，旋转排列成花朵
   - 添加花心：
     - `Shift + A` → Mesh → UV Sphere
     - 缩放到很小，放在中心
     - 颜色设为黄色 (R:1, G:0.8, B:0.2)

5. **复制花朵到树上**
   
   **手动放置（精确控制）**：
   - 选择花朵对象
   - 按 `Shift + D` 复制
   - 按 `G` 移动到树枝末端
   - 重复20-30次
   - 主要放在树枝末端和树冠外围

   **使用粒子系统（快速）**：
   - 选择树枝对象
   - 右侧属性面板 → 粒子系统 → `+` 添加新系统
   - 设置：
     - Number: 25
     - Frame Start: 1, End: 1（静态）
     - Lifetime: 100
   - Render部分：
     - Render As: Object
     - Instance Object: 选择你的花朵对象
     - Scale: 1.0
     - Scale Randomness: 0.3
   - Rotation部分：
     - Orientation Axis: Normal
     - Random: 0.5

6. **保存**
   - `Ctrl + S`

**检查清单**：
- ✓ 没有桃子
- ✓ 有20-30朵花
- ✓ 花朵是粉白色
- ✓ 树干和叶子完整

---

## 🌲 第五步：创建阶段5 - 成树期（10分钟）

### 目标：完整的树，但没有花和果

1. **打开阶段6文件**
   - File → Open → `PeachTree_Stage6_Flowering.blend`

2. **另存为阶段5**
   - File → Save As → `PeachTree_Stage5_MatureTree.blend`

3. **删除所有花朵**
   - 在Outliner中选择所有花朵对象
   - 按 `X` → Delete
   - 或删除花朵粒子系统

4. **检查树的完整性**
   - 确保树干粗壮
   - 树枝完整
   - 叶子茂密

5. **保存**
   - `Ctrl + S`

**检查清单**：
- ✓ 没有花朵
- ✓ 没有桃子
- ✓ 树干粗壮
- ✓ 叶子茂密

---

## 🌳 第六步：创建阶段4 - 小树期（20分钟）

### 目标：较小的树，树枝较少

1. **打开阶段5文件**
   - File → Open → `PeachTree_Stage5_MatureTree.blend`

2. **另存为阶段4**
   - File → Save As → `PeachTree_Stage4_YoungTree.blend`

3. **删除部分树枝**
   
   **识别主要树枝和次要树枝**：
   - 按 `Z` → Wireframe 查看结构
   - 主要树枝：从树干直接长出的粗树枝
   - 次要树枝：从主要树枝分出的细树枝

   **删除次要树枝**：
   - 按 `Tab` 进入编辑模式
   - 按 `3` 切换到面选择模式
   - 按住 `Alt` 点击次要树枝的边环
   - 按 `X` → Delete → Vertices
   - 保留3-4根主要树枝即可
   - 按 `Tab` 返回对象模式

4. **缩小整体尺寸**
   - 按 `A` 全选所有对象
   - 按 `S` 缩放
   - 输入 `0.6`
   - 按 `Enter`

5. **减少叶子数量**
   
   **如果叶子是粒子系统**：
   - 选择树枝对象
   - 右侧属性 → 粒子系统
   - 找到叶子系统
   - 修改 Number: 减少到原来的50%

   **如果叶子是单独对象**：
   - 在Outliner中选择叶子
   - 随机删除50%的叶子

6. **调整树冠**
   - 如果树冠是球体组成：
   - 选择外层的球体
   - 删除或缩小

7. **保存**
   - `Ctrl + S`

**检查清单**：
- ✓ 树比阶段5小（约60%）
- ✓ 只有3-4根主要树枝
- ✓ 叶子数量减少
- ✓ 树冠较小

---

## 🌿 第七步：创建阶段3 - 幼苗期（15分钟）

### 目标：只有主干和顶部几片叶子

1. **打开阶段4文件**
   - File → Open → `PeachTree_Stage4_YoungTree.blend`

2. **另存为阶段3**
   - File → Save As → `PeachTree_Stage3_Seedling.blend`

3. **删除所有树枝**
   - 在Outliner中找到树枝对象
   - 选择所有树枝
   - 按 `X` → Delete
   - 只保留主干（Trunk）

4. **缩短主干**
   - 选择主干对象
   - 按 `Tab` 进入编辑模式
   - 按 `Alt + A` 取消全选
   - 按 `3` 切换到面选择模式
   - 选择顶部的面
   - 按 `G` → `Z`（沿Z轴移动）
   - 向下移动，缩短主干到原来的30%
   - 按 `Tab` 返回对象模式

5. **缩小主干直径**
   - 选择主干
   - 按 `S` → `Shift + Z`（限制在XY平面缩放）
   - 输入 `0.3`
   - 按 `Enter`

6. **只保留顶部4-6片叶子**
   - 删除所有叶子粒子系统
   - 手动创建几片叶子：
     - `Shift + A` → Mesh → Plane
     - 按 `S` → `Y` → `2`（拉长）
     - 按 `Tab` 进入编辑模式
     - 选择顶部边，按 `S` 缩小（形成尖端）
     - 按 `Tab` 返回对象模式
     - 设置绿色材质
     - 复制4-6片，围绕主干顶部排列

7. **整体缩小**
   - 按 `A` 全选
   - 按 `S` → `0.3`

8. **保存**
   - `Ctrl + S`

**检查清单**：
- ✓ 没有树枝
- ✓ 只有细细的主干
- ✓ 只有4-6片小叶子在顶部
- ✓ 整体很小

---

## 🌱 第八步：创建阶段2 - 发芽期（15分钟）

### 目标：细茎 + 2片小叶子

1. **创建新文件**
   - File → New → General
   - 删除默认立方体（选中后按 `X`）

2. **创建细茎**
   - `Shift + A` → Mesh → Cylinder
   - 按 `S` 缩放到很小：`0.02`
   - 按 `S` → `Z` → `15`（拉长）
   - 按 `G` → `Z` → `0.15`（向上移动）
   - 设置材质：
     - 切换到Shading工作区
     - Base Color: 浅绿色
     - R: 0.5, G: 0.9, B: 0.5

3. **创建第一片叶子**
   - `Shift + A` → Mesh → Plane
   - 按 `S` → `0.08`（缩小）
   - 按 `S` → `Y` → `1.5`（拉长）
   - 按 `Tab` 进入编辑模式
   - 按 `1` 切换到顶点选择
   - 选择顶部两个顶点
   - 按 `S` → `0.5`（形成尖端）
   - 按 `Tab` 返回对象模式
   - 按 `G` 移动到茎的顶部
   - 位置：X: -0.05, Y: 0, Z: 0.25
   - 按 `R` → `Z` → `45`（旋转）
   - 设置材质：绿色 (R:0.3, G:0.6, B:0.3)

4. **创建第二片叶子**
   - 选择第一片叶子
   - 按 `Shift + D` 复制
   - 按 `G` 移动到另一侧
   - 位置：X: 0.05, Y: 0, Z: 0.28
   - 按 `R` → `Z` → `-45`

5. **添加小根（可选）**
   - `Shift + A` → Mesh → Cylinder
   - 极细极短
   - 放在底部
   - 颜色：浅棕色

6. **保存**
   - File → Save As → `PeachTree_Stage2_Sprout.blend`

**检查清单**：
- ✓ 有细细的茎
- ✓ 有2片小叶子
- ✓ 整体非常小
- ✓ 颜色是浅绿色

---

## 🌰 第九步：创建阶段1 - 种子期（10分钟）

### 目标：椭圆形的种子

1. **创建新文件**
   - File → New → General
   - 删除默认立方体

2. **创建种子**
   - `Shift + A` → Mesh → UV Sphere
   - 按 `S` 缩放到很小：`0.1`
   - 按 `S` → `Z` → `0.7`（压扁成椭圆）
   - 按 `Tab` 进入编辑模式
   - 按 `A` 全选
   - 右键 → Shade Smooth（平滑着色）
   - 按 `Tab` 返回对象模式

3. **设置种子材质**
   - 切换到Shading工作区
   - Base Color: 深棕色
     - R: 0.35, G: 0.25, B: 0.15
   - Roughness: 0.6
   - 添加纹理（可选）：
     - `Shift + A` → Texture → Noise Texture
     - 连接到Bump节点
     - Strength: 0.2

4. **添加细节（可选）**
   - 在种子顶部添加小凹陷：
     - 按 `Tab` 进入编辑模式
     - 选择顶部的面
     - 按 `I` 内插
     - 按 `G` → `Z` 向下移动一点

5. **保存**
   - File → Save As → `PeachTree_Stage1_Seed.blend`

**检查清单**：
- ✓ 椭圆形
- ✓ 深棕色
- ✓ 非常小
- ✓ 表面有轻微纹理

---

## 📤 第十步：导出所有阶段到Unity（30分钟）

### 对每个阶段文件执行以下操作：

1. **打开文件**
   - File → Open → 选择阶段文件

2. **准备导出**
   - 按 `A` 全选所有对象
   - Object → Apply → All Transforms（应用所有变换）
   - 如果有修改器：Object → Convert To → Mesh

3. **导出FBX**
   - File → Export → FBX (.fbx)
   - 导出设置：
     ```
     Include:
     ✓ Selected Objects (如果只选中了需要的)
     ✓ Object Types: Mesh
     
     Transform:
     Scale: 1.00
     Forward: -Z Forward
     Up: Y Up
     ✓ Apply Scalings: FBX All
     ✓ Apply Unit
     
     Geometry:
     ✓ Apply Modifiers
     ✓ Smoothing: Face
     ✓ Tangent Space
     
     Armature: (不勾选，我们没有骨骼)
     Animation: (不勾选，我们没有动画)
     ```

4. **命名和保存**
   - 文件名：`PeachTree_Stage1.fbx` 到 `PeachTree_Stage8.fbx`
   - 保存位置：Unity项目的 `Assets/Models/PeachTrees/` 文件夹

5. **导出材质纹理（如果需要）**
   - 如果使用了图片纹理：
     - 复制纹理文件到Unity的 `Assets/Textures/PeachTrees/`
   - 如果使用程序化材质：
     - 需要在Unity中重新创建材质
     - 或者在Blender中烘焙纹理：
       - Shading工作区
       - 添加Image Texture节点
       - Render → Bake → Bake Type: Combined
       - 保存烘焙的图片

---

## ✅ 完成检查清单

导出完成后，你应该有：

- [ ] 8个.blend源文件（Stage1到Stage8）
- [ ] 8个.fbx模型文件
- [ ] 所有必要的纹理文件（如果有）
- [ ] 文件大小合理（每个FBX应该在几百KB到几MB）

---

## 🎯 在Unity中使用

1. 将所有FBX文件拖到Unity的 `Assets/Models/PeachTrees/`
2. 选择每个FBX，在Inspector中检查导入设置
3. 为每个阶段创建Prefab：
   - 拖FBX到场景
   - 调整材质
   - 拖回Project面板创建Prefab
4. 在PlantGrowthController中配置这些Prefab

---

## 💡 快速技巧

### 批量操作技巧：
- 选择多个对象：按住 `Shift` 点击
- 选择相似对象：选中一个，按 `Shift + G` → Type
- 隐藏对象：选中后按 `H`，显示：`Alt + H`
- 孤立显示：选中后按 `/`（小键盘）

### 视图操作：
- 旋转视图：鼠标中键拖动
- 平移视图：`Shift + 鼠标中键`
- 缩放视图：滚轮
- 聚焦选中对象：小键盘 `.`

### 常用快捷键：
- `G` - 移动（Grab）
- `R` - 旋转（Rotate）
- `S` - 缩放（Scale）
- `X` - 删除
- `Shift + D` - 复制
- `Tab` - 切换编辑/对象模式
- `Z` - 切换显示模式
- `Ctrl + S` - 保存

---

## 🐛 常见问题

### Q: 删除树枝后留下空洞？
**A**: 进入编辑模式，选择边缘顶点，按 `F` 填充面

### Q: 模型导出后太大/太小？
**A**: 在Blender中调整Scale，或在Unity导入设置中调整Scale Factor

### Q: 材质导出后丢失？
**A**: 参考《Blender材质导入Unity颜色丢失解决方案.md》

### Q: 叶子看起来是双面的？
**A**: 在Unity中的材质设置里启用 Double Sided

---

## ⏱️ 时间估算

| 阶段 | 时间 |
|------|------|
| 准备工作 | 5分钟 |
| 阶段8（已有） | 0分钟 |
| 阶段7 | 15分钟 |
| 阶段6 | 20分钟 |
| 阶段5 | 10分钟 |
| 阶段4 | 20分钟 |
| 阶段3 | 15分钟 |
| 阶段2 | 15分钟 |
| 阶段1 | 10分钟 |
| 导出 | 30分钟 |
| **总计** | **约2.5小时** |

---

## 🎉 完成！

现在你有了完整的8个生长阶段模型，可以在Unity中实现流畅的生长动画了！

下一步：参考《桃子树生长动画实现方案.md》中的Unity集成部分。

**祝你建模顺利！** 🍑🌳
