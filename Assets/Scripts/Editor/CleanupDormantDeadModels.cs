using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace TreePlanQAQ.Editor
{
    /// <summary>
    /// 清理场景中的Dormant和Dead模型
    /// </summary>
    public class CleanupDormantDeadModels : EditorWindow
    {
        [MenuItem("TreePlanQAQ/Cleanup Dormant and Dead Models")]
        public static void ShowWindow()
        {
            GetWindow<CleanupDormantDeadModels>("清理模型");
        }
        
        private void OnGUI()
        {
            GUILayout.Label("清理Dormant和Dead模型", EditorStyles.boldLabel);
            GUILayout.Space(10);
            
            EditorGUILayout.HelpBox(
                "这个工具会从场景中删除以下对象：\n" +
                "• PlantModel_Dormant\n" +
                "• PlantModel_Dead\n\n" +
                "这些对象已经从代码中移除，不再需要。",
                MessageType.Info
            );
            
            GUILayout.Space(10);
            
            if (GUILayout.Button("执行清理", GUILayout.Height(40)))
            {
                CleanupModels();
            }
        }
        
        private static void CleanupModels()
        {
            // 查找并删除Dormant模型
            GameObject dormantModel = GameObject.Find("PlantModel_Dormant");
            if (dormantModel != null)
            {
                DestroyImmediate(dormantModel);
                Debug.Log("✅ 已删除 PlantModel_Dormant");
            }
            else
            {
                Debug.Log("⚠️ 未找到 PlantModel_Dormant");
            }
            
            // 查找并删除Dead模型
            GameObject deadModel = GameObject.Find("PlantModel_Dead");
            if (deadModel != null)
            {
                DestroyImmediate(deadModel);
                Debug.Log("✅ 已删除 PlantModel_Dead");
            }
            else
            {
                Debug.Log("⚠️ 未找到 PlantModel_Dead");
            }
            
            // 标记场景为已修改
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            
            EditorUtility.DisplayDialog(
                "清理完成",
                "已删除Dormant和Dead模型！\n请保存场景以保留更改。",
                "确定"
            );
        }
    }
}
