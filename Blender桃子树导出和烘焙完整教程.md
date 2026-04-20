# Blender桃子树导出和烘焙完整教程

## 🎯 目标

将Blender中拼好的桃子树（包含桃子）导出到Unity，并烘焙材质纹理，确保在Unity中显示正确。

---

## 📋 准备工作（5分钟）

### 检查清单

在开始之前，确保：

- [ ] 桃子树已经拼装完成
- [ ] 桃子已经放置到树上
- [ ] 所有材质已经设置好颜色
- [ ] 模型位置合理（原点在底部中心）
- [ ] 没有重叠或穿模问题

### 检查模型原点

1. 选择整个桃子树（按 `A` 全选）
2. 按 `Shift + S` → Cursor to World Origin（光标到世界原点）
3. 选择主树干对象
4. 右键 → Set Origin → Origin to 3D Cursor
5. 这样树的底部就在原点，导入Unity后位置正确

---

## 🎨 第一步：烘焙材质纹理（20-30分钟）

### 为什么要烘焙？

**烘焙的好处**：
- ✅ 将程序化材质转换为纹理图片
- ✅ Unity中材质显示更准确
- ✅ 性能更好
- ✅ 避免材质丢失问题

### 方法A：简单烘焙（推荐新手）

#### 1. 准备烘焙

**切换到Shading工作区**：
- 点击顶部的 "Shading" 标签

**选择要烘焙的对象**：
- 在Outliner中选择桃子树的所有对象
- 按住 `Shift` 点击选择多个对象
- 或按 `A` 全选

#### 2. 为每个对象创建UV映射

**检查是否有UV**：
1. 选择一个对象
2. 按 `Tab` 进入编辑模式
3. 按 `A` 全选所有面
4. 在顶部菜单：UV → Unwrap（或按 `U` → Unwrap）
5. 按 `Tab` 返回对象模式
6. 对每个对象重复此操作

**快速批量UV展开**：
```python
# 在Scripting工作区运行此脚本
import bpy

for obj in bpy.context.selected_objects:
    if obj.type == 'MESH':
        bpy.context.view_layer.objects.active = obj
        bpy.ops.object.mode_set(mode='EDIT')
        bpy.ops.mesh.select_all(action='SELECT')
        bpy.ops.uv.unwrap(method='ANGLE_BASED', margin=0.001)
        bpy.ops.object.mode_set(mode='OBJECT')
        print(f"已为 {obj.name} 创建UV")

print("完成！")
```

#### 3. 为每个材质添加Image Texture节点

**对每个材质执行以下操作**：

1. **选择对象**
   - 在Outliner中选择一个对象（比如树干）

2. **切换到Shading工作区**
   - 底部会显示节点编辑器

3. **添加Image Texture节点**
   - 按 `Shift + A` → Texture → Image Texture
   - 放在节点编辑器中（不要连接到任何地方）

4. **创建新图像**
   - 在Image Texture节点中点击 `+ New`
   - 设置：
     ```
     Name: PeachTree_Trunk_Baked
     Width: 2048
     Height: 2048
     Color: RGB (不要选Alpha)
     ```
   - 点击 OK

5. **选中这个Image Texture节点**
   - 点击节点使其高亮（白色边框）
   - ⚠️ 重要：必须选中这个节点，烘焙才会输出到这里

6. **对每个材质重复**
   - 树干材质 → PeachTree_Trunk_Baked
   - 树叶材质 → PeachTree_Leaves_Baked
   - 树枝材质 → PeachTree_Branches_Baked
   - 桃子材质 → Peach_Skin_Baked

#### 4. 烘焙设置

1. **切换到Render Properties**
   - 右侧属性面板
   - 点击相机图标（Render Properties）

2. **设置渲染引擎**
   - Render Engine: Cycles（必须用Cycles才能烘焙）

3. **切换到Render面板**
   - 在顶部菜单：Render → Render Settings

