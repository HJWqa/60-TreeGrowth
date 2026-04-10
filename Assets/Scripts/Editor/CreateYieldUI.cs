using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

/// <summary>
/// 创建产量系统UI的编辑器工具
/// </summary>
public class CreateYieldUI : EditorWindow
{
    [MenuItem("橘子树/UI设置/创建产量系统UI")]
    public static void ShowWindow()
    {
        GetWindow<CreateYieldUI>("创建产量系统UI");
    }

    private void OnGUI()
    {
        EditorGUILayout.HelpBox(
            "这个工具会创建产量系统的UI：\n" +
            "1. 产量显示（左上角）\n" +
            "2. 收获消息面板（屏幕中央）",
            MessageType.Info
        );

        EditorGUILayout.Space();

        if (GUILayout.Button("创建产量显示UI", GUILayout.Height(40)))
        {
            CreateYieldDisplay();
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("创建收获消息面板", GUILayout.Height(40)))
        {
            CreateHarvestMessagePanel();
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("创建全部UI", GUILayout.Height(40)))
        {
            CreateYieldDisplay();
            CreateHarvestMessagePanel();
        }
    }

    /// <summary>
    /// 创建产量显示UI
    /// </summary>
    private void CreateYieldDisplay()
    {
        // 查找Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            EditorUtility.DisplayDialog("错误", "场景中没有Canvas！请先创建Canvas。", "确定");
            return;
        }

        // 检查是否已存在
        Transform existing = canvas.transform.Find("YieldDisplay");
        if (existing != null)
        {
            bool overwrite = EditorUtility.DisplayDialog(
                "已存在",
                "产量显示UI已存在，是否重新创建？",
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

        // 创建产量显示
        GameObject yieldDisplay = new GameObject("YieldDisplay");
        yieldDisplay.transform.SetParent(canvas.transform, false);

        // 设置RectTransform（左上角）
        RectTransform rt = yieldDisplay.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);
        rt.anchoredPosition = new Vector2(20, -20);
        rt.sizeDelta = new Vector2(200, 50);

        // 创建背景
        GameObject background = new GameObject("Background");
        background.transform.SetParent(yieldDisplay.transform, false);
        Image bgImage = background.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.7f);
        RectTransform bgRt = background.GetComponent<RectTransform>();
        bgRt.anchorMin = Vector2.zero;
        bgRt.anchorMax = Vector2.one;
        bgRt.sizeDelta = Vector2.zero;

        // 创建文本
        GameObject textObj = new GameObject("YieldText");
        textObj.transform.SetParent(yieldDisplay.transform, false);
        Text text = textObj.AddComponent<Text>();
        text.text = "预期产量: 50kg";
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 20;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleCenter;
        RectTransform textRt = textObj.GetComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.sizeDelta = Vector2.zero;

        // 添加YieldDisplayUI组件
        YieldDisplayUI yieldUI = yieldDisplay.AddComponent<YieldDisplayUI>();
        
        // 使用反射设置私有字段
        var yieldUIType = typeof(YieldDisplayUI);
        SetField(yieldUI, yieldUIType, "yieldText", text);
        SetField(yieldUI, yieldUIType, "displayFormat", "预期产量: {0}kg");
        SetField(yieldUI, yieldUIType, "normalColor", Color.white);
        SetField(yieldUI, yieldUIType, "warningColor", Color.yellow);
        SetField(yieldUI, yieldUIType, "dangerColor", Color.red);
        SetField(yieldUI, yieldUIType, "warningThreshold", 30);
        SetField(yieldUI, yieldUIType, "dangerThreshold", 10);

        EditorUtility.SetDirty(yieldDisplay);
        Selection.activeGameObject = yieldDisplay;

