using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TreePlanQAQ.OrangeTree
{
    /// <summary>
    /// 橘子树主控制器
    /// </summary>
    public class OrangeTreeController : MonoBehaviour
    {
        [Header("配置")]
        public GrowthConfig config;
        
        [Header("阶段配置")]
        public List<StageData> stages = new List<StageData>();
        
        [Header("当前状态")]
        [SerializeField]
        [Range(0, 100)]
        private float currentGrowth = 0f;
        
        [SerializeField]
        private OrangeTreeStage currentStage = OrangeTreeStage.Seed;
        
        [Header("环境输入")]
        [SerializeField]
        [Range(-20f, 50f)]
        private float temperature = 25f;
        
        [SerializeField]
        [Range(0f, 100f)]
        private float humidity = 60f;
        
        [SerializeField]
        [Range(0f, 1000f)]
        private float sunlight = 600f;
        
        // 事件
        public event Action<OrangeTreeStage, OrangeTreeStage> OnStageChanged;
        public event Action<float> OnGrowthUpdated;
        
        // 私有变量
        private GameObject currentModel;
        private bool isTransitioning = false;
        private bool isPaused = false;
        
        // 属性
        public float CurrentGrowth => currentGrowth;
        public OrangeTreeStage CurrentStage => currentStage;
        public bool IsPaused => isPaused;
        
        private void Start()
        {
            InitializeStages();
            ShowStageModel(currentStage);
        }
        
        private void Update()
        {
            if (config == null || isTransitioning || isPaused)
            {
                if (isPaused && Time.frameCount % 60 == 0) // 每秒输出一次
                {
                    Debug.Log($"🌳 {gameObject.name} 已暂停 - isPaused = {isPaused}");
                }
                return;
            }
            
            // 计算生长
            float envFactor = config.CalculateEnvironmentFactor(temperature, humidity, sunlight);
            float growthDelta = config.baseGrowthRate * envFactor * Time.deltaTime;
            
            currentGrowth = Mathf.Clamp(currentGrowth + growthDelta, 0f, 100f);
            OnGrowthUpdated?.Invoke(currentGrowth);
            
            // 检查是否需要切换阶段
            OrangeTreeStage newStage = GetStageForGrowth(currentGrowth);
            if (newStage != currentStage)
            {
                StartCoroutine(TransitionToStage(newStage));
            }
        }
        
        /// <summary>
        /// 初始化阶段数据
        /// </summary>
        private void InitializeStages()
        {
            if (stages.Count == 0)
            {
                // 创建默认阶段配置
                stages.Add(new StageData(OrangeTreeStage.Seed, "种子", 0f));
                stages.Add(new StageData(OrangeTreeStage.Sprout, "发芽", 10f));
                stages.Add(new StageData(OrangeTreeStage.Seedling, "幼苗", 25f));
                stages.Add(new StageData(OrangeTreeStage.YoungTree, "小树", 50f));
                stages.Add(new StageData(OrangeTreeStage.MatureTree, "成树", 75f));
                stages.Add(new StageData(OrangeTreeStage.Flowering, "开花", 90f));
                stages.Add(new StageData(OrangeTreeStage.Fruiting, "结果", 95f));
                stages.Add(new StageData(OrangeTreeStage.Harvest, "成熟", 100f));
            }
        }
        
        /// <summary>
        /// 根据生长值获取对应阶段
        /// </summary>
        private OrangeTreeStage GetStageForGrowth(float growth)
        {
            for (int i = stages.Count - 1; i >= 0; i--)
            {
                if (growth >= stages[i].growthThreshold)
                {
                    return stages[i].stage;
                }
            }
            return OrangeTreeStage.Seed;
        }
        
        /// <summary>
        /// 过渡到新阶段
        /// </summary>
        private IEnumerator TransitionToStage(OrangeTreeStage newStage)
        {
            isTransitioning = true;
            
            OrangeTreeStage oldStage = currentStage;
            currentStage = newStage;
            
            Debug.Log($"🌳 阶段切换: {oldStage} → {newStage}");
            OnStageChanged?.Invoke(oldStage, newStage);
            
            // 淡出旧模型
            if (currentModel != null)
            {
                yield return StartCoroutine(FadeOutModel(currentModel, 0.5f));
                currentModel.SetActive(false);
            }
            
            // 显示新模型
            ShowStageModel(newStage);
            
            // 淡入新模型
            if (currentModel != null)
            {
                yield return StartCoroutine(FadeInModel(currentModel, 0.5f));
            }
            
            isTransitioning = false;
        }
        
        /// <summary>
        /// 显示指定阶段的模型
        /// </summary>
        private void ShowStageModel(OrangeTreeStage stage)
        {
            StageData stageData = stages.Find(s => s.stage == stage);
            
            if (stageData != null && stageData.stageModel != null)
            {
                currentModel = stageData.stageModel;
                currentModel.SetActive(true);
                
                // 使用场景中预设的 Transform 值，不再硬编码覆盖
            }
            else
            {
                Debug.LogWarning($"未找到阶段 {stage} 的模型");
            }
        }
        
        /// <summary>
        /// 淡出模型
        /// </summary>
        private IEnumerator FadeOutModel(GameObject model, float duration)
        {
            // 简单禁用，不修改 scale
            yield return null;
        }
        
        /// <summary>
        /// 淡入模型
        /// </summary>
        private IEnumerator FadeInModel(GameObject model, float duration)
        {
            // 简单启用，不修改 scale，保持场景中设置的值
            yield return null;
        }
        
        /// <summary>
        /// 设置环境参数
        /// </summary>
        public void SetEnvironment(float temp, float humid, float sun)
        {
            temperature = temp;
            humidity = humid;
            sunlight = sun;
        }
        
        /// <summary>
        /// 重置生长
        /// </summary>
        [ContextMenu("重置生长")]
        public void ResetGrowth()
        {
            currentGrowth = 0f;
            currentStage = OrangeTreeStage.Seed;
            
            if (currentModel != null)
            {
                currentModel.SetActive(false);
            }
            
            ShowStageModel(OrangeTreeStage.Seed);
            
            Debug.Log("🔄 橘子树已重置");
        }
        
        /// <summary>
        /// 设置生长值（用于测试）
        /// </summary>
        public void SetGrowth(float growth)
        {
            currentGrowth = Mathf.Clamp(growth, 0f, 100f);
            OrangeTreeStage newStage = GetStageForGrowth(currentGrowth);
            
            if (newStage != currentStage)
            {
                StartCoroutine(TransitionToStage(newStage));
            }
        }
        
        /// <summary>
        /// 暂停/继续生长
        /// </summary>
        public void TogglePause()
        {
            isPaused = !isPaused;
            Debug.Log($"🌳 OrangeTreeController ({gameObject.name}) 生长{(isPaused ? "暂停" : "继续")} - isPaused = {isPaused}");
        }
        
        /// <summary>
        /// 设置暂停状态
        /// </summary>
        public void SetPaused(bool paused)
        {
            isPaused = paused;
            Debug.Log($"🌳 OrangeTreeController ({gameObject.name}) 生长{(isPaused ? "暂停" : "继续")} - isPaused = {isPaused}");
        }
    }
}
