using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Forwards Button click events to the global click sound manager.
/// </summary>
[RequireComponent(typeof(Button))]
public class UIButtonClickSfxForwarder : MonoBehaviour, IPointerDownHandler, ISubmitHandler
{
    private Button button;
    private float lastPlayTime;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        TryPlayClick();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        TryPlayClick();
    }

    private void TryPlayClick()
    {
        if (button != null && (!button.IsInteractable() || !button.enabled))
        {
            return;
        }

        // 防止同一帧内PointerDown + Submit等重复触发叠音
        if (Time.unscaledTime - lastPlayTime < 0.03f)
        {
            return;
        }

        lastPlayTime = Time.unscaledTime;

        if (GlobalButtonClickSfx.Instance != null)
        {
            if (GlobalButtonClickSfx.Instance.TryPlaySpecialButtonSfx(button))
            {
                return;
            }

            if (!GlobalButtonClickSfx.Instance.ShouldPlayDefaultClick(button))
            {
                return;
            }

            GlobalButtonClickSfx.Instance.PlayClick();
        }
    }
}
