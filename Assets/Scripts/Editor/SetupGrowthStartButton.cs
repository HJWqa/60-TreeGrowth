using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// 设置生长开始按钮的编辑器工具
/// </summary>
public class SetupGrowthStartButton : EditorWindow
{
    [MenuItem("橘子树/UI设置/创建生长开始按钮")]
    public static void ShowWindow()
    {
        GetWindow<SetupGrowthStartButton>("创建生长开始按钮");
    }

    private void OnGUI()
    {
        EditorGUILayout.HelpBox(
            "这个工具会在提示框（TipPanel）底下创建一个独立的开始按钮\n" +
            "用于控制植物生长的启动和暂停", 
            MessageType.Info
        );

        EditorGUILayout.Space();

        if (GUILayout.Button("创建开始按钮", GUILayout.Height(40)))
        {
            CreateStartButton();
        }
    }

    private static void CreateStartButton()
    {
        // 查找 Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            EditorUtility.DisplayDialog("错误", "场景中没有找到 Canvas！", "确定");
            return;
        }

        // 查找 TipPanel
        Transform tipPanel = FindTransformByName(canvas.transform, "TipPanel");
        if (tipPanel == null)
        {
            EditorUtility.DisplayDialog("错误", "找不到 TipPanel！请先创建提示面板", "确定");
            return;
        }

        // 检查是否已存在
        Transform existingButton = FindTransformByName(canvas.transform, "GrowthStartButton");
        if (existingButton != null)
        {
            bool overwrite = EditorUtility.DisplayDialog(
                "已存在", 
                "GrowthStartButton 已存在，是否重新创建？", 
                "是", "否"
            );
            
            if (overwrite)
            {
                DestroyImmediate(existingButton.gameObject);
            }
            else
            {
                return;
            }
        }

        // 创建开始按钮
        GameObject startButtonObj = new GameObject("GrowthStartButton");
        startButtonObj.transform.SetParent(canvas.transform, false);

        // 添加 RectTransform
        RectTransform rectTransform = startButtonObj.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        
        // 获取 TipPanel 的位置，放在它下面
        RectTransform tipRect = tipPanel.GetComponent<RectTransform>();
        float tipBottom = tipRect.anchoredPosition.y - tipRect.sizeDelta.y / 2;
        
        rectTransform.anchoredPosition = new Vector2(0, tipBottom - 80); // 在提示框下方 80 像素
        rectTransform.sizeDelta = new Vector2(200, 60);

        // 添加 Image 组件
        Image image = startButtonObj.AddComponent<Image>();
        image.color = new Color(0.2f, 0.8f, 0.3f); // 绿色

        // 添加 Button 组件
        Button button = startButtonObj.AddComponent<Button>();
        
        // 设置按钮颜色
        ColorBlock colors = button.colors;
        colors.normalColor = new Color(0.2f, 0.8f, 0.3f);
        colors.highlightedColor = new Color(0.3f, 0.9f, 0.4f);
        colors.pressedColor = new Color(0.15f, 0.6f, 0.25f);
        button.colors = colors;

        // 创建按钮文本
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(startButtonObj.transform, false);

        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        textRect.anchoredPosition = Vector2.zero;

        Text text = textObj.AddComponent<Text>();
        text.text = "开始生长";
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 24;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;

        // 添加 GrowthStartController 组件
        GrowthStartController controller = startButtonObj.AddComponent<GrowthStartController>();
        
        // 使用反射设置私有字段
        var controllerType = typeof(GrowthStartController);
        var startButtonField = controllerType.GetField("startButton", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var buttonTextField = controllerType.GetField("buttonText", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (startButtonField != null)
            startButtonField.SetValue(controller, button);
        if (buttonTextField != null)
            buttonTextField.SetValue(controller, text);

        // 标记场景为已修改
        EditorUtility.SetDirty(canvas.gameObject);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
        );

        Debug.Log("✅ 生长开始按钮创建成功！");
        Debug.Log($"位置: Canvas/GrowthStartButton");
        Debug.Log($"位于 TipPanel 下方");
        
        // 选中创建的按钮
        Selection.activeGameObject = startButtonObj;
        
        EditorUtility.DisplayDialog(
            "创建成功", 
            "生长开始按钮已创建！\n\n" +
            "位置：Canvas/GrowthStartButton\n" +
            "功能：\n" +
            "- 第一次点击：开始生长\n" +
            "- 之后点击：暂停/继续生长\n\n" +
            "按钮已自动配置好所有引用", 
            "确定"
        );
    }

    private static Transform FindTransformByName(Transform parent, string name)
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
