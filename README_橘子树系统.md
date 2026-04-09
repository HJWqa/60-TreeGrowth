# 🍊 橘子树程序化模型系统

> 为Unity果树生长模拟系统设计的完整橘子树模型解决方案

---

## 📖 项目简介

这是一个完整的橘子树程序化生成系统，可以创建从种子到成熟的10个完整生长阶段。系统采用程序化建模技术，无需外部3D模型文件，完全通过代码生成真实的橘子树形态。

### ✨ 核心特性

- 🌱 **10个完整生长阶段** - 从种子到成熟的完整生命周期
- 🎨 **真实的橘子树特征** - 深绿叶、白花、橙果
- ⚡ **程序化生成** - 无需外部模型，运行时动态创建
- 🔧 **易于集成** - 完美兼容现有的PlantGrowthController系统
- 🎮 **可视化工具** - Unity编辑器内的友好界面
- 📊 **性能优化** - 使用基础几何体，低性能开销
- 🌈 **渲染管线兼容** - 支持HDRP/URP/Built-in

---

## 📁 文件结构

```
TreePlanQAQ/
├── Assets/
│   ├── Scripts/
│   │   └── Plant/
│   │       ├── OrangeTreeGenerator.cs              # 核心生成器
│   │       ├── OrangeTreePreview.cs                # 场景预览组件
│   │       └── Editor/
│   │           ├── OrangeTreeGeneratorEditor.cs    # 可视化生成工具
│   │           ├── OrangeTreePreviewEditor.cs      # 预览Inspector
│   │           └── OrangeTreeBatchGenerator.cs     # 批量生成工具
│   ├── Prefabs/
│   │   └── Plants/
│   │       └── OrangeTrees/                        # 生成的预制体目录
│   └── Configs/
│       └── OrangeTreeGrowthConfig.asset            # 橘子树生长配置
│
├── 橘子树模型使用说明.md                            # 详细使用文档
├── 橘子树快速开始.txt                               # 快速入门指南
├── 橘子树集成步骤.md                                # 完整集成流程
├── 橘子树阶段对比.txt                               # 各阶段详细对比
└── 橘子树系统完成报告.md                            # 技术报告
```

---

## 🚀 快速开始

### 方法一：一键生成（最快）

1. 打开Unity编辑器
2. 菜单栏：`TreePlanQAQ` → `Quick Generate All Orange Trees`
3. 点击"生成"按钮
4. 等待5-10秒完成

### 方法二：可视化窗口

1. 菜单栏：`TreePlanQAQ` → `Generate Orange Tree Models`
2. 在窗口中点击"生成所有橘子树模型"
3. 预制体自动保存到 `Assets/Prefabs/Plants/OrangeTrees/`

### 方法三：场景预览

1. 创建空GameObject
2. 添加 `OrangeTreePreview` 组件
3. 在Inspector中选择阶段并点击"生成预览"

---

## 🌳 生长阶段

| 阶段 | 图标 | 名称 | 特征 | 生长值 |
|------|------|------|------|--------|
| 1 | 🌰 | Seed | 椭圆形种子 | 0-10 |
| 2 | 🌱 | Sprout | 细茎+2片叶 | 10-25 |
| 3 | 🌿 | Seedling | 主茎+多层叶 | 25-50 |
| 4 | 🌳 | YoungTree | 树干+分支+树冠 | 50-75 |
| 5 | 🌲 | MatureTree | 茂密树冠 | 75-90 |
| 6 | 🌸 | Flowering | 25朵白花 | 90-95 |
| 7 | 🍏 | Fruiting | 20个青橘子 | 95-100 |
| 8 | 🍊 | Harvest | 30个橙橘子 | 100 |
| 9 | 🍂 | Dormant | 光秃枝干 | 环境触发 |
| 10 | 💀 | Dead | 枯萎倾斜 | < 死亡阈值 |

---

## 🎨 设计特色

### 颜色方案
- **叶子**: 深绿色 `RGB(38, 128, 38)` - 橘子树特有
- **树干**: 棕色 `RGB(89, 64, 38)` - 自然树皮
- **花朵**: 白色 `RGB(255, 255, 242)` - 纯白花瓣
- **青橘**: 青绿色 `RGB(102, 179, 51)` - 未成熟
- **黄橘**: 黄色 `RGB(255, 204, 51)` - 半成熟
- **橙橘**: 橙色 `RGB(255, 128, 26)` - 完全成熟

### 尺寸变化
- 种子: 0.15单位
- 成树: 2.5单位（增长16倍）
- 树冠: 最大直径2.6单位
- 果实: 直径0.08-0.16单位

### 细节特征
- 花朵有5片花瓣 + 黄色花心
- 橘子带有小绿蒂
- 分支系统呈放射状
- 树冠采用多层球体

---

## 🔧 集成到现有系统

### 步骤1：生成预制体
使用上述任一方法生成10个预制体

### 步骤2：配置PlantGrowthController
1. 打开场景中的PlantGrowthController
2. 在Stage Configs中配置10个阶段
3. 将对应的预制体拖拽到Stage Model字段

### 步骤3：设置生长配置
1. 将 `OrangeTreeGrowthConfig.asset` 拖到Growth Config字段
2. 根据需要调整参数

