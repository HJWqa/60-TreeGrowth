# Blender 写实叶子制作教程

> 用纹理贴图实现照片级真实感，性能友好

## 核心理念

写实叶子 = **简单几何体 + 高质量纹理**

- 几何体：只需要1个Plane（2个三角形）
- 纹理：真实照片或PBR材质
- 关键：透明度、法线贴图、次表面散射

---

## 方法1：使用免费PBR纹理（推荐）

### 步骤1：下载叶子纹理

**推荐网站（完全免费）：**

1. **Polyhaven.com**
   - 访问：https://polyhaven.com/textures
   - 搜索："leaf" 或 "foliage"
   - 下载格式：选择 **2K** 或 **4K**
   - 需要的贴图：
     - ✅ **Color/Diffuse**（颜色）
     - ✅ **Normal**（法线，凹凸细节）
     - ✅ **Roughness**（粗糙度）
     - ⭕ **Displacement**（置换，可选）

2. **AmbientCG.com**
   - 搜索："Leaf"
   - 下载 PNG 格式
   - 完全免费，CC0协议

3. **Textures.com**
   - 每天15张免费下载
   - 搜索："Orange Tree Leaf" 或 "Citrus Leaf"

### 步骤2：创建叶子模型

1. **Shift + A** → Mesh → Plane
2. **S** 缩放到 **0.15**
3. **S** → **Y** → **1.5**（椭圆形，橘子叶特征）
4. **保持简单**：不要细分！

### 步骤3：UV展开（重要！）

1. **Tab** 进入编辑模式
2. **A** 全选
3. **U** 键 → **Unwrap**
4. 完成！（Plane自动展开正确）

### 步骤4：设置材质节点

切换到 **Shading** 工作区（顶部标签）

#### 基础节点连接：

```
【节点布局】

Image Texture (Color)  ──→ Principled BSDF (Base Color)
Image Texture (Alpha)  ──→ Principled BSDF (Alpha)
Image Texture (Normal) ──→ Normal Map ──→ Principled BSDF (Normal)
Image Texture (Rough)  ──→ Principled BSDF (Roughness)
                            ↓
                      Material Output
```

#### 详细步骤：

**1. 添加颜色贴图**
```
Shift + A → Texture → Image Texture
点击 "Open" → 选择下载的 Color 贴图
连接：Color 输出 → Principled BSDF 的 Base Color
```

**2. 添加透明度（重要！）**
```
如果颜色贴图有Alpha通道（PNG格式）：
连接：Image Texture 的 Alpha → Principled BSDF 的 Alpha

在 Principled BSDF 上方设置：
- Blend Mode: Alpha Clip（或 Alpha Blend）
- Clip Threshold: 0.5
```

**3. 添加法线贴图（叶脉细节）**
```
Shift + A → Texture → Image Texture
加载 Normal 贴图
设置：Color Space: Non-Color（重要！）

Shift + A → Vector → Normal Map
连接：
Image Texture (Color) → Normal Map (Color)
Normal Map (Normal) → Principled BSDF (Normal)
```

**4. 添加粗糙度贴图**
```
Shift + A → Texture → Image Texture
加载 Roughness 贴图
设置：Color Space: Non-Color

连接：Color → Principled BSDF (Roughness)
```

### 步骤5：Principled BSDF 设置

```
Base Color: 连接纹理
Subsurface: 0.3（次表面散射，光线穿透效果）
Subsurface Radius: RGB(0.5, 0.8, 0.5)
Subsurface Color: 浅绿色 RGB(0.6, 0.9, 0.6)
Metallic: 0
Specular: 0.3
Roughness: 连接纹理（或 0.6）
Alpha: 连接纹理
Transmission: 0.1（轻微透光）
```

### 步骤6：查看效果

1. 按 **Z** 键 → 选择 **Material Preview** 或 **Rendered**
2. 应该能看到真实的叶子效果
3. 调整光照：**Shift + A** → Light → Sun

---

## 方法2：自己拍照制作纹理（最真实）

