# Blender桃子树建模完整教程

## 前期准备

### 1. 软件要求
- Blender 3.0或更高版本
- 建议安装插件：Botaniq（可选，用于快速生成植物）

### 2. 参考资料准备
- 桃子的真实照片（不同角度）
- 桃树的参考图片（树干、树枝、叶子形态）
- 桃子的颜色参考（成熟度不同的颜色变化）

---

## 第一部分：桃子建模

### 步骤1：创建桃子基础形状
1. 打开Blender，删除默认立方体（选中后按 `X` → Delete）
2. 添加UV球体：`Shift + A` → Mesh → UV Sphere
3. 调整球体参数：
   - Segments: 32
   - Rings: 16

### 步骤2：塑造桃子形状
1. 进入编辑模式：按 `Tab`
2. 选择顶部顶点：
   - 按 `Alt + A` 取消全选
   - 按 `3` 切换到面选择模式
   - 选中顶部的面
3. 创建桃子凹陷：
   - 按 `I` 进行内插（Inset）
   - 按 `S` 缩放到合适大小
   - 按 `G` 然后 `Z` 向下移动，形成凹陷
4. 创建桃子沟槽：
   - 按 `Ctrl + R` 添加循环切割
   - 在中间添加一条边循环
   - 选中这条边循环，按 `S` → `Shift + Z`（限制在XY平面）稍微缩小
   - 使用雕刻模式进一步细化沟槽

### 步骤3：添加细节
1. 添加细分修改器：
   - 右侧修改器面板 → Add Modifier → Subdivision Surface
   - Levels Viewport: 2
   - Render: 3
2. 平滑着色：右键 → Shade Smooth

### 步骤4：桃子材质
1. 切换到Shading工作区
2. 创建材质节点：
   - 添加 Principled BSDF（默认已有）
   - Base Color: 设置为桃子的粉红色 `#FFB6A3`
   - Subsurface: 0.1（模拟半透明效果）
   - Subsurface Color: 浅黄色 `#FFE5B4`
   - Roughness: 0.3
3. 添加绒毛效果（可选）：
   - 在修改器中添加 Particle System
   - 类型选择 Hair
   - Number: 5000
   - Hair Length: 0.002

---

## 第二部分：桃树建模

### 步骤1：创建树干
1. 添加圆柱体：`Shift + A` → Mesh → Cylinder
2. 调整参数：
   - Vertices: 16
   - Depth: 3（树干高度）
3. 进入编辑模式塑形：
   - 选择顶部边循环，按 `S` 缩小（树干上细下粗）
   - 添加多个循环切割 `Ctrl + R`，创建 4-5 个分段
   - 随机移动顶点创建不规则感：
     - 启用比例编辑：按 `O`
     - 选择个别顶点，按 `G` 移动

### 步骤2：创建主要树枝
1. 选择树干顶部的面
2. 挤出树枝：
   - 按 `E` 挤出
   - 按 `S` 缩小
   - 按 `R` 旋转调整角度
   - 重复挤出 3-4 次，形成一根树枝
3. 创建多个树枝：
   - 在树干不同高度选择面
   - 重复挤出操作
   - 建议创建 4-6 根主要树枝

### 步骤3：添加细节树枝
1. 使用 Skin Modifier 方法（更快速）：
   - 添加单个顶点：`Shift + A` → Mesh → Single Vert
   - 进入编辑模式，挤出形成树枝骨架
   - 添加 Skin Modifier
   - 添加 Subdivision Surface Modifier
2. 或手动建模：
   - 从主树枝继续挤出小树枝
   - 逐渐减小直径

### 步骤4：树干材质
1. 切换到Shading工作区
2. 创建树皮材质：
   - Base Color: 深棕色 `#5C4033`
   - Roughness: 0.8
   - 添加 Noise Texture 节点连接到 Bump
   - Bump Strength: 0.3
3. 使用纹理（推荐）：
   - 下载树皮纹理图片
   - 添加 Image Texture 节点
   - 连接到 Base Color 和 Normal

---

## 第三部分：桃树叶子建模

### 步骤1：创建单片叶子
1. 添加平面：`Shift + A` → Mesh → Plane
2. 进入编辑模式：
   - 按 `S` → `Y` 拉长叶子形状（比例约 1:2.5）
   - 选择顶部边，按 `S` 缩小形成尖端
3. 添加细节：
   - `Ctrl + R` 添加 2-3 条纵向循环切割
   - 选择中间的边，按 `S` → `Shift + Z` 稍微扩大
   - 添加 1-2 条横向循环切割
4. 创建叶脉凹陷：
   - 选择中间纵向的边
   - 按 `G` → `Z` 稍微向下移动

