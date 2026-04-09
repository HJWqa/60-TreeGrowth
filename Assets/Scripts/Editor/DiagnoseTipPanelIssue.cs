using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// 诊断提示面板问题 - 找出为什么温度湿度按钮会一起隐藏
/// </summary>
public class DiagnoseTipPanelIssue : EditorWindow
{
    private Vector2 scrollPosition;
    
    [MenuItem("工具/诊断/提示面板隐藏问题")]
    public static void ShowWindow()
    {
        GetWindow<DiagnoseTipPanelIssue>("诊断提示面板");
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(10);
        
        EditorGUILayout.HelpBox(
            "这个工具会检查：\n" +
            "1. TipPanel和TipUIController的配置\n" +
            "2. uiToHide指向哪个对象\n" +
            "3. 温度湿度按钮的父对象是谁\n" +
            "4. 是否有多个脚本在控制同一个按钮",
            MessageType.Info
        );
        
        EditorGUILayout.Space(10);
        
        if (GUILayout.Button("开始诊断", GUILayout.Height(40)))
        {
            Diagnose();
        }
        
        EditorGUILayout.Space(10);
        
        if (GUILayout.Button("一键修复所有问题", GUILayout.Height(40)))
        {
            FixAllIssues();
        }
    }

    private void Diagnose()
    {
        Debug.Log("========== 开始诊断 ==========");
        
        // 查找Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("❌ 场景中没有Canvas");
            return;
        }
        
        Debug.Log($"✅ 找到Canvas: {canvas.name}");
        
        // 查找TipPanel对象
        GameObject tipPanelObj = GameObject.Find("TipPanel");
        if (tipPanelObj == null)
        {
            Debug.LogError("❌ 找不到名为'TipPanel'的对象");
        }
        else
        {
            Debug.Log($"✅ 找到TipPanel对象");
            Debug.Log($"   父对象: {tipPanelObj.transform.parent?.name ?? "无"}");
            Debug.Log($"   子对象数量: {tipPanelObj.transform.childCount}");
            
            // 列出所有子对象
            Debug.Log("   子对象列表:");
            for (int i = 0; i < tipPanelObj.transform.childCount; i++)
            {
                Transform child = tipPanelObj.transform.GetChild(i);
                Debug.Log($"      - {child.name}");
                
                // 检查是否是温度湿度按钮
                if (child.name.Contains("Temp") || child.name.Contains("Humid"))
                {
                    Debug.LogWarning($"      ⚠️ 警告：{child.name} 是温度湿度相关的，不应该在TipPanel下！");
                }
            }
        }
        
        // 检查TipPanel脚本
        TipPanel tipPanelScript = FindObjectOfType<TipPanel>();
        if (tipPanelScript != null)
        {
            Debug.Log($"✅ 找到TipPanel脚本，挂在: {tipPanelScript.gameObject.name}");
        }
        
        // 检查TipUIController脚本
        TreePlanQAQ.UI.TipUIController tipUIController = FindObjectOfType<TreePlanQAQ.UI.TipUIController>();
        if (tipUIController != null)
        {
            Debug.Log($"✅ 找到TipUIController脚本，挂在: {tipUIController.gameObject.name}");
            
            SerializedObject so = new SerializedObject(tipUIController);
            SerializedProperty uiToHideProp = so.FindProperty("uiToHide");
            GameObject uiToHide = uiToHideProp.objectReferenceValue as GameObject;
            
            if (uiToHide != null)
            {
                Debug.Log($"   uiToHide设置为: {uiToHide.name}");
                
                // 检查uiToHide是否包含温度湿度按钮
                if (uiToHide.name == "Canvas")
                {
                    Debug.LogError("   ❌ 错误：uiToHide设置为Canvas，会隐藏所有UI！");
                }
                else if (HasTemperatureHumidityButtons(uiToHide.transform))
                {
                    Debug.LogError("   ❌ 错误：uiToHide包含温度湿度按钮，会一起隐藏！");
                }
                else
                {
                    Debug.Log("   ✅ uiToHide设置正确");
                }
            }
            else
            {
                Debug.LogWarning("   ⚠️ uiToHide未设置，会在运行时自动查找TipPanel");
            }
        }
        
