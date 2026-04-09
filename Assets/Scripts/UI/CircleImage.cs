using UnityEngine;
using UnityEngine.UI;

namespace TreePlanQAQ.UI
{
    /// <summary>
    /// 圆形图形组件 - 用于创建圆形按钮背景
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class CircleImage : MaskableGraphic
    {
        [Header("圆形设置")]
        [Tooltip("圆形的填充颜色")]
        public Color circleColor = Color.white;
        
        [Tooltip("圆形的分段数，越大越圆滑")]
        [Range(3, 100)]
        public int segments = 36;
        
        [Tooltip("是否填充圆形")]
        public bool filled = true;
        
        [Tooltip("如果不填充，边框的厚度")]
        [Range(1, 20)]
        public float thickness = 5f;
        
        protected override void Start()
        {
            base.Start();
            color = circleColor;
        }
        
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            
            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;
            float radius = Mathf.Min(width, height) / 2f;
            
            if (filled)
            {
                DrawFilledCircle(vh, radius);
            }
            else
            {
                DrawCircleOutline(vh, radius);
            }
        }
        
        private void DrawFilledCircle(VertexHelper vh, float radius)
        {
            UIVertex vertex = UIVertex.simpleVert;
            vertex.color = color;
            
            // 中心点
            vertex.position = Vector3.zero;
            vh.AddVert(vertex);
            
            // 圆周上的点
            float angleStep = 360f / segments;
            for (int i = 0; i <= segments; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                float x = Mathf.Cos(angle) * radius;
                float y = Mathf.Sin(angle) * radius;
                
                vertex.position = new Vector3(x, y);
                vh.AddVert(vertex);
                
                if (i > 0)
                {
                    vh.AddTriangle(0, i, i + 1);
                }
            }
        }
        
        private void DrawCircleOutline(VertexHelper vh, float radius)
        {
            UIVertex vertex = UIVertex.simpleVert;
            vertex.color = color;
            
            float innerRadius = radius - thickness;
            float angleStep = 360f / segments;
            
            for (int i = 0; i <= segments; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                float cos = Mathf.Cos(angle);
                float sin = Mathf.Sin(angle);
                
                // 外圆点
                vertex.position = new Vector3(cos * radius, sin * radius);
                vh.AddVert(vertex);
                
                // 内圆点
                vertex.position = new Vector3(cos * innerRadius, sin * innerRadius);
                vh.AddVert(vertex);
                
                if (i > 0)
                {
                    int index = i * 2;
                    vh.AddTriangle(index - 2, index - 1, index);
                    vh.AddTriangle(index, index - 1, index + 1);
                }
            }
        }
        
        /// <summary>
        /// 设置圆形颜色
        /// </summary>
        public void SetCircleColor(Color newColor)
        {
            circleColor = newColor;
            color = newColor;
            SetVerticesDirty();
        }
        
        /// <summary>
        /// 设置分段数
        /// </summary>
        public void SetSegments(int newSegments)
        {
            segments = Mathf.Clamp(newSegments, 3, 100);
            SetVerticesDirty();
        }
        
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            color = circleColor;
            segments = Mathf.Clamp(segments, 3, 100);
        }
#endif
    }
}
