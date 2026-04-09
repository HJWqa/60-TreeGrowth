# GitHub 上传指南

## 方法一：使用命令行（推荐）

### 前置准备

1. **安装 Git**
   - 下载：https://git-scm.com/downloads
   - 安装后重启电脑

2. **配置 Git**
   ```bash
   git config --global user.name "你的名字"
   git config --global user.email "your.email@example.com"
   ```

3. **创建 GitHub 账号**
   - 访问：https://github.com
   - 注册账号

### 步骤一：在 GitHub 创建仓库

1. 登录 GitHub
2. 点击右上角 "+" → "New repository"
3. 填写信息：
   - Repository name: `60-TreeGrowth`
   - Description: `橘子树生长模拟游戏 - 60秒挑战`
   - 选择 Public（公开）或 Private（私有）
   - **不要**勾选 "Initialize this repository with a README"
4. 点击 "Create repository"

### 步骤二：初始化本地仓库

打开命令行（CMD 或 PowerShell），进入项目目录：

```bash
cd TreePlanQAQ
```

初始化 Git 仓库：

```bash
git init
```

### 步骤三：添加文件到暂存区

```bash
# 添加所有文件
git add .

# 或者分批添加
git add Assets/
git add ProjectSettings/
git add Packages/
git add *.md
git add .gitignore
git add LICENSE
```

### 步骤四：提交到本地仓库

```bash
git commit -m "Initial commit: 橘子树生长模拟系统"
```

### 步骤五：连接远程仓库

```bash
git remote add origin https://github.com/RuoAnyi/60-TreeGrowth.git
```

### 步骤六：推送到 GitHub

```bash
# 第一次推送
git branch -M main
git push -u origin main
```

如果遇到认证问题，可能需要使用 Personal Access Token：

1. GitHub 设置 → Developer settings → Personal access tokens → Generate new token
2. 勾选 `repo` 权限
3. 生成 token 并保存
4. 推送时使用 token 作为密码

---

## 方法二：使用 GitHub Desktop（简单）

### 步骤一：安装 GitHub Desktop

1. 下载：https://desktop.github.com/
2. 安装并登录 GitHub 账号

### 步骤二：添加项目

1. 打开 GitHub Desktop
2. File → Add Local Repository
3. 选择 `TreePlanQAQ` 文件夹
4. 如果提示"This directory does not appear to be a Git repository"
   - 点击 "create a repository"

### 步骤三：创建仓库

1. Name: `60-TreeGrowth`
2. Description: `橘子树生长模拟游戏 - 60秒挑战`
3. 确保勾选 "Initialize this repository with a README"（如果没有的话）
4. Git Ignore: 选择 `Unity`
5. License: 选择 `MIT License`
6. 点击 "Create Repository"

### 步骤四：提交更改

1. 在左侧看到所有更改的文件
2. 在底部输入提交信息：`Initial commit: 橘子树生长模拟系统`
3. 点击 "Commit to main"

### 步骤五：发布到 GitHub

1. 点击顶部的 "Publish repository"
2. 选择 Public 或 Private
3. 点击 "Publish Repository"

完成！

---

## 方法三：使用 Visual Studio Code

### 步骤一：安装 VS Code

1. 下载：https://code.visualstudio.com/
2. 安装 Git 扩展（通常已内置）

### 步骤二：打开项目

1. 打开 VS Code
2. File → Open Folder
3. 选择 `TreePlanQAQ` 文件夹

### 步骤三：初始化 Git

1. 点击左侧的 Source Control 图标（分支图标）
2. 点击 "Initialize Repository"

### 步骤四：提交更改

1. 在 Source Control 面板中看到所有更改
2. 点击 "+" 添加所有文件到暂存区
3. 在顶部输入提交信息：`Initial commit: 橘子树生长模拟系统`
4. 点击 "✓" 提交

### 步骤五：推送到 GitHub

1. 点击 "..." → "Remote" → "Add Remote"
2. 输入仓库 URL：`https://github.com/RuoAnyi/60-TreeGrowth.git`
3. 点击 "..." → "Push"

---

## 常见问题

### Q1: 文件太大无法上传？

GitHub 单个文件限制 100MB。如果有大文件：

**方案1：使用 Git LFS**
```bash
git lfs install
git lfs track "*.fbx"
git lfs track "*.blend"
git add .gitattributes
git commit -m "Add Git LFS"
```

**方案2：排除大文件**
在 `.gitignore` 中添加：
```
*.fbx
*.blend
*.psd
```

### Q2: 推送失败，提示认证错误？

**Windows:**
1. 控制面板 → 凭据管理器
2. 删除 GitHub 相关凭据
3. 重新推送，输入新的 token

**使用 Personal Access Token:**
```bash
git remote set-url origin https://你的token@github.com/你的用户名/TreePlanQAQ.git
```

### Q3: 如何更新已上传的项目？

```bash
# 添加更改
git add .

# 提交
git commit -m "更新说明"

# 推送
git push
```

### Q4: 如何忽略 Unity 生成的文件？

已经创建了 `.gitignore` 文件，它会自动忽略：
- Library/
- Temp/
- Obj/
- Build/
- Logs/
- .vs/
- *.csproj
- *.sln

### Q5: 项目太大怎么办？

**检查大小：**
```bash
du -sh TreePlanQAQ
```

**减小大小：**
1. 删除 Library 文件夹（会自动重新生成）
2. 删除 Temp 文件夹
3. 删除 Build 文件夹
4. 压缩纹理和模型
5. 使用 Git LFS 管理大文件

---

## 推荐的 .gitignore 内容

已经为你创建了 `.gitignore` 文件，包含：

- Unity 生成的文件夹（Library, Temp, Obj, Build）
- Visual Studio 文件（.vs, .csproj, .sln）
- 构建产物（.apk, .exe, .app）
- 日志文件（*.log）
- 系统文件（.DS_Store）

---

## 后续维护

### 每次修改后更新：

```bash
# 1. 查看更改
git status

# 2. 添加更改
git add .

# 3. 提交
git commit -m "描述你的更改"

# 4. 推送
git push
```

### 创建分支开发新功能：

```bash
# 创建并切换到新分支
git checkout -b feature/新功能名称

# 开发完成后合并
git checkout main
git merge feature/新功能名称
git push
```

### 查看历史：

```bash
git log --oneline
```

---

## 完成检查清单

上传前请确认：

- [ ] 已创建 `.gitignore` 文件
- [ ] 已创建 `README.md` 文件
- [ ] 已创建 `LICENSE` 文件
- [ ] 删除了 Library 文件夹
- [ ] 删除了 Temp 文件夹
- [ ] 删除了 .vs 文件夹
- [ ] 检查没有敏感信息（密码、密钥等）
- [ ] 项目可以正常运行
- [ ] 文档齐全

---

## 项目链接示例

上传成功后，你的项目地址将是：

```
https://github.com/RuoAnyi/60-TreeGrowth
```

可以在 README.md 中添加徽章：

```markdown
![Unity](https://img.shields.io/badge/Unity-2021.3+-black?logo=unity)
![License](https://img.shields.io/badge/license-MIT-blue)
![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20Mac%20%7C%20Linux-lightgrey)
```

---

## 需要帮助？

- Git 官方文档：https://git-scm.com/doc
- GitHub 帮助：https://docs.github.com/
- Unity 版本控制：https://docs.unity3d.com/Manual/Versioncontrol.html

祝你上传成功！🎉
