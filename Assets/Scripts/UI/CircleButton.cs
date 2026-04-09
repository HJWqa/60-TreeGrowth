using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 圆形按钮组件
/// 通过设置Image的fillAmount和type来创建圆形效果
/// </summary>
[RequireComponent(typeof(Image))]
public class CircleButton : MonoBehaviour
{
    private Image buttonImage;
    
    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        SetupCircleButton();
    }
    
    private void SetupCircleButton()
    {
        if (buttonImage != null)
        {
            // 设置为Filled类型以支持圆形
            buttonImage.type = Image.Type.Filled;
            buttonImage.fillMethod = Image.FillMethod.Radial360;
            buttonImage.fillAmount = 1f;
            buttonImage.fillClockwise = true;
            buttonImage.fillOrigin = 0;
        }
    }
    
    private void OnValidate()
    {
        if (buttonImage == null)
            buttonImage = GetComponent<Image>();
        
        SetupCircleButton();
    }
}