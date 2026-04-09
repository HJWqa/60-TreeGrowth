# Unity MCP Server 配置指南

## 问题诊断
当前错误：`fetch failed` - Kiro 无法连接到 Unity MCP Server

## 原因
Unity MCP Server 没有在端口 8080 上运行

## 解决步骤

### 1. 在 Unity Editor 中启动 MCP Server

打开你的 Unity 项目后，尝试以下方法之一：

#### 方法 A：通过菜单启动
- 查找菜单：`Tools` → `Unity MCP` → `Start Server`
- 或者：`Window` → `Unity MCP` → `Server Settings`

#### 方法 B：检查自动启动设置
- 打开 `Edit` → `Preferences` → `Unity MCP`
- 确保 "Auto Start Server" 选项已启用
- 设置端口为 `8080`（默认）

### 2. 验证 Server 是否启动

在 Unity Console 中查找类似以下的日志：
```
[Unity MCP] Server started on http://localhost:8080
[Unity MCP] SSE endpoint: http://localhost:8080/sse
```

### 3. 测试连接

在 PowerShell 中运行：
```powershell
Test-NetConnection -ComputerName localhost -Port 8080
```

应该返回 `TcpTestSucceeded : True`

### 4. 重新连接 Kiro

- 在 Kiro 中打开命令面板（Ctrl+Shift+P）
- 搜索 "MCP: Reconnect Server"
- 选择 `unity-mcp`

## 当前 Kiro MCP 配置

```json
{
  "unity-mcp": {
    "type": "sse",
    "url": "http://localhost:8080/sse",
    "disabled": false
  }
}
```

## 常见问题

### Q: 找不到 Unity MCP 菜单？
A: 确保包已正确安装，检查 `Packages/manifest.json` 中是否有：
```json
"com.singtaa.unity-mcp": "https://github.com/Singtaa/UnityMCP.git"
```

### Q: 端口 8080 被占用？
A: 
1. 在 Unity MCP 设置中更改端口（例如 8081）
2. 同时更新 Kiro 的 `mcp.json` 中的 URL

### Q: Server 启动但仍然连接失败？
A: 
1. 检查防火墙设置
2. 尝试重启 Unity Editor
3. 检查 Unity Console 中的错误日志

## 下一步

1. 在 Unity Editor 中找到并启动 MCP Server
2. 确认 Unity Console 显示 Server 已启动
3. 在 Kiro 中重新连接 MCP Server
