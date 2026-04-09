using UnityEngine;

/// <summary>
/// 在相机后面显示背景图片
/// </summary>
public class BackgroundImage : MonoBehaviour
{
    [Header("背景图片")]
    public Texture2D backgroundTexture;
    
    [Header("设置")]
    public float distance = 10f; // 距离相机的距离
    
    private GameObject backgroundQuad;
    private Camera mainCamera;
    
    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("找不到主相机！");
            return;
        }
        
        CreateBackgroundQuad();
    }
    
    private void CreateBackgroundQuad()
    {
        // 创建背景平面
        backgroundQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        backgroundQuad.name = "BackgroundImage";
        backgroundQuad.transform.SetParent(mainCamera.transform);
        
        // 移除碰撞体
        Destroy(backgroundQuad.GetComponent<Collider>());
        
        // 设置位置（在相机前方）
        backgroundQuad.transform.localPosition = new Vector3(0, 0, distance);
        backgroundQuad.transform.localRotation = Quaternion.identity;
        
        // 根据相机视野调整大小
        float height = 2f * distance * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float width = height * mainCamera.aspect;
        backgroundQuad.transform.localScale = new Vector3(width, height, 1);
        
        // 创建材质
        if (backgroundTexture != null)
        {
            Material mat = new Material(Shader.Find("Unlit/Texture"));
            mat.mainTexture = backgroundTexture;
            backgroundQuad.GetComponent<Renderer>().material = mat;
        }
        
        // 设置渲染层级，确保在最后面
        backgroundQuad.layer = LayerMask.NameToLayer("Default");
    }
    
    private void Update()
    {
        // 如果需要，可以让背景跟随相机旋转
        // backgroundQuad.transform.rotation = mainCamera.transform.rotation;
    }
}
