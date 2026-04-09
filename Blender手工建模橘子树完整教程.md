# Blender 手工建模写实橘子树完整教程

> 从零开始，手把手教你在Blender中创建专业级别的橘子树模型

## 目录
1. [准备工作](#准备工作)
2. [第一部分：树干建模](#第一部分树干建模)
3. [第二部分：树枝系统](#第二部分树枝系统)
4. [第三部分：叶子建模](#第三部分叶子建模)
5. [第四部分：橘子建模](#第四部分橘子建模)
6. [第五部分：材质和纹理](#第五部分材质和纹理)
7. [第六部分：导出到Unity](#第六部分导出到unity)

---

## 准备工作

### 安装Blender

1. 访问 https://www.blender.org/download/
2. 下载最新版本（推荐4.0+）
3. 安装并启动

### Blender基础操作（必须掌握）

**鼠标操作：**
- 左键：选择
- 中键拖动：旋转视图
- 中键滚轮：缩放
- Shift + 中键：平移视图

**快捷键（重要！）：**
- `G`：移动（Grab）
- `R`：旋转（Rotate）
- `S`：缩放（Scale）
- `Tab`：切换编辑模式/物体模式
- `Z`：切换着色模式
- `X`：删除
- `Shift + A`：添加物体
- `Ctrl + Z`：撤销
- `1/2/3`：切换顶点/边/面选择模式

### 设置工作环境

1. 启动Blender，删除默认的立方体（选中后按`X` → Delete）
2. 设置单位：
   - 右侧属性面板 → Scene Properties（场景图标）
   - Units → Unit System: Metric
   - Length: Meters
3. 保存文件：`File` → `Save As` → `OrangeTree.blend`

---

## 第一部分：树干建模

### 步骤1：创建基础圆柱体

1. `Shift + A` → Mesh → Cylinder
2. 在左下角的操作面板中设置：
   - Vertices: 16（16个顶点，足够平滑）
   - Radius: 0.2m
   - Depth: 2.5m
3. 按`Tab`进入编辑模式

### 步骤2：添加细分

1. 确保在编辑模式
2. 全选（`A`键）
3. 右键 → Subdivide（细分）
4. 在左下角设置 Number of Cuts: 8
5. 现在树干有9层顶点环

### 步骤3：塑造树干形状

**底部加粗：**
1. 按`3`切换到面选择模式
2. 选中底部的面
3. 按`S`（缩放），然后输入`1.3`，回车
4. 按`G`然后`Z`，向下移动一点

**顶部收窄：**
1. 选中顶部的面
2. 按`S`缩放到`0.7`

**添加不规则性：**
1. 按`1`切换到顶点选择模式
2. 启用比例编辑：按`O`键（顶部会显示图标）
3. 随机选择一些顶点
4. 按`G`移动，会影响周围的顶点
5. 轻微调整，让树干看起来不那么规则

### 步骤4：添加树皮细节

**方法A：使用修改器（简单）**
1. 退出编辑模式（`Tab`）
2. 右侧属性面板 → Modifier Properties（扳手图标）
3. Add Modifier → Subdivision Surface
   - Levels Viewport: 2
   - Render: 3
4. Add Modifier → Displace（置换）
   - 点击 New 创建纹理
   - Strength: 0.05
   - 在纹理设置中选择 Clouds 类型

**方法B：手工雕刻（高级）**
1. 切换到 Sculpting 工作区（顶部标签）
2. 使用 Draw 笔刷添加凸起
3. 使用 Grab 笔刷调整形状
4. 使用 Crease 笔刷添加裂纹

### 步骤5：添加树根

1. 回到编辑模式
2. 选中底部的面
3. 按`E`（挤出），然后`S`（缩放）到`1.2`
4. 再次`E`挤出，`S`缩放到`0.8`
5. 按`G`然后`Z`向下移动，埋入地面

---

## 第二部分：树枝系统

### 方法A：使用Skin Modifier（推荐）

这是最快速且效果好的方法。

**步骤1：创建主分支骨架**

1. `Shift + A` → Curve → Bezier
2. 进入编辑模式（`Tab`）
3. 选中曲线的控制点
4. 按`G`移动，塑造分支形状
5. 按`E`挤出新的控制点，创建分支路径
6. 调整手柄（按`V`切换手柄类型）使曲线平滑

**步骤2：转换为网格并添加厚度**

1. 退出编辑模式
2. 右键点击曲线 → Convert to → Mesh
3. 进入编辑模式
4. 全选（`A`）
5. 添加 Skin Modifier：
   - Modifier Properties → Add Modifier → Skin
6. 调整粗细：
   - 选择顶点
   - `Ctrl + A`调整半径
   - 根部粗，末端细

**步骤3：创建5-6个主分支**

1. 重复上述步骤创建多个分支
2. 分支起点在树干顶部附近
3. 向外向上倾斜30-50度
4. 长度0.8-1.5米

**步骤4：添加次级分支**

1. 在主分支上再次使用曲线
2. 创建更细更短的分支
3. 每个主分支上3-5个次级分支

### 方法B：手工建模（精确控制）

**步骤1：从树干挤出分支**

1. 选中树干
2. 进入编辑模式
3. 按`3`切换到面选择模式
4. 在树干上半部分选择一个面
5. 按`I`（Inset）内插，缩小面
6. 按`E`挤出，创建分支
7. 按`S`缩放调整粗细
8. 重复`E`和`S`创建分支段

**步骤2：添加细分和平滑**

1. 选中分支
2. 右键 → Subdivide
3. 添加 Subdivision Surface 修改器
4. 使用比例编辑（`O`）调整形状

**步骤3：创建分叉**

1. 选中分支末端的面
2. 按`E`挤出
3. 按`S`缩放
4. 再次选中这个面
5. 使用 Inset（`I`）创建两个小面
6. 分别挤出，形成分叉

---

## 第三部分：叶子建模

### 步骤1：创建单片叶子

**基础形状：**
1. `Shift + A` → Mesh → Plane
2. 按`S`缩放到`0.15`
3. 进入编辑模式
4. 按`S`然后`Y`，缩放Y轴到`1.5`（椭圆形）

**添加细节：**
1. 全选（`A`）
2. 右键 → Subdivide，设置 Cuts: 3
3. 按`1`切换到顶点模式
4. 选择边缘的顶点
5. 按`G`然后`Z`，轻微向下移动（叶子边缘下垂）

**添加叶脉：**
1. 选择中间的边
2. 按`Ctrl + B`（倒角）
3. 滚动鼠标滚轮增加段数
4. 轻微向上移动（`G`然后`Z`）

**使叶子弯曲：**
1. 添加 Simple Deform 修改器
2. Mode: Bend
3. Angle: 15度
4. Axis: Y

### 步骤2：创建叶子变体

1. 复制叶子（`Shift + D`）
2. 按`R`旋转不同角度
3. 按`S`缩放不同大小（0.8-1.2倍）
4. 创建5-6个变体

### 步骤3：使用粒子系统分布叶子

**创建叶子集合：**
1. 选中所有叶子变体
2. `M`键 → New Collection → "Leaves"

**在分支上添加粒子：**
1. 选中一个分支
2. Properties → Particle Properties
3. 点击`+`添加粒子系统
4. 设置：
   - Number: 20-30（每个分支的叶子数）
   - Hair（毛发类型）
   - Advanced → Rotation → Randomize: 1.0
5. Render → Render As: Collection
6. Instance Collection: Leaves
7. Scale: 1.0, Scale Randomness: 0.3

**调整分布：**
- Emission → Source: Faces
- Velocity → Normal: 0.1（向外生长）
- Rotation → Phase: 随机旋转

---

## 第四部分：橘子建模

### 步骤1：创建橘子基础形状

1. `Shift + A` → Mesh → UV Sphere
2. 设置：
   - Segments: 32
   - Rings: 16
3. 按`S`缩放到`0.12`
4. 按`S`然后`Z`然后`0.9`（稍微压扁）

### 步骤2：添加橘皮纹理

**方法A：使用修改器**
1. Add Modifier → Subdivision Surface（Level 2）
2. Add Modifier → Displace
   - New Texture → Type: Voronoi
   - Strength: 0.01（轻微凹凸）

**方法B：雕刻细节**
1. 切换到 Sculpting 工作区
2. 使用 Draw 笔刷
3. Strength: 0.1
4. 在表面添加小凹点

### 步骤3：添加橘子蒂（顶部）

1. 在橘子顶部添加小球体
2. 缩放到很小（`S` → `0.02`）
3. 改变颜色为深绿色

### 步骤4：添加底部花萼痕迹

1. 选中橘子
2. 进入编辑模式
3. 选择底部的面
4. 按`I`（Inset）内插
5. 按`E`然后`S`，向内挤出并缩小
6. 改变颜色为深橙色

### 步骤5：批量放置橘子

**手动放置：**
1. 复制橘子（`Shift + D`）
2. 放置在分支末端附近
3. 按`Ctrl + P` → Object（Keep Transform）父子关联到分支
4. 创建15-25个橘子

**使用粒子系统（快速）：**
1. 创建橘子集合
2. 在树枝上添加粒子系统
3. 设置较少的数量（15-20）
4. 调整位置使其看起来挂在枝条上

---

## 第五部分：材质和纹理

### 准备工作

1. 切换到 Shading 工作区（顶部标签）
2. 将视图切换到 Material Preview 或 Rendered 模式（`Z`键）

### 树干材质

**步骤1：创建基础材质**
1. 选中树干
2. Material Properties → New
3. 命名为 "Bark"

**步骤2：设置节点**


在Shading工作区的节点编辑器中：

**节点连接：**
```
Image Texture → Principled BSDF → Material Output
```

**详细步骤：**

1. **添加 Image Texture 节点**
   - `Shift + A` → Texture → Image Texture
   - 点击 "Open" 加载树皮纹理图片

2. **连接节点**
   - Image Texture 的 Color → Principled BSDF 的 Base Color
   - Image Texture 的 Alpha → Principled BSDF 的 Alpha（如果有透明）

3. **设置 Principled BSDF**
   ```
   Base Color: 连接纹理
   Roughness: 0.8（粗糙的树皮）
   Specular: 0.2（低反光）
   ```

### 叶子材质（重要！）

**基础设置：**

1. **选中叶子对象**
2. **Material Properties → New**
3. **命名为 "LeafMaterial"**

**Principled BSDF 设置：**
```
Base Color: RGB(0.15, 0.50, 0.15) - 深绿色
Roughness: 0.6
Specular: 0.3
Sheen: 0.2（叶子的柔和光泽）
```

**高级：添加次表面散射（SSS）**

这会让叶子看起来半透明，光线可以穿透：

```
Subsurface: 0.3
Subsurface Radius: RGB(0.5, 0.8, 0.5)
Subsurface Color: 浅绿色 RGB(0.6, 0.9, 0.6)
```

**添加叶子纹理（可选）：**

1. **下载叶子纹理**
   - 访问 https://polyhaven.com/
   - 搜索 "leaf texture"
   - 下载免费的PBR纹理

2. **在节点编辑器中添加**
   ```
   Shift + A → Texture → Image Texture
   加载叶子纹理
   连接到 Base Color
   ```

3. **添加法线贴图（叶脉细节）**
   ```
   Shift + A → Texture → Image Texture（加载Normal Map）
   Shift + A → Vector → Normal Map
   
   连接：
   Image Texture → Normal Map → Principled BSDF (Normal)
   ```

### 橘子材质

**基础橙色材质：**

```
Base Color: RGB(1.0, 0.50, 0.10) - 橙色
Roughness: 0.4（有点光泽）
Specular: 0.5
Subsurface: 0.5（半透明效果）
Subsurface Color: RGB(1.0, 0.7, 0.3)
```

**添加橘皮纹理：**

1. **使用 Noise Texture 创建橘皮效果**
   ```
   Shift + A → Texture → Noise Texture
   Scale: 50（很细的纹理）
   Detail: 5
   Roughness: 0.7
   ```

2. **连接到 Bump 节点**
   ```
   Noise Texture → ColorRamp → Bump → Normal
   ```

3. **调整 Bump 强度**
   ```
   Strength: 0.1（轻微凹凸）
   ```

---

## 第六部分：导出到Unity

### 步骤1：准备导出

**1. 应用所有修改器：**

选中所有对象，然后：
```
Object → Convert to → Mesh
```

或者逐个应用修改器：
```
选中对象 → Modifier Properties
点击每个修改器旁边的下拉箭头 → Apply
```

**2. 合并相似对象（可选）：**

如果有多个树枝：
```
选中所有树枝
Ctrl + J（合并）
```

**3. 检查比例：**
```
选中树
按 N 打开侧边栏
Transform → Scale 应该是 (1, 1, 1)
如果不是，按 Ctrl + A → Scale（应用缩放）
```

### 步骤2：导出FBX

**1. 选中要导出的对象**
   - 树干
   - 树枝
   - 叶子（如果用粒子系统，需要先转换）

**2. 转换粒子系统为实际网格：**
```
选中有粒子的对象
Modifier Properties → Particle System
点击下拉箭头 → Convert（转换）
```

**3. 导出FBX：**
```
File → Export → FBX (.fbx)
```

**4. FBX导出设置（重要！）：**

```
【Include（包含）】
☑ Selected Objects（只导出选中的）
☑ Mesh（网格）
☑ Armature（如果有骨骼）

【Transform（变换）】
Scale: 1.00
☑ Apply Scalings: FBX All
Forward: -Z Forward
Up: Y Up

【Geometry（几何）】
☑ Apply Modifiers（应用修改器）
Smoothing: Face（面平滑）

【Armature（骨架）】
☐ Add Leaf Bones（不勾选）
```

**5. 选择保存位置：**
```
保存到：Unity项目/Assets/Models/OrangeTree.fbx
```

### 步骤3：在Unity中配置

**1. 导入设置：**

选中FBX文件，在Inspector中：

```
【Model】
Scale Factor: 1
☑ Read/Write Enabled
☑ Generate Colliders（如果需要碰撞）

【Rig】
Animation Type: None

【Materials】
Location: Use External Materials (Legacy)
点击 "Extract Materials"
选择保存位置：Assets/Materials/OrangeTree/
```

**2. 创建材质：**

Unity会自动创建材质，但需要手动设置：

**叶子材质（HDRP）：**
```
Shader: HDRP/Lit
Surface Type: Opaque
Base Map: 叶子纹理（如果有）
Base Color: 深绿色
Normal Map: 叶子法线贴图
Smoothness: 0.4
☑ Subsurface Scattering
  Subsurface Mask: 0.5
```

**树干材质：**
```
Shader: HDRP/Lit
Base Map: 树皮纹理
Normal Map: 树皮法线贴图
Smoothness: 0.2
```

**橘子材质：**
```
Shader: HDRP/Lit
Base Color: 橙色
Smoothness: 0.6
☑ Subsurface Scattering
  Subsurface Mask: 0.3
```

### 步骤4：优化性能

**1. 创建LOD（细节层次）：**

```
选中树模型
添加 LOD Group 组件
创建3个LOD级别：
- LOD 0: 完整模型（0-15米）
- LOD 1: 简化50%（15-30米）
- LOD 2: Billboard（30米+）
```

**2. 合并网格：**

```csharp
// 在Unity中使用脚本合并
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    [ContextMenu("Combine Meshes")]
    void CombineMeshes()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
}
```

**3. 启用GPU Instancing：**

在所有材质中：
```
☑ Enable GPU Instancing
```

---

## 完整工作流程总结

### Blender部分：

1. ✅ 创建树干（圆柱体 + 细分 + 修改器）
2. ✅ 创建树枝（曲线 + Skin Modifier）
3. ✅ 创建叶子（Plane + 椭圆形 + 细节）
4. ✅ 使用粒子系统分布叶子
5. ✅ 创建橘子（球体 + 细节）
6. ✅ 添加材质和纹理
7. ✅ 应用修改器
8. ✅ 导出FBX

### Unity部分：

1. ✅ 导入FBX
2. ✅ 提取材质
3. ✅ 配置HDRP材质
4. ✅ 添加LOD
5. ✅ 优化性能
6. ✅ 创建预制体

---

## 常见问题解答

**Q: 导出后叶子消失了？**
A: 粒子系统需要先转换为网格（Modifier → Convert）

**Q: Unity中模型太大/太小？**
A: 在Blender中应用缩放（Ctrl + A → Scale），或在Unity导入设置中调整Scale Factor

**Q: 材质是粉红色？**
A: 需要手动设置HDRP材质，Unity不会自动转换

**Q: 叶子没有透明边缘？**
A: 需要使用带Alpha通道的叶子纹理，并设置Surface Type为Transparent

**Q: 性能太差？**
A: 使用LOD，合并网格，启用GPU Instancing

---

## 推荐资源

### 免费纹理网站：
- **Polyhaven.com** - 完全免费的PBR纹理
- **Textures.com** - 每天15张免费
- **AmbientCG.com** - 免费CC0纹理

### 学习资源：
- **Blender Guru** - YouTube频道，树木建模教程
- **CG Cookie** - Blender专业教程
- **Unity Learn** - HDRP材质教程

### Blender插件推荐：
- **Sapling Tree Gen** - 自动生成树木（内置）
- **Botaniq** - 专业植物库（付费）
- **The Grove** - 程序化树木生成（付费）

---

## 最终检查清单

导出前检查：
- [ ] 所有修改器已应用
- [ ] 粒子系统已转换为网格
- [ ] 缩放已应用（Scale = 1,1,1）
- [ ] 材质已正确设置
- [ ] 对象命名清晰
- [ ] 原点位置正确（树干底部）

Unity导入后检查：
- [ ] 模型大小正确
- [ ] 材质已提取
- [ ] HDRP材质已配置
- [ ] 法线贴图已应用
- [ ] LOD已设置
- [ ] 碰撞体已添加（如需要）

---

## 下一步建议

完成基础橘子树后，你可以：

1. **创建不同生长阶段**
   - 调整树枝数量
   - 改变叶子密度
   - 添加/移除橘子

2. **添加动画**
   - 风吹效果（使用Cloth模拟）
   - 生长动画（使用Shape Keys）

3. **创建变体**
   - 不同大小的树
   - 不同形状的树冠
   - 不同颜色的橘子

4. **集成到游戏系统**
   - 连接到你的生长系统
   - 添加交互功能
   - 实现采摘系统

---

恭喜！你现在掌握了从零开始在Blender中手工建模橘子树，并导入Unity的完整流程！🎉🍊

如果遇到任何问题，随时问我！
