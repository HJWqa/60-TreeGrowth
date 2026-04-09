# 修复 Unity MCP 配置脚本

$configPath = "$env:APPDATA\Kiro\User\globalStorage\kiro.kiroagent\da22e7a0c280bcb0eb68b2539df905fd\74a08cf8613c7dec4db7b264470db812\c75cf148\.kiro\settings\mcp.json"

Write-Host "正在读取配置文件: $configPath"

if (Test-Path $configPath) {
    $config = Get-Content $configPath -Raw | ConvertFrom-Json
    
    # 更新 unityMCP 配置
    $config.mcpServers.unityMCP = @{
        type = "http"
        url = "http://127.0.0.1:5173/mcp"
        headers = @{
            Authorization = "Bearer SFH8BTr8VE54U8K5lx5N7sJfaXpkUzHB"
        }
        env = @{}
        disabled = $false
        autoApprove = @(
            "unity_scene_list",
            "unity_scene_load",
            "unity_gameobject_find",
            "unity_gameobject_create",
            "unity_component_list",
            "unity_component_add",
            "unity_component_get_properties",
            "unity_component_set_property",
            "unity_transform_get",
            "unity_transform_set",
            "unity_editor_execute_menu_item",
            "unity_editor_log",
            "unity_project_list_files",
            "unity_project_read_text",
            "unity_project_write_text",
            "unity_assets_refresh"
        )
    }
    
    # 保存配置
    $config | ConvertTo-Json -Depth 10 | Set-Content $configPath -Encoding UTF8
    
    Write-Host "✅ 配置已更新!"
    Write-Host "请在 Kiro 中重新连接 MCP 服务器"
} else {
    Write-Host "❌ 配置文件不存在: $configPath"
}