4. **找到Bake设置**
   - 向下滚动找到 "Bake" 部分
   - 如果没有，确保Render Engine是Cycles

5. **配置烘焙选项**
   ```
   Bake Type: Combined (烘焙所有效果)
   
   Influence:
   ✓ Direct (直接光照)
   ✓ Indirect (间接光照)
   ✓ Color (颜色)
   
   Selected to Active: ✗ (不勾选)
   
   Output:
   Margin: 16 px (边缘扩展)
   ✓ Clear Image (清除图像)
   ```

#### 5. 执行烘焙

**对每个对象单独烘焙**：

1. **选择树干对象**
   - 在Outliner中点击树干

2. **确保Image Texture节点被选中**
   - 在Shading工作区检查节点是否高亮

3. **点击Bake按钮**
   - 在Render Properties → Bake → 点击 "Bake"
   - 等待进度条完成（可能需要几秒到几分钟）

4. **保存烘焙的图像**
   - 烘焙完成后
   - 在Image Texture节点中
   - Image → Save As
   - 保存位置：创建文件夹 `Textures/PeachTree/`
   - 文件名：`PeachTree_Trunk_Baked.png`
   - 格式：PNG

5. **对其他对象重复**
   - 树叶 → 烘焙 → 保存为 `PeachTree_Leaves_Baked.png`
   - 树枝 → 烘焙 → 保存为 `PeachTree_Branches_Baked.png`
   - 桃子 → 烘焙 → 保存为 `Peach_Skin_Baked.png`

#### 6. 使用烘焙的纹理

**将烘焙的纹理连接到材质**：

1. **在Shading工作区**
   - 选择树干对象

2. **连接Image Texture到Base Color**
   - 将Image Texture节点的 "Color" 输出
   - 连接到Principled BSDF的 "Base Color" 输入
   - 拖动连线即可

3. **对所有材质重复**

4. **测试效果**
   - 按 `Z` → Material Preview
   - 检查颜色是否正确

---

### 方法B：快速烘焙（使用插件）

如果你觉得方法A太复杂，可以使用Blender的自动烘焙功能：

1. **选择所有对象**
   - 按 `A` 全选

2. **使用Simple Bake插件**
   - 如果没有，可以手动按方法A操作

---

## 📦 第二步：导出FBX（10分钟）

### 导出前准备

#### 1. 应用所有变换

1. **选择所有对象**
   - 按 `A` 全选

2. **应用变换**
   - Object → Apply → All Transforms
   - 或按 `Ctrl + A` → All Transforms

3. **应用修改器**
   - Object → Convert To → Mesh
   - 这会将所有修改器（如Subdivision Surface）应用到模型

#### 2. 检查命名

确保对象有清晰的名称：
```
PeachTree_Trunk
PeachTree_Branches
PeachTree_Leaves
Peach_001
Peach_002
...
```

#### 3. 清理场景

1. **删除不需要的对象**
   - 灯光（Unity会用自己的灯光）
   - 相机（Unity会用自己的相机）
   - 辅助对象

2. **只保留模型对象**

### 导出FBX设置

1. **File → Export → FBX (.fbx)**

2. **导出设置**（重要！）：

```
【Include 包含】
✓ Selected Objects (如果只选中了需要的)
或
✓ Visible Objects (导出所有可见对象)

Object Types:
✓ Mesh (网格)
✗ Camera (相机)
✗ Light (灯光)
✗ Empty (空对象)

【Transform 变换】
Scale: 1.00
✓ Apply Scalings: FBX All
Forward: -Z Forward
Up: Y Up
✓ Apply Unit
✓ Use Space Transform

【Geometry 几何】
✓ Apply Modifiers (应用修改器)
Smoothing: Face (面平滑)
✗ Loose Edges (不导出松散边)
✗ Tangent Space (不需要切线空间)

【Armature 骨骼】
✗ Add Leaf Bones (我们没有骨骼)

【Bake Animation 烘焙动画】
✗ (我们没有动画)
```