### 步骤4：运行测试
点击播放按钮，观察橘子树生长

详细步骤请参考：`橘子树集成步骤.md`

---

## 💻 代码示例

### 生成单个橘子树
```csharp
using TreePlanQAQ.Plant;

GameObject tree = OrangeTreeGenerator.CreateOrangeTreeModel(PlantStage.Harvest);
tree.transform.position = new Vector3(0, 0, 0);
```

### 生成所有阶段
```csharp
PlantStage[] stages = {
    PlantStage.Seed, PlantStage.Sprout, PlantStage.Seedling,
    PlantStage.YoungTree, PlantStage.MatureTree, PlantStage.Flowering,
    PlantStage.Fruiting, PlantStage.Harvest, PlantStage.Dormant, PlantStage.Dead
};

for (int i = 0; i < stages.Length; i++)
{
    GameObject tree = OrangeTreeGenerator.CreateOrangeTreeModel(stages[i]);
    tree.transform.position = new Vector3(i * 3, 0, 0);
}
```

### 自定义颜色
在 `OrangeTreeGenerator.cs` 顶部修改：
```csharp
private static readonly Color OrangeLeafColor = new Color(0.15f, 0.5f, 0.15f);
private static readonly Color OrangeFruitOrange = new Color(1f, 0.5f, 0.1f);
```

---

## 📊 性能数据

| 阶段 | 对象数 | 估计顶点数 | 内存占用 |
|------|--------|-----------|---------|
| Seed | 1 | ~380 | 极低 |
| Sprout | 3 | ~1,140 | 极低 |
| Seedling | 8 | ~3,040 | 低 |
| YoungTree | 8 | ~3,040 | 低 |
| MatureTree | 11 | ~4,180 | 中 |
| Flowering | 161 | ~61,180 | 高 |
| Fruiting | 51 | ~19,380 | 中 |
| Harvest | 91 | ~34,580 | 中高 |
| Dormant | 4 | ~1,520 | 低 |
| Dead | 2 | ~760 | 极低 |

**优化建议**：
- 开花和成熟阶段对象较多，建议使用LOD
- 场景中多棵树时使用对象池
- 远距离可以降低果实数量

---

## 🎯 使用场景

### 适合
- ✅ 原型开发和快速迭代
- ✅ 教育和演示项目
- ✅ 独立游戏开发
- ✅ 移动平台游戏
- ✅ 程序化内容生成

### 不适合
- ❌ AAA级写实游戏（建议使用专业3D模型）
- ❌ VR近距离观察（需要更高精度模型）
- ❌ 电影级渲染（需要高模和贴图）

---

## 🐛 常见问题

### Q: 模型是粉红色的？
**A**: 检查渲染管线设置，确保shader正确加载

### Q: 找不到生成菜单？
**A**: 等待Unity编译完成，确保Editor文件夹结构正确

### Q: 生长速度太快/太慢？
**A**: 调整 `OrangeTreeGrowthConfig` 中的 `Base Growth Rate`

### Q: 想修改橘子颜色？
**A**: 编辑 `OrangeTreeGenerator.cs` 顶部的颜色常量

### Q: 如何添加更多果实？
**A**: 在生成器的 `CreateFruitingTree` 和 `CreateHarvestTree` 方法中增加循环次数

更多问题请参考：`橘子树集成步骤.md` 的"常见问题解决"章节

---

## 📚 文档索引

| 文档 | 用途 | 适合人群 |
|------|------|---------|
| `README_橘子树系统.md` | 项目总览 | 所有人 |
| `橘子树快速开始.txt` | 快速入门 | 新手 |
| `橘子树集成步骤.md` | 完整集成流程 | 开发者 |
| `橘子树模型使用说明.md` | 详细使用文档 | 开发者 |
| `橘子树阶段对比.txt` | 各阶段详细信息 | 美术/策划 |
| `橘子树系统完成报告.md` | 技术细节 | 技术人员 |

---

## 🔄 版本历史

### v1.0 (2026-04-06)
- ✅ 初始版本发布
- ✅ 10个完整生长阶段
- ✅ Unity编辑器工具
- ✅ 场景预览组件
- ✅ 完整文档

---

## 🎓 学习资源

### 扩展阅读
- Unity程序化建模技术
- 植物生长模拟算法
- 游戏中的生态系统设计

### 相关项目
- 果树生长模拟系统（本项目的母项目）
- Unity植物生成工具
- 程序化内容生成（PCG）

---

## 🤝 贡献

如果你想改进这个系统：

1. 添加更多果树类型（苹果、梨、桃等）
2. 实现更真实的树叶材质
3. 添加风吹动画效果
4. 优化性能和LOD系统
5. 添加更多交互功能

---

## 📄 许可

本项目为教育和学习目的创建，可自由使用和修改。

---

## 💬 支持

如有问题或建议，请参考文档或联系开发团队。

---

**开发者**: Kiro AI Assistant  
**创建日期**: 2026-04-06  
**版本**: 1.0  
**项目**: TreePlanQAQ - 果树生长模拟系统

---

## 🎉 开始使用

现在就打开Unity，运行 `TreePlanQAQ → Quick Generate All Orange Trees`，
让你的第一棵橘子树生长起来吧！🍊🌳

**祝你开发顺利！**
