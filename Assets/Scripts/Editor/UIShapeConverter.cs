using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace TreePlanQAQ.Editor
{
    /// <summary>
    /// UI形状转换工具 - 将按钮转换为圆形和三角形
    /// </summary>
    public class UIShapeConverter : EditorWindow
    {
        [MenuItem("TreePlanQAQ/UI Tools/Convert Buttons to Shapes")]
        public static void ShowWindow()
        {
            GetWindow<UIShapeConverter>("UI形状转换");
        }
        
        private void OnGUI()
        {
            GUILayout.Label("UI形状转换工具", EditorStyles.boldLabel);
            GUILayout.Space(10);
            
            EditorGUILayout.HelpBox(
                "这个工具会创建圆形背景的按钮，\n" +
                "温度/湿度调节按钮会在圆形背景中包含三角形图标 ⏶⏷",
                MessageType.Info
            );
            
            GUILayout.Space(10);
            
            if (GUILayout.Button("🎯 自动设置所有UI形状（推荐）", GUILayout.Height(50)))
            {
                AutoConvertAllButtons();
            }
            
            GUILayout.Space(20);
            GUILayout.Label("手动转换选中的按钮：", EditorStyles.boldLabel);
            GUILayout.Space(5);
            
            if (GUILayout.Button("转换为纯圆形按钮", GUILayout.Height(35)))
            {
                foreach (GameObject obj in Selection.gameObjects)
                {
                    ConvertToCircleButton(obj, false);
                }
            }
            
            GUILayout.Space(5);
            
            if (GUILayout.Button("转换为圆形+正三角形 ⏶", GUILayout.Height(35)))
            {
                foreach (GameObject obj in Selection.gameObjects)
                {
                    ConvertToCircleWithTriangle(obj, true);
                }
            }
            
            GUILayout.Space(5);
            
            if (GUILayout.Button("转换为圆形+倒三角形 ⏷", GUILayout.Height(35)))
            {
                foreach (GameObject obj in Selection.gameObjects)
                {
                    ConvertToCircleWithTriangle(obj, false);
                }
            }
            
            GUILayout.Space(10);
            
            EditorGUILayout.HelpBox(
                "按钮命名规范：\n" +
                "• 暂停/重置：pause, reset, 暂停, 重置\n" +
                "• 增加按钮：up, increase, 上, 增加\n" +
                "• 减少按钮：down, decrease, 下, 减少",
                MessageType.None
            );
        }
        
        private void ConvertToCircle()
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                Button button = obj.GetComponent<Button>();
                if (button != null)
                {
                    // 移除现有的Image组件的Sprite
                    Image image = obj.GetComponent<Image>();
                    if (image != null)
                    {
                        // 使用Unity内置的圆形Sprite
                        image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
                        image.type = Image.Type.Simple;
                        
                        // 设置为正方形以保持圆形
                        RectTransform rect = obj.GetComponent<RectTransform>();
                        float size = Mathf.Min(rect.sizeDelta.x, rect.sizeDelta.y);
                        rect.sizeDelta = new Vector2(size, size);
                        
                        EditorUtility.SetDirty(obj);
                        Debug.Log($"✅ 已将 {obj.name} 转换为圆形按钮");
                    }
                }
            }
        }
        
        private void ConvertToTriangle(bool pointUp)
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                Button button = obj.GetComponent<Button>();
                if (button != null)
                {
                    // 移除Image组件
                    Image image = obj.GetComponent<Image>();
                    if (image != null)
                    {
                        DestroyImmediate(image);
                    }
                    
                    // 添加TriangleImage组件
                    var triangleImage = obj.AddComponent<TreePlanQAQ.UI.TriangleImage>();
                    triangleImage.pointUp = pointUp;
                    triangleImage.triangleColor = Color.white;
                    
                    // 设置按钮的target graphic
                    button.targetGraphic = triangleImage;
                    
                    EditorUtility.SetDirty(obj);
                    Debug.Log($"✅ 已将 {obj.name} 转换为{(pointUp ? "正" : "倒")}三角形按钮");
                }
            }
        }
        
        private void AutoConvertAllButtons()
        {
            // 查找场景中的所有按钮
            Button[] allButtons = FindObjectsOfType<Button>();
            
            int convertedCount = 0;
            
            foreach (Button button in allButtons)
            {
                // 先清理旧的TriangleImage组件
                var oldTriangle = button.GetComponent<TreePlanQAQ.UI.TriangleImage>();
                if (oldTriangle != null)
                {
                    DestroyImmediate(oldTriangle);
                }
                
                string name = button.gameObject.name.ToLower();
                
                // 根据按钮名称自动判断类型
                if (name.Contains("pause") || name.Contains("reset") || 
                    name.Contains("暂停") || name.Contains("重置") ||
                    name.Contains("circle") || name.Contains("圆"))
                {
                    // 转换为纯圆形按钮（暂停/重置）
                    ConvertToCircleButton(button.gameObject, name.Contains("pause") || name.Contains("暂停"));
                    convertedCount++;
                    Debug.Log($"✅ {button.gameObject.name} -> 圆形按钮");
                }
                else if (name.Contains("up") || name.Contains("increase") || 
                         name.Contains("上") || name.Contains("增加") || name.Contains("加"))
                {
                    // 转换为圆形背景+正三角形
                    ConvertToCircleWithTriangle(button.gameObject, true);
                    convertedCount++;
                    Debug.Log($"✅ {button.gameObject.name} -> 圆形+正三角形 ⏶");
                }
                else if (name.Contains("down") || name.Contains("decrease") || 
                         name.Contains("下") || name.Contains("减少") || name.Contains("减"))
                {
                    // 转换为圆形背景+倒三角形
                    ConvertToCircleWithTriangle(button.gameObject, false);
                    convertedCount++;
                    Debug.Log($"✅ {button.gameObject.name} -> 圆形+倒三角形 ⏷");
                }
            }
            
            if (convertedCount > 0)
            {
                EditorUtility.DisplayDialog(
                    "转换完成",
                    $"成功转换了 {convertedCount} 个按钮！\n\n" +
                    "所有按钮都已设置为圆形背景。\n" +
                    "温度/湿度调节按钮包含三角形图标。",
                    "确定"
                );
            }
            else
            {
                EditorUtility.DisplayDialog(
                    "未找到按钮",
                    "场景中没有找到可以转换的按钮。\n\n" +
                    "请确保按钮名称包含关键词：\n" +
                    "- 圆形：pause, reset, 暂停, 重置\n" +
                    "- 圆形+正三角：up, increase, 上, 增加\n" +
                    "- 圆形+倒三角：down, decrease, 下, 减少",
                    "确定"
                );
            }
        }
        
        /// <summary>
        /// 转换为纯圆形按钮（用于暂停/重置）
        /// </summary>
        private void ConvertToCircleButton(GameObject obj, bool isPause)
        {
            // 移除旧的TriangleImage组件
            var oldTriangle = obj.GetComponent<TreePlanQAQ.UI.TriangleImage>();
            if (oldTriangle != null)
            {
                DestroyImmediate(oldTriangle);
            }
            
            // 移除旧的Image组件
            Image oldImage = obj.GetComponent<Image>();
            if (oldImage != null)
            {
                DestroyImmediate(oldImage);
            }
            
            // 添加CircleImage组件（纯色圆形）
            var circleImage = obj.AddComponent<TreePlanQAQ.UI.CircleImage>();
            circleImage.filled = true;
            circleImage.segments = 36;
            circleImage.circleColor = isPause ? new Color(0.4f, 0.8f, 0.4f) : new Color(1f, 0.6f, 0.2f);
            
            // 设置大小为正方形 - 240x240
            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(240f, 240f); // 再大一倍
            
            // 设置Button的target graphic
            Button button = obj.GetComponent<Button>();
            if (button != null)
            {
                button.targetGraphic = circleImage;
            }
            
            EditorUtility.SetDirty(obj);
        }
        
        /// <summary>
        /// 转换为圆形背景+三角形图标的按钮
        /// </summary>
        private void ConvertToCircleWithTriangle(GameObject obj, bool pointUp)
        {
            Button button = obj.GetComponent<Button>();
            if (button == null) return;
            
            // 1. 移除按钮上的TriangleImage组件（如果有）
            var oldTriangle = obj.GetComponent<TreePlanQAQ.UI.TriangleImage>();
            if (oldTriangle != null)
            {
                DestroyImmediate(oldTriangle);
            }
            
            // 2. 移除旧的Image组件，使用CircleImage代替
            Image oldImage = obj.GetComponent<Image>();
            if (oldImage != null)
            {
                DestroyImmediate(oldImage);
            }
            
            // 3. 添加CircleImage组件（纯色圆形，无边框）
            var circleImage = obj.AddComponent<TreePlanQAQ.UI.CircleImage>();
            circleImage.filled = true; // 填充模式
            circleImage.segments = 36; // 圆滑度
            circleImage.circleColor = pointUp ? new Color(1f, 0.4f, 0.4f) : new Color(0.4f, 0.6f, 1f);
            
            // 4. 设置按钮大小为正方形（圆形）- 240x240
            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(240f, 240f); // 再大一倍
            
            // 5. 删除旧的子对象
            for (int i = obj.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(obj.transform.GetChild(i).gameObject);
            }
            
            // 6. 创建三角形图标子对象
            GameObject triangleObj = new GameObject("TriangleIcon");
            triangleObj.transform.SetParent(obj.transform, false);
            
            RectTransform triangleRect = triangleObj.AddComponent<RectTransform>();
            triangleRect.anchorMin = new Vector2(0.5f, 0.5f);
            triangleRect.anchorMax = new Vector2(0.5f, 0.5f);
            triangleRect.pivot = new Vector2(0.5f, 0.5f);
            triangleRect.anchoredPosition = Vector2.zero;
            triangleRect.sizeDelta = new Vector2(100f, 100f); // 三角形也相应变大
            
            // 7. 添加三角形组件 - 白色
            var triangleImage = triangleObj.AddComponent<TreePlanQAQ.UI.TriangleImage>();
            triangleImage.pointUp = pointUp;
            triangleImage.triangleColor = Color.white; // 白色三角形
            
            // 8. 设置按钮的target graphic为背景圆形
            button.targetGraphic = circleImage;
            
            EditorUtility.SetDirty(obj);
            
            Debug.Log($"✅ {obj.name} 已转换为纯色圆形背景+白色三角形图标");
        }
    }
    
    /// <summary>
    /// 快速菜单项 - 右键点击按钮时显示
    /// </summary>
    public static class UIShapeContextMenu
    {
        [MenuItem("GameObject/UI/Convert to Circle Button", false, 0)]
        private static void ConvertToCircle(MenuCommand command)
        {
            GameObject obj = command.context as GameObject;
            if (obj != null)
            {
                Button button = obj.GetComponent<Button>();
                if (button != null)
                {
                    Image image = obj.GetComponent<Image>();
                    if (image != null)
                    {
                        image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
                        image.type = Image.Type.Simple;
                        
                        RectTransform rect = obj.GetComponent<RectTransform>();
                        float size = Mathf.Min(rect.sizeDelta.x, rect.sizeDelta.y);
                        rect.sizeDelta = new Vector2(size, size);
                        
                        EditorUtility.SetDirty(obj);
                        Debug.Log($"✅ {obj.name} 已转换为圆形按钮");
                    }
                }
            }
        }
        
        [MenuItem("GameObject/UI/Convert to Triangle Button ▲", false, 1)]
        private static void ConvertToTriangleUp(MenuCommand command)
        {
            GameObject obj = command.context as GameObject;
            if (obj != null)
            {
                ConvertToTriangleButton(obj, true);
            }
        }
        
        [MenuItem("GameObject/UI/Convert to Triangle Button ▼", false, 2)]
        private static void ConvertToTriangleDown(MenuCommand command)
        {
            GameObject obj = command.context as GameObject;
            if (obj != null)
            {
                ConvertToTriangleButton(obj, false);
            }
        }
        
        private static void ConvertToTriangleButton(GameObject obj, bool pointUp)
        {
            Button button = obj.GetComponent<Button>();
            if (button != null)
            {
                Image image = obj.GetComponent<Image>();
                if (image != null)
                {
                    Object.DestroyImmediate(image);
                }
                
                var triangleImage = obj.AddComponent<TreePlanQAQ.UI.TriangleImage>();
                triangleImage.pointUp = pointUp;
                triangleImage.triangleColor = Color.white;
                button.targetGraphic = triangleImage;
                
                EditorUtility.SetDirty(obj);
                Debug.Log($"✅ {obj.name} 已转换为{(pointUp ? "正" : "倒")}三角形按钮");
            }
        }
    }
}
