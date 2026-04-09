using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace TreePlanQAQ.Editor
{
    /// <summary>
    /// 清理旧橘子树系统
    /// </summary>
    public class CleanupOldSystem : EditorWindow
    {
        [MenuItem("TreePlanQAQ/Cleanup Old System")]
        public static void ShowWindow()
        {
            GetWindow<CleanupOldSystem>("清理旧系统");
        }
        
        private void OnGUI()
        {
            GUILayout.Label("清理旧橘子树系统", EditorStyles.boldLabel);
            GUILayout.Space(10);
            
            EditorGUILayout.HelpBox(
                "这个工具会删除场景中旧系统的对象：\n\n" +
                "• EnvironmentManager (旧)\n" +
                "• EnvironmentCanvas (旧)\n" +
                "• GameObject (1) 和 GameObject (2)\n" +
                "• FruitTree (旧)\n" +
                "• OrangeTreeGenerator\n" +
                "• OrangeTreePreview_Test\n\n" +
                "注意：这个操作不可撤销！",
                MessageType.Warning
            );
            
            GUILayout.Space(10);
            
            if (GUILayout.Button("扫描旧对象", GUILayout.Height(30)))
            {
                ScanOldObjects();
            }
            
            GUILayout.Space(10);
            
            GUI.backgroundColor = new Color(1f, 0.5f, 0.5f);
            if (GUILayout.Button("删除所有旧对象", GUILayout.Height(40)))
            {
                if (EditorUtility.DisplayDialog(
                    "确认删除",
                    "确定要删除所有旧系统对象吗？\n这个操作不可撤销！",
                    "删除",
                    "取消"))
                {
                    CleanupAll();
                }
            }
            GUI.backgroundColor = Color.white;
        }
        
        private void ScanOldObjects()
        {
            string[] oldObjectNames = new string[]
            {
                "EnvironmentManager",
                "EnvironmentCanvas",
                "GameObject (1)",
                "GameObject (2)",
                "FruitTree",
                "OrangeTreeGenerator",
                "OrangeTreePreview_Test"
            };
            
            int foundCount = 0;
            string foundList = "";
            
            foreach (string name in oldObjectNames)
            {
                GameObject obj = GameObject.Find(name);
                if (obj != null)
                {
                    foundCount++;
                    foundList += $"• {name}\n";
                }
            }
            
            if (foundCount > 0)
            {
                EditorUtility.DisplayDialog(
                    "扫描结果",
                    $"找到 {foundCount} 个旧对象：\n\n{foundList}",
                    "确定"
                );
            }
            else
            {
                EditorUtility.DisplayDialog(
                    "扫描结果",
                    "没有找到旧对象，场景已清理干净！",
                    "确定"
                );
            }
        }
        
        private void CleanupAll()
        {
            string[] oldObjectNames = new string[]
            {
                "EnvironmentManager",
                "EnvironmentCanvas",
                "GameObject (1)",
                "GameObject (2)",
                "FruitTree",
                "OrangeTreeGenerator",
                "OrangeTreePreview_Test"
            };
            
            int deletedCount = 0;
            
            foreach (string name in oldObjectNames)
            {
                GameObject obj = GameObject.Find(name);
                if (obj != null)
                {
                    Debug.Log($"删除旧对象: {name}");
                    DestroyImmediate(obj);
                    deletedCount++;
                }
            }
            
            // 标记场景为已修改
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            
            EditorUtility.DisplayDialog(
                "清理完成",
                $"已删除 {deletedCount} 个旧对象！\n\n" +
                "请保存场景以保留更改。\n" +
                "现在可以使用新的橘子树系统了。",
                "确定"
            );
            
            Debug.Log($"✅ 清理完成！删除了 {deletedCount} 个旧对象");
        }
    }
}
