using UnityEngine;
using UnityEngine.UI;
using TreePlanQAQ.OrangeTree;

/// <summary>
/// 左上角图标控制器
/// 管理设置、提示、暂停三个图标按钮
/// </summary>
public class TopIconsController : MonoBehaviour
{
    [Header("图标按钮")]
    [SerializeField] private Button settingsButton;      // 齿轮图标
    [SerializeField] private Button hintButton;          // 感叹号图标
    [SerializeField] private Button pauseButton;         // 三角形图标
    
    [Header("图标文本")]
    [SerializeField] private Text pauseIconText;         // 暂停按钮的图标文本
    
    [Header("面板引用")]
    [SerializeField] private GameObject settingsPanel;   // 设置面板
    [SerializeField] private GameObject hintPanel;       // 提示面板
    
    [Header("目标控制器")]
    [SerializeField] private OrangeTreeController treeController;
    
    private bool isPaused = false;
    private bool wasPausedBeforePanel = false; // 记录打开面板前的暂停状态

    private void Start()
    {
        // 自动查找树控制器
        if (treeController == null)
        {
            treeController = FindObjectOfType<OrangeTreeController>();
        }
        
        // 绑定按钮事件
        if (settingsButton != null)
        {
            settingsButton.onClick.RemoveAllListeners();
            settingsButton.onClick.AddListener(OnSettingsClicked);
        }
        
        if (hintButton != null)
        {
            hintButton.onClick.RemoveAllListeners();
            hintButton.onClick.AddListener(OnHintClicked);
        }
        
        if (pauseButton != null)
        {
            pauseButton.onClick.RemoveAllListeners();
            pauseButton.onClick.AddListener(OnPauseClicked);
        }
        
        // 初始化面板状态（隐藏）
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
        
        if (hintPanel != null)
            hintPanel.SetActive(false);
        
        // 初始化暂停图标
        UpdatePauseIcon();
    }

    /// <summary>
    /// 设置按钮点击
    /// </summary>
    private void OnSettingsClicked()
    {
        if (settingsPanel != null)
        {
            bool isActive = settingsPanel.activeSelf;
            
            if (!isActive)
            {
                // 打开设置面板
                // 记录当前暂停状态
                if (treeController != null)
                {
                    wasPausedBeforePanel = treeController.IsPaused;
                    
                    // 如果正在运行，则暂停
                    if (!treeController.IsPaused)
                    {
                        treeController.TogglePause();
                        Debug.Log("打开设置面板，暂停果树生长");
                    }
                }
                
                settingsPanel.SetActive(true);
                
                // 关闭提示面板
                if (hintPanel != null)
                {
                    hintPanel.SetActive(false);
                }
            }
            else
            {
                // 关闭设置面板
                settingsPanel.SetActive(false);
                
                // 恢复之前的暂停状态
                if (treeController != null && !wasPausedBeforePanel && treeController.IsPaused)
                {
                    treeController.TogglePause();
                    Debug.Log("关闭设置面板，恢复果树生长");
                }
            }
            
            Debug.Log($"设置面板: {(settingsPanel.activeSelf ? "打开" : "关闭")}");
        }
        else
        {
            Debug.LogWarning("设置面板未设置！");
        }
    }

    /// <summary>
    /// 提示按钮点击
    /// </summary>
    private void OnHintClicked()
    {
        if (hintPanel != null)
        {
            bool isActive = hintPanel.activeSelf;
            
            if (!isActive)
            {
                // 打开提示面板
                // 记录当前暂停状态
                if (treeController != null)
                {
                    wasPausedBeforePanel = treeController.IsPaused;
                    
                    // 如果正在运行，则暂停
                    if (!treeController.IsPaused)
                    {
                        treeController.TogglePause();
                        Debug.Log("打开提示面板，暂停果树生长");
                    }
                }
                
                hintPanel.SetActive(true);
                
                // 关闭设置面板
                if (settingsPanel != null)
                {
                    settingsPanel.SetActive(false);
                }
            }
            else
            {
                // 关闭提示面板
                hintPanel.SetActive(false);
                
                // 恢复之前的暂停状态
                if (treeController != null && !wasPausedBeforePanel && treeController.IsPaused)
                {
                    treeController.TogglePause();
                    Debug.Log("关闭提示面板，恢复果树生长");
                }
            }
            
            Debug.Log($"提示面板: {(hintPanel.activeSelf ? "打开" : "关闭")}");
        }
        else
        {
            Debug.LogWarning("提示面板未设置！");
        }
    }

    /// <summary>
    /// 暂停按钮点击
    /// </summary>
    private void OnPauseClicked()
    {
        if (treeController != null)
        {
            treeController.TogglePause();
            isPaused = treeController.IsPaused;
            UpdatePauseIcon();
            
            Debug.Log($"游戏状态: {(isPaused ? "暂停" : "继续")}");
        }
        else
        {
            Debug.LogWarning("树控制器未找到！");
        }
    }

    /// <summary>
    /// 更新暂停图标
    /// </summary>
    private void UpdatePauseIcon()
    {
        if (pauseIconText != null && treeController != null)
        {
            isPaused = treeController.IsPaused;
            
            // 图标保持白色，不改变颜色
            // 可以通过改变图标符号来区分状态
            if (isPaused)
            {
                pauseIconText.text = "▶"; // 暂停时显示播放图标
            }
            else
            {
                pauseIconText.text = "⏸"; // 播放时显示暂停图标（更紧凑的符号）
            }
            
            pauseIconText.color = Color.white; // 始终保持白色
        }
    }

    /// <summary>
    /// 关闭所有面板（供外部调用）
    /// </summary>
    public void CloseAllPanels()
    {
        bool anyPanelWasOpen = false;
        
        if (settingsPanel != null && settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(false);
            anyPanelWasOpen = true;
        }
        
        if (hintPanel != null && hintPanel.activeSelf)
        {
            hintPanel.SetActive(false);
            anyPanelWasOpen = true;
        }
        
        // 如果有面板被关闭，且之前不是暂停状态，则恢复运行
        if (anyPanelWasOpen && treeController != null && !wasPausedBeforePanel && treeController.IsPaused)
        {
            treeController.TogglePause();
            Debug.Log("关闭所有面板，恢复果树生长");
        }
    }

    private void OnDestroy()
    {
        if (settingsButton != null)
            settingsButton.onClick.RemoveAllListeners();
        
        if (hintButton != null)
            hintButton.onClick.RemoveAllListeners();
        
        if (pauseButton != null)
            pauseButton.onClick.RemoveAllListeners();
    }
}
