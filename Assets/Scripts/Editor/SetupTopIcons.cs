using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TreePlanQAQ.OrangeTree;

/// <summary>
/// 设置左上角图标的编辑器工具
/// </summary>
public class SetupTopIcons : EditorWindow
{
    [MenuItem("橘子树/UI设置/创建左上角图标")]
    public static void ShowWindow()
    {
        GetWindow<SetupTopIcons>("创建左上角图标");
    }

    private void OnGUI()
    {
        EditorGUILayout.HelpBox(
            "这个工具会在左上角创建三个图标按钮：\n" +
            "1. 齿轮图标 - 游戏设置\n" +
            "2. 感叹号图标 - 温度湿度提示\n" +
            "3. 三角形图标 - 暂停/继续", 
            MessageType.Info
        );

        EditorGUILayout.Space();

        if (GUILayout.Button("创建左上角图标", GUILayout.Height(40)))
        {
            CreateTopIcons();
        }
    }

    private static void CreateTopIcons()
    {
        // 查找 Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            EditorUtility.DisplayDialog("错误", "场景中没有找到 Canvas！", "确定");
            return;
        }

        // 创建容器
        GameObject container = new GameObject("TopIconsContainer");
        container.transform.SetParent(canvas.transform, false);

        RectTransform containerRect = container.AddComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(0, 1);
        containerRect.anchorMax = new Vector2(0, 1);
        containerRect.pivot = new Vector2(0, 1);
        containerRect.anchoredPosition = new Vector2(20, -20);
        containerRect.sizeDelta = new Vector2(200, 60);

        // 添加水平布局组
        HorizontalLayoutGroup layout = container.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = 15;
        layout.childAlignment = TextAnchor.MiddleLeft;
        layout.childControlWidth = false;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;

        // 创建三个图标按钮（统一灰色圆圈背景，白色图标）
        Color circleColor = new Color(0.5f, 0.5f, 0.5f); // 灰色圆圈
        Button settingsBtn = CreateIconButton(container.transform, "SettingsButton", "⚙", circleColor, Color.white);
        Button hintBtn = CreateIconButton(container.transform, "HintButton", "!", circleColor, Color.white);
        Button pauseBtn = CreateIconButton(container.transform, "PauseButton", "▶", circleColor, Color.white);

        // 创建设置面板
        GameObject settingsPanel = CreateSettingsPanel(canvas.transform);
        
        // 创建提示面板
        GameObject hintPanel = CreateHintPanel(canvas.transform);

        // 添加控制器
        TopIconsController controller = container.AddComponent<TopIconsController>();
        
        // 获取暂停按钮的图标文本
        Text pauseIconText = pauseBtn.transform.Find("Icon").GetComponent<Text>();
        
        // 使用反射设置字段
        var controllerType = typeof(TopIconsController);
        SetField(controller, controllerType, "settingsButton", settingsBtn);
        SetField(controller, controllerType, "hintButton", hintBtn);
        SetField(controller, controllerType, "pauseButton", pauseBtn);
        SetField(controller, controllerType, "pauseIconText", pauseIconText);
        SetField(controller, controllerType, "settingsPanel", settingsPanel);
        SetField(controller, controllerType, "hintPanel", hintPanel);