3. **文件命名和保存**
   - 文件名：`PeachTree_Stage8_Harvest.fbx`
   - 保存位置：Unity项目的 `Assets/Models/PeachTrees/` 文件夹

4. **点击 Export FBX**

---

## 🎮 第三步：导入Unity并设置材质（15分钟）

### 1. 复制文件到Unity

**复制纹理文件**：
```
从：Blender项目/Textures/PeachTree/
到：Unity项目/Assets/Textures/PeachTrees/

文件：
- PeachTree_Trunk_Baked.png
- PeachTree_Leaves_Baked.png
- PeachTree_Branches_Baked.png
- Peach_Skin_Baked.png
```

**FBX文件**：
```
从：Blender导出位置
到：Unity项目/Assets/Models/PeachTrees/

文件：
- PeachTree_Stage8_Harvest.fbx
```

### 2. Unity导入设置

1. **选择FBX文件**
   - 在Unity Project面板中点击FBX

2. **Inspector设置**：

```
【Model】
Scale Factor: 1
✓ Convert Units
✓ Bake Axis Conversion

Meshes:
✓ Read/Write Enabled (如果需要运行时修改)
✗ Generate Colliders (手动添加更好)
✓ Keep Quads (保持四边形)
Normals: Import
Tangents: Calculate Mikktspace

【Rig】
Animation Type: None (我们没有动画)

【Animation】
✗ Import Animation (没有动画)

【Materials】
Material Creation Mode: Standard (Legacy)
Location: Use External Materials (Legacy)
Naming: By Base Texture Name
Search: Recursive-Up
```

3. **点击 Apply**

### 3. 创建材质

Unity可能会自动创建材质，但通常需要手动调整：

#### 方法1：手动创建材质

1. **创建材质文件夹**
   - 在Project面板：右键 → Create → Folder
   - 命名：`Materials/PeachTrees`

2. **创建材质**
   - 右键 → Create → Material
   - 命名：`PeachTree_Trunk`

3. **设置材质**
   - 选择材质
   - Inspector中：
     ```
     Shader: Standard (或URP/Lit，取决于渲染管线)
     
     Albedo: 
     - 点击方框
     - 选择 PeachTree_Trunk_Baked
     
     Metallic: 0
     Smoothness: 0.2-0.4 (根据需要调整)
     ```

4. **创建其他材质**
   - PeachTree_Leaves
   - PeachTree_Branches
   - Peach_Skin

#### 方法2：使用自动生成的材质

1. **展开FBX文件**
   - 在Project面板点击FBX左侧的小箭头

2. **查看Materials子文件夹**
   - Unity可能已经创建了材质

3. **分配纹理**
   - 选择每个材质
   - 在Albedo中分配对应的烘焙纹理

### 4. 应用材质到模型

1. **拖FBX到场景**
   - 从Project拖到Hierarchy或Scene视图

2. **检查材质**
   - 选择模型
   - 在Inspector中查看Mesh Renderer
   - Materials列表应该显示所有材质

3. **手动分配材质（如果需要）**
   - 展开模型的子对象
   - 选择树干对象
   - Mesh Renderer → Materials → Element 0
   - 拖入 PeachTree_Trunk 材质

### 5. 调整和优化

**调整位置**：
- 确保树的底部在地面上
- Position Y = 0

**调整光照**：
- 如果颜色看起来太暗或太亮
- 调整材质的Smoothness和Metallic

**添加碰撞体**（可选）：
- 选择树干
- Add Component → Mesh Collider
- 或使用简单的Capsule Collider

---

## 🎨 第四步：处理特殊情况

### 如果材质在Unity中是粉红色

**原因**：Shader不兼容或纹理丢失

**解决方案**：
1. 检查渲染管线（Built-in/URP/HDRP）
2. 选择正确的Shader
3. 重新分配纹理

### 如果颜色不对

**解决方案**：
1. 在Blender中重新烘焙
2. 确保烘焙时Bake Type是 "Combined"
3. 检查Unity中纹理的导入设置：
   - sRGB (Color Texture): ✓ 勾选

