# Unity 写实风格橘子树制作完整教程

## 目录
1. [方法一：使用Unity Tree Creator（推荐新手）](#方法一unity-tree-creator)
2. [方法二：使用ProBuilder手工建模](#方法二probuilder手工建模)
3. [方法三：使用Blender建模后导入](#方法三blender建模)
4. [材质和纹理优化](#材质优化)
5. [性能优化技巧](#性能优化)

---

## 方法一：Unity Tree Creator（推荐新手）

Unity内置的Tree Creator可以快速创建写实的树木。

### 步骤1：创建树木对象

1. 在Hierarchy中右键 → `3D Object` → `Tree`
2. Unity会自动创建一个树对象和对应的Tree资源

### 步骤2：打开Tree编辑器

1. 选中场景中的Tree对象
2. 在Inspector中点击 `Edit Tree` 按钮
3. Tree编辑器窗口会打开

### 步骤3：配置树干

在Tree编辑器中：

```
Tree Hierarchy（左侧面板）
├─ Tree Root
   └─ Tree Branch Group（树干）
```

**树干设置：**
- `Distribution` → `Group Seed`: 调整随机种子改变形状
- `Branch` → `Length`: 2-3（树干高度）
- `Branch` → `Radius`: 0.3-0.4（树干粗细）
- `Geometry` → `Cap Smoothing`: 0.5（顶部平滑度）
- `Material` → 选择棕色材质

**树皮纹理：**
- 在Project中创建材质：`Assets/Materials/OrangeBark.mat`
- 设置颜色：RGB(90, 65, 40)
- 添加Normal Map增加凹凸感

### 步骤4：添加主要分支

1. 选中 `Tree Branch Group`
2. 点击 `Add Branch Group` 按钮
3. 新的分支组会出现

**主分支设置：**
- `Distribution` → `Frequency`: 5-6（5-6个主分支）
- `Distribution` → `Distribution`: 0.6-0.9（分支在树干上半部分）
- `Branch` → `Length`: 1.5-2.0
- `Branch` → `Radius`: 0.15-0.2
- `Geometry` → `Growth Angle`: 30-50度（向上倾斜）

### 步骤5：添加次级分支

1. 选中主分支组
2. 再次点击 `Add Branch Group`
3. 创建更细的次级分支

**次级分支设置：**
- `Frequency`: 8-12
- `Length`: 0.5-1.0
- `Radius`: 0.05-0.1
- `Growth Angle`: 40-60度

### 步骤6：添加叶子

1. 选中次级分支组
2. 点击 `Add Leaf Group`

**叶子设置：**
- `Distribution` → `Frequency`: 20-30（每个分支的叶子数量）
- `Geometry` → `Size`: 0.3-0.5
- `Geometry` → `Rotation`: 随机旋转
- `Material` → 使用叶子纹理

**创建橘子树叶纹理：**

1. 在Photoshop/GIMP中创建512x512的图片
2. 画一个椭圆形的深绿色叶子
3. 添加叶脉纹理
4. 保存为PNG（带透明通道）
5. 导入Unity，设置为 `Sprite (2D and UI)`
6. 在Tree编辑器中选择这个纹理

### 步骤7：添加橘子（重点！）

Tree Creator本身不支持果实，我们需要手动添加：

**方法A：使用脚本自动添加橘子**

创建脚本 `Assets/Scripts/Plant/TreeFruitPlacer.cs`：

```csharp
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class TreeFruitPlacer : MonoBehaviour
{
    [Header("橘子设置")]
    public GameObject orangePrefab;
    public int fruitCount = 30;
    public float minHeight = 1.5f;
    public float maxHeight = 3.5f;
    public float radiusFromCenter = 1.2f;
    
    [ContextMenu("生成橘子")]
    public void PlaceOranges()
    {
        // 清除旧的橘子
        foreach (Transform child in transform)
        {
            if (child.name.StartsWith("Orange_"))
                DestroyImmediate(child.gameObject);
        }
        
        // 生成新橘子
        for (int i = 0; i < fruitCount; i++)
        {
            // 球面随机分布
            float theta = Random.Range(0f, Mathf.PI * 2f);
            float phi = Random.Range(0.3f, 1.2f);
            float r = Random.Range(radiusFromCenter * 0.7f, radiusFromCenter);
            
            Vector3 position = new Vector3(
                r * Mathf.Sin(phi) * Mathf.Cos(theta),
                Random.Range(minHeight, maxHeight),
                r * Mathf.Sin(phi) * Mathf.Sin(theta)
            );
            
            GameObject orange = Instantiate(orangePrefab, transform);
            orange.transform.localPosition = position;
            orange.name = $"Orange_{i}";
            orange.transform.localScale = Vector3.one * Random.Range(0.12f, 0.16f);
        }
    }
}
```

**创建橘子预制体：**

1. 创建一个Sphere：`GameObject` → `3D Object` → `Sphere`
2. 缩放为 (0.15, 0.14, 0.15)（稍微扁平）
3. 创建橘子材质：
   - Albedo: 橙色 RGB(255, 140, 0)
   - Smoothness: 0.6（光泽）
   - Metallic: 0
4. 添加小的绿色球体作为蒂（顶部）
5. 保存为预制体：`Assets/Prefabs/Plants/Orange.prefab`

### 步骤8：优化和调整

**Wind Zone（风效果）：**
1. 创建 `GameObject` → `Wind Zone`
2. 设置 `Mode`: Directional
3. `Main`: 0.3
4. `Turbulence`: 0.5
5. 树叶会随风摆动

**LOD（细节层次）：**
Tree Creator自动生成LOD，在Tree编辑器中：
- `LOD Group` → 调整不同距离的细节级别
- 远距离使用Billboard（2D贴图）

---

## 方法二：ProBuilder手工建模

ProBuilder是Unity的建模工具，可以直接在Unity中建模。

### 安装ProBuilder

1. 打开 `Window` → `Package Manager`
2. 搜索 `ProBuilder`
3. 点击 `Install`

### 创建树干

1. `Tools` → `ProBuilder` → `ProBuilder Window`
2. 点击 `New Shape` → `Cylinder`
3. 设置参数：
   - Height: 2.5
   - Radius: 0.2
   - Height Segments: 8（用于弯曲）
4. 进入 `Vertex Mode`（顶点模式）
5. 选择顶部顶点，向上拉伸
6. 选择中间顶点，轻微偏移（模拟不规则）

### 创建树枝

1. 选择树干顶部的面
2. 使用 `Extrude`（挤出）工具
3. 挤出5-6个方向的分支
4. 每个分支再次挤出创建次级分支
5. 使用 `Bevel`（倒角）工具平滑连接处

### 创建叶子

**方法A：使用Plane + 纹理**
1. 创建 `Plane`
2. 缩放为叶子形状
3. 应用叶子纹理（带Alpha通道）
4. 复制多个，手动放置在枝条末端

**方法B：使用ProBuilder建模**
1. 创建椭圆形的Plane
2. 使用 `Subdivide` 增加细节
3. 轻微弯曲（选择中间顶点向下移动）
4. 添加材质

### 添加细节

**树皮纹理：**
- 使用UV展开：`ProBuilder` → `UV Editor`
- 应用高质量树皮纹理
- 添加Normal Map增加凹凸感

**橘子：**
- 使用之前创建的橘子预制体
- 手动放置或使用脚本

---

## 方法三：Blender建模后导入

这是最专业的方法，可以创建最真实的效果。

### Blender建模步骤

1. **安装Blender**（免费）：https://www.blender.org/

2. **创建树干：**
   - 添加 `Cylinder`
   - 使用 `Subdivision Surface` 修改器增加平滑度
   - 使用 `Proportional Editing` 调整形状
   - 添加 `Skin Modifier` 创建有机形状

3. **创建树枝：**
   - 使用 `Curve`（曲线）工具
   - 绘制树枝路径
   - 设置 `Bevel Depth` 给曲线添加厚度
   - 转换为Mesh

4. **使用Sapling Tree Gen插件：**
   - Blender内置插件
   - `Edit` → `Preferences` → `Add-ons`
   - 搜索 `Sapling Tree Gen`，启用
   - `Add` → `Curve` → `Sapling Tree Gen`
   - 调整参数自动生成树木结构

5. **添加叶子：**
   - 使用 `Particle System`（粒子系统）
   - 创建单个叶子模型
   - 设置为粒子实例
   - 调整分布和密度

6. **导出到Unity：**
   - `File` → `Export` → `FBX`
   - 设置：
     - Scale: 1.0
     - Apply Scalings: FBX All
     - Forward: -Z Forward
     - Up: Y Up
   - 导出到Unity的 `Assets/Models/` 文件夹

### Unity中配置

1. 选中导入的FBX模型
2. Inspector中设置：
   - `Scale Factor`: 1
   - `Generate Colliders`: 根据需要
   - `Materials` → `Extract Materials`
3. 应用材质和纹理

---

## 材质优化

### HDRP材质设置（你的项目使用HDRP）

**树干材质：**
```
Shader: HDRP/Lit
Surface Type: Opaque
Base Map: 树皮纹理
Normal Map: 树皮法线贴图
Smoothness: 0.2-0.3
Metallic: 0
```

**叶子材质：**
```
Shader: HDRP/Lit
Surface Type: Transparent (如果需要透明边缘)
Base Map: 叶子纹理（带Alpha）
Normal Map: 叶子法线
Smoothness: 0.4
Subsurface Scattering: 启用（模拟光线穿透）
  - Thickness: 0.5
```

**橘子材质：**
```
Shader: HDRP/Lit
Base Color: 橙色
Normal Map: 橘皮纹理（小凹点）
Smoothness: 0.6-0.7（光泽）
Subsurface Scattering: 启用
  - Thickness: 0.3
```

### 创建真实的橘皮纹理

使用Substance Designer或在线工具：
1. 访问：https://www.textures.com/
2. 搜索 "orange peel texture"
3. 下载免费纹理
4. 或使用Photoshop：
   - 创建橙色底色
   - 添加 `Filter` → `Noise` → `Add Noise`
   - 使用 `Filter` → `Blur` → `Gaussian Blur` 轻微模糊
   - 添加小的黑点（毛孔）

---

## 性能优化

### LOD（Level of Detail）设置

1. 选中树模型
2. 添加 `LOD Group` 组件
3. 创建3个LOD级别：

**LOD 0（近距离）：**
- 完整模型
- 所有叶子和细节
- 距离：0-15米

**LOD 1（中距离）：**
- 减少叶子数量（50%）
- 简化树枝
- 距离：15-30米

**LOD 2（远距离）：**
- 使用Billboard（2D贴图）
- 只显示轮廓
- 距离：30米以上

### 批处理优化

```csharp
// 合并相同材质的网格
using UnityEngine;

public class TreeBatcher : MonoBehaviour
{
    [ContextMenu("合并网格")]
    void CombineMeshes()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        
        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }
        
        MeshFilter mf = gameObject.AddComponent<MeshFilter>();
        mf.mesh = new Mesh();
        mf.mesh.CombineMeshes(combine);
        gameObject.AddComponent<MeshRenderer>();
    }
}
```

### GPU Instancing

在材质中启用：
- 勾选 `Enable GPU Instancing`
- 可以高效渲染大量相同的树

---

## 完整工作流程总结

### 推荐流程（新手）：

1. 使用 **Unity Tree Creator** 创建基础树木结构
2. 使用脚本自动添加橘子
3. 调整材质和光照
4. 设置LOD优化性能

### 推荐流程（进阶）：

1. 在 **Blender** 中使用Sapling插件生成树木
2. 手工调整细节
3. 导出FBX到Unity
4. 配置HDRP材质
5. 添加橘子和优化

### 时间估算：

- Tree Creator方法：30分钟-1小时
- ProBuilder方法：2-3小时
- Blender方法：3-5小时（含学习时间）

---

## 额外资源

### 免费纹理网站：
- https://www.textures.com/ （每天15张免费）
- https://polyhaven.com/ （完全免费，高质量）
- https://www.cgbookcase.com/ （PBR纹理）

### 学习资源：
- Unity Tree Creator官方文档
- Blender Sapling教程：YouTube搜索 "Blender tree tutorial"
- HDRP材质教程：Unity Learn

---

## 常见问题

**Q: 叶子看起来很假？**
A: 启用Subsurface Scattering（次表面散射），模拟光线穿透叶子

**Q: 性能太差？**
A: 使用LOD，减少叶子数量，启用GPU Instancing

**Q: 橘子位置不对？**
A: 调整TreeFruitPlacer脚本中的minHeight、maxHeight和radiusFromCenter参数

**Q: 树木太规则？**
A: 在Tree Creator中调整Group Seed，或手动调整顶点位置

---

## 下一步

现在你可以：
1. 创建不同生长阶段的树（调整分支数量和叶子密度）
2. 添加开花效果（白色小花代替部分叶子）
3. 创建枯萎效果（改变材质颜色，减少叶子）
4. 集成到你现有的生长系统中

祝你创建出漂亮的橘子树！🍊