        // 标记场景为已修改
        EditorUtility.SetDirty(canvas.gameObject);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
        );

        Debug.Log("✅ 左上角图标创建成功！");
        Selection.activeGameObject = container;
        
        EditorUtility.DisplayDialog(
            "创建成功", 
            "左上角图标已创建！\n\n" +
            "包含：\n" +
            "- ⚙ 设置按钮\n" +
            "- ! 提示按钮\n" +
            "- ▶ 暂停/继续按钮\n\n" +
            "所有引用已自动配置", 
            "确定"
        );
    }

    private static Button CreateIconButton(Transform parent, string name, string icon, Color circleColor, Color iconColor)
    {
        GameObject btnObj = new GameObject(name);
        btnObj.transform.SetParent(parent, false);

        RectTransform rect = btnObj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(50, 50);

        // 添加圆形背景
        Image image = btnObj.AddComponent<Image>();
        image.color = circleColor;
        image.type = Image.Type.Filled;
        image.fillMethod = Image.FillMethod.Radial360;
        
        // 使用 Unity 内置的圆形精灵
        Texture2D circleTexture = CreateCircleTexture(50);
        Sprite circleSprite = Sprite.Create(
            circleTexture,
            new Rect(0, 0, circleTexture.width, circleTexture.height),
            new Vector2(0.5f, 0.5f)
        );
        image.sprite = circleSprite;

        Button button = btnObj.AddComponent<Button>();
        
        // 设置按钮颜色（保持灰色圆圈）
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = new Color(0.9f, 0.9f, 0.9f);
        colors.pressedColor = new Color(0.7f, 0.7f, 0.7f);
        button.colors = colors;

        TopIconPressGrayController pressGrayController = btnObj.AddComponent<TopIconPressGrayController>();

        // 创建图标文本
        GameObject textObj = new GameObject("Icon");
        textObj.transform.SetParent(btnObj.transform, false);

        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;

        Text text = textObj.AddComponent<Text>();
        text.text = icon;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 64; // 从 32 放大到 64（两倍）
        text.alignment = TextAnchor.MiddleCenter;
        text.color = iconColor;

        if (pressGrayController != null)
        {
            pressGrayController.RefreshCache();
        }

        return button;
    }

    // 创建圆形纹理
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
                    // 圆形内部
                    pixels[y * size + x] = Color.white;
                }
                else
                {
                    // 圆形外部透明
                    pixels[y * size + x] = Color.clear;
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }

    private static GameObject CreateSettingsPanel(Transform parent)
    {
        GameObject panel = new GameObject("SettingsPanel");
        panel.transform.SetParent(parent, false);

        RectTransform rect = panel.AddComponent<RectTransform>();
        // 设置为屏幕正中间
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero; // 正中心
        rect.sizeDelta = new Vector2(400, 300);

        Image image = panel.AddComponent<Image>();
        image.color = new Color(0.2f, 0.2f, 0.2f, 0.95f);

        // 添加标题
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(panel.transform, false);

        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0, 1);
        titleRect.anchorMax = new Vector2(1, 1);
        titleRect.pivot = new Vector2(0.5f, 1);
        titleRect.anchoredPosition = new Vector2(0, -10);
        titleRect.sizeDelta = new Vector2(-20, 40);

        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "游戏设置";
        titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        titleText.fontSize = 24;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = Color.white;

        // 添加内容文本
        GameObject contentObj = new GameObject("Content");
        contentObj.transform.SetParent(panel.transform, false);

        RectTransform contentRect = contentObj.AddComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0, 0);
        contentRect.anchorMax = new Vector2(1, 1);
        contentRect.pivot = new Vector2(0.5f, 0.5f);
        contentRect.anchoredPosition = new Vector2(0, -10);
        contentRect.sizeDelta = new Vector2(-20, -60);

        Text contentText = contentObj.AddComponent<Text>();
        contentText.text = "音量、画质等设置\n（待实现）";
        contentText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        contentText.fontSize = 18;
        contentText.alignment = TextAnchor.MiddleCenter;
        contentText.color = new Color(0.8f, 0.8f, 0.8f);

        panel.SetActive(false);
        return panel;
    }

    private static GameObject CreateHintPanel(Transform parent)
    {
        GameObject panel = new GameObject("EnvironmentHintPanel");
        panel.transform.SetParent(parent, false);

        RectTransform rect = panel.AddComponent<RectTransform>();
        // 设置为屏幕正中间
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero; // 正中心
        rect.sizeDelta = new Vector2(500, 350);

        Image image = panel.AddComponent<Image>();
        image.color = new Color(0.15f, 0.3f, 0.15f, 0.95f);

        // 添加标题
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(panel.transform, false);

        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0, 1);
        titleRect.anchorMax = new Vector2(1, 1);
        titleRect.pivot = new Vector2(0.5f, 1);
        titleRect.anchoredPosition = new Vector2(0, -10);
        titleRect.sizeDelta = new Vector2(-20, 40);

        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "环境参数提示";
        titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        titleText.fontSize = 24;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = new Color(1f, 1f, 0.8f);

        // 添加内容文本
        GameObject contentObj = new GameObject("Content");
        contentObj.transform.SetParent(panel.transform, false);

        RectTransform contentRect = contentObj.AddComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0, 0);
        contentRect.anchorMax = new Vector2(1, 1);
        contentRect.pivot = new Vector2(0.5f, 0.5f);
        contentRect.anchoredPosition = new Vector2(0, -10);
        contentRect.sizeDelta = new Vector2(-40, -60);

        Text contentText = contentObj.AddComponent<Text>();
        contentText.text = 
            "温度湿度合理的范围是：\n\n" +
            "🌡️ 温度：15°C - 30°C\n" +
            "   最佳温度：20°C - 25°C\n\n" +
            "💧 湿度：40% - 80%\n" +
            "   最佳湿度：50% - 70%\n\n" +
            "☀️ 光照：400 - 800 lux\n" +
            "   最佳光照：600 lux";
        contentText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        contentText.fontSize = 18;
        contentText.alignment = TextAnchor.UpperLeft;
        contentText.color = Color.white;

        panel.SetActive(false);
        return panel;
    }

    private static void SetField(object obj, System.Type type, string fieldName, object value)
    {
        var field = type.GetField(fieldName, 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
            field.SetValue(obj, value);
    }
}
