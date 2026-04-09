# 🚀 快速上传到 GitHub

## 你的仓库信息

- **用户名**: RuoAnyi
- **仓库名**: 60-TreeGrowth
- **仓库地址**: https://github.com/RuoAnyi/60-TreeGrowth

---

## 方法一：命令行上传（5步完成）

### 第1步：打开命令行

在 `TreePlanQAQ` 文件夹中，按住 Shift 键，右键点击空白处，选择"在此处打开 PowerShell 窗口"或"在此处打开命令提示符"。

### 第2步：初始化 Git

```bash
git init
```

### 第3步：添加所有文件

```bash
git add .
```

### 第4步：提交

```bash
git commit -m "Initial commit: 橘子树生长模拟系统"
```

### 第5步：推送到 GitHub

```bash
git branch -M main
git remote add origin https://github.com/RuoAnyi/60-TreeGrowth.git
git push -u origin main
```

**如果提示需要登录：**
- 输入你的 GitHub 用户名：`RuoAnyi`
- 输入密码（建议使用 Personal Access Token）

完成！🎉

---

## 方法二：GitHub Desktop（最简单）

### 第1步：下载安装

下载地址：https://desktop.github.com/

### 第2步：登录

打开 GitHub Desktop，登录你的 GitHub 账号（RuoAnyi）

### 第3步：添加项目

1. File → Add Local Repository
2. 选择 `TreePlanQAQ` 文件夹
3. 如果提示不是 Git 仓库，点击 "create a repository"

### 第4步：发布

1. 点击顶部的 "Publish repository"
2. Name: `60-TreeGrowth`
3. Description: `橘子树生长模拟游戏 - 60秒挑战`
4. 取消勾选 "Keep this code private"（如果想公开）
5. 点击 "Publish Repository"

完成！🎉

---

## ⚠️ 上传前检查

### 必须删除的文件夹（减小大小）：

```
TreePlanQAQ/Library/     ← 删除这个
TreePlanQAQ/Temp/        ← 删除这个
TreePlanQAQ/.vs/         ← 删除这个（如果有）
TreePlanQAQ/Obj/         ← 删除这个（如果有）
```

**如何删除：**
1. 在文件资源管理器中打开 `TreePlanQAQ` 文件夹
2. 找到这些文件夹并删除
3. 这些文件夹会在 Unity 打开项目时自动重新生成

### 检查清单：

- [ ] 已删除 Library 文件夹
- [ ] 已删除 Temp 文件夹
- [ ] 已删除 .vs 文件夹
- [ ] 已创建 .gitignore 文件（已完成✅）
- [ ] 已创建 README.md 文件（已完成✅）
- [ ] 已创建 LICENSE 文件（已完成✅）

---

## 🔑 获取 Personal Access Token

如果推送时需要密码，建议使用 Token：

### 步骤：

1. 登录 GitHub
2. 点击右上角头像 → Settings
3. 左侧菜单最底部 → Developer settings
4. Personal access tokens → Tokens (classic)
5. Generate new token (classic)
6. 勾选 `repo` 权限
7. 点击 Generate token
8. **复制并保存 token**（只显示一次！）

### 使用 Token：

推送时，用户名输入：`RuoAnyi`
密码输入：你的 token（不是 GitHub 密码）

---

## 📊 上传后的项目结构

```
https://github.com/RuoAnyi/60-TreeGrowth
├── TreePlanQAQ/              # Unity 项目文件夹
│   ├── Assets/               # 资源文件
│   ├── Packages/             # 包管理
│   ├── ProjectSettings/      # 项目设置
│   ├── README.md            # 项目说明
│   ├── LICENSE              # 许可证
│   └── .gitignore           # Git 忽略文件
└── README.md                 # 仓库说明（可选）
```

---

## 🎯 后续更新

每次修改后，只需3步：

```bash
git add .
git commit -m "描述你的更改"
git push
```

---

## ❓ 常见问题

### Q: 推送失败，提示 "remote: Repository not found"？

**解决：**
1. 确认仓库已在 GitHub 上创建
2. 检查仓库地址是否正确：`https://github.com/RuoAnyi/60-TreeGrowth.git`
3. 确认你有权限访问这个仓库

### Q: 推送失败，提示 "Authentication failed"？

**解决：**
1. 使用 Personal Access Token 而不是密码
2. 或者使用 GitHub Desktop（自动处理认证）

### Q: 文件太大无法上传？

**解决：**
1. 确保已删除 Library、Temp 文件夹
2. 检查是否有大型资源文件（.fbx, .blend, .psd）
3. 考虑使用 Git LFS 管理大文件

### Q: 如何查看上传进度？

**命令行：**
推送时会显示进度条

**GitHub Desktop：**
底部会显示上传进度

---

## ✅ 验证上传成功

上传完成后，访问：

```
https://github.com/RuoAnyi/60-TreeGrowth
```

你应该能看到：
- 项目文件列表
- README.md 内容显示在页面底部
- 绿色的 "Code" 按钮

---

## 🎉 恭喜！

你的项目已成功上传到 GitHub！

现在你可以：
- 分享项目链接给其他人
- 在其他电脑上克隆项目
- 使用 GitHub 进行版本控制
- 与他人协作开发

---

需要帮助？查看详细教程：[GitHub上传指南.md](GitHub上传指南.md)
