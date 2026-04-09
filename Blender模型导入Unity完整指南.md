# Blender模型导入Unity完整指南

> 从Blender导出到Unity HDRP的完整流程

## 目录
1. [Blender导出准备](#blender导出准备)
2. [导出FBX文件](#导出fbx文件)
3. [Unity导入设置](#unity导入设置)
4. [材质配置](#材质配置)
5. [常见问题解决](#常见问题解决)

---

## 第一部分：Blender导出准备

### 步骤1：检查模型

**1. 检查缩放**
```
选中所有对象
按 N 打开侧边栏
Transform → Scale 应该是 (1, 1, 1)

如果不是：
Ctrl + A → Scale（应用缩放）
```

**2. 检查原点位置**
```
原点应该在模型底部（树干底部）

如果不对：
Tab（编辑模式）
选中底部顶点
Shift + S → Cursor to Selected（光标到选中位置）
Tab（物体模式）
右键 → Set Origin → Origin to 3D Cursor
```

**3. 检查旋转**
```
Rotation 应该是 (0, 0, 0)

如果不是：
Ctrl + A → Rotation（应用旋转）
```

**4. 检查位置**
```
Location 可以是任意值
但建议在原点附近 (0, 0, 0)
```

### 步骤2：应用所有修改器

**重要！必须应用修改器，否则Unity看不到效果**

```
选中对象
右侧 → Modifier Properties（扳手图标）

对每个修改器：
点击下拉箭头 → Apply（应用）

或者一次性应用所有：
Object → Convert to → Mesh
```

**常见修改器：**
- Subdivision Surface（细分）
- Solidify（厚度）
- Mirror（镜像）
- Array（阵列）
- Simple Deform（变形）

### 步骤3：合并对象（可选但推荐）

**如果有多个部分（树干、树枝、叶子）：**

**方法A：全部合并（最简单）**
```
A（全选所有对象）
Ctrl + J（合并成一个对象）
命名：OrangeTree
```

**方法B：分组合并（更灵活）**
```
树干+树枝：选中 → Ctrl + J → 命名 Trunk
所有叶子：选中 → Ctrl + J → 命名 Leaves
所有橘子：选中 → Ctrl + J → 命名 Oranges
```

### 步骤4：检查材质

```
Material Properties
确保每个对象都有材质
材质名称清晰（如：LeafMaterial, BarkMaterial）
```

### 步骤5：最终检查

```
- [ ] 所有缩放已应用 (Scale = 1,1,1)
- [ ] 所有旋转已应用 (Rotation = 0,0,0)
- [ ] 所有修改器已应用
- [ ] 对象已合并（或分组合理）
- [ ] 原点位置正确（底部）
- [ ] 材质已设置
- [ ] 模型大小合适（树高2-3米）
```

### 步骤6：保存Blender文件

```
Ctrl + S
保存为：OrangeTree_Final.blend
```

---

## 第二部分：导出FBX文件

### 步骤1：选择要导出的对象

```
方法A：导出所有
A（全选）

方法B：只导出选中的
选中要导出的对象
```

### 步骤2：打开导出菜单

```
File → Export → FBX (.fbx)
```

### 步骤3：FBX导出设置（重要！）

**【Include（包含）】**
```
☑ Selected Objects（只导出选中的，推荐）
☐ Active Collection（不勾选）
☑ Object Types:
  ☑ Mesh（网格）
  ☐ Camera（不需要）
  ☐ Light（不需要）
  ☐ Armature（除非有骨骼动画）
  ☐ Empty（不需要）
```

**【Transform（变换）】**
```
Scale: 1.00（保持1.00）
☑ Apply Scalings: FBX All（重要！）
Forward: -Z Forward（Unity标准）
Up: Y Up（Unity标准）
☑ Apply Unit（应用单位）
☐ Use Space Transform（不勾选）
☑ Apply Transform（应用变换）
```

**【Geometry（几何）】**
```
☑ Apply Modifiers（应用修改器，重要！）
Smoothing: Face（面平滑）
☐ Export Subdivision Surface（不勾选，已应用）
☑ Triangulate Faces（三角化面，推荐）
```

**【Armature（骨架）】**
```
☐ Add Leaf Bones（不勾选）
```

**【Bake Animation（烘焙动画）】**
```
☐ 不勾选（除非有动画）
```

### 步骤4：选择保存位置

**重要：直接保存到Unity项目文件夹**

```
路径：你的Unity项目/Assets/Models/

例如：
C:/Users/你的用户名/Desktop/Program/TreePlanQAQ/TreePlanQAQ/Assets/Models/OrangeTree.fbx

文件名：OrangeTree.fbx
```

### 步骤5：点击导出

```
点击右上角 "Export FBX" 按钮
等待导出完成
```

---

## 第三部分：Unity导入设置

### 步骤1：Unity自动导入

```
Unity会自动检测到新文件
等待导入完成（看底部进度条）
```

### 步骤2：选中FBX文件

```
在Project窗口中：
Assets/Models/OrangeTree.fbx
点击选中
```

### 步骤3：Inspector导入设置

**【Model标签】**

```
Scene:
- Scale Factor: 1（保持1）
- ☐ Convert Units（不勾选）

Meshes:
- ☑ Read/Write Enabled（启用读写，重要！）
- ☑ Optimize Mesh（优化网格）
- ☑ Generate Colliders（如果需要碰撞）
- Normals: Import（导入法线）
- Blend Shape Normals: Import
- Tangents: Calculate Mikk T Space

Geometry:
- ☑ Keep Quads（保持四边形，可选）
- Weld Vertices: ☑
- Index Format: Auto
```

**【Rig标签】**

```
Animation Type: None（无动画）
```

**【Animation标签】**

```
☐ Import Animation（不勾选，除非有动画）
```

**【Materials标签】**

```
Location: Use External Materials (Legacy)
Naming: By Base Texture Name
Search: Recursive-Up

点击 "Extract Textures..." 按钮
选择保存位置：Assets/Materials/OrangeTree/Textures/

点击 "Extract Materials..." 按钮
选择保存位置：Assets/Materials/OrangeTree/
```

### 步骤4：应用设置

```
点击底部 "Apply" 按钮
等待Unity重新导入
```

---

## 第四部分：材质配置（HDRP）

### 步骤1：找到提取的材质

```
Project窗口：
Assets/Materials/OrangeTree/
会看到自动生成的材质文件
```

### 步骤2：配置树干材质

**选中 BarkMaterial（或类似名称）**

```
Inspector → Shader: HDRP/Lit

【Surface Options】
Surface Type: Opaque（不透明）
Rendering Pass: Default
Blend Mode: Alpha
☑ Double-Sided（双面，可选）

【Surface Inputs】
Base Map: 
  - 如果有纹理：拖入树皮纹理
  - 如果没有：点击颜色块
  - 设置颜色：棕色 RGB(0.4, 0.25, 0.15)

Normal Map: 
  - 如果有：拖入法线贴图
  - 设置 Normal Strength: 1.0

Mask Map:
  - Metallic: 0（不是金属）
  - Ambient Occlusion: 1.0
  - Detail Mask: 1.0
  - Smoothness: 0.2（粗糙）

【Emission】
☐ Emission（不勾选）

【Advanced Options】
☑ Enable GPU Instancing（重要！性能优化）
```

### 步骤3：配置叶子材质

**选中 LeafMaterial**

```
Shader: HDRP/Lit

【Surface Options】
Surface Type: Transparent（如果有透明边缘）
Rendering Pass: Transparent
Blend Mode: Alpha
☑ Double-Sided（双面，重要！）
☑ Back Then Front Rendering（背面优先）

【Surface Inputs】
Base Map: 
  - 颜色：深绿色 RGB(0.15, 0.50, 0.15)

Normal Map: 
  - 如果有叶脉法线贴图：拖入

Mask Map:
  - Metallic: 0
  - Smoothness: 0.4（有点光泽）

【Subsurface Scattering（次表面散射）】
☑ Enable（启用，重要！）
Subsurface Mask: 0.5
Thickness Map: 可选
Diffusion Profile: 选择 "Foliage"（叶子预设）
  如果没有：
  - 右键 → Create → Rendering → HDRP Diffusion Profile
  - 命名：FoliageDiffusion
  - 设置 Scattering Distance: RGB(0.5, 0.8, 0.5)

【Transparency】
Alpha Cutoff: 0.5（如果用Alpha Clip）

【Advanced Options】
☑ Enable GPU Instancing
☑ Double Sided GI（全局光照双面）
```

### 步骤4：配置橘子材质

**选中 OrangeMaterial**

```
Shader: HDRP/Lit

【Surface Options】
Surface Type: Opaque

【Surface Inputs】
Base Map: 
  - 颜色：橙色 RGB(1.0, 0.50, 0.10)

Mask Map:
  - Metallic: 0
  - Smoothness: 0.6（有光泽）

【Subsurface Scattering】
☑ Enable
Subsurface Mask: 0.3
Diffusion Profile: 创建 "Orange" 配置
  - Scattering Distance: RGB(1.0, 0.7, 0.3)

【Advanced Options】
☑ Enable GPU Instancing
```

---

## 第五部分：场景中使用

### 步骤1：拖入场景

```
从Project窗口拖动 OrangeTree.fbx 到 Hierarchy
或拖到 Scene 视图中
```

### 步骤2：调整位置

```
选中树对象
Inspector → Transform:
- Position: (0, 0, 0)
- Rotation: (0, 0, 0)
- Scale: (1, 1, 1)
```

### 步骤3：添加碰撞体（可选）

```
Add Component → Mesh Collider
☑ Convex（如果需要物理交互）
```

### 步骤4：创建预制体

```
从Hierarchy拖动树对象到Project窗口
保存位置：Assets/Prefabs/Plants/
命名：OrangeTree.prefab
```

---

## 第六部分：性能优化

### 1. 启用GPU Instancing

```
所有材质：
☑ Enable GPU Instancing
```

### 2. 创建LOD（细节层次）

```
选中树对象
Add Component → LOD Group

LOD 0（近景，0-15米）：
- 拖入完整模型

LOD 1（中景，15-30米）：
- 创建简化版模型（减少50%多边形）
- 拖入

LOD 2（远景，30米+）：
- 创建Billboard（2D贴图）
- 或极简模型
```

### 3. 合并网格（如果有多个子对象）

```
选中树对象
Add Component → Mesh Combiner（需要脚本）

或手动：
在Blender中已经合并（推荐）
```

### 4. 光照贴图设置

```
选中树对象
Inspector → Mesh Renderer:
☑ Contribute Global Illumination
Receive Global Illumination: Lightmaps
Scale In Lightmap: 1.0
```

---

## 常见问题解决

### 问题1：模型太大/太小

**原因：**Blender和Unity单位不匹配

**解决方法A：在Blender中**
```
选中模型
S → 0.1（缩小10倍）
Ctrl + A → Scale（应用缩放）
重新导出
```

**解决方法B：在Unity中**
```
选中FBX
Inspector → Model → Scale Factor: 0.1
Apply
```

### 问题2：材质是粉红色

**原因：**材质Shader不兼容HDRP

**解决：**
```
选中材质
Shader → HDRP/Lit
重新配置材质参数
```

### 问题3：叶子只显示一面

**原因：**没有启用双面渲染

**解决：**
```
叶子材质：
Surface Options → ☑ Double-Sided
```

### 问题4：叶子边缘有白边

**原因：**透明度设置不对

**解决：**
```
Surface Type: Transparent
Blend Mode: Alpha
或
Surface Type: Opaque
Blend Mode: Alpha Clip
Alpha Cutoff: 0.5
```

### 问题5：模型旋转了90度

**原因：**Blender和Unity坐标系不同

**解决：**
```
在Blender导出时：
Forward: -Z Forward
Up: Y Up

或在Unity中：
Rotation: (-90, 0, 0)
```

### 问题6：法线反了（黑色）

**原因：**面朝向错误

**解决A：在Blender中**
```
Tab（编辑模式）
A（全选）
Alt + N → Recalculate Outside（重新计算法线）
```

**解决B：在Unity中**
```
材质：
☑ Double-Sided（双面渲染）
```

### 问题7：纹理丢失

**原因：**纹理路径不对

**解决：**
```
1. 确保纹理在Unity项目中
2. 选中FBX → Materials → Extract Textures
3. 手动拖动纹理到材质槽
```

### 问题8：模型很卡

**原因：**多边形太多

**解决：**
```
1. 在Blender中减少Subdivision级别
2. 使用LOD系统
3. 启用GPU Instancing
4. 合并网格
```

---

## 快速检查清单

### Blender导出前：
- [ ] Scale已应用 (1,1,1)
- [ ] Rotation已应用 (0,0,0)
- [ ] 所有修改器已应用
- [ ] 对象已合并
- [ ] 原点在底部
- [ ] 材质已设置
- [ ] 文件已保存

### Unity导入后：
- [ ] FBX已导入
- [ ] Import Settings已配置
- [ ] 材质已提取
- [ ] Shader改为HDRP/Lit
- [ ] 叶子材质启用双面
- [ ] 次表面散射已配置
- [ ] GPU Instancing已启用
- [ ] 预制体已创建

---

## 完整工作流程总结

```
【Blender】
1. 完成建模
2. 应用所有修改器（Ctrl + A → All）
3. 合并对象（Ctrl + J）
4. 检查缩放/旋转/原点
5. 保存文件
6. File → Export → FBX
7. 设置导出参数
8. 保存到Unity项目/Assets/Models/

【Unity】
1. 等待自动导入
2. 选中FBX → Inspector
3. 配置Model/Rig/Materials设置
4. Extract Materials
5. 配置HDRP材质
6. 拖入场景测试
7. 创建预制体
8. 优化性能
```

---

## 推荐文件结构

```
Unity项目/
└── Assets/
    ├── Models/
    │   └── Plants/
    │       └── OrangeTree.fbx
    ├── Materials/
    │   └── OrangeTree/
    │       ├── BarkMaterial.mat
    │       ├── LeafMaterial.mat
    │       ├── OrangeMaterial.mat
    │       └── Textures/
    │           ├── Bark_Color.png
    │           ├── Bark_Normal.png
    │           ├── Leaf_Color.png
    │           └── Leaf_Normal.png
    └── Prefabs/
        └── Plants/
            └── OrangeTree.prefab
```

---

## 下一步

完成导入后，你可以：
1. 集成到生长系统
2. 添加交互功能
3. 创建不同生长阶段的变体
4. 添加动画（风吹效果）
5. 优化性能（LOD、合批）

祝你导入顺利！🍊