### 步骤2：叶子材质
1. 创建双面材质：
   - Base Color: 绿色 `#6B8E23`
   - Subsurface: 0.2（叶子半透明）
   - Transmission: 0.1
2. 添加叶脉纹理：
   - 使用 Voronoi Texture 或下载叶子纹理
   - 连接到 Bump 和 Color Ramp

### 步骤3：分布叶子到树上
1. 使用粒子系统：
   - 选择树枝
   - 添加 Particle System
   - 类型：Hair
   - Render As: Object
   - Instance Object: 选择叶子对象
   - Number: 200-500（根据需要调整）
2. 调整分布：
   - 在 Vertex Groups 中创建权重绘制
   - 控制叶子在树枝末端更密集

---

## 第四部分：组装桃子到树上

### 步骤1：放置桃子
1. 复制桃子对象：`Shift + D`
2. 手动放置到树枝上：
   - 按 `G` 移动
   - 按 `R` 旋转
   - 按 `S` 缩放（创建大小变化）
3. 创建 10-20 个桃子实例

### 步骤2：使用粒子系统（可选）
1. 选择树枝
2. 添加新的 Particle System
3. 设置：
   - Instance Object: 桃子
   - Number: 15-30
   - 调整 Random Scale 创建大小变化

### 步骤3：添加桃子柄
1. 创建圆柱体：非常细小
2. 连接桃子和树枝
3. 材质设置为深棕色

---

## 第五部分：整体优化

### 步骤1：场景布置
1. 添加地面：`Shift + A` → Mesh → Plane，放大
2. 添加环境光照：
   - 切换到 Shading 工作区
   - World Properties → Surface
   - 添加 Environment Texture（天空HDR）

### 步骤2：渲染设置
1. 切换到 Cycles 渲染引擎
2. 设置采样：
   - Viewport: 128
   - Render: 256
3. 启用 Denoising

### 步骤3：摄像机设置
1. 选择摄像机：按 `0` 进入摄像机视图
2. 调整位置：
   - 按 `G` 移动
   - 按 `R` 旋转
3. 设置景深（可选）：
   - Camera Properties → Depth of Field
   - Focus Object: 选择树

---

## 第六部分：导出到Unity

### 步骤1：准备导出
1. 选择所有对象：`A`
2. 应用所有修改器：
   - Object → Apply → All Modifiers
3. 合并材质（可选）

### 步骤2：导出FBX
1. File → Export → FBX (.fbx)
2. 设置导出选项：
   - Scale: 1.00
   - Forward: -Z Forward
   - Up: Y Up
   - Apply Scalings: FBX All
   - 勾选：
     - ✓ Selected Objects（如果只导出选中的）
     - ✓ Mesh
     - ✓ Apply Modifiers
     - ✓ Bake Animation（如果有动画）
3. 点击 Export FBX

### 步骤3：导出材质纹理
1. 烘焙纹理（如果使用程序化材质）：
   - 切换到 Shading 工作区
   - 为每个材质添加 Image Texture 节点
   - Render → Bake
   - 保存烘焙的纹理
2. 导出纹理文件到Unity项目的 Assets 文件夹

---

## 常见问题解决

### 问题1：桃子表面不够光滑
- 增加 Subdivision Surface 级别
- 使用 Shade Smooth
- 检查法线方向

### 问题2：树枝看起来太规则
- 添加更多循环切割
- 使用比例编辑随机移动顶点
- 添加 Displace Modifier

### 问题3：叶子穿模
- 调整粒子系统的 Random Rotation
- 减少叶子数量
- 手动调整问题区域

### 问题4：导出到Unity后材质丢失
- 确保纹理文件在Unity项目中
- 重新分配材质
- 参考《Blender材质导入Unity颜色丢失解决方案.md》

---

## 进阶技巧

### 1. 使用几何节点（Blender 3.0+）
- 可以程序化生成树枝和叶子
- 更容易调整和修改

### 2. 添加风吹动画
- 使用 Cloth Simulation 或 Force Field
- 为叶子添加轻微摆动

### 3. 创建生长阶段
- 复制树模型
- 创建不同生长阶段：幼苗、小树、成树
- 调整树枝数量和桃子数量

### 4. LOD（细节层次）优化
- 创建高、中、低多边形版本
- 用于Unity中的性能优化

---

## 时间估算

- 桃子建模：30-45分钟
- 树干和树枝：1-2小时
- 叶子建模和分布：45分钟-1小时
- 材质和纹理：1-1.5小时
- 整体优化和导出：30分钟

**总计：约4-6小时**（取决于细节程度和经验）

---

## 相关教程参考

- 《Blender手工建模橘子树完整教程.md》
- 《Blender模型导入Unity完整指南.md》
- 《Blender材质导入Unity颜色丢失解决方案.md》
- 《Blender导出FBX详细设置.md》
