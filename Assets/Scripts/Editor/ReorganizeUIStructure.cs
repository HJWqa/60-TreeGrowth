using UnityEngine;
using UnityEditor;

namespace TreePlanQAQ.Editor
{
    /// <summary>
    /// 重新组织UI结构
    /// </summary>
    public class ReorganizeUIStructure : EditorWindow
    {
        [MenuItem("TreePlanQAQ/UI Tools/Reorganize UI Structure")]
        public static void ShowWindow()
        {
            GetWindow<ReorganizeUIStructure>("重组UI结构");
        }
        
        [MenuItem("TreePlanQAQ/UI Tools/Quick Reorganize UI")]
        public static void QuickReorganize()
        {
            ReorganizeUI();
            EditorUtility.DisplayDialog(
                "完成",
                "UI结构已重新组织！\n\n" +
                "现在的结构：\n" +
                "• GrowthUI（主游戏UI）\n" +
                "  - EnvironmentControlPanel\n" +
                "  - 其他游戏UI\n" +
                "• TipPanel（独立的提示UI）",
                "确定"
            );
        }
        
        private void OnGUI()
        {
            GUILayout.Label("重组UI结构", EditorStyles.boldLabel);
            GUILayout.Space(10);
            
            EditorGUILayout.HelpBox(
                "这个工具会重新组织UI结构：\n\n" +
                "正确的结构应该是：\n" +
                "Canvas\n" +
                "├── GrowthUI（主游戏UI，常驻）\n" +
                "│   ├── EnvironmentControlPanel\n" +
                "│   ├── 温度湿度按钮\n" +
                "│   └── 其他游戏UI\n" +
                "└── TipPanel（提示UI，点击后消失）\n" +
                "    └── 知道了按钮",
                MessageType.Info
            );
            
            GUILayout.Space(20);
            
            if (GUILayout.Button("🔄 自动重组UI结构", GUILayout.Height(50)))
            {
                QuickReorganize();
            }
            
            GUILayout.Space(10);
            
            EditorGUILayout.HelpBox(
                "操作说明：\n" +
                "1. 会将EnvironmentControlPanel移到Canvas下\n" +
                "2. TipPanel保持独立\n" +
                "3. 点击'知道了'只会隐藏TipPanel\n" +
                "4. 游戏控制UI会一直显示",
                MessageType.None
            );
        }
        
        private static void ReorganizeUI()
        {
            // 查找Canvas
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("❌ 找不到Canvas！");
                return;
            }
            
            // 查找EnvironmentControlPanel
            Transform envPanel = FindTransformByName(canvas.transform, "EnvironmentControlPanel");
            if (envPanel == null)
            {
                Debug.LogWarning("⚠️ 找不到EnvironmentControlPanel");
            }
            else
            {
                // 将EnvironmentControlPanel移到Canvas下
                envPanel.SetParent(canvas.transform, true);
                Debug.Log("✅ EnvironmentControlPanel已移到Canvas下");
            }
            
            // 查找所有温度湿度按钮，确保它们在正确的位置
            string[] buttonNames = new string[]
            {
                "TempUpButton", "TempDownButton",
                "HumidityUpButton", "HumidityDownButton",
                "TemperatureLabel", "HumidityLabel"
            };
            
            foreach (var buttonName in buttonNames)
            {
                Transform button = FindTransformByName(canvas.transform, buttonName);
                if (button != null)
                {
                    // 如果按钮在TipPanel下，移到EnvironmentControlPanel或Canvas下
                    if (button.parent != null && button.parent.name == "TipPanel")
                    {
                        if (envPanel != null)
                        {
                            button.SetParent(envPanel, true);
                        }
                        else
                        {
                            button.SetParent(canvas.transform, true);
                        }
                        Debug.Log($"✅ {buttonName}已移出TipPanel");
                    }
                }
            }
            
            // 确保TipPanel独立
            Transform tipPanel = FindTransformByName(canvas.transform, "TipPanel");
            if (tipPanel != null)
            {
                tipPanel.SetParent(canvas.transform, true);
                Debug.Log("✅ TipPanel已设置为独立面板");
            }
            
            // 创建或确保GrowthUI存在
            Transform growthUI = FindTransformByName(canvas.transform, "GrowthUI");
            if (growthUI == null)
            {
                GameObject growthUIObj = new GameObject("GrowthUI");
                growthUIObj.transform.SetParent(canvas.transform, false);
                
                RectTransform rect = growthUIObj.AddComponent<RectTransform>();
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
                
                growthUI = growthUIObj.transform;
                Debug.Log("✅ 已创建GrowthUI容器");
            }
            
            // 将EnvironmentControlPanel移到GrowthUI下
            if (envPanel != null && growthUI != null)
            {
                envPanel.SetParent(growthUI, true);
                Debug.Log("✅ EnvironmentControlPanel已移到GrowthUI下");
            }
            
            Debug.Log("=============================");
            Debug.Log("UI结构重组完成！");
            Debug.Log("现在的结构：");
            Debug.Log("Canvas");
            Debug.Log("├── GrowthUI（主游戏UI）");
            Debug.Log("│   └── EnvironmentControlPanel");
            Debug.Log("└── TipPanel（提示UI）");
            Debug.Log("=============================");
        }
        
        /// <summary>
        /// 递归查找Transform
        /// </summary>
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
}
