using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Bootstraps global click sound for all UI buttons in loaded scenes.
/// </summary>
public class GlobalButtonClickSfx : MonoBehaviour
{
    private const string ClickClipResourcePath = "Audio/Minecraft Click";
    private const string PopupOpenClipResourcePath = "Audio/Chest_open";
    private const string PopupCloseClipResourcePath = "Audio/Chest_close1.ogg";
    private const float RescanInterval = 1.5f;
    private const float MinPlayInterval = 0.03f;

    private static GlobalButtonClickSfx instance;

    [SerializeField] private AudioClip clickClip;
    [SerializeField] private AudioClip popupOpenClip;
    [SerializeField] private AudioClip popupCloseClip;
    [SerializeField] [Range(0f, 1f)] private float clickVolume = 1f;
    [SerializeField] [Range(0f, 1f)] private float popupOpenVolume = 1f;
    [SerializeField] [Range(0f, 1f)] private float popupCloseVolume = 1f;

    // 这些按钮用于打开弹窗：不播放默认Minecraft Click，由业务逻辑改播Chest_open
    private static readonly HashSet<string> PopupOpenButtonNames = new HashSet<string>
    {
        "SettingsButton",
        "HintButton",
        "ResetButton"
    };

    // 这些按钮用于关闭弹窗：统一播放Chest_close1
    private static readonly HashSet<string> PopupCloseButtonNames = new HashSet<string>
    {
        "NoButton"
    };

    // 前两个CloseButton所在的面板名称（SettingsPanel和HintPanel的CloseButton）
    private static readonly HashSet<string> PopupCloseFirstTwoButtonPanelNames = new HashSet<string>
    {
        "SettingsPanel",
        "HintPanel",
        "EnvironmentHintPanel"  // 替代名称
    };

    private float nextRescanTime;
    private float lastPlayTime;

    public static GlobalButtonClickSfx Instance => instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        if (instance != null)
        {
            return;
        }

        GameObject go = new GameObject("GlobalButtonClickSfx");
        DontDestroyOnLoad(go);
        instance = go.AddComponent<GlobalButtonClickSfx>();
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        LoadClickClipIfNeeded();
        LoadPopupOpenClipIfNeeded();
        LoadPopupCloseClipIfNeeded();

        SceneManager.sceneLoaded += OnSceneLoaded;
        AttachForwardersInScene(SceneManager.GetActiveScene());
    }

    private void Update()
    {
        if (Time.unscaledTime < nextRescanTime)
        {
            return;
        }

        nextRescanTime = Time.unscaledTime + RescanInterval;
        AttachForwardersInScene(SceneManager.GetActiveScene());
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            instance = null;
        }
    }

    public void PlayClick()
    {
        if (!enabled)
        {
            return;
        }

        if (Time.unscaledTime - lastPlayTime < MinPlayInterval)
        {
            return;
        }

        if (clickClip == null)
        {
            LoadClickClipIfNeeded();
        }

        if (clickClip == null)
        {
            return;
        }

        lastPlayTime = Time.unscaledTime;

        AudioSource.PlayClipAtPoint(clickClip, GetPlayPosition(), clickVolume);
    }

    public void PlayPopupOpen()
    {
        if (!enabled)
        {
            return;
        }

        if (Time.unscaledTime - lastPlayTime < MinPlayInterval)
        {
            return;
        }

        if (popupOpenClip == null)
        {
            LoadPopupOpenClipIfNeeded();
        }

        if (popupOpenClip == null)
        {
            return;
        }

        lastPlayTime = Time.unscaledTime;
        AudioSource.PlayClipAtPoint(popupOpenClip, GetPlayPosition(), popupOpenVolume);
    }

    public bool ShouldPlayDefaultClick(Button button)
    {
        if (button == null)
        {
            return false;
        }

        string buttonName = button.gameObject.name;
        if (PopupOpenButtonNames.Contains(buttonName) || PopupCloseButtonNames.Contains(buttonName))
        {
            return false;
        }

        // 检查CloseButton是否属于前两个panel（如果是，不播放默认Click）
        if (buttonName == "CloseButton" && IsCloseButtonInFirstTwoPanels(button))
        {
            return false;
        }

        return true;
    }

    public bool TryPlaySpecialButtonSfx(Button button)
    {
        if (button == null)
        {
            return false;
        }

        string buttonName = button.gameObject.name;
        
        // 检查是否是NoButton（第一类关闭按钮）
        if (PopupCloseButtonNames.Contains(buttonName))
        {
            PlayPopupClose();
            return true;
        }

        // 检查是否是CloseButton且属于前两个panel（第二类关闭按钮）
        if (buttonName == "CloseButton" && IsCloseButtonInFirstTwoPanels(button))
        {
            PlayPopupClose();
            return true;
        }

        return false;
    }

    private bool IsCloseButtonInFirstTwoPanels(Button button)
    {
        if (button == null)
        {
            return false;
        }

        // 从按钮的parent往上查找，看是否属于前两个panel
        Transform current = button.transform.parent;
        while (current != null)
        {
            if (PopupCloseFirstTwoButtonPanelNames.Contains(current.gameObject.name))
            {
                return true;
            }
            current = current.parent;
        }

        return false;
    }

    private void LoadClickClipIfNeeded()
    {
        if (clickClip != null)
        {
            return;
        }

        clickClip = Resources.Load<AudioClip>(ClickClipResourcePath);
        if (clickClip == null)
        {
            Debug.LogWarning("[Audio] Click clip not found at Resources/Audio/Minecraft Click");
        }
    }

    private void LoadPopupOpenClipIfNeeded()
    {
        if (popupOpenClip != null)
        {
            return;
        }

        popupOpenClip = Resources.Load<AudioClip>(PopupOpenClipResourcePath);
        if (popupOpenClip == null)
        {
            Debug.LogWarning("[Audio] Popup open clip not found at Resources/Audio/Chest_open");
        }
    }

    private void LoadPopupCloseClipIfNeeded()
    {
        if (popupCloseClip != null)
        {
            return;
        }

        popupCloseClip = Resources.Load<AudioClip>(PopupCloseClipResourcePath);
        if (popupCloseClip == null)
        {
            Debug.LogWarning("[Audio] Popup close clip not found at Resources/Audio/Chest_close1.ogg");
        }
    }

    public void PlayPopupClose()
    {
        if (!enabled)
        {
            return;
        }

        if (Time.unscaledTime - lastPlayTime < MinPlayInterval)
        {
            return;
        }

        if (popupCloseClip == null)
        {
            LoadPopupCloseClipIfNeeded();
        }

        if (popupCloseClip == null)
        {
            return;
        }

        lastPlayTime = Time.unscaledTime;
        AudioSource.PlayClipAtPoint(popupCloseClip, GetPlayPosition(), popupCloseVolume);
    }

    private Vector3 GetPlayPosition()
    {
        Camera mainCamera = Camera.main;
        return mainCamera != null ? mainCamera.transform.position : Vector3.zero;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AttachForwardersInScene(scene);
    }

    private void AttachForwardersInScene(Scene scene)
    {
        if (!scene.IsValid() || !scene.isLoaded)
        {
            return;
        }

        GameObject[] roots = scene.GetRootGameObjects();
        foreach (GameObject root in roots)
        {
            Button[] buttons = root.GetComponentsInChildren<Button>(true);
            foreach (Button button in buttons)
            {
                if (button.GetComponent<UIButtonClickSfxForwarder>() == null)
                {
                    button.gameObject.AddComponent<UIButtonClickSfxForwarder>();
                }
            }
        }
    }
}
