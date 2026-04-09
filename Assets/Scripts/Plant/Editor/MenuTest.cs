using UnityEngine;
using UnityEditor;

namespace TreePlanQAQ.Plant.Editor
{
    /// <summary>
    /// 测试菜单项是否正常工作
    /// </summary>
    public static class MenuTest
    {
        [MenuItem("TreePlanQAQ/Test Menu Item")]
        public static void TestMenuItem()
        {
            Debug.Log("✅ 菜单项工作正常！");
            EditorUtility.DisplayDialog("测试成功", "TreePlanQAQ菜单项工作正常！", "确定");
        }
    }
}
