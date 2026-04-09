using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace TreePlanQAQ.Editor
{
    /// <summary>
    /// 恢复按钮中的三角形图标
    /// </summary>
    public class RestoreTriangleIcons : EditorWindow
    {
        [MenuItem("TreePlanQAQ/UI Tools/Restore Triangle Icons")]
        public static void ShowWindow()
        {
            GetWindow<RestoreTriangleIcons>("恢复三角形图标");
        }
        
        [MenuItem("TreePlanQAQ/UI Tools/Quick Restore All Triangles")]
        public static void QuickRestore()
        {
            int count = RestoreAllTriangles();
            EditorUtility.DisplayDialog(
                "完成",
                $"已恢复 {count} 个按钮的三角形图标！",
                "确定"
            );
        }
        
        private void OnGUI()
        {
            GUILayout.Label("恢复三角形图标", EditorStyles.boldLabel);
            GUILayout.Space(10);
            
            EditorGUILayout.HelpBox(
                "这个工具会为圆形按钮添加三角形图标\n" +
                "适用于温度和湿度调节按钮",
                MessageType.Info
            );
            
            GUILayout.Space(20);
            
            if (GUILayout.Button("🔺 恢复所有三角形图标", GUILayout.Height(50)))
            {
                QuickRestore();
            }
        }
        
        private static int RestoreAllTriangles()
        {
            int restoredCount = 0;
            Button[] allButtons = FindObjectsOfType<Button>();
            
            foreach (var button in allButtons)
            {
                string name = button.gameObject.name.ToLower();
                
                // 检查是否是温度/湿度按钮
                bool isUpButton = name.Contains("up") || name.Contains("上") || name.Contains("increase") || name.Contains("加");
                bool isDownButton = name.Contains("down") || name.Contains("下") || name.Contains("decrease") || name.Contains("减");
                
                if (isUpButton || isDownButton)
                {
                    // 检查是否已经有TriangleIcon
                    Transform existingIcon = button.transform.Find("TriangleIcon");
                    if (existingIcon != null)
                    {
                        DestroyImmediate(existingIcon.gameObject);
                    }
                    
                    // 创建新的三角形图标
                    GameObject triangleObj = new GameObject("TriangleIcon");
                    triangleObj.transform.SetParent(button.transform, false);
                    
                    RectTransform triangleRect = triangleObj.AddComponent<RectTransform>();
                    triangleRect.anchorMin = new Vector2(0.5f, 0.5f);
                    triangleRect.anchorMax = new Vector2(0.5f, 0.5f);
                    triangleRect.pivot = new Vector2(0.5f, 0.5f);
                    triangleRect.anchoredPosition = Vector2.zero;
                    triangleRect.sizeDelta = new Vector2(100f, 100f);
                    
                    // 添加TriangleImage组件
                    var triangleImage = triangleObj.AddComponent<TreePlanQAQ.UI.TriangleImage>();
                    triangleImage.pointUp = isUpButton;
                    triangleImage.triangleColor = Color.white;
                    
                    EditorUtility.SetDirty(button.gameObject);
                    restoredCount++;
                    
                    Debug.Log($"✅ 已恢复 {button.name} 的三角形图标");
                }
            }
            
            return restoredCount;
        }
    }
}
