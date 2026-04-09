using UnityEngine;
using UnityEditor;
using System.IO;

namespace TreePlanQAQ.Editor
{
    /// <summary>
    /// 修复Unity MCP的Node.js路径问题
    /// </summary>
    [InitializeOnLoad]
    public class FixUnityMcpNodePath
    {
        static FixUnityMcpNodePath()
        {
            // 在编辑器启动时静默设置Node.js路径到环境变量
            SetNodePathSilent();
        }
        
        /// <summary>
        /// 静默设置Node.js路径（不显示弹窗）
        /// </summary>
        private static void SetNodePathSilent()
        {
            string nodePath = @"C:\Program Files\nodejs";
            
            if (!System.IO.Directory.Exists(nodePath))
                return;
            
            string currentPath = System.Environment.GetEnvironmentVariable("PATH", System.EnvironmentVariableTarget.Process);
            
            if (!currentPath.Contains(nodePath))
            {
                string newPath = nodePath + ";" + currentPath;
                System.Environment.SetEnvironmentVariable("PATH", newPath, System.EnvironmentVariableTarget.Process);
            }
        }
        
        [MenuItem("TreePlanQAQ/Fix Unity MCP Node.js Path")]
        public static void SetNodePath()
        {
            string nodePath = @"C:\Program Files\nodejs";
            
            // 检查Node.js是否存在
            if (!Directory.Exists(nodePath))
            {
                Debug.LogError($"Node.js not found at: {nodePath}");
                EditorUtility.DisplayDialog(
                    "Node.js Not Found",
                    $"Node.js not found at:\n{nodePath}\n\nPlease install Node.js 18+ from https://nodejs.org",
                    "OK"
                );
                return;
            }
            
            // 获取当前PATH环境变量
            string currentPath = System.Environment.GetEnvironmentVariable("PATH", System.EnvironmentVariableTarget.Process);
            
            // 检查Node.js路径是否已在PATH中
            if (!currentPath.Contains(nodePath))
            {
                // 添加Node.js路径到PATH
                string newPath = nodePath + ";" + currentPath;
                System.Environment.SetEnvironmentVariable("PATH", newPath, System.EnvironmentVariableTarget.Process);
                Debug.Log($"✅ Added Node.js to PATH: {nodePath}");
            }
            else
            {
                Debug.Log($"✅ Node.js already in PATH: {nodePath}");
            }
            
            // 验证node命令是否可用
            try
            {
                var process = new System.Diagnostics.Process();
                process.StartInfo.FileName = "node";
                process.StartInfo.Arguments = "--version";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                
                if (process.ExitCode == 0)
                {
                    Debug.Log($"✅ Node.js is working: {output.Trim()}");
                    EditorUtility.DisplayDialog(
                        "Node.js Path Fixed",
                        $"Node.js is now accessible!\nVersion: {output.Trim()}\n\nPlease restart Unity for Unity MCP to detect Node.js.",
                        "OK"
                    );
                }
                else
                {
                    Debug.LogError("❌ Node.js command failed");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"❌ Failed to run node command: {e.Message}");
            }
        }
        
        [MenuItem("TreePlanQAQ/Restart Unity MCP Server")]
        public static void RestartMcpServer()
        {
            Debug.Log("请在Unity MCP窗口中手动重启服务器");
            EditorUtility.DisplayDialog(
                "Restart Unity MCP Server",
                "请按以下步骤操作：\n\n" +
                "1. 打开菜单: Window > Unity MCP > Server\n" +
                "2. 点击 'Stop Server' 按钮\n" +
                "3. 等待几秒\n" +
                "4. 点击 'Start Server' 按钮\n\n" +
                "如果仍然无法启动，请重启Unity编辑器。",
                "OK"
            );
        }
    }
}