### 步骤1：拍摄叶子照片

**拍摄要求：**
- 光线均匀（阴天或室内柔光）
- 叶子平铺在白纸上
- 相机垂直向下拍摄
- 高分辨率（至少2000x2000像素）
- 拍摄正反两面

### 步骤2：处理照片（Photoshop/GIMP）

**制作Color贴图：**
1. 打开照片
2. 使用魔棒工具选择背景
3. 删除背景（透明）
4. 调整颜色：
   - 增加饱和度
   - 调整亮度/对比度
5. 保存为 **PNG**（保留透明度）

**制作Alpha贴图（可选）：**
1. 复制Color贴图
2. 转为黑白
3. 叶子部分：白色
4. 背景：黑色
5. 保存为 **PNG**

### 步骤3：在Blender中使用

按照方法1的步骤4-6操作

---

## 方法3：使用Blender插件（快速）

### Botaniq插件（付费，但效果好）

1. 访问：https://polygoniq.com/botaniq/
2. 购买并安装
3. 直接使用预制的高质量叶子

### 免费替代：Sapling Tree Gen

1. Blender内置插件
2. **Edit** → **Preferences** → **Add-ons**
3. 搜索 "Sapling"
4. 启用插件
5. **Add** → **Curve** → **Sapling Tree Gen**
6. 可以生成叶子

---

## 橘子树叶特征（重要！）

### 真实橘子叶特点：

1. **形状**：椭圆形，尖端略尖
2. **颜色**：深绿色，有光泽
3. **叶脉**：中央主脉明显
4. **边缘**：光滑或轻微波浪
5. **大小**：5-10厘米长
6. **质感**：革质，较厚

### Blender中模拟：

```
形状：Plane → S → Y → 1.5（椭圆）
颜色：深绿色 RGB(0.15, 0.50, 0.15)
光泽：Roughness: 0.4（较光滑）
厚度：Subsurface: 0.3
```

---

## 性能优化技巧

### 1. 使用纹理图集（Texture Atlas）

将多片叶子合并到一张纹理上：

```
一张2048x2048纹理包含：
- 4-6种不同角度的叶子
- 每片叶子占用512x512区域
- 在UV编辑器中调整UV坐标
```

### 2. LOD（细节层次）

```
LOD 0（近景）：
- 使用完整PBR材质
- 法线贴图 + 粗糙度贴图

LOD 1（中景）：
- 只用颜色贴图
- 简化材质

LOD 2（远景）：
- 用简单绿色材质
- 不用纹理
```

### 3. 合并叶子网格

```
选中所有叶子
Ctrl + J 合并
减少Draw Call
```

---

## Unity HDRP材质设置

### 导入Blender模型后：

1. **提取材质**
   - 选中FBX → Inspector → Materials
   - Extract Materials

2. **设置HDRP/Lit材质**

```
Surface Options:
- Surface Type: Transparent（如果有透明边缘）
- Rendering Pass: Transparent
- Blend Mode: Alpha
- ☑ Double-Sided（双面显示）

Surface Inputs:
- Base Map: 叶子颜色纹理
- Normal Map: 叶子法线贴图
- Mask Map:
  - Metallic: 0
  - Ambient Occlusion: 白色
  - Detail Mask: 白色
  - Smoothness: 0.4
- ☑ Subsurface Scattering
  - Subsurface Mask: 0.3
  - Thickness Map: 可选

Emission:
- 不需要（除非要发光效果）
```

3. **优化设置**

```
☑ Enable GPU Instancing（重要！）
☑ Double Sided GI（全局光照双面）
```

---

## 完整工作流程

### Blender部分：

1. ✅ 创建Plane（椭圆形）
2. ✅ UV展开
3. ✅ 下载/制作纹理
4. ✅ 设置材质节点
5. ✅ 添加次表面散射
6. ✅ 复制10-20片叶子
7. ✅ 随机旋转和缩放
8. ✅ 导出FBX

### Unity部分：

