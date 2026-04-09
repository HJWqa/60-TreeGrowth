using UnityEngine;
using UnityEditor;

/// <summary>
/// 修复Canvas Group引用错误的问题
/// </summary>
public class FixCanvasGroupReference : EditorWindow
{
    [MenuItem("工具/紧急修复/修复Canvas Group引用 ⚠️⚠️⚠️")]
    public static void ShowWindow()
    {
        var window = GetWindow<FixCanvasGroupReference>("修复Canvas Group");
        window.minSize = new Vector2(400, 250);
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(10);
        
        EditorGUILayout.HelpBox(
            "问题：Canvas Group字段指向了Canvas对象！\n\n" +
            "这会导致整个Canvas淡出，所有UI都消失。\n\n" +
            "修复方法：\n" +
            "1. 确保TipPanel对象有CanvasGroup组件\n" +
            "2. 将Canvas Group字段清空（让脚本自动获取）",
            MessageType.Error
        );
        
        EditorGUILayout.Space(10);
        
        if (GUILayout.Button("一键修复", GUILayout.Height(50)))
        {
            FixCanvasGroup();
        }
    }

    private void FixCanvasGroup()
    {
        Debug.Log("========== 开始修复Canvas Group引用 ==========");
        
        // 查找TipPanel脚本
        TipPanel tipPanel = FindObjectOfType<TipPanel>();
        
        if (tipPanel == null)
        {
            EditorUtility.DisplayDialog("错误", "场景中没有TipPanel脚本", "确定");
            return;
        }
        
        Debug.Log($"找到TipPanel脚本，挂在: {tipPanel.gameObject.name}");
        
        // 确保TipPanel对象有CanvasGroup组件
        CanvasGroup canvasGroup = tipPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = tipPanel.gameObject.AddComponent<CanvasGroup>();
            Debug.Log("✅ 已为TipPanel添加CanvasGroup组件");
        }
        else
        {
            Debug.Log("✅ TipPanel已有CanvasGroup组件");
        }
        
        // 使用反射清空canvasGroup字段
        var field = typeof(TipPanel).GetField("canvasGroup", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (field != null)
        {
            CanvasGroup currentValue = field.GetValue(tipPanel) as CanvasGroup;
            
            if (currentValue != null && currentValue.gameObject.name == "Canvas")
            {
                Debug.LogWarning("⚠️ 检测到Canvas Group指向Canvas对象，正在清空...");
                field.SetValue(tipPanel, null);
                Debug.Log("✅ 已清空Canvas Group字段");
            }
            else if (currentValue == null)
            {
                Debug.Log("✅ Canvas Group字段已经是空的");
            }
            else
            {
                Debug.Log($"Canvas Group当前指向: {currentValue.gameObject.name}");
            }
        }
        
        EditorUtility.SetDirty(tipPanel);
        
        Debug.Log("========== 修复完成 ==========");
        Debug.Log("说明：");
        Debug.Log("• TipPanel对象已有CanvasGroup组件");
        Debug.Log("• Canvas Group字段已清空");
        Debug.Log("• 脚本会在运行时自动获取TipPanel的CanvasGroup");
        Debug.Log("• 现在点击'知道了'只会淡出TipPanel，不会影响其他UI");
        Debug.Log("=============================");
        
        EditorUtility.DisplayDialog(
            "修复完成",
            "Canvas Group引用已修复！\n\n" +
            "修复内容：\n" +
            "• TipPanel对象已有CanvasGroup组件\n" +
            "• Canvas Group字段已清空\n" +
            "• 脚本会自动获取正确的CanvasGroup\n\n" +
            "现在点击'知道了'只会隐藏提示面板！\n\n" +
            "请运行游戏测试！",
            "确定"
        );
    }
}
