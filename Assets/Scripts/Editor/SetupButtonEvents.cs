using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine.Events;

namespace TreePlanQAQ.Editor
{
    /// <summary>
    /// 设置按钮事件
    /// </summary>
    public class SetupButtonEvents : EditorWindow
    {
        [MenuItem("TreePlanQAQ/Setup Button Events")]
        public static void Setup()
        {
            // 查找 GrowthUI
            GameObject growthUI = GameObject.Find("GrowthUI");
            if (growthUI == null)
            {
                EditorUtility.DisplayDialog("错误", "找不到 GrowthUI 对象", "确定");
                return;
            }
            
            var uiController = growthUI.GetComponent<TreePlanQAQ.OrangeTree.GrowthUIController>();
            if (uiController == null)
            {
                EditorUtility.DisplayDialog("错误", "找不到 GrowthUIController 组件", "确定");
                return;
            }
            
            // 查找按钮
            Transform panel = growthUI.transform.Find("Panel");
            if (panel == null)
            {
                EditorUtility.DisplayDialog("错误", "找不到 Panel", "确定");
                return;
            }
            
            Transform pauseButtonTrans = panel.Find("PauseButtonText");
            Transform resetButtonTrans = panel.Find("ResetButtonText");
            
            if (pauseButtonTrans == null || resetButtonTrans == null)
            {
                EditorUtility.DisplayDialog("错误", "找不到按钮对象", "确定");
                return;
            }
            
            Button pauseButton = pauseButtonTrans.GetComponent<Button>();
            Button resetButton = resetButtonTrans.GetComponent<Button>();
            
            if (pauseButton == null || resetButton == null)
            {
                EditorUtility.DisplayDialog("错误", "按钮缺少 Button 组件", "确定");
                return;
            }
            
            // 清除现有事件
            pauseButton.onClick.RemoveAllListeners();
            resetButton.onClick.RemoveAllListeners();
            
            // 添加暂停按钮事件
            UnityAction pauseAction = new UnityAction(() => {
                var controller = FindObjectOfType<TreePlanQAQ.OrangeTree.OrangeTreeController>();
                if (controller != null)
                {
                    controller.TogglePause();
                    // 更新按钮文字
                    var text = pauseButtonTrans.GetComponent<Text>();
                    if (text != null)
                    {
                        text.text = controller.IsPaused ? "[ 继续 ]" : "[ 暂停 ]";
                    }
                }
            });
            
            // 添加重置按钮事件
            UnityAction resetAction = new UnityAction(() => {
                var controller = FindObjectOfType<TreePlanQAQ.OrangeTree.OrangeTreeController>();
                if (controller != null)
                {
                    controller.ResetGrowth();
                }
            });
            
            // 使用 UnityEventTools 添加持久化监听器
            UnityEventTools.AddPersistentListener(pauseButton.onClick, pauseAction);
            UnityEventTools.AddPersistentListener(resetButton.onClick, resetAction);
            
            // 标记为已修改
            EditorUtility.SetDirty(pauseButton);
            EditorUtility.SetDirty(resetButton);
            
            // 保存场景
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
            );
            
            EditorUtility.DisplayDialog("成功", "按钮事件已设置！现在可以在运行时点击按钮了。", "确定");
        }
    }
}