1. ✅ 导入FBX和纹理
2. ✅ 提取材质
3. ✅ 配置HDRP材质
4. ✅ 设置透明度和双面
5. ✅ 启用GPU Instancing
6. ✅ 测试效果

---

## 推荐纹理资源

### 免费资源：

1. **Polyhaven.com**
   - 完全免费
   - 高质量PBR纹理
   - 推荐：搜索 "leaf" 或 "foliage"

2. **AmbientCG.com**
   - CC0协议
   - 无需注册
   - 推荐：Leaves001-020系列

3. **Textures.com**
   - 每天15张免费
   - 需要注册
   - 搜索："Citrus Leaf"

### 付费资源（高质量）：

1. **Quixel Megascans**
   - 照片扫描级别
   - 需要Epic Games账号
   - 部分免费

2. **Poliigon.com**
   - 专业级纹理
   - 月费制

---

## 常见问题

**Q: 叶子边缘有白边？**
A: 
- 检查Alpha贴图是否正确
- 设置 Blend Mode: Alpha Clip
- 调整 Clip Threshold

**Q: 叶子太亮/太暗？**
A: 
- 调整 Base Color 的亮度
- 或在纹理节点后添加 ColorRamp 节点

**Q: 叶子没有透光效果？**
A: 
- 启用 Subsurface Scattering
- 设置 Subsurface: 0.3
- 设置 Transmission: 0.1

**Q: 性能太差？**
A: 
- 减少叶子数量
- 使用纹理图集
- 合并网格（Ctrl + J）
- 启用GPU Instancing

**Q: Unity中叶子是粉红色？**
A: 
- 需要手动配置HDRP材质
- 确保纹理已正确导入
- 检查材质Shader是否为HDRP/Lit

---

## 进阶技巧

### 1. 添加风吹动画

在Unity中使用Shader Graph：
- 添加 Simple Noise 节点
- 连接到 Vertex Position
- 只影响叶子顶部顶点

### 2. 季节变化

创建多个材质变体：
- 春季：嫩绿色 RGB(0.4, 0.7, 0.3)
- 夏季：深绿色 RGB(0.15, 0.5, 0.15)
- 秋季：黄绿色 RGB(0.6, 0.6, 0.2)
- 冬季：枯黄色 RGB(0.5, 0.4, 0.2)

### 3. 损伤效果

使用 Mask 贴图：
- 添加破洞
- 添加虫咬痕迹
- 添加枯萎边缘

---

## 快速检查清单

导出前：
- [ ] UV已正确展开
- [ ] 纹理已加载
- [ ] Alpha通道已设置
- [ ] 法线贴图Color Space设为Non-Color
- [ ] 材质已应用
- [ ] 叶子已随机旋转和缩放

Unity导入后：
- [ ] 纹理已导入
- [ ] 材质已提取
- [ ] HDRP材质已配置
- [ ] 透明度已设置
- [ ] 双面渲染已启用
- [ ] GPU Instancing已启用

---

## 最终效果对比

| 方法 | 真实度 | 性能 | 制作时间 |
|------|--------|------|----------|
| 简单绿色材质 | ⭐ | ⭐⭐⭐⭐⭐ | 1分钟 |
| 颜色纹理 | ⭐⭐⭐ | ⭐⭐⭐⭐ | 5分钟 |
| PBR纹理 | ⭐⭐⭐⭐ | ⭐⭐⭐ | 15分钟 |
| 完整PBR+SSS | ⭐⭐⭐⭐⭐ | ⭐⭐ | 30分钟 |

**推荐**：
- 移动端/低配：颜色纹理
- PC游戏：PBR纹理
- 高端展示：完整PBR+SSS

---

## 总结

写实叶子的关键：
1. ✅ 使用真实照片纹理
2. ✅ 正确设置透明度
3. ✅ 添加法线贴图（叶脉细节）
4. ✅ 启用次表面散射（透光效果）
5. ✅ 双面渲染
6. ✅ 随机变化（旋转、缩放、颜色）

记住：**好的纹理比复杂的几何体更重要！**

祝你制作出漂亮的写实叶子！🍃
