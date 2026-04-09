using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 提示面板控制器
/// 显示温度湿度提示信息，点击"知道了"按钮后进入主场景
/// </summary>
public class TipPanel : MonoBehaviour
{
    [Header("UI引用")]
    [SerializeField] private Button confirmButton;
    [SerializeField] private string targetSceneName = "OutdoorsScene";
    
    [Header("淡入淡出设置")]
    [SerializeField] private float fadeOutDuration = 0.5f;
    [SerializeField] private CanvasGroup canvasGroup;

    private void Start()
    {
        if (confirmButton != null)
        {
            confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        }
        else
        {
            Debug.LogError("确认按钮未设置！");
        }

        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }

    private void OnConfirmButtonClicked()
    {
        if (canvasGroup != null)
        {
            StartCoroutine(FadeOutAndHide());
        }
        else
        {
            HidePanel();
        }
    }

    private System.Collections.IEnumerator FadeOutAndHide()
    {
        float elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = 1f - (elapsed / fadeOutDuration);
            yield return null;
        }
        
        HidePanel();
    }

    private void HidePanel()
    {
        // 只隐藏提示面板，不切换场景
        gameObject.SetActive(false);
        Debug.Log("✅ 提示面板已隐藏");
    }
    
    // 如果需要切换场景，可以从外部调用这个方法
    public void LoadTargetScene()
    {
        SceneManager.LoadScene(targetSceneName);
    }

    private void OnDestroy()
    {
        if (confirmButton != null)
        {
            confirmButton.onClick.RemoveListener(OnConfirmButtonClicked);
        }
    }
}
