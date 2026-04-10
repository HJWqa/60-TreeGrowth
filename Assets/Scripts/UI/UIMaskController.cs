using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 统一管理 UI Mask 的显示状态。
/// 在关闭面板后调用 OnPanelClosed，可在无目标面板显示时自动隐藏 Mask。
/// </summary>
public static class UIMaskController
{
    private static readonly Color32 VisibleMaskColor = new Color32(0, 0, 0, 145); // #91000000
    private static readonly Color32 HiddenMaskColor = new Color32(0, 0, 0, 0);     // #00000000
    private const bool EnableDebugLogs = true;

    // 需要与 Mask 联动的面板名称
    private static readonly string[] TrackedPanelNames =
    {
        "SettingsPanel",
        "TipPanel",
        "EnvironmentHintPanel",
        "ResetConfirmPanel",
        "MessagePanel"
    };

    // 常见的遮罩对象命名，优先查找 Mask
    private static readonly string[] MaskObjectNames =
    {
        "Mask",
        "UIMask",
        "PanelMask"
    };

    /// <summary>
    /// 面板打开后调用：将遮罩调为黑色145透明度。
    /// </summary>
    public static void OnPanelOpened()
    {
        Log($"🟢 OnPanelOpened -> 目标颜色 #{ColorToHex(VisibleMaskColor)}");
        ApplyMaskColor(VisibleMaskColor);
    }

    /// <summary>
    /// 面板关闭后调用：若没有任何受跟踪面板处于显示状态，则关闭 Mask。
    /// </summary>
    public static void OnPanelClosed(params GameObject[] knownPanels)
    {
        Log("🟡 OnPanelClosed -> 开始检查是否仍有面板打开");
        if (IsAnyTrackedPanelOpen(knownPanels))
        {
            Log("⏸️ 仍有目标面板处于开启状态，Mask保持当前颜色");
            return;
        }

        Log($"⚫ 所有目标面板已关闭 -> 目标颜色 #{ColorToHex(HiddenMaskColor)}");
        ApplyMaskColor(HiddenMaskColor);
    }

    private static bool IsAnyTrackedPanelOpen(GameObject[] knownPanels)
    {
        if (knownPanels != null)
        {
            foreach (GameObject panel in knownPanels)
            {
                if (panel != null && panel.activeInHierarchy)
                {
                    Log($"📌 knownPanels 命中开启面板: {panel.name} (activeSelf={panel.activeSelf}, activeInHierarchy={panel.activeInHierarchy})");
                    return true;
                }
            }
        }

        foreach (string panelName in TrackedPanelNames)
        {
            GameObject panel = GameObject.Find(panelName);
            if (panel != null && panel.activeInHierarchy)
            {
                Log($"📌 TrackedPanelNames 命中开启面板: {panel.name} (activeSelf={panel.activeSelf}, activeInHierarchy={panel.activeInHierarchy})");
                return true;
            }
        }

        Log("✅ 未检测到任何开启的目标面板");
        return false;
    }

    private static void ApplyMaskColor(Color32 color)
    {
        GameObject maskObject = FindMaskObject();
        if (maskObject == null)
        {
            Log("❌ 未找到Mask对象，无法应用颜色");
            return;
        }

        if (!maskObject.activeSelf)
        {
            maskObject.SetActive(true);
            Log($"ℹ️ Mask对象原本未激活，已自动激活: {maskObject.name}");
        }

        // 仅修改Image颜色，不改组件整体透明度
        Image image = maskObject.GetComponent<Image>();
        if (image == null)
        {
            image = maskObject.GetComponentInChildren<Image>(true);
            if (image != null)
            {
                Log($"🔎 在子节点找到Image: {image.gameObject.name}");
            }
        }

        if (image != null)
        {
            Color32 before = image.color;
            image.color = color;
            Log($"🎨 颜色写入成功: 对象={image.gameObject.name}, Before=#{ColorToHex(before)}, After=#{ColorToHex((Color32)image.color)}");
        }
        else
        {
            Log($"❌ Mask对象没有Image组件: {maskObject.name}");
        }
    }

    private static GameObject FindMaskObject()
    {
        foreach (string maskName in MaskObjectNames)
        {
            GameObject mask = GameObject.Find(maskName);
            if (mask != null)
            {
                Log($"✅ 通过GameObject.Find命中Mask: {mask.name}");
                return mask;
            }
        }

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        // 先做精确名匹配（可找到未激活对象）
        foreach (GameObject obj in allObjects)
        {
            if (!obj.scene.IsValid() || !obj.scene.isLoaded)
            {
                continue;
            }

            foreach (string maskName in MaskObjectNames)
            {
                if (obj.name == maskName)
                {
                    Log($"✅ 通过Resources精确命中Mask: {obj.name}");
                    return obj;
                }
            }
        }

        // 再做包含名兜底（例如：MainMask、UIMaskPanel）
        foreach (GameObject obj in allObjects)
        {
            if (!obj.scene.IsValid() || !obj.scene.isLoaded)
            {
                continue;
            }

            string lowerName = obj.name.ToLowerInvariant();
            if (lowerName.Contains("mask"))
            {
                Log($"⚠️ 通过包含名兜底命中Mask: {obj.name}");
                return obj;
            }
        }

        Log("❌ FindMaskObject未找到任何匹配对象");
        return null;
    }

    private static string ColorToHex(Color32 color)
    {
        return $"{color.a:X2}{color.r:X2}{color.g:X2}{color.b:X2}";
    }

    private static void Log(string message)
    {
        if (!EnableDebugLogs)
        {
            return;
        }

        Debug.Log($"[UIMaskController] {message}");
    }
}