        Debug.Log("✅ 产量显示UI创建成功！");
        EditorUtility.DisplayDialog("创建成功", "产量显示UI已创建在左上角！", "确定");
    }

    /// <summary>
    /// 创建收获消息面板
    /// </summary>
    private void CreateHarvestMessagePanel()
    {
        // 查找Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            EditorUtility.DisplayDialog("错误", "场景中没有Canvas！请先创建Canvas。", "确定");
            return;
        }

        // 检查是否已存在
        Transform existing = canvas.transform.Find("HarvestMessagePanel");
        if (existing != null)
        {
            bool overwrite = EditorUtility.DisplayDialog(
                "已存在",
                "收获消息面板已存在，是否重新创建？",
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

        // 创建面板
        GameObject panel = new GameObject("HarvestMessagePanel");
        panel.transform.SetParent(canvas.transform, false);

        // 设置RectTransform（屏幕中央）
        RectTransform panelRt = panel.AddComponent<RectTransform>();
        panelRt.anchorMin = new Vector2(0.5f, 0.5f);
        panelRt.anchorMax = new Vector2(0.5f, 0.5f);
        panelRt.pivot = new Vector2(0.5f, 0.5f);
        panelRt.anchoredPosition = Vector2.zero;
        panelRt.sizeDelta = new Vector2(500, 250);

        // 创建半透明背景遮罩
        GameObject overlay = new GameObject("Overlay");
        overlay.transform.SetParent(panel.transform, false);
        Image overlayImage = overlay.AddComponent<Image>();
        overlayImage.color = new Color(0, 0, 0, 0.5f);
        RectTransform overlayRt = overlay.GetComponent<RectTransform>();
        overlayRt.anchorMin = Vector2.zero;
        overlayRt.anchorMax = Vector2.one;
        overlayRt.offsetMin = new Vector2(-1000, -1000);
        overlayRt.offsetMax = new Vector2(1000, 1000);

        // 创建消息背景
        GameObject background = new GameObject("Background");
        background.transform.SetParent(panel.transform, false);
        Image bgImage = background.AddComponent<Image>();
        bgImage.color = new Color(0.2f, 0.6f, 0.2f, 0.95f); // 绿色背景
        RectTransform bgRt = background.GetComponent<RectTransform>();
        bgRt.anchorMin = Vector2.zero;
        bgRt.anchorMax = Vector2.one;
        bgRt.sizeDelta = Vector2.zero;

        // 创建标题文本
        GameObject titleObj = new GameObject("TitleText");
        titleObj.transform.SetParent(panel.transform, false);
        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "🎉 收获成功！";
        titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        titleText.fontSize = 32;
        titleText.fontStyle = FontStyle.Bold;
        titleText.color = Color.white;
        titleText.alignment = TextAnchor.MiddleCenter;
        RectTransform titleRt = titleObj.GetComponent<RectTransform>();
        titleRt.anchorMin = new Vector2(0, 0.6f);
        titleRt.anchorMax = new Vector2(1, 0.9f);
        titleRt.sizeDelta = Vector2.zero;

        // 创建消息文本
        GameObject messageObj = new GameObject("MessageText");
        messageObj.transform.SetParent(panel.transform, false);
        Text messageText = messageObj.AddComponent<Text>();
        messageText.text = "恭喜您！您已收获 50 公斤的橘子！";
        messageText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        messageText.fontSize = 24;
        messageText.color = Color.white;
        messageText.alignment = TextAnchor.MiddleCenter;
        RectTransform messageRt = messageObj.GetComponent<RectTransform>();
        messageRt.anchorMin = new Vector2(0.1f, 0.4f);
        messageRt.anchorMax = new Vector2(0.9f, 0.6f);
        messageRt.sizeDelta = Vector2.zero;

        // 创建关闭按钮
        GameObject buttonObj = new GameObject("CloseButton");
        buttonObj.transform.SetParent(panel.transform, false);
        Button button = buttonObj.AddComponent<Button>();
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = new Color(0.8f, 0.8f, 0.8f, 1f);
        RectTransform buttonRt = buttonObj.GetComponent<RectTransform>();
        buttonRt.anchorMin = new Vector2(0.3f, 0.1f);
        buttonRt.anchorMax = new Vector2(0.7f, 0.3f);
        buttonRt.sizeDelta = Vector2.zero;

        // 按钮文本
        GameObject buttonTextObj = new GameObject("Text");
        buttonTextObj.transform.SetParent(buttonObj.transform, false);
        Text buttonText = buttonTextObj.AddComponent<Text>();
        buttonText.text = "确定";
        buttonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        buttonText.fontSize = 20;
        buttonText.color = Color.black;
        buttonText.alignment = TextAnchor.MiddleCenter;
        RectTransform buttonTextRt = buttonTextObj.GetComponent<RectTransform>();
        buttonTextRt.anchorMin = Vector2.zero;
        buttonTextRt.anchorMax = Vector2.one;
        buttonTextRt.sizeDelta = Vector2.zero;

        // 添加HarvestMessageUI组件
        HarvestMessageUI harvestUI = panel.AddComponent<HarvestMessageUI>();
        
        // 使用反射设置私有字段
        var harvestUIType = typeof(HarvestMessageUI);
        SetField(harvestUI, harvestUIType, "messagePanel", panel);
        SetField(harvestUI, harvestUIType, "messageText", messageText);
        SetField(harvestUI, harvestUIType, "closeButton", button);
        SetField(harvestUI, harvestUIType, "autoHideDuration", 5f);
        SetField(harvestUI, harvestUIType, "autoHide", true);

        // 初始隐藏面板
        panel.SetActive(false);

        EditorUtility.SetDirty(panel);
        Selection.activeGameObject = panel;

        Debug.Log("✅ 收获消息面板创建成功！");
        EditorUtility.DisplayDialog("创建成功", "收获消息面板已创建！\n初始状态为隐藏，收获时会自动显示。", "确定");
    }

    /// <summary>
    /// 使用反射设置私有字段
    /// </summary>
    private void SetField(object obj, System.Type type, string fieldName, object value)
    {
        var field = type.GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
        {
            field.SetValue(obj, value);
        }
        else
        {
            Debug.LogWarning($"找不到字段: {fieldName}");
        }
    }
}