        // 查找温度湿度按钮
        Debug.Log("\n检查温度湿度按钮位置:");
        string[] buttonNames = { "TempUpButton", "TempDownButton", "HumidUpButton", "HumidDownButton" };
        foreach (string buttonName in buttonNames)
        {
            GameObject button = GameObject.Find(buttonName);
            if (button != null)
            {
                Debug.Log($"   {buttonName}:");
                Debug.Log($"      父对象: {button.transform.parent?.name ?? "无"}");
                
                if (button.transform.parent?.name == "TipPanel")
                {
                    Debug.LogError($"      ❌ 错误：{buttonName}在TipPanel下，会一起隐藏！");
                }
                else
                {
                    Debug.Log($"      ✅ 位置正确");
                }
            }
            else
            {
                Debug.LogWarning($"   ⚠️ 找不到{buttonName}");
            }
        }
        
        Debug.Log("\n========== 诊断完成 ==========");
        Debug.Log("请查看上面的日志，找出标记为❌的问题");
    }

    private bool HasTemperatureHumidityButtons(Transform parent)
    {
        // 递归检查是否包含温度湿度按钮
        if (parent.name.Contains("Temp") || parent.name.Contains("Humid"))
        {
            if (parent.name.Contains("Button") || parent.name.Contains("Label"))
            {
                return true;
            }
        }
        
        for (int i = 0; i < parent.childCount; i++)
        {
            if (HasTemperatureHumidityButtons(parent.GetChild(i)))
            {
                return true;
            }
        }
        
        return false;
    }

    private void FixAllIssues()
    {
        Debug.Log("========== 开始一键修复 ==========");
        
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            EditorUtility.DisplayDialog("错误", "场景中没有Canvas", "确定");
            return;
        }
        
        // 1. 确保TipPanel在Canvas下
        GameObject tipPanelObj = GameObject.Find("TipPanel");
        if (tipPanelObj != null && tipPanelObj.transform.parent != canvas.transform)
        {
            tipPanelObj.transform.SetParent(canvas.transform, true);
            Debug.Log("✅ TipPanel已移到Canvas下");
        }
        
        // 2. 创建或找到EnvironmentControlPanel
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
        
        // 3. 移动所有温度湿度按钮到EnvironmentControlPanel
        string[] elementNames = { 
            "TempUpButton", "TempDownButton", "HumidUpButton", "HumidDownButton",
            "TemperatureLabel", "HumidityLabel", "TempLabel", "HumidLabel"
        };
        
        int movedCount = 0;
        foreach (string elementName in elementNames)
        {
            GameObject element = GameObject.Find(elementName);
            if (element != null && element.transform.parent != envPanel)
            {
                element.transform.SetParent(envPanel, true);
                Debug.Log($"✅ {elementName}已移到EnvironmentControlPanel下");
                movedCount++;
            }
        }
        
        // 4. 修复TipUIController的uiToHide设置
        TreePlanQAQ.UI.TipUIController tipUIController = FindObjectOfType<TreePlanQAQ.UI.TipUIController>();
        if (tipUIController != null && tipPanelObj != null)
        {
            SerializedObject so = new SerializedObject(tipUIController);
            SerializedProperty uiToHideProp = so.FindProperty("uiToHide");
            
            if (uiToHideProp.objectReferenceValue != tipPanelObj)
            {
                uiToHideProp.objectReferenceValue = tipPanelObj;
                so.ApplyModifiedProperties();
                Debug.Log("✅ TipUIController的uiToHide已设置为TipPanel");
            }
        }
        
        // 5. 确保TipPanel只包含提示相关的UI
        if (tipPanelObj != null)
        {
            Debug.Log("\nTipPanel当前包含的子对象:");
            for (int i = tipPanelObj.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = tipPanelObj.transform.GetChild(i);
                Debug.Log($"   - {child.name}");
                
                // 如果是温度湿度相关的，移到EnvironmentControlPanel
                if ((child.name.Contains("Temp") || child.name.Contains("Humid")) && 
                    !child.name.Contains("提示") && !child.name.Contains("Tip"))
                {
                    child.SetParent(envPanel, true);
                    Debug.Log($"   ✅ {child.name}已从TipPanel移到EnvironmentControlPanel");
                    movedCount++;
                }
            }
        }
        
        EditorUtility.SetDirty(canvas.gameObject);
        
        Debug.Log($"\n========== 修复完成，移动了{movedCount}个元素 ==========");
        
        EditorUtility.DisplayDialog(
            "修复完成",
            $"已修复所有问题！\n\n" +
            $"• 移动了{movedCount}个元素到EnvironmentControlPanel\n" +
            $"• TipPanel和EnvironmentControlPanel现在是独立的\n" +
            $"• 点击'知道了'只会隐藏TipPanel\n\n" +
            $"请运行游戏测试！",
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
