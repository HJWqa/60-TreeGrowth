using UnityEngine;
using UnityEngine.UI;

namespace TreePlanQAQ.UI
{
    /// <summary>
    /// 提示UI控制器 - 点击"知道了"按钮后隐藏UI
    /// </summary>
    public class TipUIController : MonoBehaviour
    {
        [Header("UI引用")]
        [Tooltip("知道了按钮")]
        public Button confirmButton;
        
        [Tooltip("要隐藏的UI对象（通常是Canvas或Panel）")]
        public GameObject uiToHide;
        
        [Header("隐藏方式")]
        [Tooltip("是否使用淡出动画")]
        public bool useFadeOut = true;
        
        [Tooltip("淡出持续时间（秒）")]
        public float fadeOutDuration = 0.5f;
        
        private CanvasGroup canvasGroup;
        private bool isFading = false;
        
        private void Start()
        {
            // 如果没有指定要隐藏的对象，查找名为"TipPanel"的对象
            if (uiToHide == null)
            {
                // 尝试查找TipPanel
                GameObject tipPanel = GameObject.Find("TipPanel");
                if (tipPanel != null)
                {
                    uiToHide = tipPanel;
                    Debug.Log("✅ 自动找到TipPanel作为要隐藏的对象");
                }
                else
                {
                    // 如果找不到TipPanel，默认隐藏自己所在的GameObject
                    uiToHide = gameObject;
                    Debug.LogWarning("⚠️ 未找到TipPanel，将隐藏当前GameObject");
                }
            }
            
            // 如果使用淡出动画，确保有CanvasGroup组件
            if (useFadeOut)
            {
                canvasGroup = uiToHide.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = uiToHide.AddComponent<CanvasGroup>();
                }
            }
            
            // 绑定按钮事件
            if (confirmButton != null)
            {
                confirmButton.onClick.RemoveAllListeners();
                confirmButton.onClick.AddListener(OnConfirmButtonClicked);
                Debug.Log("✅ 知道了按钮事件已绑定");
            }
            else
            {
                Debug.LogWarning("⚠️ confirmButton 未设置！请在Inspector中拖入按钮");
            }
        }
        
        /// <summary>
        /// 点击"知道了"按钮
        /// </summary>
        private void OnConfirmButtonClicked()
        {
            Debug.Log("点击了知道了按钮");
            
            if (useFadeOut && !isFading)
            {
                // 使用淡出动画
                StartCoroutine(FadeOutAndHide());
            }
            else if (!isFading)
            {
                // 直接隐藏
                HideUI();
            }
        }
        
        /// <summary>
        /// 淡出并隐藏UI
        /// </summary>
        private System.Collections.IEnumerator FadeOutAndHide()
        {
            isFading = true;
            float elapsedTime = 0f;
            
            while (elapsedTime < fadeOutDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = 1f - (elapsedTime / fadeOutDuration);
                canvasGroup.alpha = alpha;
                yield return null;
            }
            
            canvasGroup.alpha = 0f;
            HideUI();
            isFading = false;
        }
        
        /// <summary>
        /// 隐藏UI
        /// </summary>
        private void HideUI()
        {
            if (uiToHide != null)
            {
                uiToHide.SetActive(false);
                UIMaskController.OnPanelClosed(uiToHide);
                Debug.Log($"✅ 已隐藏UI: {uiToHide.name}");
            }
        }
        
        /// <summary>
        /// 显示UI（可以从外部调用）
        /// </summary>
        public void ShowUI()
        {
            if (uiToHide != null)
            {
                uiToHide.SetActive(true);
                UIMaskController.OnPanelOpened();
                
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 1f;
                }
                
                Debug.Log($"✅ 已显示UI: {uiToHide.name}");
            }
        }
    }
}
