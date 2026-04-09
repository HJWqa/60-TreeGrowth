# Unity MCP 断连问题修复指南

## 问题描述
Unity MCP包无法检测到Node.js，导致MCP服务器无法启动。

错误信息：
```
[UnityMcp] Node.js not found. Please install Node.js 18+ from https://nodejs.org
```

## 解决方案

### 方法1: 使用修复脚本（推荐）

1. **等待Unity编译完成**
   - 新创建的FixUnityMcpNodePath.cs脚本会自动编译

2. **运行修复脚本**
   - 菜单: `TreePlanQAQ/Fix Unity MCP Node.js Path`
   - 脚本会自动将Node.js路径添加到环境变量

3. **重启Unity编辑器**
   - 完全关闭Unity
   - 重新打开项目
   - Unity MCP应该能够检测到Node.js

4. **验证MCP服务器**
   - 菜单: `Window > Unity MCP > Server`
   - 检查服务器状态
   - 如果显示"Running"，说明修复成功

### 方法2: 手动配置环境变量

如果方法1不起作用，可以手动配置：

1. **打开系统环境变量**
   - 右键"此电脑" > 属性
   - 高级系统设置 > 环境变量

2. **编辑PATH变量**
   - 在"系统变量"中找到"Path"
   - 点击"编辑"
   - 添加: `C:\Program Files\nodejs`
   - 确保这一项在列表的前面

3. **重启Unity**
   - 完全关闭Unity
   - 重新打开项目

### 方法3: 重新安装Unity MCP包

如果以上方法都不起作用：

1. **移除Unity MCP包**
   - 菜单: `Window > Package Manager`
   - 找到"Unity MCP"包
   - 点击"Remove"

2. **清理缓存**
   - 删除 `Library/UnityMcp` 目录
   - 删除 `Library/PackageCache/com.singtaa.unity-mcp@*` 目录

3. **重新安装**
   - 在Package Manager中重新添加Unity MCP包
   - 等待安装完成

4. **重启Unity**

## 验证Node.js安装

在命令行中运行：
```bash
node --version
```

应该显示：`v20.10.0` 或更高版本

如果没有显示版本号，说明Node.js未正确安装或PATH配置有问题。

## 常见问题

### Q: 修复脚本运行后仍然无法连接？
A: 需要完全重启Unity编辑器，而不是重新加载项目。

### Q: Unity MCP窗口在哪里？
A: 菜单 `Window > Unity MCP > Server`

### Q: 如何手动启动MCP服务器？
A: 
1. 打开Unity MCP窗口
2. 点击"Start Server"按钮
3. 查看Console是否有错误信息

### Q: 服务器启动但无法连接？
A: 检查防火墙设置，确保允许Node.js和Unity通信。

## 测试MCP连接

修复后，可以通过以下方式测试：

1. **在Kiro中测试**
   - 尝试调用Unity MCP工具
   - 例如: `mcp_unity_mcp_unity_bridge_ping`

2. **在Unity中测试**
   - 打开Unity MCP窗口
   - 查看"Server Status"应该显示"Running"
   - 查看"Connection Status"应该显示"Connected"

## 配置文件位置

- **Unity MCP设置**: `ProjectSettings/McpSettings.json`
- **MCP服务器**: `Library/UnityMcp/Server/`
- **Kiro MCP配置**: `.kiro/settings/mcp.json`

## 端口配置

Unity MCP使用以下端口：
- HTTP端口: 5173
- IPC端口: 52100

如果这些端口被占用，可以在`McpSettings.json`中修改。

## 完全重置MCP

如果所有方法都失败，可以完全重置：

1. 关闭Unity
2. 删除以下目录：
   - `Library/UnityMcp`
   - `Library/PackageCache/com.singtaa.unity-mcp@*`
3. 删除文件：
   - `ProjectSettings/McpSettings.json`
4. 重新打开Unity
5. 重新安装Unity MCP包

## 注意事项

- Unity MCP需要Node.js 18或更高版本
- 确保Unity有足够的权限访问Node.js
- 某些杀毒软件可能会阻止Node.js进程
- 如果使用VPN，可能会影响本地连接

## 成功标志

修复成功后，你应该看到：
- Unity Console中没有"Node.js not found"错误
- Unity MCP窗口显示服务器正在运行
- Kiro可以成功调用Unity MCP工具
- 可以通过MCP工具操作Unity场景

## 需要帮助？

如果问题仍然存在：
1. 检查Unity Console的完整错误日志
2. 检查Node.js是否正确安装
3. 尝试在命令行中手动运行MCP服务器
4. 查看Unity MCP包的GitHub Issues
