using UnityEngine;
using UnityEditor;

namespace TreePlanQAQ.Editor
{
    /// <summary>
    /// 修复Input System错误
    /// </summary>
    public class FixInputSystemError
    {
        [MenuItem("TreePlanQAQ/Fix/Fix Input System Error (Critical!)")]
        public static void FixInputSystem()
        {
            EditorUtility.DisplayDialog(
                "修复Input System错误",
                "请按照以下步骤操作：\n\n" +
                "1. Edit → Project Settings\n" +
                "2. 点击左侧 Player\n" +
                "3. 展开 Other Settings\n" +
                "4. 找到 Active Input Handling\n" +
                "5. 选择 'Input Manager (Old)' 或 'Both'\n" +
                "6. 关闭Project Settings窗口\n" +
                "7. 重启Unity编辑器\n\n" +
                "这会解决 InvalidOperationException 错误",
                "我知道了"
            );
            
            // 尝试自动设置（可能需要Unity重启）
            Debug.Log("=== Input System 修复指南 ===");
            Debug.Log("1. Edit → Project Settings → Player");
            Debug.Log("2. Other Settings → Active Input Handling");
            Debug.Log("3. 选择 'Input Manager (Old)'");
            Debug.Log("4. 重启Unity");
            Debug.Log("=============================");
        }
        
        [MenuItem("TreePlanQAQ/Fix/Disable Error Pause")]
        public static void DisableErrorPause()
        {
            // 禁用错误时自动暂停
            EditorPrefs.SetBool("ErrorPause", false);
            
            EditorUtility.DisplayDialog(
                "已禁用错误暂停",
                "Unity现在不会在发生错误时自动暂停。\n\n" +
                "但你仍然需要修复Input System错误！\n\n" +
                "请运行: TreePlanQAQ > Fix > Fix Input System Error",
                "确定"
            );
            
            Debug.Log("✅ 已禁用错误时自动暂停");
        }
    }
}
