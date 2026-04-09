using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// 设置右上角重置按钮的编辑器工具
/// </summary>
public class SetupResetButton : EditorWindow
{
    [MenuItem("橘子树/UI设置/创建右上角重置按钮")]
    public static void ShowWindow()
    {
        GetWindow<SetupResetButton>("创建重置按钮");
    }

    private void OnGUI()
    {
        EditorGUILayout.HelpBox(
            "这个工具会在右上角创建一个重置按钮\n" +
            "点击后会显示确认对话框，确认后重新加载场景", 
            MessageType.Info
        );

        EditorGUILayout.Space();

        if (GUILayout.Button("创建重置按钮", GUILayout.Height(40)))
        {
            CreateResetButton();
        }
    }

    private static void CreateResetButton()
    {
        // 查找 Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            EditorUtility.DisplayDialog("错误", "场景中没有找到 Canvas！", "确定");
            return;
        }

        // 检查是否已存在
        Transform existing = canvas.transform.Find("ResetButton");
        if (existing != null)
        {
            bool overwrite = EditorUtility.DisplayDialog(
                "已存在", 
                "重置按钮已存在，是否重新创建？", 
                "是", "否"
            );
            
            if (overwrite)
            {
                DestroyImmediate(existing.gameObject);
            }
            else
            {
                return;
            }
        }

        // 创建重置按钮
        GameObject resetBtn = CreateResetButtonObject(canvas.transform);
        
        // 创建确认对话框
        GameObject confirmPanel = CreateConfirmPanel(canvas.transform);

        // 添加控制器
        ResetButtonController controller = resetBtn.AddComponent<ResetButtonController>();
        
        // 获取按钮组件
        Button resetButton = resetBtn.GetComponent<Button>();
        Button yesButton = confirmPanel.transform.Find("YesButton").GetComponent<Button>();
        Button noButton = confirmPanel.transform.Find("NoButton").GetComponent<Button>();
        
        // 使用反射设置字段
        var controllerType = typeof(ResetButtonController);
        SetField(controller, controllerType, "resetButton", resetButton);
        SetField(controller, controllerType, "confirmPanel", confirmPanel);
        SetField(controller, controllerType, "confirmYesButton", yesButton);
        SetField(controller, controllerType, "confirmNoButton", noButton);

