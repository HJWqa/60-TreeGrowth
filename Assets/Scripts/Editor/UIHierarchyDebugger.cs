using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Text;

/// <summary>
/// UI层级调试工具 - 显示完整的UI层级结构
/// </summary>
public class UIHierarchyDebugger : EditorWindow
{
    private Vector2 scrollPosition;
    private StringBuilder hierarchyText = new StringBuilder();

    [MenuItem("工具/调试/显示UI层级结构")]
    public static void ShowWindow()
    {
        GetWindow<UIHierarchyDebugger>("UI层级调试");
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(10);
        
        EditorGUILayout.HelpBox(
            "这个工具会显示场景中所有Canvas的完整层级结构，\n" +
            "帮助你找出为什么温度湿度按钮会跟着提示面板一起隐藏。",
            MessageType.Info
        );
        
        EditorGUILayout.Space(10);
        
        if (GUILayout.Button("刷新层级结构", GUILayout.Height(40)))
        {
            RefreshHierarchy();
        }
        
        EditorGUILayout.Space(10);
        
        if (GUILayout.Button("自动修复（将按钮移出TipPanel）", GUILayout.Height(40)))
        {
            AutoFix();
        }
        
        EditorGUILayout.Space(10);
        
        // 显示层级结构
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        EditorGUILayout.TextArea(hierarchyText.ToString(), GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();
    }

    private void RefreshHierarchy()
    {
        hierarchyText.Clear();
        hierarchyText.AppendLine("========== UI层级结构 ==========\n");
        
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        
        if (canvases.Length == 0)
        {
            hierarchyText.AppendLine("❌ 场景中没有找到Canvas");
            return;
        }
        
        foreach (Canvas canvas in canvases)
        {
            hierarchyText.AppendLine($"Canvas: {canvas.name}");
            PrintHierarchy(canvas.transform, 1);
            hierarchyText.AppendLine();
        }
        
        hierarchyText.AppendLine("=============================");
        
        Repaint();
    }

    private void PrintHierarchy(Transform parent, int level)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            
            // 缩进
            string indent = new string(' ', level * 2);
            string prefix = level == 1 ? "├── " : "│   " + (i == parent.childCount - 1 ? "└── " : "├── ");
            
            // 获取对象信息
            string info = $"{indent}{prefix}{child.name}";
            
            // 标记重要对象
            if (child.name.Contains("TipPanel"))
            {
                info += " ⚠️ [提示面板]";
            }
            else if (child.name.Contains("EnvironmentControlPanel"))
            {
                info += " ✅ [环境控制面板]";
            }
            else if (child.name.Contains("Button") && (child.name.Contains("Temp") || child.name.Contains("Humid")))
            {
                info += " 🔴 [温度湿度按钮]";
                
                // 检查是否在TipPanel下
                if (IsChildOf(child, "TipPanel"))
                {
                    info += " ❌ 错误：在TipPanel下！";
                }
            }
            
            // 显示激活状态
            if (!child.gameObject.activeSelf)
            {
                info += " [未激活]";
            }
            
            hierarchyText.AppendLine(info);
            
            // 递归显示子对象
            if (child.childCount > 0)
            {
                PrintHierarchy(child, level + 1);
            }
        }
    }

    private bool IsChildOf(Transform child, string parentName)
    {
        Transform current = child.parent;
        while (current != null)
        {
            if (current.name == parentName)
                return true;
            current = current.parent;
        }
        return false;
    }

    private void AutoFix()
    {
        Debug.Log("========== 开始自动修复 ==========");
        
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            EditorUtility.DisplayDialog("错误", "场景中没有找到Canvas！", "确定");
            return;
        }
        
        // 查找或创建EnvironmentControlPanel
        Transform envPanel = FindTransformByName(canvas.transform, "EnvironmentControlPanel");
        if (envPanel == null)
        {
            // 创建EnvironmentControlPanel
            GameObject envPanelObj = new GameObject("EnvironmentControlPanel");
            envPanel = envPanelObj.transform;
            envPanel.SetParent(canvas.transform, false);
            
            // 添加RectTransform
            RectTransform rectTransform = envPanelObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(1, 0.5f);
            rectTransform.anchorMax = new Vector2(1, 0.5f);
            rectTransform.pivot = new Vector2(1, 0.5f);
            rectTransform.anchoredPosition = new Vector2(-50, 0);
            rectTransform.sizeDelta = new Vector2(200, 400);
            
            Debug.Log("✅ 创建了EnvironmentControlPanel");
        }
        else if (envPanel.parent != canvas.transform)
        {
            // 移动到Canvas下
            envPanel.SetParent(canvas.transform, true);
            Debug.Log("✅ EnvironmentControlPanel已移到Canvas下");
        }
        
        // 查找TipPanel
        Transform tipPanel = FindTransformByName(canvas.transform, "TipPanel");
        if (tipPanel != null && tipPanel.parent != canvas.transform)
        {
            tipPanel.SetParent(canvas.transform, true);
            Debug.Log("✅ TipPanel已移到Canvas下");
        }
        
        // 移动所有温度湿度相关的按钮和标签到EnvironmentControlPanel
        string[] elementNames = { 
            "TempUpButton", "TempDownButton", "HumidUpButton", "HumidDownButton",
            "TemperatureLabel", "HumidityLabel", "TempLabel", "HumidLabel"
        };
        
        int movedCount = 0;
        foreach (string elementName in elementNames)
        {
            Transform element = FindTransformByName(canvas.transform, elementName);
            if (element != null && element.parent != envPanel)
            {
                element.SetParent(envPanel, true);
                Debug.Log($"✅ {elementName}已移到EnvironmentControlPanel下");
                movedCount++;
            }
        }
        
        // 确保EnvironmentControlPanel始终激活
        if (envPanel != null)
        {
            envPanel.gameObject.SetActive(true);
        }
        
        EditorUtility.SetDirty(canvas.gameObject);
        
        Debug.Log($"========== 修复完成，移动了{movedCount}个元素 ==========");
        
        // 刷新显示
        RefreshHierarchy();
        
        EditorUtility.DisplayDialog(
            "修复完成",
            $"已将{movedCount}个温度湿度相关元素移到EnvironmentControlPanel下。\n\n" +
            "现在点击'知道了'按钮只会隐藏TipPanel，\n" +
            "温度湿度按钮会保持显示。",
            "确定"
        );
    }

    private Transform FindTransformByName(Transform parent, string name)
    {
        if (parent.name == name)
            return parent;
        
        foreach (Transform child in parent)
        {
            Transform result = FindTransformByName(child, name);
            if (result != null)
                return result;
        }
        
        return null;
    }
}
