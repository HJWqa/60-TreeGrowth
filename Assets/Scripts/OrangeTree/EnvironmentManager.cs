using System;
using UnityEngine;

namespace TreePlanQAQ.OrangeTree
{
    /// <summary>
    /// 环境管理器
    /// </summary>
    public class EnvironmentManager : MonoBehaviour
    {
        [Header("环境参数")]
        [SerializeField]
        [Range(-20f, 50f)]
        private float temperature = 25f;
        
        [SerializeField]
        [Range(0f, 100f)]
        private float humidity = 60f;
        
        [SerializeField]
        [Range(0f, 1000f)]
        private float sunlight = 600f;
        
        [Header("自动变化")]
        public bool autoChange = false;
        
        [Range(0.1f, 5f)]
        public float changeSpeed = 1f;
        
        // 事件
        public event Action<float, float, float> OnEnvironmentChanged;
        
        // 属性
        public float Temperature => temperature;
        public float Humidity => humidity;
        public float Sunlight => sunlight;
        
        // 单例
        public static EnvironmentManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Update()
        {
            if (autoChange)
            {
                // 自动变化环境参数（用于演示）
                float time = Time.time * changeSpeed;
                temperature = 25f + Mathf.Sin(time * 0.5f) * 10f;
                humidity = 60f + Mathf.Cos(time * 0.3f) * 20f;
                sunlight = 600f + Mathf.Sin(time * 0.7f) * 300f;
                
                NotifyEnvironmentChanged();
            }
        }
        
        /// <summary>
        /// 设置温度
        /// </summary>
        public void SetTemperature(float value)
        {
            temperature = Mathf.Clamp(value, -20f, 50f);
            NotifyEnvironmentChanged();
        }
        
        /// <summary>
        /// 设置湿度
        /// </summary>
        public void SetHumidity(float value)
        {
            humidity = Mathf.Clamp(value, 0f, 100f);
            NotifyEnvironmentChanged();
        }
        
        /// <summary>
        /// 设置光照
        /// </summary>
        public void SetSunlight(float value)
        {
            sunlight = Mathf.Clamp(value, 0f, 1000f);
            NotifyEnvironmentChanged();
        }
        
        /// <summary>
        /// 通知环境变化
        /// </summary>
        private void NotifyEnvironmentChanged()
        {
            OnEnvironmentChanged?.Invoke(temperature, humidity, sunlight);
            
            // 更新所有橘子树
            OrangeTreeController[] trees = FindObjectsOfType<OrangeTreeController>();
            foreach (var tree in trees)
            {
                tree.SetEnvironment(temperature, humidity, sunlight);
            }
        }
        
        private void OnValidate()
        {
            // 在Inspector中修改值时也触发更新
            if (Application.isPlaying)
            {
                NotifyEnvironmentChanged();
            }
        }
    }
}
