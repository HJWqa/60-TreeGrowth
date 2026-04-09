using UnityEngine;

namespace TreePlanQAQ.OrangeTree
{
    /// <summary>
    /// 调试用的简单按钮UI - 使用 OnGUI 绘制
    /// </summary>
    public class DebugButtonUI : MonoBehaviour
    {
        private OrangeTreeController treeController;
        
        private void Start()
        {
            treeController = FindObjectOfType<OrangeTreeController>();
            if (treeController == null)
            {
                Debug.LogError("找不到 OrangeTreeController！");
            }
        }
        
        private void OnGUI()
        {
            if (treeController == null) return;
            
            // 设置按钮样式
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 20;
            buttonStyle.fixedWidth = 120;
            buttonStyle.fixedHeight = 50;
            
            // 暂停按钮
            if (GUI.Button(new Rect(10, Screen.height - 70, 120, 50), 
                treeController.IsPaused ? "继续" : "暂停", buttonStyle))
            {
                treeController.TogglePause();
                Debug.Log($"点击了暂停按钮 - 当前状态: {(treeController.IsPaused ? "已暂停" : "运行中")}");
            }
            
            // 重置按钮
            if (GUI.Button(new Rect(140, Screen.height - 70, 120, 50), 
                "重置", buttonStyle))
            {
                treeController.ResetGrowth();
                Debug.Log("点击了重置按钮");
            }
            
            // 显示状态
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontSize = 16;
            labelStyle.normal.textColor = Color.white;
            
            GUI.Label(new Rect(10, Screen.height - 100, 300, 30), 
                $"状态: {(treeController.IsPaused ? "已暂停" : "运行中")} | 生长: {treeController.CurrentGrowth:F1}%", 
                labelStyle);
            
            // 绘制进度条
            float barWidth = 300f;
            float barHeight = 30f;
            float barX = 10f;
            float barY = Screen.height - 140f;
            
            // 背景
            GUI.Box(new Rect(barX, barY, barWidth, barHeight), "");
            
            // 填充
            float fillWidth = barWidth * (treeController.CurrentGrowth / 100f);
            GUI.color = new Color(0.2f, 0.8f, 0.3f, 1f); // 绿色
            GUI.Box(new Rect(barX, barY, fillWidth, barHeight), "");
            GUI.color = Color.white;
            
            // 进度文字
            GUIStyle progressStyle = new GUIStyle(GUI.skin.label);
            progressStyle.fontSize = 14;
            progressStyle.alignment = TextAnchor.MiddleCenter;
            progressStyle.normal.textColor = Color.white;
            GUI.Label(new Rect(barX, barY, barWidth, barHeight), 
                $"{treeController.CurrentGrowth:F1}%", progressStyle);
        }
    }
}
