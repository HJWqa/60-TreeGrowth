using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TreePlanQAQ.OrangeTree;

/// <summary>
/// 收获消息UI
/// 显示收获完成的提示信息
/// </summary>
public class HarvestMessageUI : MonoBehaviour
{
    private const string HarvestClipResourcePath = "Audio/tada";

    [Header("UI引用")]
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private Text messageText;
    [SerializeField] private Button closeButton;
    
    [Header("显示设置")]
    [SerializeField] private float autoHideDuration = 5f; // 自动隐藏时间（秒）
    [SerializeField] private bool autoHide = true; // 是否自动隐藏
    [SerializeField] [Range(0f, 1f)] private float harvestSfxVolume = 1f;
    
    private OrangeTreeController treeController;
    private Coroutine autoHideCoroutine;
    private AudioClip harvestClip;
    
    private void Start()
    {
        // 查找树控制器
        treeController = FindObjectOfType<OrangeTreeController>();
        if (treeController != null)
        {
            // 订阅收获完成事件
            treeController.OnHarvestComplete += OnHarvestComplete;
        }
        
        // 绑定关闭按钮
        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(HideMessage);
        }
        
        // 初始隐藏消息面板
        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
        }
    }
    
    /// <summary>
    /// 收获完成回调
    /// </summary>
    private void OnHarvestComplete(int yield)
    {
        ShowMessage(yield);
    }
    
    /// <summary>
    /// 显示收获消息
    /// </summary>
    public void ShowMessage(int yield)
    {
        if (messagePanel == null || messageText == null) return;
        
        // 设置消息文本
        messageText.text = $"恭喜您！您已收获 {yield} 公斤的橘子！";
        
        // 显示面板
        messagePanel.SetActive(true);
        UIMaskController.OnPanelOpened();
        PlayHarvestSfx();
        
        Debug.Log($"🎉 显示收获消息: {yield}kg");
        
        // 自动隐藏
        if (autoHide)
        {
            if (autoHideCoroutine != null)
            {
                StopCoroutine(autoHideCoroutine);
            }
            autoHideCoroutine = StartCoroutine(AutoHideAfterDelay());
        }
    }
    
    /// <summary>
    /// 隐藏消息
    /// </summary>
    public void HideMessage()
    {
        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
            UIMaskController.OnPanelClosed(messagePanel);
        }
        
        if (autoHideCoroutine != null)
        {
            StopCoroutine(autoHideCoroutine);
            autoHideCoroutine = null;
        }
    }
    
    /// <summary>
    /// 延迟后自动隐藏
    /// </summary>
    private IEnumerator AutoHideAfterDelay()
    {
        yield return new WaitForSeconds(autoHideDuration);
        HideMessage();
    }

    private void PlayHarvestSfx()
    {
        if (harvestClip == null)
        {
            harvestClip = Resources.Load<AudioClip>(HarvestClipResourcePath);
        }

        if (harvestClip == null)
        {
            Debug.LogWarning("[Audio] Harvest clip not found at Resources/Audio/tada");
            return;
        }

        Vector3 playPos = Vector3.zero;
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            playPos = mainCamera.transform.position;
        }

        AudioSource.PlayClipAtPoint(harvestClip, playPos, harvestSfxVolume);
    }
    
    private void OnDestroy()
    {
        if (treeController != null)
        {
            treeController.OnHarvestComplete -= OnHarvestComplete;
        }
        
        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
        }
    }
}
