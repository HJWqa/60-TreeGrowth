using UnityEngine;
using UnityEditor;

namespace TreePlanQAQ.Editor
{
    public class AddSimpleButtonTest : EditorWindow
    {
        [MenuItem("TreePlanQAQ/Add Simple Button Test")]
        public static void AddTest()
        {
            GameObject pauseBtn = GameObject.Find("PauseButtonText");
            GameObject resetBtn = GameObject.Find("ResetButtonText");
            
            if (pauseBtn != null)
            {
                var test = pauseBtn.GetComponent<TreePlanQAQ.OrangeTree.SimpleButtonTest>();
                if (test == null)
                {
                    test = pauseBtn.AddComponent<TreePlanQAQ.OrangeTree.SimpleButtonTest>();
                }
                test.buttonType = TreePlanQAQ.OrangeTree.SimpleButtonTest.ButtonType.Pause;
                EditorUtility.SetDirty(pauseBtn);
                Debug.Log("已添加暂停按钮测试组件");
            }
            
            if (resetBtn != null)
            {
                var test = resetBtn.GetComponent<TreePlanQAQ.OrangeTree.SimpleButtonTest>();
                if (test == null)
                {
                    test = resetBtn.AddComponent<TreePlanQAQ.OrangeTree.SimpleButtonTest>();
                }
                test.buttonType = TreePlanQAQ.OrangeTree.SimpleButtonTest.ButtonType.Reset;
                EditorUtility.SetDirty(resetBtn);
                Debug.Log("已添加重置按钮测试组件");
            }
            
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
            );
            
            EditorUtility.DisplayDialog("完成", "已添加按钮测试组件！\n现在进入 Play 模式点击按钮试试。", "确定");
        }
    }
}
