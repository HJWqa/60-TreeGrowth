using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TreePlanQAQ.OrangeTree;

/// <summary>
/// 重置按钮控制器
/// 控制整个场景的重新开始
/// </summary>
public class ResetButtonController : MonoBehaviour
{
    [Header("按钮引用")]
    [SerializeField] private Button resetButton;
    
    [Header("确认面板")]
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private Button confirmYesButton;
    [SerializeField] private Button confirmNoButton;
    
    [Header("重置选项")]
    [SerializeField] private bool showConfirmDialog = true; // 是否显示确认对话框

    private void Start()
    {
        // 绑定重置按钮事件
        if (resetButton != null)
        {
            resetButton.onClick.RemoveAllListeners();
            resetButton.onClick.AddListener(OnResetClicked);
        }
        
        // 绑定确认对话框按钮
        if (confirmYesButton != null)
        {
            confirmYesButton.onClick.RemoveAllListeners();
            confirmYesButton.onClick.AddListener(OnConfirmYes);
        }
        
        if (confirmNoButton != null)
        {
            confirmNoButton.onClick.RemoveAllListeners();
            confirmNoButton.onClick.AddListener(OnConfirmNo);
        }
        
        // 初始隐藏确认面板
        if (confirmPanel != null)
        {
            confirmPanel.SetActive(false);
        }
    }

    /// <summary>
    /// 重置按钮点击
    /// </summary>
    private void OnResetClicked()
    {
        if (showConfirmDialog && confirmPanel != null)
        {
            // 显示确认对话框
            confirmPanel.SetActive(true);
            Debug.Log("显示重置确认对话框");
        }
        else
        {
            // 直接重置
            ResetScene();
        }
    }

    /// <summary>
    /// 确认对话框 - 是
    /// </summary>
    private void OnConfirmYes()
    {
        if (confirmPanel != null)
        {
            confirmPanel.SetActive(false);
        }
        
        ResetScene();
    }

    /// <summary>
    /// 确认对话框 - 否
    /// </summary>
    private void OnConfirmNo()
    {
        if (confirmPanel != null)
        {
            confirmPanel.SetActive(false);
        }
        
        Debug.Log("取消重置");
    }

    /// <summary>
    /// 重置场景
    /// </summary>
    private void ResetScene()
    {
        Debug.Log("重置场景...");
        
        // 在重新加载场景前，清理所有可能的状态
        CleanupBeforeReset();
        
        // 重新加载当前场景（这会重置所有对象和UI）
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    /// <summary>
    /// 重置前的清理工作
    /// </summary>
    private void CleanupBeforeReset()
    {
        // 关闭所有可能打开的面板
        TopIconsController topIcons = FindObjectOfType<TopIconsController>();
        if (topIcons != null)
        {
            topIcons.CloseAllPanels();
        }
        
        // 关闭确认对话框
        if (confirmPanel != null)
        {
            confirmPanel.SetActive(false);
        }
        
        // 清理 PlayerPrefs 中的临时数据（如果有的话）
        // PlayerPrefs.DeleteKey("TempData");
        
        Debug.Log("清理完成，准备重新加载场景");
    }

    /// <summary>
    /// 手动重置所有系统（备用方案）
    /// </summary>
    private void ResetAllSystems()
    {
        // 重置树控制器
        OrangeTreeController treeController = FindObjectOfType<OrangeTreeController>();
        if (treeController != null)
        {
            treeController.ResetGrowth();
        }
        
        // 重置环境管理器
        EnvironmentManager envManager = EnvironmentManager.Instance;
        if (envManager != null)
        {
            envManager.SetTemperature(25f);
            envManager.SetHumidity(60f);
            envManager.SetSunlight(600f);
        }
        
        // 重置生长开始控制器
        GrowthStartController startController = FindObjectOfType<GrowthStartController>();
        if (startController != null)
        {
            startController.ResetGrowthState();
        }
        
        // 关闭所有面板
        TopIconsController topIcons = FindObjectOfType<TopIconsController>();
        if (topIcons != null)
        {
            topIcons.CloseAllPanels();
        }
        
        Debug.Log("所有系统已重置");
    }

    /// <summary>
    /// 公共方法：直接重置（供外部调用）
    /// </summary>
    public void ResetSceneDirectly()
    {
        ResetScene();
    }

    private void OnDestroy()
    {
        if (resetButton != null)
            resetButton.onClick.RemoveAllListeners();
        
        if (confirmYesButton != null)
            confirmYesButton.onClick.RemoveAllListeners();
        
        if (confirmNoButton != null)
            confirmNoButton.onClick.RemoveAllListeners();
    }
}
