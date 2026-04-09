using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace TreePlanQAQ.Editor
{
    /// <summary>
    /// 自动添加控制按钮到 GrowthUI
    /// </summary>
    public class AddControlButtons : EditorWindow
    {
        [MenuItem("TreePlanQAQ/Add Control Buttons to UI")]
        public static void AddButtons()
        {
            // 查找 GrowthUI
            GameObject growthUI = GameObject.Find("GrowthUI");
            if (growthUI == null)
            {
                EditorUtility.DisplayDialog("错误", "找不到 GrowthUI 对象", "确定");
                return;
            }
            
            // 查找 Panel
            Transform panel = growthUI.transform.Find("Panel");
            if (panel == null)
            {
                EditorUtility.DisplayDialog("错误", "找不到 Panel 对象", "确定");
                return;
            }
            
            // 创建按钮容器
            GameObject buttonContainer = new GameObject("ButtonContainer");
            buttonContainer.transform.SetParent(panel, false);
            
            RectTransform containerRect = buttonContainer.AddComponent<RectTransform>();
            containerRect.anchorMin = new Vector2(0.5f, 0f);
            containerRect.anchorMax = new Vector2(0.5f, 0f);
            containerRect.pivot = new Vector2(0.5f, 0f);
            containerRect.anchoredPosition = new Vector2(0f, 20f);
            containerRect.sizeDelta = new Vector2(300f, 50f);
            
            // 添加水平布局
            HorizontalLayoutGroup layout = buttonContainer.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 20f;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = false;
            
            // 创建暂停按钮
            GameObject pauseButton = CreateButton("PauseButton", "暂停", buttonContainer.transform);
            
            // 创建重置按钮
            GameObject resetButton = CreateButton("ResetButton", "重置", buttonContainer.transform);
            
            // 查找 GrowthUIController 并设置引用
            var uiController = growthUI.GetComponent<TreePlanQAQ.OrangeTree.GrowthUIController>();
            if (uiController != null)
            {
                SerializedObject so = new SerializedObject(uiController);
                so.FindProperty("pauseButton").objectReferenceValue = pauseButton.GetComponent<Button>();
                so.FindProperty("resetButton").objectReferenceValue = resetButton.GetComponent<Button>();
                so.FindProperty("pauseButtonText").objectReferenceValue = pauseButton.GetComponentInChildren<Text>();
                so.ApplyModifiedProperties();
            }
            
            // 标记场景为已修改
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
            );
            
            EditorUtility.DisplayDialog("成功", "已添加控制按钮！", "确定");
        }
        
        private static GameObject CreateButton(string name, string text, Transform parent)
        {
            // 创建按钮对象
            GameObject buttonObj = new GameObject(name);
            buttonObj.transform.SetParent(parent, false);
            
            // 添加 RectTransform
            RectTransform rect = buttonObj.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(120f, 40f);
            
            // 添加 Image 组件
            Image image = buttonObj.AddComponent<Image>();
            image.color = new Color(0.2f, 0.6f, 0.8f, 1f);
            
            // 添加 Button 组件
            Button button = buttonObj.AddComponent<Button>();
            button.targetGraphic = image;
            
            // 创建文本子对象
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);
            
            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            
            Text textComponent = textObj.AddComponent<Text>();
            textComponent.text = text;
            textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            textComponent.fontSize = 18;
            textComponent.alignment = TextAnchor.MiddleCenter;
            textComponent.color = Color.white;
            
            return buttonObj;
        }
    }
}
