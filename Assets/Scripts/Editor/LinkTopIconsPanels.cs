using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// 自动连接左上角图标按钮和面板的工具
/// </summary>
public class LinkTopIconsPanels : EditorWindow
{
    [MenuItem("橘子树/UI设置/连接图标按钮和面板")]
    public static void ShowWindow()
    {
        GetWindow<LinkTopIconsPanels>("连接图标和面板");
    }

    private void OnGUI()
    {
        EditorGUILayout.HelpBox(
            "这个工具会自动连接：\n" +
            "1. 设置按钮 ⚙ → SettingsPanel\n" +
            "2. 提示按钮 ! → EnvironmentHintPanel\n" +
            "3. 暂停按钮 ⏸ → 树控制器", 
            MessageType.Info
        );

        EditorGUILayout.Space();

        if (GUILayout.Button("自动连接所有引用", GUILayout.Height(40)))
        {
            LinkAllReferences();
        }
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("检查连接状态", GUILayout.Height(30)))
        {
            CheckConnectionStatus();
        }
    }

    private static void LinkAllReferences()
    {
        // 查找 TopIconsContainer
        GameObject container = GameObject.Find("TopIconsContainer");
        if (container == null)
        {
            EditorUtility.DisplayDialog("错误", "找不到 TopIconsContainer！\n请先创建左上角图标", "确定");
            return;
        }

        // 获取 TopIconsController 组件
        TopIconsController controller = container.GetComponent<TopIconsController>();
        if (controller == null)
        {
            EditorUtility.DisplayDialog("错误", "TopIconsContainer 上没有 TopIconsController 组件！", "确定");
            return;
        }

        // 查找 Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            EditorUtility.DisplayDialog("错误", "场景中没有找到 Canvas！", "确定");
            return;
        }

        int successCount = 0;
        int totalCount = 0;

        // 连接设置按钮
        totalCount++;
        Button settingsBtn = FindButton(container.transform, "SettingsButton");
        if (settingsBtn != null)
        {
            SetField(controller, "settingsButton", settingsBtn);
            successCount++;
            Debug.Log("✅ 设置按钮已连接");
        }
        else
        {
            Debug.LogWarning("⚠️ 找不到 SettingsButton");
        }

        // 连接提示按钮
        totalCount++;
        Button hintBtn = FindButton(container.transform, "HintButton");
        if (hintBtn != null)
        {
            SetField(controller, "hintButton", hintBtn);
            successCount++;
            Debug.Log("✅ 提示按钮已连接");
        }
        else
        {
            Debug.LogWarning("⚠️ 找不到 HintButton");
        }

        // 连接暂停按钮
        totalCount++;
        Button pauseBtn = FindButton(container.transform, "PauseButton");
        if (pauseBtn != null)
        {
            SetField(controller, "pauseButton", pauseBtn);
            
            // 连接暂停按钮的图标文本
            Transform iconTransform = pauseBtn.transform.Find("Icon");
            if (iconTransform != null)
            {
                Text iconText = iconTransform.GetComponent<Text>();
                if (iconText != null)
                {
                    SetField(controller, "pauseIconText", iconText);
                    Debug.Log("✅ 暂停按钮和图标文本已连接");
                }
            }
            successCount++;
        }
        else
        {
            Debug.LogWarning("⚠️ 找不到 PauseButton");
        }

        // 连接设置面板
        totalCount++;
        GameObject settingsPanel = FindGameObject(canvas.transform, "SettingsPanel");
        if (settingsPanel != null)
        {
            SetField(controller, "settingsPanel", settingsPanel);
            successCount++;
            Debug.Log("✅ 设置面板已连接");
        }
        else
        {
            Debug.LogWarning("⚠️ 找不到 SettingsPanel");
        }

        // 连接提示面板
        totalCount++;
        GameObject hintPanel = FindGameObject(canvas.transform, "EnvironmentHintPanel");
        if (hintPanel != null)
        {
            SetField(controller, "hintPanel", hintPanel);
            successCount++;
            Debug.Log("✅ 提示面板已连接");
        }
        else
        {
            Debug.LogWarning("⚠️ 找不到 EnvironmentHintPanel");
        }

        // 连接树控制器
        totalCount++;
        TreePlanQAQ.OrangeTree.OrangeTreeController treeController = FindObjectOfType<TreePlanQAQ.OrangeTree.OrangeTreeController>();
        if (treeController != null)
        {
            SetField(controller, "treeController", treeController);
            successCount++;
            Debug.Log("✅ 树控制器已连接");
        }
        else
        {
            Debug.LogWarning("⚠️ 找不到 OrangeTreeController（这是可选的）");
        }

        // 标记为已修改
        EditorUtility.SetDirty(controller);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
        );

        // 显示结果
        string message = $"连接完成！\n\n成功：{successCount}/{totalCount}\n\n";
        if (successCount == totalCount)
        {
            message += "所有引用都已正确连接！";
            EditorUtility.DisplayDialog("成功", message, "确定");
        }
        else
        {
            message += "部分引用未能连接，请查看控制台了解详情。";
            EditorUtility.DisplayDialog("部分成功", message, "确定");
        }

        // 选中 TopIconsContainer
        Selection.activeGameObject = container;
    }

    private static void CheckConnectionStatus()
    {
        GameObject container = GameObject.Find("TopIconsContainer");
        if (container == null)
        {
            EditorUtility.DisplayDialog("错误", "找不到 TopIconsContainer！", "确定");
            return;
        }

        TopIconsController controller = container.GetComponent<TopIconsController>();
        if (controller == null)
        {
            EditorUtility.DisplayDialog("错误", "找不到 TopIconsController 组件！", "确定");
            return;
        }

        // 使用反射检查字段
        var type = typeof(TopIconsController);
        string status = "连接状态检查：\n\n";

        status += CheckField(controller, type, "settingsButton", "设置按钮");
        status += CheckField(controller, type, "hintButton", "提示按钮");
        status += CheckField(controller, type, "pauseButton", "暂停按钮");
        status += CheckField(controller, type, "pauseIconText", "暂停图标文本");
        status += CheckField(controller, type, "settingsPanel", "设置面板");
        status += CheckField(controller, type, "hintPanel", "提示面板");
        status += CheckField(controller, type, "treeController", "树控制器（可选）");

        Debug.Log(status);
        EditorUtility.DisplayDialog("连接状态", status, "确定");
    }

    private static string CheckField(object obj, System.Type type, string fieldName, string displayName)
    {
        var field = type.GetField(fieldName, 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (field != null)
        {
            var value = field.GetValue(obj);
            if (value != null && !value.Equals(null))
            {
                return $"✅ {displayName}: 已连接\n";
            }
            else
            {
                return $"❌ {displayName}: 未连接\n";
            }
        }
        else
        {
            return $"⚠️ {displayName}: 字段不存在\n";
        }
    }

    private static Button FindButton(Transform parent, string name)
    {
        Transform child = parent.Find(name);
        if (child != null)
        {
            return child.GetComponent<Button>();
        }
        return null;
    }

    private static GameObject FindGameObject(Transform parent, string name)
    {
        if (parent.name == name)
            return parent.gameObject;

        foreach (Transform child in parent)
        {
            GameObject result = FindGameObject(child, name);
            if (result != null)
                return result;
        }

        return null;
    }

    private static void SetField(object obj, string fieldName, object value)
    {
        var type = obj.GetType();
        var field = type.GetField(fieldName, 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (field != null)
        {
            field.SetValue(obj, value);
        }
        else
        {
            Debug.LogWarning($"字段 {fieldName} 不存在");
        }
    }
}
