# Blender顶点颜色导出到Unity完整教程

## 问题原因
Blender中使用顶点颜色（Vertex Color）或Color Attribute的模型，导出到Unity后会丢失颜色，显示为白色。

这是因为：
1. FBX格式对顶点颜色的支持有限
2. Unity需要特殊的Shader才能显示顶点颜色
3. 导出设置不正确

---

## 🎯 解决方案一：烘焙顶点颜色到纹理（推荐）

这是最可靠的方法，将顶点颜色转换为纹理贴图。

### 在Blender中操作：

#### 步骤1：准备UV展开
1. 选中模型，按 `Tab` 进入编辑模式
2. 按 `A` 全选所有顶点
3. 按 `U` → 选择 "Smart UV Project"
4. 保持默认设置，点击 OK

#### 步骤2：创建纹理图像
1. 切换到 **Shading** 工作区
2. 在 Shader Editor 中，按 `Shift + A`
3. 选择 **Texture → Image Texture**
4. 点击 "New" 创建新图像：
   - Name: `OrangeTree_Baked`
   - Width/Height: `2048` 或 `4096`（根据需要）
   - Color: Black（黑色）
   - Alpha: 不勾选
5. 点击 OK

#### 步骤3：设置烘焙
1. 选中新创建的 Image Texture 节点（重要！）
2. 切换到 **Rendering** 属性面板（相机图标）
3. 找到 **Bake** 部分
4. 设置：
   - Bake Type: **Diffuse**
   - 取消勾选 "Direct" 和 "Indirect"
   - 只勾选 "Color"
5. 点击 **Bake** 按钮

#### 步骤4：保存纹理
1. 烘焙完成后，在 Shader Editor 中选中 Image Texture 节点
2. 点击 **Image → Save As**
3. 保存为 `OrangeTree_Diffuse.png`
4. 保存到Unity项目的 `Assets/Textures/` 文件夹

#### 步骤5：应用纹理到材质
1. 删除或断开 Color Attribute 节点
2. 将 Image Texture 节点连接到 Principled BSDF 的 Base Color
3. 确认在3D视图中能看到正确的颜色

#### 步骤6：导出FBX
1. **File → Export → FBX (.fbx)**
2. 重要设置：
   - Path Mode: **Copy**
   - 勾选 **Embed Textures**（嵌入纹理）
   - Mesh → 勾选 **Apply Modifiers**
   - 取消勾选 **Add Leaf Bones**
3. 导出到Unity项目的 `Assets/Models/` 文件夹

### 在Unity中操作：

#### 步骤1：导入模型
1. Unity会自动导入FBX和纹理
2. 选中FBX文件，在Inspector中：
   - Materials → Location: **Use Embedded Materials**
   - 点击 "Extract Materials"
   - 选择保存位置（如 `Assets/Materials/`）

#### 步骤2：设置材质
1. 在Project窗口找到提取的材质
2. 选中材质，在Inspector中：
   - Shader: **Standard** 或 **Universal Render Pipeline/Lit**
   - Albedo: 拖入 `OrangeTree_Diffuse.png`
3. 调整其他参数：
   - Smoothness: 0.3-0.5
   - Metallic: 0

---

## 🎯 解决方案二：使用顶点颜色Shader（高级）

如果你想保留顶点颜色数据，需要自定义Shader。

### 在Blender中操作：

#### 导出设置
1. **File → Export → FBX (.fbx)**
2. 重要设置：
   - Mesh → 勾选 **Vertex Colors**
   - Mesh → 勾选 **Apply Modifiers**
3. 导出

### 在Unity中操作：

#### 步骤1：创建顶点颜色Shader

创建文件 `Assets/Shaders/VertexColor.shader`：

```shader
Shader "Custom/VertexColor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        fixed4 _Color;

        struct Input
        {
            float2 uv_MainTex;
            float4 color : COLOR; // 顶点颜色
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color * IN.color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
```

