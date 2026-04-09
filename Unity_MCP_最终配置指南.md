# Unity MCP 最终配置指南 (zabaglione/mcp-server-unity)

## 状态
✅ Node.js MCP 服务器已安装
✅ Kiro 配置已更新
⏳ 需要在 Unity 中安装 Newtonsoft JSON 包

## 完整设置步骤

### 1. 在 Unity 中安装 Newtonsoft JSON 包

1. 打开 Unity Editor (TreePlanQAQ 项目)
2. 菜单: `Window > Package Manager`
3. 点击左上角 `+` 按钮
4. 选择 `Add package by name...`
5. 输入: `com.unity.nuget.newtonsoft-json`
6. 点击 `Add`

### 2. 重新加载 Kiro

1. 按 `Ctrl+Shift+P`
2. 输入 "Developer: Reload Window"
3. 或者重启 Kiro

### 3. 测试连接

在 Kiro 中尝试以下命令:
- "列出 Unity 项目中的所有脚本"
- "获取项目信息"
- "创建一个简单的测试脚本"

## 技术细节

### MCP 服务器位置
```
C:\Users\36785\mcp-server-unity\
```

### Kiro 配置
```json
{
  "mcpServers": {
    "unity": {
      "command": "node",
      "args": ["C:\\Users\\36785\\mcp-server-unity\\build\\simple-index.js"],
      "env": {
        "UNITY_PROJECT_PATH": "C:\\Users\\36785\\Desktop\\Program\\TreePlanQAQ\\TreePlanQAQ"
      },
      "disabled": false,
      "autoApprove": [
        "create_script",
        "read_file",
        "write_file",
        "list_files",
        "get_project_info"
      ]
    }
  }
}
```

### 工作原理

1. **Kiro** 通过 stdio 协议与 Node.js MCP 服务器通信
2. **Node.js MCP 服务器** 通过 HTTP (端口 23457) 与 Unity Editor 通信
3. **Unity Editor** 运行一个 HTTP 服务器来接收命令

### 与 Singtaa/UnityMCP 的区别

| 特性 | zabaglione/mcp-server-unity | Singtaa/UnityMCP |
|------|----------------------------|------------------|
| 协议 | stdio (Kiro 兼容) | HTTP (Kiro 不兼容) |
| 安装 | 需要 Node.js + Unity 包 | 仅需 Unity 包 |
| 功能 | 脚本创建、文件管理 | 完整的 Unity API |
| 状态 | ✅ 可用 | ❌ 与 Kiro 不兼容 |

## 可用功能

### 脚本管理
- 创建 Unity 脚本 (MonoBehaviour, ScriptableObject 等)
- 读取和修改现有脚本
- 智能代码生成

### Shader 管理
- 创建自定义 Shader
- 支持 Built-in、URP、HDRP

### 项目组织
- 创建文件夹结构
- 移动和重命名资源
- 列出项目文件

### 项目信息
- 获取 Unity 版本
- 检测渲染管线
- 查看项目设置

## 故障排除

### 问题: MCP 服务器无法连接到 Unity
**症状**: 日志显示 "Unity HTTP server is not responding"

**解决方案**:
1. 确保 Unity Editor 已打开
2. 确保已安装 Newtonsoft JSON 包
3. 检查 Unity 控制台是否有错误
4. 重启 Unity Editor

### 问题: Kiro 无法找到 MCP 服务器
**解决方案**:
1. 验证 Node.js 已安装: `node --version`
2. 验证构建文件存在: `C:\Users\36785\mcp-server-unity\build\simple-index.js`
3. 重新加载 Kiro

### 问题: 权限错误
**解决方案**:
- 确保 Unity 项目路径正确
- 检查文件系统权限

## 下一步

安装 Newtonsoft JSON 包后,你就可以:
1. 通过自然语言创建 Unity 脚本
2. 管理项目文件和文件夹
3. 获取项目信息和配置
4. 创建和修改 Shader

---

配置完成后,你就可以通过 Kiro 与 Unity 项目无缝交互了! 🎮
