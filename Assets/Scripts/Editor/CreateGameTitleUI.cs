using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace TreePlanQAQ.Editor
{
    /// <summary>
    /// 创建游戏标题UI的编辑器工具
    /// </summary>
    public class CreateGameTitleUI : EditorWindow
    {
        private string titleText = "种植果树大挑战";
        private int fontSize = 60;
        private Color textColor = Color.white;
        private bool addShadow = true;
        private bool addOutline = true;
        private float topOffset = 50f;
        
        [MenuItem("TreePlanQAQ/UI Tools/Create Game Title")]
        public static void ShowWindow()
        {
            GetWindow<CreateGameTitleUI>("创建游戏标题");
        }
        
        [MenuItem("TreePlanQAQ/UI Tools/Quick Create Title (Default Settings)")]
        public static void QuickCreateTitle()
        {
            CreateTitle("种植果树大挑战", 60, Color.white, true, true, 50f);
            EditorUtility.DisplayDialog("完成", "游戏标题已创建！\n\n标题：种植果树大挑战\n位置：顶部中心", "确定");
        }
        
        private void OnGUI()
        {
            GUILayout.Label("创建游戏标题", EditorStyles.boldLabel);
            GUILayout.Space(10);
            
            EditorGUILayout.HelpBox(
                "这个工具会在Canvas顶部创建游戏标题\n" +
                "如果场景中没有Canvas，会自动创建",
                MessageType.Info
            );
            
            GUILayout.Space(10);
            
            // 标题文字
            titleText = EditorGUILayout.TextField("标题文字", titleText);
            
            // 字体大小
            fontSize = EditorGUILayout.IntSlider("字体大小", fontSize, 20, 120);
            
            // 文字颜色
            textColor = EditorGUILayout.ColorField("文字颜色", textColor);
            
            // 距离顶部距离
            topOffset = EditorGUILayout.Slider("距离顶部", topOffset, 0f, 200f);
            
            GUILayout.Space(10);
            
            // 效果选项
            addShadow = EditorGUILayout.Toggle("添加阴影", addShadow);
            addOutline = EditorGUILayout.Toggle("添加描边", addOutline);
            
            GUILayout.Space(20);
            
            // 创建按钮
            if (GUILayout.Button("创建标题", GUILayout.Height(40)))
            {
                CreateTitle(titleText, fontSize, textColor, addShadow, addOutline, topOffset);
                EditorUtility.DisplayDialog("完成", $"游戏标题已创建！\n\n标题：{titleText}", "确定");
            }
            
            GUILayout.Space(10);
            
            if (GUILayout.Button("使用默认设置创建", GUILayout.Height(30)))
            {
                CreateTitle("种植果树大挑战", 60, Color.white, true, true, 50f);
                EditorUtility.DisplayDialog("完成", "游戏标题已创建！\n\n标题：种植果树大挑战", "确定");
            }
        }
        
        /// <summary>
        /// 创建标题UI
        /// </summary>
        private static void CreateTitle(string title, int size, Color color, bool shadow, bool outline, float offset)
        {
            // 查找或创建Canvas
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("Canvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();
                
                // 创建EventSystem
                if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
                {
                    GameObject eventSystem = new GameObject("EventSystem");
                    eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                    eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
                }
                
                Debug.Log("✅ 已创建Canvas和EventSystem");
            }
            
            // 检查是否已存在GameTitle
            Transform existingTitle = canvas.transform.Find("GameTitle");
            if (existingTitle != null)
            {
                if (EditorUtility.DisplayDialog(
                    "标题已存在",
                    "场景中已经有GameTitle，是否替换？",
                    "替换",
                    "取消"))
                {
                    DestroyImmediate(existingTitle.gameObject);
                }
                else
                {
                    return;
                }
            }
            
            // 创建标题GameObject
            GameObject titleObj = new GameObject("GameTitle");
            titleObj.transform.SetParent(canvas.transform, false);
            
            // 设置RectTransform
            RectTransform rectTransform = titleObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 1f); // 顶部中心
            rectTransform.anchorMax = new Vector2(0.5f, 1f);
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = new Vector2(0, -offset);
            rectTransform.sizeDelta = new Vector2(800, 100);
            
            // 添加Text组件
            Text textComponent = titleObj.AddComponent<Text>();
            textComponent.text = title;
            textComponent.fontSize = size;
            textComponent.color = color;
            textComponent.alignment = TextAnchor.MiddleCenter;
            textComponent.fontStyle = FontStyle.Bold;
            
            // 尝试使用更好的字体
            Font arialFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
            if (arialFont != null)
            {
                textComponent.font = arialFont;
            }
            
            // 添加阴影
            if (shadow)
            {
                Shadow shadowComponent = titleObj.AddComponent<Shadow>();
                shadowComponent.effectColor = new Color(0, 0, 0, 0.5f);
                shadowComponent.effectDistance = new Vector2(3, -3);
            }
            
            // 添加描边
            if (outline)
            {
                Outline outlineComponent = titleObj.AddComponent<Outline>();
                outlineComponent.effectColor = new Color(0, 0, 0, 0.8f);
                outlineComponent.effectDistance = new Vector2(2, -2);
            }
            
            // 选中新创建的对象
            Selection.activeGameObject = titleObj;
            
            Debug.Log($"✅ 已创建游戏标题: {title}");
        }
    }
}
