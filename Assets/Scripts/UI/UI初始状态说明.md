# UI 初始状态说明

## 概述

当场景加载或重置后，所有UI组件都会自动回到以下初始状态。

## 各UI组件的初始状态

### 1. 左上角图标（TopIconsController）

**初始状态：**
- ⚙ 设置按钮：显示
- ! 提示按钮：显示
- ⏸ 暂停按钮：显示，图标为暂停符号（因为初始是暂停状态）

**面板状态：**
- 设置面板：隐藏
- 提示面板：隐藏

**代码位置：** `TopIconsController.Start()`
```csharp
// 初始化面板状态（隐藏）
if (settingsPanel != null)
    settingsPanel.SetActive(false);

if (hintPanel != null)
    hintPanel.SetActive(false);
```

---

### 2. 右上角重置按钮（ResetButtonController）

**初始状态：**
- ↻ 重置按钮：显示

**面板状态：**
- 确认对话框：隐藏

**代码位置：** `ResetButtonController.Start()`
```csharp
// 初始隐藏确认面板
if (confirmPanel != null)
{
    confirmPanel.SetActive(false);
}
```

---

### 3. 生长开始按钮（GrowthStartController）

**初始状态：**
- 按钮显示："开始生长"
- 生长状态：未开始（isGrowthStarted = false）
- 树生长：暂停

**代码位置：** `GrowthStartController.Start()`
```csharp
// 初始化按钮文本和状态
UpdateButtonState(); // 显示"开始生长"

// 初始时暂停生长
if (treeController != null && !treeController.IsPaused)
{
    treeController.TogglePause();
}
```

---

### 4. 左侧信息面板（GrowthUIController）

**初始状态：**
- 阶段文本：显示当前阶段（通常是"种子"）
- 生长进度：0%
- 进度条：空
- 温度显示：默认值（如 25°C）
- 湿度显示：默认值（如 60%）
- 光照显示：默认值（如 600 lux）

**代码位置：** `GrowthUIController.Start()`
```csharp
// 初始更新
UpdateUI();
UpdatePauseButtonText();
```

---

### 5. 右侧环境控制面板（EnvironmentController）

**初始状态：**
- 温度控制按钮：显示
- 湿度控制按钮：显示
- 温度显示：默认值（如 25°C）
- 湿度显示：默认值（如 60%）

**代码位置：** `EnvironmentController.Start()`
```csharp
// 订阅环境变化事件，更新右侧显示（如果有）
if (environmentManager != null)
{
    environmentManager.OnEnvironmentChanged += UpdateDisplay;
}

// 初始化显示
UpdateDisplay(environmentManager.Temperature, environmentManager.Humidity, environmentManager.Sunlight);
```

---

### 6. 提示面板（TipUIController）

**初始状态：**
- 提示面板：显示（如果设置为初始显示）
- 提示文本：显示初始提示内容
- "知道了"按钮：显示

**代码位置：** `TipUIController.Start()`
```csharp
// 根据配置决定是否初始显示
if (showOnStart && uiToHide != null)
{
    uiToHide.SetActive(true);
}
```

---

## 重置流程

### 1. 用户点击重置按钮

```
用户点击 ↻ 重置按钮
  ↓
显示确认对话框
  ↓
用户点击"确定"
  ↓
执行 CleanupBeforeReset()
  ├─ 关闭所有打开的面板
  ├─ 关闭确认对话框
  └─ 清理临时数据
  ↓
执行 SceneManager.LoadScene()
  ↓
场景重新加载
  ↓
所有 MonoBehaviour 的 Start() 方法重新执行
  ↓
所有UI回到初始状态
```

### 2. 重置前清理（CleanupBeforeReset）

```csharp
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
    
    Debug.Log("清理完成，准备重新加载场景");
}
```

### 3. 场景重新加载

```csharp
Scene currentScene = SceneManager.GetActiveScene();
SceneManager.LoadScene(currentScene.name);
```

**效果：**
- 销毁所有当前场景中的对象
- 重新实例化场景中的所有对象
- 所有脚本的 Start() 方法重新执行
- 所有变量恢复到初始值
- 所有UI组件恢复到初始状态

---

## 初始状态检查清单

重置后，请确认以下状态：

### UI显示状态
- [ ] 左上角三个图标按钮显示
- [ ] 右上角重置按钮显示
- [ ] 生长开始按钮显示"开始生长"
- [ ] 左侧信息面板显示初始数据
- [ ] 右侧环境控制面板显示

### UI隐藏状态
- [ ] 设置面板隐藏
- [ ] 提示面板隐藏
- [ ] 重置确认对话框隐藏
- [ ] 环境提示面板隐藏

### 数据状态
- [ ] 橘子树阶段：种子
- [ ] 生长进度：0%
- [ ] 温度：默认值（25°C）
- [ ] 湿度：默认值（60%）
- [ ] 光照：默认值（600 lux）
- [ ] 生长状态：暂停

### 按钮状态
- [ ] 开始按钮文本："开始生长"
- [ ] 暂停按钮图标：暂停符号
- [ ] 所有按钮可点击

---

## 常见问题

**Q: 重置后某个面板还是显示的？**
A: 检查该面板的初始化代码，确保在 Start() 方法中设置为隐藏

**Q: 重置后数据没有恢复？**
A: 场景重新加载会完全重置所有数据，如果数据没恢复，可能是使用了 PlayerPrefs 或 DontDestroyOnLoad

**Q: 重置后按钮不工作？**
A: 检查按钮的事件绑定是否在 Start() 方法中正确设置

**Q: 想要保留某些数据不被重置？**
A: 使用 PlayerPrefs 保存数据，或使用 DontDestroyOnLoad 保持对象

---

## 调试技巧

### 1. 检查初始化顺序

在各个 Start() 方法中添加日志：

```csharp
private void Start()
{
    Debug.Log($"[{GetType().Name}] Start() 执行");
    // 初始化代码...
}
```

### 2. 检查面板状态

在场景加载后检查：

```csharp
void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    Debug.Log($"场景加载完成: {scene.name}");
    // 检查各个面板的 activeSelf 状态
}
```

### 3. 使用 Unity Editor 检查

- 运行游戏后暂停
- 在 Hierarchy 中检查各个 UI 对象的激活状态
- 在 Inspector 中检查各个组件的值

---

## 最佳实践

1. **所有UI初始状态在 Start() 中设置**
   - 不要依赖 Inspector 中的默认勾选状态
   - 明确在代码中设置初始状态

2. **使用 SetActive(false) 隐藏面板**
   - 不要使用 CanvasGroup.alpha = 0
   - 不要使用移出屏幕的方式

3. **清理事件监听器**
   - 在 OnDestroy() 中移除所有监听器
   - 避免内存泄漏

4. **测试重置功能**
   - 在不同游戏状态下测试重置
   - 确保所有情况下都能正确重置

5. **文档化初始状态**
   - 在代码注释中说明初始状态
   - 便于团队协作和维护
