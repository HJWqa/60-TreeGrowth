using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// 修复提示面板和环境控制面板的结构
/// 确保它们是两个独立的系统
/// </summary>
public class FixTipPanelStructure : EditorWindow
{
    [MenuItem("工具/修复UI结构/分离提示面板和环境控制面板")]
    public static void ShowWindow()
    {
        GetWindow<FixTipPanelStructure>("分离UI面板");
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(10);
        
        EditorGUILayout.HelpBox(
            "这个工具会确保：\n" +
            "1. TipPanel（提示面板）是独立的\n" +
            "2. EnvironmentControlPanel（环境控制面板）是独立的\n" +
            "3. 点击'知道了'只会隐藏TipPanel\n" +
            "4. 温度湿度按钮会一直显示",
            MessageType.Info
        );
        
        EditorGUILayout.Space(10);
        
        EditorGUILayout.HelpBox(
            "目标结构：\n" +
            "Canvas\n" +
            "├── TipPanel（提示面板，点击后消失）\n" +
            "│   ├── 提示文本\n" +
            "│   └── 知道了按钮\n" +
            "└── EnvironmentControlPanel（环境控制，常驻）\n" +
            "    ├── 温度标签和按钮\n" +
            "    └── 湿度标签和按钮",
            MessageType.None
        );
        
        EditorGUILayout.Space(10);
        
        if (GUILayout.Button("执行修复", GUILayout.Height(40)))
        {
            FixUIStructure();
        }
        
        EditorGUILayout.Space(10);
        
        if (GUILayout.Button("检查当前结构", GUILayout.Height(30)))
        {
            CheckCurrentStructure();
        }
    }

    private void FixUIStructure()
    {
        Debug.Log("========== 开始修复UI结构 ==========");
        
        // 查找Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            EditorUtility.DisplayDialog("错误", "场景中没有找到Canvas！", "确定");
            return;
        }
        
        // 查找TipPanel
        Transform tipPanel = FindTransformByName(canvas.transform, "TipPanel");
        if (tipPanel == null)
        {
            Debug.LogWarning("⚠️ 找不到TipPanel");
        }
        else
        {
            // 确保TipPanel直接在Canvas下
            if (tipPanel.parent != canvas.transform)
            {
                tipPanel.SetParent(canvas.transform, true);
                Debug.Log("✅ TipPanel已移到Canvas下");
            }
            
            // 确保TipPanel有CanvasGroup组件（用于淡出）
            if (tipPanel.GetComponent<CanvasGroup>() == null)
            {
                tipPanel.gameObject.AddComponent<CanvasGroup>();
                Debug.Log("✅ 为TipPanel添加了CanvasGroup组件");
            }
            
            // 检查TipUIController组件
            var tipController = tipPanel.GetComponent<TreePlanQAQ.UI.TipUIController>();
            if (tipController == null)
            {
                tipController = tipPanel.gameObject.AddComponent<TreePlanQAQ.UI.TipUIController>();
                Debug.Log("✅ 为TipPanel添加了TipUIController组件");
            }
            
            // 确保uiToHide设置为TipPanel自己
            SerializedObject so = new SerializedObject(tipController);
            SerializedProperty uiToHideProp = so.FindProperty("uiToHide");
            if (uiToHideProp.objectReferenceValue != tipPanel.gameObject)
            {
                uiToHideProp.objectReferenceValue = tipPanel.gameObject;
                so.ApplyModifiedProperties();
                Debug.Log("✅ TipUIController的uiToHide已设置为TipPanel自己");
            }
        }
        
        // 查找EnvironmentControlPanel
        Transform envPanel = FindTransformByName(canvas.transform, "EnvironmentControlPanel");
        if (envPanel == null)
        {
            Debug.LogWarning("⚠️ 找不到EnvironmentControlPanel");
        }
        else
        {
            // 确保EnvironmentControlPanel直接在Canvas下
            if (envPanel.parent != canvas.transform)
            {
                envPanel.SetParent(canvas.transform, true);
                Debug.Log("✅ EnvironmentControlPanel已移到Canvas下");
            }
            
            // 确保EnvironmentControlPanel始终激活
            if (!envPanel.gameObject.activeSelf)
            {
                envPanel.gameObject.SetActive(true);
                Debug.Log("✅ EnvironmentControlPanel已激活");
            }
        }
        
        // 确保温度湿度按钮在EnvironmentControlPanel下
        string[] buttonNames = { "TempUpButton", "TempDownButton", "HumidUpButton", "HumidDownButton" };
        foreach (string buttonName in buttonNames)
        {
            Transform button = FindTransformByName(canvas.transform, buttonName);
            if (button != null && envPanel != null)
            {
                // 如果按钮不在EnvironmentControlPanel下，移动它
                if (button.parent != envPanel)
                {
                    button.SetParent(envPanel, true);
                    Debug.Log($"✅ {buttonName}已移到EnvironmentControlPanel下");
                }
            }
        }
        
        EditorUtility.SetDirty(canvas.gameObject);
        
        Debug.Log("========== UI结构修复完成 ==========");
        Debug.Log("Canvas");
        Debug.Log("├── TipPanel（提示面板）");
        Debug.Log("└── EnvironmentControlPanel（环境控制）");
        Debug.Log("=============================");
        
        EditorUtility.DisplayDialog(
            "修复完成",
            "UI结构已修复！\n\n" +
            "• TipPanel和EnvironmentControlPanel现在是独立的\n" +
            "• 点击'知道了'只会隐藏TipPanel\n" +
            "• 温度湿度按钮会一直显示",
            "确定"
        );
    }

    private void CheckCurrentStructure()
    {
        Debug.Log("========== 检查当前UI结构 ==========");
        
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("❌ 场景中没有Canvas");
            return;
        }
        
        Debug.Log($"Canvas: {canvas.name}");
        
        Transform tipPanel = FindTransformByName(canvas.transform, "TipPanel");
        if (tipPanel != null)
        {
            Debug.Log($"  ├── TipPanel (父对象: {tipPanel.parent.name})");
            Debug.Log($"      激活状态: {tipPanel.gameObject.activeSelf}");
            Debug.Log($"      有CanvasGroup: {tipPanel.GetComponent<CanvasGroup>() != null}");
            
            var tipController = tipPanel.GetComponent<TreePlanQAQ.UI.TipUIController>();
            if (tipController != null)
            {
                SerializedObject so = new SerializedObject(tipController);
                SerializedProperty uiToHideProp = so.FindProperty("uiToHide");
                GameObject uiToHide = uiToHideProp.objectReferenceValue as GameObject;
                Debug.Log($"      uiToHide设置为: {(uiToHide != null ? uiToHide.name : "null")}");
            }
        }
        else
        {
            Debug.LogWarning("  ⚠️ 找不到TipPanel");
        }
        
        Transform envPanel = FindTransformByName(canvas.transform, "EnvironmentControlPanel");
        if (envPanel != null)
        {
            Debug.Log($"  └── EnvironmentControlPanel (父对象: {envPanel.parent.name})");
            Debug.Log($"      激活状态: {envPanel.gameObject.activeSelf}");
        }
        else
        {
            Debug.LogWarning("  ⚠️ 找不到EnvironmentControlPanel");
        }
        
        Debug.Log("=============================");
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
