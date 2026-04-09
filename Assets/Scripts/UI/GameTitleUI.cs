using UnityEngine;
using UnityEngine.UI;

namespace TreePlanQAQ.UI
{
    /// <summary>
    /// 游戏标题UI - 在顶部显示"种植果树大挑战"
    /// </summary>
    public class GameTitleUI : MonoBehaviour
    {
        [Header("标题设置")]
        [Tooltip("标题文本")]
        public string titleText = "种植果树大挑战";
        
        [Tooltip("字体大小")]
        public int fontSize = 60;
        
        [Tooltip("文字颜色")]
        public Color textColor = Color.white;
        
        [Tooltip("是否加粗")]
        public bool bold = true;
        
        [Tooltip("是否添加阴影")]
        public bool addShadow = true;
        
        [Tooltip("是否添加描边")]
        public bool addOutline = true;
        
        [Header("位置设置")]
        [Tooltip("距离顶部的距离")]
        public float topOffset = 50f;
        
        private Text titleTextComponent;
        
        private void Start()
        {
            CreateTitleUI();
        }
        
        /// <summary>
        /// 创建标题UI
        /// </summary>
        private void CreateTitleUI()
        {
            // 查找Canvas
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                canvas = FindObjectOfType<Canvas>();
            }
            
            if (canvas == null)
            {
                Debug.LogError("❌ 找不到Canvas！请确保场景中有Canvas");
                return;
            }
            
            // 创建标题GameObject
            GameObject titleObj = new GameObject("GameTitle");
            titleObj.transform.SetParent(canvas.transform, false);
            
            // 添加RectTransform
            RectTransform rectTransform = titleObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 1f); // 顶部中心
            rectTransform.anchorMax = new Vector2(0.5f, 1f);
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = new Vector2(0, -topOffset);
            rectTransform.sizeDelta = new Vector2(800, 100);
            
            // 添加Text组件
            titleTextComponent = titleObj.AddComponent<Text>();
            titleTextComponent.text = titleText;
            titleTextComponent.fontSize = fontSize;
            titleTextComponent.color = textColor;
            titleTextComponent.alignment = TextAnchor.MiddleCenter;
            titleTextComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            
            if (bold)
            {
                titleTextComponent.fontStyle = FontStyle.Bold;
            }
            
            // 添加阴影
            if (addShadow)
            {
                Shadow shadow = titleObj.AddComponent<Shadow>();
                shadow.effectColor = new Color(0, 0, 0, 0.5f);
                shadow.effectDistance = new Vector2(3, -3);
            }
            
            // 添加描边
            if (addOutline)
            {
                Outline outline = titleObj.AddComponent<Outline>();
                outline.effectColor = new Color(0, 0, 0, 0.8f);
                outline.effectDistance = new Vector2(2, -2);
            }
            
            Debug.Log($"✅ 已创建游戏标题: {titleText}");
        }
        
        /// <summary>
        /// 更新标题文本
        /// </summary>
        public void SetTitle(string newTitle)
        {
            titleText = newTitle;
            if (titleTextComponent != null)
            {
                titleTextComponent.text = newTitle;
            }
        }
        
        /// <summary>
        /// 更新标题颜色
        /// </summary>
        public void SetColor(Color newColor)
        {
            textColor = newColor;
            if (titleTextComponent != null)
            {
                titleTextComponent.color = newColor;
            }
        }
    }
}
