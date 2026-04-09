using UnityEngine;
using UnityEditor;

namespace TreePlanQAQ.Editor
{
    public class SetGrowthSpeed : EditorWindow
    {
        [MenuItem("TreePlanQAQ/Set Growth Speed/1 Minute (Fast)")]
        public static void SetFast()
        {
            SetSpeed(1.67f, "1分钟");
        }
        
        [MenuItem("TreePlanQAQ/Set Growth Speed/30 Seconds (Very Fast)")]
        public static void SetVeryFast()
        {
            SetSpeed(3.33f, "30秒");
        }
        
        [MenuItem("TreePlanQAQ/Set Growth Speed/2 Minutes (Normal)")]
        public static void SetNormal()
        {
            SetSpeed(0.83f, "2分钟");
        }
        
        [MenuItem("TreePlanQAQ/Set Growth Speed/5 Minutes (Slow)")]
        public static void SetSlow()
        {
            SetSpeed(0.33f, "5分钟");
        }
        
        private static void SetSpeed(float rate, string description)
        {
            // 查找配置文件
            string[] guids = AssetDatabase.FindAssets("t:GrowthConfig");
            if (guids.Length == 0)
            {
                EditorUtility.DisplayDialog("错误", "找不到 GrowthConfig 配置文件！", "确定");
                return;
            }
            
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            var config = AssetDatabase.LoadAssetAtPath<TreePlanQAQ.OrangeTree.GrowthConfig>(path);
            
            if (config != null)
            {
                config.baseGrowthRate = rate;
                EditorUtility.SetDirty(config);
                AssetDatabase.SaveAssets();
                
                EditorUtility.DisplayDialog("成功", 
                    $"生长速度已设置为: {description}\n" +
                    $"Base Growth Rate = {rate:F2}\n\n" +
                    $"重新进入 Play 模式生效。", 
                    "确定");
            }
        }
    }
}
