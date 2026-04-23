using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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
        
        [Header("产量系统")]
        [SerializeField]
        private int maxYield = 50; // 最大产量（公斤）
        
        [SerializeField]
        private int currentYield = 50; // 当前产量（公斤）
        
        [SerializeField]
        private int yieldLossPerStop = 1; // 每次停止生长损失的产量（公斤）
        
        [Header("环境输入")]
        [SerializeField]
        [Range(-20f, 50f)]
        private float temperature = 25f;
        
        [SerializeField]
        [Range(0f, 100f)]
        private float humidity = 70f;  // 在65-80范围内
        
        [SerializeField]
        [Range(0f, 1000f)]
        private float sunlight = 600f;
        
        // 事件
        public event Action<OrangeTreeStage, OrangeTreeStage> OnStageChanged;
        public event Action<float> OnGrowthUpdated;
        public event Action<int> OnYieldChanged; // 产量变化事件
        public event Action<int> OnHarvestComplete; // 收获完成事件
        public event Action<bool> OnPauseStateChanged; // 暂停状态变化事件
        
        // 私有变量
        private GameObject currentModel;
        private bool isTransitioning = false;
        private bool isPaused = false;
        private bool wasGrowingSuitableLastFrame = true; // 上一帧环境是否适宜
        private bool hasHarvested = false; // 是否已经收获
        private readonly HashSet<OrangeTreeStage> missingModelWarningShown = new HashSet<OrangeTreeStage>();
        
        // 属性
        public float CurrentGrowth => currentGrowth;
        public OrangeTreeStage CurrentStage => currentStage;
        public bool IsPaused => isPaused;
        public int CurrentYield => currentYield;
        public int MaxYield => maxYield;
        
        private void Start()
        {
            // 过滤掉场景里的空控制器（无配置、无阶段、无子层级），避免其误激活 Seed 模型。
            if (config == null && stages.Count == 0 && transform.childCount == 0)
            {
                Debug.LogWarning($"[{name}] 检测到空 OrangeTreeController，已自动禁用以避免模型显示冲突。");
                enabled = false;
                return;
            }

            InitializeStages();
            HideAllStageModels();
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
            
            // 检查环境是否适合生长
            bool environmentSuitable = IsEnvironmentSuitable();
            
            // 检测环境从适宜变为不适宜（停止生长）
            if (wasGrowingSuitableLastFrame && !environmentSuitable)
            {
                OnGrowthStopped();
            }
            
            // 检测环境从不适宜恢复到适宜（恢复生长）
            if (!wasGrowingSuitableLastFrame && environmentSuitable)
            {
                ShowNormalModel();
                Debug.Log($"✅ 环境恢复适宜，继续生长");
            }
            
            // 更新上一帧状态
            wasGrowingSuitableLastFrame = environmentSuitable;
            
            // 只有在环境适宜时才生长
            if (environmentSuitable)
            {
                // 在适宜范围内就按基础速率生长，不计算环境因子
                float growthDelta = config.baseGrowthRate * Time.deltaTime;
                
                float oldGrowth = currentGrowth;
                currentGrowth = Mathf.Clamp(currentGrowth + growthDelta, 0f, 100f);
                OnGrowthUpdated?.Invoke(currentGrowth);
                
                // 检查是否达到100%（收获）
                if (oldGrowth < 100f && currentGrowth >= 100f && !hasHarvested)
                {
                    OnHarvestReached();
                }
                
                // 检查是否需要切换阶段
                OrangeTreeStage newStage = GetStageForGrowth(currentGrowth);
                if (newStage != currentStage)
                {
                    StartCoroutine(TransitionToStage(newStage));
                }
            }
            else
            {
                // 环境不适宜时停止生长
                if (Time.frameCount % 120 == 0) // 每2秒输出一次
                {
                    Debug.Log($"⚠️ 环境不适宜，生长已停止 - 温度:{temperature:F1}°C, 湿度:{humidity:F1}%, 当前产量:{currentYield}kg");
                }
            }
        }
        
        /// <summary>
        /// 检查环境是否适合生长
        /// </summary>
        private bool IsEnvironmentSuitable()
        {
            // 温度范围：12.8 ~ 37°C
            bool tempOk = temperature >= 12.8f && temperature <= 37f;
            
            // 湿度范围：65% ~ 80%
            bool humidOk = humidity >= 65f && humidity <= 80f;
            
            // 温度和湿度都在范围内才适合生长
            return tempOk && humidOk;
        }
        
        /// <summary>
        /// 生长停止时调用（环境从适宜变为不适宜）
        /// </summary>
        private void OnGrowthStopped()
        {
            // 减少产量
            currentYield = Mathf.Max(0, currentYield - yieldLossPerStop);
            OnYieldChanged?.Invoke(currentYield);
            
            // 切换到死亡模型
            ShowDiedModel();
            
            Debug.Log($"⚠️ 生长停止！产量减少 {yieldLossPerStop}kg，当前产量: {currentYield}kg");
        }
        
        /// <summary>
        /// 显示死亡模型
        /// </summary>
        private void ShowDiedModel()
        {
            StageData stageData = stages.Find(s => s.stage == currentStage);
            if (stageData != null && stageData.diedModel != null)
            {
                HideAllStageModels();
                
                // 显示死亡模型
                stageData.diedModel.SetActive(true);
                Debug.Log($"💀 切换到死亡模型: {currentStage}");
            }
        }
        
        /// <summary>
        /// 显示正常模型（环境恢复时）
        /// </summary>
        private void ShowNormalModel()
        {
            StageData stageData = stages.Find(s => s.stage == currentStage);
            if (stageData != null)
            {
                // 隐藏死亡模型
                if (stageData.diedModel != null)
                {
                    stageData.diedModel.SetActive(false);
                }
                
                // 显示正常模型
                if (currentModel != null)
                {
                    currentModel.SetActive(true);
                    Debug.Log($"🌱 恢复正常模型: {currentStage}");
                }
            }
        }
        
        /// <summary>
        /// 达到100%生长时调用（收获）
        /// </summary>
        private void OnHarvestReached()
        {
            hasHarvested = true;
            OnHarvestComplete?.Invoke(currentYield);
            
            Debug.Log($"🎉 恭喜您！您已收获 {currentYield} 公斤的橘子！");
            
            // 停止环境动态变化
            TreePlanQAQ.OrangeTree.EnvironmentManager envManager = FindObjectOfType<TreePlanQAQ.OrangeTree.EnvironmentManager>();
            if (envManager != null)
            {
                envManager.StopDynamicChange();
                Debug.Log("🌡️ 环境动态变化已停止");
            }
            
            // 显示收获提示（可以通过UI显示）
            ShowHarvestMessage(currentYield);
        }
        
        /// <summary>
        /// 显示收获消息
        /// </summary>
        private void ShowHarvestMessage(int yield)
        {
            // 查找并调用HarvestMessageUI
            HarvestMessageUI harvestUI = FindObjectOfType<HarvestMessageUI>();
            if (harvestUI != null)
            {
                harvestUI.ShowMessage(yield);
            }
            else
            {
                Debug.LogWarning("⚠️ 未找到HarvestMessageUI组件，无法显示收获消息");
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
                stages.Add(new StageData(OrangeTreeStage.Fruiting, "结果", 90f));
                stages.Add(new StageData(OrangeTreeStage.Harvest, "成熟", 100f));
            }
            else
            {
                NormalizeLegacyStageData();

                // 检查并补全缺失的阶段，比如后期在枚举里加了新阶段但 Inspector 还没更新的情况
                foreach (OrangeTreeStage stageEnum in Enum.GetValues(typeof(OrangeTreeStage)))
                {
                    if (!stages.Exists(s => s.stage == stageEnum))
                    {
                        float fallbackThreshold = (float)stageEnum * 15f; // 简单猜测个阈值
                        stages.Add(new StageData(stageEnum, stageEnum.ToString(), fallbackThreshold));
                        Debug.LogWarning($"自动在阶段列表中补全了缺失的阶段: {stageEnum}");
                    }
                }
                
                // 确保数据仍然按阈值排序
                stages.Sort((a, b) => a.growthThreshold.CompareTo(b.growthThreshold));
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
            currentStage = stage;

            // 先统一隐藏所有阶段模型，避免场景初始状态下多模型同时可见。
            HideAllStageModels();
            
            StageData stageData = GetBestStageData(currentStage);
            if (stageData == null)
            {
                stageData = new StageData(currentStage, currentStage.ToString(), (float)currentStage * 15f);
                stages.Add(stageData);
            }
            
            // 1. 尝试直接使用已配置的模型
            if (stageData.stageModel != null)
            {
                currentModel = stageData.stageModel;
                currentModel.SetActive(true);
                missingModelWarningShown.Remove(currentStage);
            }
            // 2. 如果数据存在但模型为空，尝试自动在子物体中寻找
            else
            {
                Transform modelTrans = FindModelTransformForStage(currentStage);

                if (modelTrans != null)
                {
                    stageData.stageModel = modelTrans.gameObject;
                    currentModel = stageData.stageModel;
                    currentModel.SetActive(true);
                    missingModelWarningShown.Remove(currentStage);
                    Debug.Log($"自动绑定了阶模型: {currentStage}");
                }
                else
                {
                    // 最后兜底：如果已显示出一个模型，则不再把当前状态判定为缺模型，避免误报。
                    if (currentModel != null && currentModel.activeInHierarchy)
                    {
                        missingModelWarningShown.Remove(currentStage);
                        return;
                    }

                    if (!missingModelWarningShown.Contains(currentStage))
                    {
                        missingModelWarningShown.Add(currentStage);
                        Debug.LogWarning($"[{name}] 未找到阶段 {currentStage} 的模型，请在 Inspector 中手动指定，或确保子物体名称与阶段名称（{currentStage} 或 OrangeTree_{currentStage}）一致。");
                    }
                }
            }
        }

        /// <summary>
        /// 隐藏所有阶段的正常模型和死亡模型，确保同一时刻只显示一个模型。
        /// </summary>
        private void HideAllStageModels()
        {
            HashSet<GameObject> processed = new HashSet<GameObject>();

            for (int i = 0; i < stages.Count; i++)
            {
                StageData stage = stages[i];
                if (stage == null)
                {
                    continue;
                }

                if (stage.stageModel != null && processed.Add(stage.stageModel))
                {
                    stage.stageModel.SetActive(false);
                }

                if (stage.diedModel != null && processed.Add(stage.diedModel))
                {
                    stage.diedModel.SetActive(false);
                }
            }
        }

        /// <summary>
        /// 按阶段名定位模型，支持精确匹配和规范化匹配（忽略大小写、下划线等分隔符）。
        /// </summary>
        private Transform FindModelTransformForStage(OrangeTreeStage stage)
        {
            string stageName = stage.ToString();
            string prefixedName = $"OrangeTree_{stage}";
            string plantName = $"Plant_{stage}";

            // 先尝试直系查找，性能更好。
            Transform modelTrans = transform.Find(stageName);
            if (modelTrans != null)
            {
                return modelTrans;
            }

            modelTrans = transform.Find(prefixedName);
            if (modelTrans != null)
            {
                return modelTrans;
            }

            modelTrans = transform.Find(plantName);
            if (modelTrans != null)
            {
                return modelTrans;
            }

            // 再递归精确查找。
            modelTrans = FindChildRecursive(transform, stageName);
            if (modelTrans != null)
            {
                return modelTrans;
            }

            modelTrans = FindChildRecursive(transform, prefixedName);
            if (modelTrans != null)
            {
                return modelTrans;
            }

            modelTrans = FindChildRecursive(transform, plantName);
            if (modelTrans != null)
            {
                return modelTrans;
            }

            // 如果模型不在当前控制器子层级下，继续在整个场景里找。
            modelTrans = FindSceneTransformByName(stageName, prefixedName, plantName);
            if (modelTrans != null)
            {
                return modelTrans;
            }

            // 最后做一次规范化模糊匹配，兼容例如 OrangeTree-MatureTree、orangetree mature tree。
            string normalizedStage = NormalizeName(stageName);
            string normalizedPrefixed = NormalizeName(prefixedName);
            string normalizedPlant = NormalizeName(plantName);

            modelTrans = FindChildByNormalizedName(transform, normalizedStage, normalizedPrefixed, normalizedPlant);
            if (modelTrans != null)
            {
                return modelTrans;
            }

            return FindSceneTransformByNormalizedName(normalizedStage, normalizedPrefixed, normalizedPlant);
        }

        /// <summary>
        /// 获取最优阶段数据：优先返回已绑定模型的条目，避免重复阶段数据导致误判。
        /// </summary>
        private StageData GetBestStageData(OrangeTreeStage stage)
        {
            StageData fallback = null;

            for (int i = 0; i < stages.Count; i++)
            {
                StageData item = stages[i];
                if (item == null || item.stage != stage)
                {
                    continue;
                }

                if (item.stageModel != null)
                {
                    return item;
                }

                if (fallback == null)
                {
                    fallback = item;
                }
            }

            // 兼容历史数据：某些旧场景里 stage 值未升级，但 displayName 仍可识别语义。
            for (int i = 0; i < stages.Count; i++)
            {
                StageData item = stages[i];
                if (item == null || item.stageModel == null)
                {
                    continue;
                }

                if (IsDisplayNameForStage(stage, item.displayName))
                {
                    return item;
                }
            }

            return fallback;
        }

        /// <summary>
        /// 兼容历史序列化：修正旧版本 Fruiting/Harvest 的枚举值与名称映射。
        /// </summary>
        private void NormalizeLegacyStageData()
        {
            for (int i = 0; i < stages.Count; i++)
            {
                StageData item = stages[i];
                if (item == null)
                {
                    continue;
                }

                OrangeTreeStage originalStage = item.stage;
                OrangeTreeStage mappedStage = MapLegacyStage(item);
                item.stage = mappedStage;

                if (string.IsNullOrWhiteSpace(item.displayName))
                {
                    item.displayName = GetDefaultDisplayName(mappedStage);
                }

                if (originalStage != mappedStage)
                {
                    Debug.Log($"[{name}] 修正旧阶段映射: {originalStage} -> {mappedStage}");
                }
            }
        }

        private OrangeTreeStage MapLegacyStage(StageData item)
        {
            int rawStage = (int)item.stage;
            string displayName = item.displayName ?? string.Empty;

            bool nameLooksFruiting = displayName.Contains("结果") || displayName.Contains("Fruiting", StringComparison.OrdinalIgnoreCase);
            bool nameLooksHarvest = displayName.Contains("成熟") || displayName.Contains("Harvest", StringComparison.OrdinalIgnoreCase);

            // 旧数据常见问题：结果阶段被写成 6，成熟阶段被写成 7。
            if (rawStage == 7)
            {
                return OrangeTreeStage.Harvest;
            }

            if (rawStage == 6 && nameLooksFruiting)
            {
                return OrangeTreeStage.Fruiting;
            }

            if (rawStage == 5 && nameLooksHarvest)
            {
                return OrangeTreeStage.Harvest;
            }

            if (rawStage == 6 && nameLooksHarvest && item.growthThreshold >= 90f)
            {
                return OrangeTreeStage.Harvest;
            }

            if (rawStage == 6 && item.growthThreshold < 90f)
            {
                return OrangeTreeStage.Fruiting;
            }

            if (rawStage < 0 || rawStage > (int)OrangeTreeStage.Harvest)
            {
                return OrangeTreeStage.Seed;
            }

            return item.stage;
        }

        private bool IsDisplayNameForStage(OrangeTreeStage stage, string displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName))
            {
                return false;
            }

            switch (stage)
            {
                case OrangeTreeStage.Fruiting:
                    return displayName.Contains("结果") || displayName.Contains("Fruiting", StringComparison.OrdinalIgnoreCase);
                case OrangeTreeStage.Harvest:
                    return displayName.Contains("成熟") || displayName.Contains("Harvest", StringComparison.OrdinalIgnoreCase);
                case OrangeTreeStage.MatureTree:
                    return displayName.Contains("成树") || displayName.Contains("Mature", StringComparison.OrdinalIgnoreCase);
                default:
                    return false;
            }
        }

        private string GetDefaultDisplayName(OrangeTreeStage stage)
        {
            switch (stage)
            {
                case OrangeTreeStage.Seed:
                    return "种子";
                case OrangeTreeStage.Sprout:
                    return "发芽";
                case OrangeTreeStage.Seedling:
                    return "幼苗";
                case OrangeTreeStage.YoungTree:
                    return "小树";
                case OrangeTreeStage.MatureTree:
                    return "成树";
                case OrangeTreeStage.Fruiting:
                    return "结果";
                case OrangeTreeStage.Harvest:
                    return "成熟";
                default:
                    return stage.ToString();
            }
        }

        /// <summary>
        /// 在层级后代中按名称递归查找子物体。
        /// </summary>
        private Transform FindChildRecursive(Transform parent, string childName)
        {
            if (parent == null)
            {
                return null;
            }

            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                if (child.name == childName)
                {
                    return child;
                }

                Transform found = FindChildRecursive(child, childName);
                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }

        /// <summary>
        /// 通过规范化名称在后代中查找，支持包含匹配。
        /// </summary>
        private Transform FindChildByNormalizedName(Transform parent, string normalizedNameA, string normalizedNameB)
        {
            return FindChildByNormalizedName(parent, normalizedNameA, normalizedNameB, string.Empty);
        }

        private Transform FindChildByNormalizedName(Transform parent, string normalizedNameA, string normalizedNameB, string normalizedNameC)
        {
            if (parent == null)
            {
                return null;
            }

            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                string normalizedChild = NormalizeName(child.name);

                bool isMatch = normalizedChild == normalizedNameA
                    || normalizedChild == normalizedNameB
                    || (!string.IsNullOrEmpty(normalizedNameC) && normalizedChild == normalizedNameC)
                    || normalizedChild.Contains(normalizedNameA)
                    || normalizedChild.Contains(normalizedNameB);

                if (!string.IsNullOrEmpty(normalizedNameC))
                {
                    isMatch = isMatch || normalizedChild.Contains(normalizedNameC);
                }

                if (isMatch)
                {
                    return child;
                }

                Transform found = FindChildByNormalizedName(child, normalizedNameA, normalizedNameB, normalizedNameC);
                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }

        /// <summary>
        /// 在整个场景内按名称查找模型。
        /// </summary>
        private Transform FindSceneTransformByName(params string[] candidateNames)
        {
            Transform[] allTransforms = FindObjectsOfType<Transform>(true);
            for (int i = 0; i < allTransforms.Length; i++)
            {
                Transform candidate = allTransforms[i];
                if (candidate == null)
                {
                    continue;
                }

                for (int j = 0; j < candidateNames.Length; j++)
                {
                    if (!string.IsNullOrEmpty(candidateNames[j]) && candidate.name == candidateNames[j])
                    {
                        return candidate;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 在整个场景内做规范化名称匹配。
        /// </summary>
        private Transform FindSceneTransformByNormalizedName(params string[] normalizedNames)
        {
            Transform[] allTransforms = FindObjectsOfType<Transform>(true);
            for (int i = 0; i < allTransforms.Length; i++)
            {
                Transform candidate = allTransforms[i];
                if (candidate == null)
                {
                    continue;
                }

                string normalizedCandidate = NormalizeName(candidate.name);
                for (int j = 0; j < normalizedNames.Length; j++)
                {
                    string normalizedTarget = normalizedNames[j];
                    if (string.IsNullOrEmpty(normalizedTarget))
                    {
                        continue;
                    }

                    if (normalizedCandidate == normalizedTarget || normalizedCandidate.Contains(normalizedTarget))
                    {
                        return candidate;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 把名称转为仅含小写字母数字的形式，便于稳健比较。
        /// </summary>
        private string NormalizeName(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder(input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (char.IsLetterOrDigit(c))
                {
                    builder.Append(char.ToLowerInvariant(c));
                }
            }

            return builder.ToString();
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
            currentYield = maxYield; // 重置产量到最大值
            hasHarvested = false;
            wasGrowingSuitableLastFrame = true;
            
            if (currentModel != null)
            {
                currentModel.SetActive(false);
            }
            
            ShowStageModel(OrangeTreeStage.Seed);
            
            OnYieldChanged?.Invoke(currentYield);
            
            Debug.Log($"🔄 橘子树已重置 - 产量: {currentYield}kg");
        }
        
        /// <summary>
        /// 设置生长值（用于测试）
        /// </summary>
        public void SetGrowth(float growth)
        {
            if (isPaused)
            {
                Debug.Log($"[{name}] 当前处于暂停状态，忽略外部设置生长值: {growth:F1}");
                return;
            }

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
            SetPaused(!isPaused);
        }
        
        /// <summary>
        /// 设置暂停状态
        /// </summary>
        public void SetPaused(bool paused)
        {
            OrangeTreeController[] controllers = FindObjectsOfType<OrangeTreeController>(true);
            bool anyChanged = false;

            for (int i = 0; i < controllers.Length; i++)
            {
                OrangeTreeController controller = controllers[i];
                if (controller == null)
                {
                    continue;
                }

                anyChanged |= controller.ApplyPausedState(paused);
            }

            if (!anyChanged)
            {
                return;
            }

            Debug.Log($"🌳 橘子树已{(paused ? "暂停" : "继续")}（已同步场景内所有控制器）");
        }

        private bool ApplyPausedState(bool paused)
        {
            if (isPaused == paused)
            {
                return false;
            }

            isPaused = paused;
            OnPauseStateChanged?.Invoke(isPaused);
            return true;
        }
    }
}
