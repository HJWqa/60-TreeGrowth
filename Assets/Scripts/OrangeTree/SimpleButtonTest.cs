using UnityEngine;
using UnityEngine.EventSystems;

namespace TreePlanQAQ.OrangeTree
{
    /// <summary>
    /// 简单的按钮测试 - 检测鼠标点击
    /// </summary>
    public class SimpleButtonTest : MonoBehaviour, IPointerClickHandler
    {
        public enum ButtonType
        {
            Pause,
            Reset
        }
        
        public ButtonType buttonType = ButtonType.Pause;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"按钮被点击: {buttonType}");
            
            var treeController = FindObjectOfType<OrangeTreeController>();
            if (treeController == null)
            {
                Debug.LogError("找不到 OrangeTreeController！");
                return;
            }
            
            switch (buttonType)
            {
                case ButtonType.Pause:
                    treeController.TogglePause();
                    
                    // 更新文字
                    var text = GetComponent<UnityEngine.UI.Text>();
                    if (text != null)
                    {
                        text.text = treeController.IsPaused ? "[ 继续 ]" : "[ 暂停 ]";
                    }
                    
                    Debug.Log($"生长状态: {(treeController.IsPaused ? "已暂停" : "继续中")}");
                    break;
                    
                case ButtonType.Reset:
                    treeController.ResetGrowth();
                    Debug.Log("橘子树已重置到种子阶段");
                    break;
            }
        }
    }
}
