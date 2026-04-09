using UnityEngine;
using UnityEngine.UI;

namespace TreePlanQAQ.UI
{
    /// <summary>
    /// 三角形图形组件 - 用于创建向上或向下的三角形按钮
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class TriangleImage : MaskableGraphic
    {
        [Header("三角形设置")]
        [Tooltip("true=正三角形(向上), false=倒三角形(向下)")]
        public bool pointUp = true;
        
        [Tooltip("三角形的填充颜色")]
        public Color triangleColor = Color.white;
        
        protected override void Start()
        {
            base.Start();
            color = triangleColor;
        }
        
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            
            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;
            
            UIVertex vertex = UIVertex.simpleVert;
            vertex.color = color;
            
            if (pointUp)
            {
                // 正三角形 (▲)
                // 顶点
                vertex.position = new Vector3(0, height / 2);
                vh.AddVert(vertex);
                // 左下角
                vertex.position = new Vector3(-width / 2, -height / 2);
                vh.AddVert(vertex);
                // 右下角
                vertex.position = new Vector3(width / 2, -height / 2);
                vh.AddVert(vertex);
            }
            else
            {
                // 倒三角形 (▼)
                // 底部顶点
                vertex.position = new Vector3(0, -height / 2);
                vh.AddVert(vertex);
                // 右上角
                vertex.position = new Vector3(width / 2, height / 2);
                vh.AddVert(vertex);
                // 左上角
                vertex.position = new Vector3(-width / 2, height / 2);
                vh.AddVert(vertex);
            }
            
            vh.AddTriangle(0, 1, 2);
        }
        
        /// <summary>
        /// 切换三角形方向
        /// </summary>
        public void ToggleDirection()
        {
            pointUp = !pointUp;
            SetVerticesDirty();
        }
        
        /// <summary>
        /// 设置三角形颜色
        /// </summary>
        public void SetTriangleColor(Color newColor)
        {
            triangleColor = newColor;
            color = newColor;
            SetVerticesDirty();
        }
        
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            color = triangleColor;
        }
#endif
    }
}