### 如果模型太大或太小

**解决方案**：
1. 在Unity中调整Scale
2. 或在Blender中重新导出，调整Scale Factor

### 如果叶子是单面的

**解决方案**：
1. 选择叶子材质
2. 在Shader设置中：
   - Rendering Mode: Cutout 或 Fade
   - 或启用 Double Sided（URP/HDRP）

---

## 📋 完整检查清单

### Blender导出前
- [ ] 所有对象已命名
- [ ] UV已展开
- [ ] 材质已烘焙
- [ ] 烘焙纹理已保存
- [ ] 变换已应用
- [ ] 修改器已应用
- [ ] 原点位置正确
- [ ] 删除了不需要的对象

### Unity导入后
- [ ] FBX导入设置正确
- [ ] 纹理文件已复制
- [ ] 材质已创建
- [ ] 纹理已分配到材质
- [ ] 材质已应用到模型
- [ ] 颜色显示正确
- [ ] 位置和缩放正确
- [ ] 创建了Prefab

---

## 🚀 快速流程总结

### 最简化流程（如果不烘焙）

1. **Blender**：
   - 选择所有对象
   - Object → Apply → All Transforms
   - File → Export → FBX
   - 使用推荐设置导出

2. **Unity**：
   - 拖FBX到Assets/Models
   - 手动创建材质
   - 设置颜色
   - 拖到场景

### 完整流程（推荐）

1. **Blender烘焙**：
   - 为每个对象创建UV
   - 为每个材质添加Image Texture节点
   - 烘焙并保存纹理
   - 导出FBX

2. **Unity导入**：
   - 复制纹理文件
   - 导入FBX
   - 创建材质
   - 分配纹理
   - 测试效果

---

## 💡 专业技巧

### 技巧1：批量烘焙

如果有多个阶段的树要烘焙，创建一个Python脚本自动化：

```python
import bpy
import os

# 设置输出路径
output_path = "C:/YourPath/Textures/"

# 烘焙所有选中对象
for obj in bpy.context.selected_objects:
    if obj.type == 'MESH':
        # 设置为活动对象
        bpy.context.view_layer.objects.active = obj
        
        # 创建图像
        img_name = f"{obj.name}_Baked"
        img = bpy.data.images.new(img_name, 2048, 2048)
        
        # 为材质添加图像节点
        for mat_slot in obj.material_slots:
            if mat_slot.material:
                mat = mat_slot.material
                mat.use_nodes = True
                nodes = mat.node_tree.nodes
                
                # 添加图像纹理节点
                img_node = nodes.new('ShaderNodeTexImage')
                img_node.image = img
                img_node.select = True
                nodes.active = img_node
        
        # 烘焙
        bpy.ops.object.bake(type='COMBINED')
        
        # 保存图像
        img.filepath_raw = os.path.join(output_path, f"{img_name}.png")
        img.file_format = 'PNG'
        img.save()
        
        print(f"已烘焙: {obj.name}")

print("全部完成！")
```

### 技巧2：使用Prefab Variant

在Unity中为不同生长阶段创建Prefab Variant：
1. 创建基础Prefab
2. 右键 → Create → Prefab Variant
3. 修改特定阶段的属性

### 技巧3：LOD优化

为远距离显示创建低模版本：
1. 在Blender中创建简化版本
2. Unity中使用LOD Group组件
3. 设置不同距离的模型切换

---

## 🎉 完成！

现在你的桃子树应该已经成功导入Unity并显示正确了！

**下一步**：
1. 创建其他生长阶段（参考《Blender桃子树逆向建模详细步骤.md》）
2. 集成到PlantGrowthController
3. 测试生长动画

**需要帮助？**
- 材质问题：参考《Blender材质导入Unity颜色丢失解决方案.md》
- 导出问题：参考《Blender导出FBX详细设置.md》

祝你成功！🍑🌳
