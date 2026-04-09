using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TreePlanQAQ.OrangeTree;

namespace TreePlanQAQ.Editor
{
    /// <summary>
    /// UI错误修复工具
    /// </summary>
    public class UIErrorFixer : EditorWindow
    {
        [MenuItem("TreePlanQAQ/Fix/Fix UI Errors")]
        public static void ShowWindow()
        {
            GetWindow<UIErrorFixer>("修复UI错误");
        }
        
        [MenuItem("TreePlanQAQ/Fix/Quick Fix All UI Issues")]
        public static void QuickFixAll()
        {
            int fixedCount = 0;
            
            // 1. 修复GrowthUIController的引用
            fixedCount += FixGrowthUIControllers();
            
            // 2. 检查并创建OrangeTreeController
            fixedCount += EnsureOrangeTreeController();
            
            EditorUtility.DisplayDialog(
                "修复完成",
                $"已修复 {fixedCount} 个问题！\n\n" +
                "请检查Console查看详细信息。",
                "确定"
            );
        }
        
        private void OnGUI()
        {
            GUILayout.Label("UI错误修复工具", EditorStyles.boldLabel);
            GUILayout.Space(10);
            
            EditorGUILayout.HelpBox(
                "这个工具会自动修复常见的UI错误：\n" +
                "• 自动连接按钮引用\n" +
                "• 创建缺失的OrangeTreeController\n" +
                "• 修复空引用问题",
                MessageType.Info
            );
            
            GUILayout.Space(20);
            
            if (GUILayout.Button("🔧 一键修复所有问题", GUILayout.Height(50)))
            {
                QuickFixAll();
            }
            
            GUILayout.Space(10);
            
            if (GUILayout.Button("修复GrowthUIController引用", GUILayout.Height(35)))
            {
                int count = FixGrowthUIControllers();
                EditorUtility.DisplayDialog("完成", $"已修复 {count} 个GrowthUIController", "确定");
            }
            
            GUILayout.Space(5);
            
            if (GUILayout.Button("创建OrangeTreeController", GUILayout.Height(35)))
            {
                int count = EnsureOrangeTreeController();
                EditorUtility.DisplayDialog("完成", count > 0 ? "已创建OrangeTreeController" : "OrangeTreeController已存在", "确定");
            }
        }
        
        /// <summary>
        /// 修复GrowthUIController的引用
        /// </summary>
        private static int FixGrowthUIControllers()
        {
            int fixedCount = 0;
            GrowthUIController[] controllers = FindObjectsOfType<GrowthUIController>();
            
            foreach (var controller in controllers)
            {
                bool hasFixed = false;
                
                // 查找按钮
                if (controller.pauseButton == null)
                {
                    Button pauseBtn = FindButtonByName("pause", "暂停", "Pause");
                    if (pauseBtn != null)
                    {
                        SerializedObject so = new SerializedObject(controller);
                        so.FindProperty("pauseButton").objectReferenceValue = pauseBtn;
                        so.ApplyModifiedProperties();
                        Debug.Log($"✅ 已连接暂停按钮: {pauseBtn.name}");
                        hasFixed = true;
                    }
                }
                
                if (controller.resetButton == null)
                {
                    Button resetBtn = FindButtonByName("reset", "重置", "Reset");
                    if (resetBtn != null)
                    {
                        SerializedObject so = new SerializedObject(controller);
                        so.FindProperty("resetButton").objectReferenceValue = resetBtn;
                        so.ApplyModifiedProperties();
                        Debug.Log($"✅ 已连接重置按钮: {resetBtn.name}");
                        hasFixed = true;
                    }
                }
                
                if (hasFixed)
                {
                    EditorUtility.SetDirty(controller);
                    fixedCount++;
                }
            }
            
            return fixedCount;
        }
        
        /// <summary>
        /// 确保场景中有OrangeTreeController
        /// </summary>
        private static int EnsureOrangeTreeController()
        {
            OrangeTreeController existing = FindObjectOfType<OrangeTreeController>();
            if (existing != null)
            {
                Debug.Log("✅ OrangeTreeController已存在");
                return 0;
            }
            
            // 创建新的GameObject
            GameObject treeObj = new GameObject("OrangeTree");
            treeObj.AddComponent<OrangeTreeController>();
            
            Debug.Log("✅ 已创建OrangeTreeController");
            return 1;
        }
        
        /// <summary>
        /// 根据名称查找按钮
        /// </summary>
        private static Button FindButtonByName(params string[] keywords)
        {
            Button[] allButtons = FindObjectsOfType<Button>();
            
            foreach (var button in allButtons)
            {
                string name = button.gameObject.name.ToLower();
                foreach (var keyword in keywords)
                {
                    if (name.Contains(keyword.ToLower()))
                    {
                        return button;
                    }
                }
            }
            
            return null;
        }
    }
}