        // 标记场景为已修改
        EditorUtility.SetDirty(canvas.gameObject);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
        );

        Debug.Log("✅ 重置按钮创建成功！");
        Selection.activeGameObject = resetBtn;
        
        EditorUtility.DisplayDialog(
            "创建成功", 
            "右上角重置按钮已创建！\n\n" +
            "功能：\n" +
            "- 点击显示确认对话框\n" +
            "- 确认后重新加载场景\n" +
            "- 所有数据恢复初始状态", 
            "确定"
        );
    }

    private static GameObject CreateResetButtonObject(Transform parent)
    {
        GameObject btnObj = new GameObject("ResetButton");
        btnObj.transform.SetParent(parent, false);

        RectTransform rect = btnObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(1, 1);
        rect.anchoredPosition = new Vector2(-20, -20);
        rect.sizeDelta = new Vector2(50, 50);

        // 创建圆形背景
        Image image = btnObj.AddComponent<Image>();
        image.color = new Color(0.5f, 0.5f, 0.5f); // 灰色圆圈
        
        Texture2D circleTexture = CreateCircleTexture(50);
        Sprite circleSprite = Sprite.Create(
            circleTexture,
            new Rect(0, 0, circleTexture.width, circleTexture.height),
            new Vector2(0.5f, 0.5f)
        );
        image.sprite = circleSprite;

        Button button = btnObj.AddComponent<Button>();
        
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = new Color(0.9f, 0.9f, 0.9f);
        colors.pressedColor = new Color(0.7f, 0.7f, 0.7f);
        button.colors = colors;

        // 创建图标文本
        GameObject textObj = new GameObject("Icon");
        textObj.transform.SetParent(btnObj.transform, false);

        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;

        Text text = textObj.AddComponent<Text>();
        text.text = "↻"; // 重置/刷新符号
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 64;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;

        return btnObj;
    }

    private static GameObject CreateConfirmPanel(Transform parent)
    {
        GameObject panel = new GameObject("ResetConfirmPanel");
        panel.transform.SetParent(parent, false);

        RectTransform rect = panel.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(400, 200);

        Image image = panel.AddComponent<Image>();
        image.color = new Color(0.2f, 0.2f, 0.2f, 0.95f);

        // 添加标题
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(panel.transform, false);

        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0, 1);
        titleRect.anchorMax = new Vector2(1, 1);
        titleRect.pivot = new Vector2(0.5f, 1);
        titleRect.anchoredPosition = new Vector2(0, -20);
        titleRect.sizeDelta = new Vector2(-40, 40);

        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "确认重置";
        titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        titleText.fontSize = 24;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = new Color(1f, 0.8f, 0.2f);

        // 添加提示文本
        GameObject messageObj = new GameObject("Message");
        messageObj.transform.SetParent(panel.transform, false);

        RectTransform messageRect = messageObj.AddComponent<RectTransform>();
        messageRect.anchorMin = new Vector2(0, 0.5f);
        messageRect.anchorMax = new Vector2(1, 1);
        messageRect.pivot = new Vector2(0.5f, 0.5f);
        messageRect.anchoredPosition = new Vector2(0, -20);
        messageRect.sizeDelta = new Vector2(-40, -80);

        Text messageText = messageObj.AddComponent<Text>();
        messageText.text = "确定要重置场景吗？\n\n所有进度将会丢失！";
        messageText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        messageText.fontSize = 18;
        messageText.alignment = TextAnchor.MiddleCenter;
        messageText.color = Color.white;

        // 创建按钮容器
        GameObject buttonContainer = new GameObject("ButtonContainer");
        buttonContainer.transform.SetParent(panel.transform, false);

        RectTransform containerRect = buttonContainer.AddComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(0, 0);
        containerRect.anchorMax = new Vector2(1, 0);
        containerRect.pivot = new Vector2(0.5f, 0);
        containerRect.anchoredPosition = new Vector2(0, 20);
        containerRect.sizeDelta = new Vector2(-40, 50);

        HorizontalLayoutGroup layout = buttonContainer.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = 20;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = true;

        // 创建"是"按钮
        GameObject yesBtn = CreateConfirmButton(buttonContainer.transform, "YesButton", "确定", new Color(0.8f, 0.3f, 0.3f));
        
        // 创建"否"按钮
        GameObject noBtn = CreateConfirmButton(buttonContainer.transform, "NoButton", "取消", new Color(0.3f, 0.3f, 0.3f));

        panel.SetActive(false);
        return panel;
    }

    private static GameObject CreateConfirmButton(Transform parent, string name, string text, Color color)
    {
        GameObject btnObj = new GameObject(name);
        btnObj.transform.SetParent(parent, false);

        Image image = btnObj.AddComponent<Image>();
        image.color = color;

        Button button = btnObj.AddComponent<Button>();
        
        ColorBlock colors = button.colors;
        colors.normalColor = color;
        colors.highlightedColor = color * 1.2f;
        colors.pressedColor = color * 0.8f;
        button.colors = colors;

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(btnObj.transform, false);

        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;

        Text btnText = textObj.AddComponent<Text>();
        btnText.text = text;
        btnText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        btnText.fontSize = 20;
        btnText.alignment = TextAnchor.MiddleCenter;
        btnText.color = Color.white;

        return btnObj;
    }

    private static Texture2D CreateCircleTexture(int size)
    {
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color[] pixels = new Color[size * size];
        
        float center = size / 2f;
        float radius = size / 2f;
        
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dx = x - center;
                float dy = y - center;
                float distance = Mathf.Sqrt(dx * dx + dy * dy);
                
                if (distance <= radius)
                {
                    pixels[y * size + x] = Color.white;
                }
                else
                {
                    pixels[y * size + x] = Color.clear;
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }

    private static void SetField(object obj, System.Type type, string fieldName, object value)
    {
        var field = type.GetField(fieldName, 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
            field.SetValue(obj, value);
    }
}
