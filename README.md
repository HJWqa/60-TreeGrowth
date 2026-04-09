# 🍊 橘子树生长模拟系统

一个基于 Unity 的写实橘子树生长模拟游戏，包含完整的环境控制系统和交互式UI。

## 📋 项目简介

这是一个60秒挑战果树成长的模拟游戏，玩家需要通过调节温度、湿度等环境参数来促进橘子树的生长。

### 主要特性

- 🌳 **写实3D橘子树模型** - 使用Blender手工建模
- 📊 **8个生长阶段** - 从种子到成熟果实
- 🌡️ **环境参数控制** - 温度、湿度、光照实时调节
- 🎮 **完整UI系统** - 信息面板、控制面板、设置面板
- ⏸️ **游戏控制** - 暂停、继续、重置功能
- 💡 **智能提示** - 环境参数合理范围提示

## 🎯 游戏玩法

1. 点击"开始生长"按钮启动游戏
2. 通过右侧按钮调节温度和湿度
3. 观察左侧面板的生长进度
4. 在60秒内让橘子树成长到最佳状态

### 环境参数最佳范围

- 🌡️ **温度**: 20°C - 25°C（范围：15°C - 30°C）
- 💧 **湿度**: 50% - 70%（范围：40% - 80%）
- ☀️ **光照**: 600 lux（范围：400 - 800 lux）

## 🛠️ 技术栈

- **游戏引擎**: Unity 2021.3+
- **3D建模**: Blender 3.0+
- **编程语言**: C#
- **版本控制**: Git

## 📁 项目结构

```
TreePlanQAQ/
├── Assets/
│   ├── Configs/              # 配置文件
│   ├── Materials/            # 材质
│   ├── Models/               # 3D模型
│   ├── Prefabs/              # 预制体
│   ├── Scenes/               # 场景文件
│   ├── Scripts/              # C#脚本
│   │   ├── Editor/          # 编辑器工具
│   │   ├── OrangeTree/      # 橘子树系统
│   │   └── UI/              # UI控制器
│   └── Textures/            # 纹理贴图
├── Packages/                 # Unity包
└── ProjectSettings/          # 项目设置
```

## 🚀 快速开始

### 环境要求

- Unity 2021.3 或更高版本
- Git
- Windows/Mac/Linux

### 安装步骤

1. **克隆仓库**
   ```bash
   git clone https://github.com/RuoAnyi/60-TreeGrowth.git
   cd 60-TreeGrowth
   ```

2. **打开项目**
   - 启动 Unity Hub
   - 点击"打开"
   - 选择 `60-TreeGrowth/TreePlanQAQ` 文件夹

3. **运行游戏**
   - 打开场景：`Assets/Scenes/MainScene.unity`
   - 点击播放按钮

## 🎨 3D模型制作

项目包含完整的Blender建模教程：

- [Blender手工建模橘子树完整教程](Blender手工建模橘子树完整教程.md)
- [Blender手搓叶子详细教程](Blender手搓叶子详细教程.md)
- [Blender模型导入Unity完整指南](Blender模型导入Unity完整指南.md)

## 📖 开发文档

- [橘子树系统完成报告](橘子树系统完成报告.md)
- [Unity写实橘子树制作教程](Unity写实橘子树制作教程.md)
- [橘子树集成步骤](橘子树集成步骤.md)
- [使用检查清单](使用检查清单.txt)

## 🎮 UI系统

### 左上角图标
- ⚙️ **设置** - 游戏设置面板
- ❗ **提示** - 环境参数提示
- ⏸️ **暂停/继续** - 控制游戏运行

### 右上角
- 🔄 **重置** - 重新开始游戏

### 左侧信息面板
- 当前生长阶段
- 生长进度条
- 环境参数显示

### 右侧控制面板
- 温度调节按钮
- 湿度调节按钮

## 🔧 编辑器工具

项目包含多个Unity编辑器工具，位于菜单：`橘子树 > UI设置`

- 创建左上角图标
- 创建右上角重置按钮
- 创建生长开始按钮
- 连接图标按钮和面板
- UI层级调试器

## 📝 系统架构

### 核心系统

1. **OrangeTreeController** - 橘子树生长控制
2. **EnvironmentManager** - 环境参数管理
3. **GrowthUIController** - 生长UI控制
4. **EnvironmentController** - 环境控制UI

### UI系统

1. **TopIconsController** - 左上角图标控制
2. **ResetButtonController** - 重置按钮控制
3. **GrowthStartController** - 开始按钮控制
4. **TipUIController** - 提示UI控制

## 🐛 已知问题

- 无

## 🔮 未来计划

- [ ] 添加音效系统
- [ ] 添加粒子效果
- [ ] 添加更多树种
- [ ] 添加天气系统
- [ ] 添加成就系统
- [ ] 多语言支持

## 🤝 贡献

欢迎提交 Issue 和 Pull Request！

## 📄 许可证

本项目采用 MIT 许可证 - 详见 [LICENSE](LICENSE) 文件

## 👨‍💻 作者

- RuoAnyi - [GitHub](https://github.com/RuoAnyi)

## 🙏 致谢

- Unity Technologies
- Blender Foundation
- 所有贡献者

## 📞 联系方式

- GitHub: [@RuoAnyi](https://github.com/RuoAnyi)
- 项目地址: [60-TreeGrowth](https://github.com/RuoAnyi/60-TreeGrowth)

---

⭐ 如果这个项目对你有帮助，请给个星标！