#### 步骤2：应用Shader
1. 选中模型的材质
2. Shader 选择 **Custom/VertexColor**
3. 如果有纹理，拖入 Main Tex

---

## 🎯 解决方案三：在Unity中重新上色（最简单）

如果Blender中的颜色不重要，直接在Unity中设置：

### 方法A：使用材质
1. 在Unity中选中模型
2. 在Inspector中找到Materials
3. 点击材质，设置Albedo颜色：
   - 橘子：RGB(255, 128, 0) 或 Hex #FF8000
   - 叶子：RGB(80, 150, 50) 或 Hex #509632
   - 树干：RGB(101, 67, 33) 或 Hex #654321

### 方法B：分离对象后分别上色
如果模型在Blender中已经分离了不同部分：
1. 在Unity Hierarchy中展开模型
2. 选中子对象（如 Oranges）
3. 创建新材质并应用

---

## 🔍 检查清单：为什么没有颜色？

### 在Blender中检查：
- [ ] 切换到 Material Preview 或 Rendered 模式能看到颜色
- [ ] Shader Editor 中节点正确连接
- [ ] 如果使用纹理，纹理文件已保存
- [ ] 导出FBX时勾选了正确的选项

### 在Unity中检查：
- [ ] 模型导入设置正确（Materials → Use Embedded Materials）
- [ ] 材质已提取（Extract Materials）
- [ ] Shader设置正确（Standard 或 URP/Lit）
- [ ] 如果使用纹理，纹理已正确导入
- [ ] 场景中有光源（Directional Light）

---

## 💡 推荐工作流程

### 最佳实践：
1. **在Blender中**：
   - 使用顶点颜色快速预览
   - 完成后烘焙到纹理
   - 导出时嵌入纹理

2. **在Unity中**：
   - 使用Standard Shader + 纹理贴图
   - 这样最稳定，兼容性最好

### 为什么推荐烘焙纹理？
- ✅ 兼容性好，所有平台都支持
- ✅ 性能更好（GPU优化）
- ✅ 可以后期编辑纹理
- ✅ 支持更多细节（法线、粗糙度等）
- ✅ 不需要自定义Shader

---

## 🚀 快速操作流程（推荐）

### 在Blender（5分钟）：
1. 选中模型 → `U` → Smart UV Project
2. Shading工作区 → 添加 Image Texture → New（2048x2048）
3. Rendering面板 → Bake Type: Diffuse → 只勾选Color → Bake
4. Image → Save As → 保存到Unity的Assets/Textures/
5. 将Image Texture连接到Base Color
6. File → Export → FBX → 勾选Embed Textures → 导出

### 在Unity（2分钟）：
1. 导入FBX和纹理
2. 选中FBX → Extract Materials
3. 选中材质 → 拖入纹理到Albedo
4. 完成！

---

## ⚠️ 常见错误

### 错误1：Unity中模型是粉红色
**原因**：材质丢失或Shader不兼容
**解决**：
- 检查Shader是否支持当前渲染管线（Built-in/URP/HDRP）
- 重新Extract Materials

### 错误2：纹理模糊或拉伸
**原因**：UV展开不正确
**解决**：
- 在Blender中重新UV展开
- 使用更高分辨率的纹理（4096x4096）

### 错误3：颜色太暗
**原因**：光照设置问题
**解决**：
- 在Unity场景中添加Directional Light
- 调整材质的Smoothness和Metallic
- 检查Blender导出时的光照烘焙设置

### 错误4：部分区域没有颜色
**原因**：UV重叠或烘焙不完整
**解决**：
- 在Blender中检查UV是否重叠（UV Editor）
- 增加烘焙的Margin（边距）设置
- 确保所有面都有UV坐标

---

## 📚 相关文档

- [Blender模型导入Unity完整指南.md](./Blender模型导入Unity完整指南.md)
- [Unity写实橘子树制作教程.md](./Unity写实橘子树制作教程.md)
- [Blender整体模型橘子换色教程.md](./Blender整体模型橘子换色教程.md)
