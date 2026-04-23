using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TreePlanQAQ.OrangeTree
{
    /// <summary>
    /// TopIcons 按下时的灰化辅助组件
    /// 只处理按钮子节点的图标/文字，避免和 Button 自身的颜色过渡冲突
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class TopIconPressGrayController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private Color pressedColor = new Color(0.7f, 0.7f, 0.7f, 1f);
        [SerializeField, Min(0.01f)] private float transitionDuration = 0.12f;

        private readonly List<Graphic> childGraphics = new List<Graphic>();
        private readonly Dictionary<Graphic, Color> originalColors = new Dictionary<Graphic, Color>();

        private Button button;
        private bool isPressed;
        private bool pressedStateApplied;
        private Coroutine transitionCoroutine;
        private Action onPressedFullyGray;
        private Action onPressCancelled;

        private void Awake()
        {
            button = GetComponent<Button>();
            CacheChildGraphics();
        }

        private void OnEnable()
        {
            StopTransition();
            isPressed = false;
            pressedStateApplied = false;
            ApplyPressedState(false);
        }

        /// <summary>
        /// 手动同步一次缓存，适合编辑器脚本在运行时补挂组件后调用
        /// </summary>
        public void RefreshCache()
        {
            CacheChildGraphics();
            ApplyPressedState(isPressed);
        }

        public void SetPressCallbacks(Action pressedFullyGray, Action pressCancelled)
        {
            onPressedFullyGray = pressedFullyGray;
            onPressCancelled = pressCancelled;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SetPressed(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SetPressed(false);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isPressed)
            {
                if (pressedStateApplied)
                {
                    onPressCancelled?.Invoke();
                }

                SetPressed(false);
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            // 键盘/手柄导航时不强制灰化，避免和默认选中态冲突。
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (isPressed)
            {
                if (pressedStateApplied)
                {
                    onPressCancelled?.Invoke();
                }

                SetPressed(false);
            }
        }

        private void SetPressed(bool pressed)
        {
            if (isPressed == pressed)
            {
                return;
            }

            isPressed = pressed;
            StartTransition(pressed);
        }

        private void StartTransition(bool pressed)
        {
            StopTransition();
            transitionCoroutine = StartCoroutine(AnimatePressedState(pressed));
        }

        private void StopTransition()
        {
            if (transitionCoroutine != null)
            {
                StopCoroutine(transitionCoroutine);
                transitionCoroutine = null;
            }
        }

        private IEnumerator AnimatePressedState(bool pressed)
        {
            float duration = Mathf.Max(transitionDuration, 0.01f);
            Color[] startColors = new Color[childGraphics.Count];
            Color[] targetColors = new Color[childGraphics.Count];

            for (int i = 0; i < childGraphics.Count; i++)
            {
                Graphic graphic = childGraphics[i];
                if (graphic == null)
                {
                    continue;
                }

                if (!originalColors.TryGetValue(graphic, out Color originalColor))
                {
                    originalColor = graphic.color;
                    originalColors[graphic] = originalColor;
                }

                startColors[i] = graphic.color;
                targetColors[i] = pressed ? pressedColor : originalColor;
            }

            pressedStateApplied = false;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float t = Mathf.Clamp01(elapsed / duration);

                for (int i = 0; i < childGraphics.Count; i++)
                {
                    Graphic graphic = childGraphics[i];
                    if (graphic == null)
                    {
                        continue;
                    }

                    graphic.color = Color.Lerp(startColors[i], targetColors[i], t);
                }

                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            for (int i = 0; i < childGraphics.Count; i++)
            {
                Graphic graphic = childGraphics[i];
                if (graphic == null)
                {
                    continue;
                }

                graphic.color = targetColors[i];
            }

            pressedStateApplied = pressed;

            if (pressed)
            {
                onPressedFullyGray?.Invoke();
            }

            transitionCoroutine = null;
        }

        private void CacheChildGraphics()
        {
            childGraphics.Clear();
            originalColors.Clear();

            if (button == null)
            {
                button = GetComponent<Button>();
            }

            Graphic rootGraphic = button != null ? button.targetGraphic : null;
            Graphic[] graphics = GetComponentsInChildren<Graphic>(true);

            foreach (Graphic graphic in graphics)
            {
                if (graphic == null)
                {
                    continue;
                }

                if (graphic == rootGraphic)
                {
                    continue;
                }

                childGraphics.Add(graphic);
                originalColors[graphic] = graphic.color;
            }
        }

        private void ApplyPressedState(bool pressed)
        {
            for (int i = 0; i < childGraphics.Count; i++)
            {
                Graphic graphic = childGraphics[i];

                if (graphic == null)
                {
                    continue;
                }

                if (!originalColors.TryGetValue(graphic, out Color originalColor))
                {
                    originalColor = graphic.color;
                    originalColors[graphic] = originalColor;
                }

                graphic.color = pressed ? pressedColor : originalColor;
            }
        }
    }
}