using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// 紧急修复 - 所有UI都消失的问题
/// </summary>
public class EmergencyFixUIHiding : EditorWindow
{
    [MenuItem("工具/紧急修复/所有UI都消失问题 ⚠️")]
    public static void ShowWindow()
    {
        GetWindow<EmergencyFixUIHiding>("紧急修复");
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(10);
        
        EditorGUILayout.HelpBox(
            "问题：点击'知道了'后所有UI都消失\n\n" +
            "原因：TipUIController的uiToHide被设置成了Canvas\n\n" +
            "这个工具会：\n" +
            "1. 找到TipUIController\n" +
            "2. 将uiToHide改为只隐藏TipPanel\n" +
            "3. 确保温度湿度按钮独立存在",
            MessageType.Warning
        );
        
        EditorGUILayout.Space(10);
        
        if (GUILayout.Button("立即修复", GUILayout.Height(50)))
        {
            EmergencyFix();
        }
        
        EditorGUILayout.Space(10);
        
        if (GUILayout.Button("查看当前配置", GUILayout.Height(30)))
        {
            CheckCurrentConfig();
        }
    }

    private void CheckCurrentConfig()
    {
        Debug.Log("========== 检查当前配置 ==========");
        
        // 查找TipUIController
        TreePlanQAQ.UI.TipUIController tipUIController = FindObjectOfType<TreePlanQAQ.UI.TipUIController>();
        
        if (tipUIController == null)
        {
            Debug.LogWarning("⚠️ 场景中没有TipUIController组件");
            return;
        }
        
        Debug.Log($"✅ 找到TipUIController，挂在: {tipUIController.gameObject.name}");
        
        SerializedObject so = new SerializedObject(tipUIController);
        SerializedProperty uiToHideProp = so.FindProperty("uiToHide");
        GameObject uiToHide = uiToHideProp.objectReferenceValue as GameObject;
        
        if (uiToHide == null)
        {
            Debug.LogWarning("⚠️ uiToHide未设置");
        }
        else
        {
            Debug.Log($"当前uiToHide设置为: {uiToHide.name}");
            
            if (uiToHide.GetComponent<Canvas>() != null)
            {
                Debug.LogError("❌ 错误：uiToHide是Canvas！这会隐藏所有UI！");
            }
            else if (uiToHide.name == "TipPanel")
            {
                Debug.Log("✅ 设置正确：uiToHide是TipPanel");
            }
            else
            {
                Debug.LogWarning($"⚠️ uiToHide是{uiToHide.name}，请确认这是否正确");
            }
        }
        
        Debug.Log("=============================");
    }

    private void EmergencyFix()
    {
        Debug.Log("========== 开始紧急修复 ==========");
        
        // 查找Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            EditorUtility.DisplayDialog("错误", "场景中没有Canvas", "确定");
            return;
        }
        
        Debug.Log($"✅ 找到Canvas: {canvas.name}");
        
        // 查找或创建TipPanel
        GameObject tipPanelObj = GameObject.Find("TipPanel");
        if (tipPanelObj == null)
        {
            // 创建TipPanel
            tipPanelObj = new GameObject("TipPanel");
            tipPanelObj.transform.SetParent(canvas.transform, false);
            
            RectTransform rectTransform = tipPanelObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            
            tipPanelObj.AddComponent<CanvasGroup>();
            
            Debug.Log("✅ 创建了TipPanel");
        }
        else
        {
            Debug.Log($"✅ 找到TipPanel");
            
            // 确保TipPanel在Canvas下
            if (tipPanelObj.transform.parent != canvas.transform)
            {
                tipPanelObj.transform.SetParent(canvas.transform, true);
                Debug.Log("✅ TipPanel已移到Canvas下");
            }
        }
        
        // 查找或创建EnvironmentControlPanel
        Transform envPanel = FindTransformByName(canvas.transform, "EnvironmentControlPanel");
        if (envPanel == null)
        {
            GameObject envPanelObj = new GameObject("EnvironmentControlPanel");
            envPanel = envPanelObj.transform;
            envPanel.SetParent(canvas.transform, false);
            
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
            envPanel.SetParent(canvas.transform, true);
            Debug.Log("✅ EnvironmentControlPanel已移到Canvas下");
        }
        
        // 移动温度湿度按钮到EnvironmentControlPanel
        string[] buttonNames = { 
            "TempUpButton", "TempDownButton", "HumidUpButton", "HumidDownButton",
            "TemperatureLabel", "HumidityLabel", "TempLabel", "HumidLabel"
        };
        
        int movedCount = 0;
        foreach (string buttonName in buttonNames)
        {
            GameObject button = GameObject.Find(buttonName);
            if (button != null)
            {
                // 如果按钮在TipPanel下或Canvas下，移到EnvironmentControlPanel
                if (button.transform.parent == tipPanelObj.transform || 
                    button.transform.parent == canvas.transform)
                {
                    button.transform.SetParent(envPanel, true);
                    Debug.Log($"✅ {buttonName}已移到EnvironmentControlPanel下");
                    movedCount++;
                }
            }
        }
        
        // 修复TipUIController
        TreePlanQAQ.UI.TipUIController tipUIController = FindObjectOfType<TreePlanQAQ.UI.TipUIController>();
        if (tipUIController != null)
        {
            SerializedObject so = new SerializedObject(tipUIController);
            SerializedProperty uiToHideProp = so.FindProperty("uiToHide");
            
            GameObject currentUiToHide = uiToHideProp.objectReferenceValue as GameObject;
            
            // 如果uiToHide是Canvas或不是TipPanel，修复它
            if (currentUiToHide == null || 
                currentUiToHide.GetComponent<Canvas>() != null || 
                currentUiToHide != tipPanelObj)
            {
                uiToHideProp.objectReferenceValue = tipPanelObj;
                so.ApplyModifiedProperties();
                Debug.Log("✅ TipUIController的uiToHide已修复为TipPanel");
            }
            else
            {
                Debug.Log("✅ TipUIController的uiToHide已经正确设置");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ 场景中没有TipUIController组件");
        }
        
        // 确保EnvironmentControlPanel始终激活
        if (envPanel != null)
        {
            envPanel.gameObject.SetActive(true);
        }
        
        EditorUtility.SetDirty(canvas.gameObject);
        
        Debug.Log($"\n========== 修复完成 ==========");
        Debug.Log("修复内容：");
        Debug.Log($"• 移动了{movedCount}个温度湿度元素");
        Debug.Log("• TipUIController的uiToHide已设置为TipPanel");
        Debug.Log("• TipPanel和EnvironmentControlPanel已分离");
        Debug.Log("=============================");
        
        EditorUtility.DisplayDialog(
            "修复完成",
            "已修复所有UI消失的问题！\n\n" +
            "修复内容：\n" +
            $"• 移动了{movedCount}个温度湿度元素到独立面板\n" +
            "• TipUIController现在只会隐藏TipPanel\n" +
            "• 温度湿度按钮会保持显示\n\n" +
            "请运行游戏测试！",
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
