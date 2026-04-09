using UnityEngine;
using UnityEditor;

namespace TreePlanQAQ.Editor
{
    public class AddDebugButtonUI : EditorWindow
    {
        [MenuItem("TreePlanQAQ/Add Debug Button UI")]
        public static void Add()
        {
            // 查找或创建 DebugUI 对象
            GameObject debugUI = GameObject.Find("DebugUI");
            if (debugUI == null)
            {
                debugUI = new GameObject("DebugUI");
            }
            
            // 添加组件
            var component = debugUI.GetComponent<TreePlanQAQ.OrangeTree.DebugButtonUI>();
            if (component == null)
            {
                debugUI.AddComponent<TreePlanQAQ.OrangeTree.DebugButtonUI>();
            }
            
            EditorUtility.SetDirty(debugUI);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
            );
            
            EditorUtility.DisplayDialog("完成", 
                "已添加调试按钮UI！\n\n进入 Play 模式后，你会在屏幕左下角看到两个按钮：\n- 暂停/继续\n- 重置\n\n这些按钮使用 OnGUI 绘制，100% 可以点击。", 
                "确定");
        }
    }
}
