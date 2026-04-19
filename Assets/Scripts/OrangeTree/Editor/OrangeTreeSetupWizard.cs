using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace TreePlanQAQ.OrangeTree.Editor
{
    /// <summary>
    /// 橘子树系统设置向导
    /// </summary>
    public class OrangeTreeSetupWizard : EditorWindow
    {
        private GameObject[] stageModels = new GameObject[7];
        private GrowthConfig config;
        
        [MenuItem("TreePlanQAQ/Orange Tree Setup Wizard")]
        public static void ShowWindow()
        {
            var window = GetWindow<OrangeTreeSetupWizard>("橘子树设置向导");
            window.minSize = new Vector2(500, 600);
        }
        
        private void OnGUI()
        {
            GUILayout.Label("橘子树系统设置向导", EditorStyles.boldLabel);
            EditorGUILayout.Space(10);
            
            EditorGUILayout.HelpBox(
                "这个向导会帮助你快速设置橘子树生长系统。\n" +
                "请按照步骤配置模型和参数。",
                MessageType.Info
            );
            
            EditorGUILayout.Space(10);
            
            // 步骤1: 配置文件
            GUILayout.Label("步骤1: 生长配置", EditorStyles.boldLabel);
            config = (GrowthConfig)EditorGUILayout.ObjectField("Growth Config", config, typeof(GrowthConfig), false);
            
            if (config == null)
            {
                if (GUILayout.Button("创建新配置"))
                {
                    CreateNewConfig();
                }
            }
            
            EditorGUILayout.Space(10);
            
            // 步骤2: 模型配置
            GUILayout.Label("步骤2: 阶段模型", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("将7个阶段的模型拖拽到下面的槽位中", MessageType.Info);
            
            string[] stageNames = { "种子", "发芽", "幼苗", "小树", "成树", "结果", "成熟" };
            for (int i = 0; i < 7; i++)
            {
                stageModels[i] = (GameObject)EditorGUILayout.ObjectField(
                    $"{i}. {stageNames[i]}",
                    stageModels[i],
                    typeof(GameObject),
                    true
                );
            }
            
            EditorGUILayout.Space(10);
            
            // 步骤3: 创建系统
            GUILayout.Label("步骤3: 创建系统", EditorStyles.boldLabel);
            
            GUI.enabled = config != null && AllModelsAssigned();
            
            if (GUILayout.Button("创建橘子树系统", GUILayout.Height(40)))
            {
                CreateOrangeTreeSystem();
            }
            
            GUI.enabled = true;
            
            if (!AllModelsAssigned())
            {
                EditorGUILayout.HelpBox("请先配置所有7个阶段的模型", MessageType.Warning);
            }
        }
        
        private bool AllModelsAssigned()
        {
            foreach (var model in stageModels)
            {
                if (model == null)
                    return false;
            }
            return true;
        }
        
        private void CreateNewConfig()
        {
            string path = EditorUtility.SaveFilePanelInProject(
                "创建生长配置",
                "OrangeTreeConfig",
                "asset",
                "选择保存位置"
            );
            
            if (!string.IsNullOrEmpty(path))
            {
                config = CreateInstance<GrowthConfig>();
                AssetDatabase.CreateAsset(config, path);
                AssetDatabase.SaveAssets();
                
                Debug.Log($"✅ 创建配置文件: {path}");
            }
        }
        
        private void CreateOrangeTreeSystem()
        {
            // 创建主对象
            GameObject treeRoot = new GameObject("OrangeTree");
            treeRoot.transform.position = Vector3.zero;
            
            // 添加控制器
            OrangeTreeController controller = treeRoot.AddComponent<OrangeTreeController>();
            controller.config = config;
            
            // 配置阶段
            controller.stages.Clear();
            string[] stageNames = { "种子", "发芽", "幼苗", "小树", "成树", "结果", "成熟" };
            float[] thresholds = { 0f, 10f, 25f, 50f, 75f, 90f, 100f };
            
            for (int i = 0; i < 7; i++)
            {
                // 实例化模型
                GameObject modelInstance = Instantiate(stageModels[i], treeRoot.transform);
                modelInstance.name = $"Stage_{i}_{stageNames[i]}";
                modelInstance.transform.localPosition = Vector3.zero;
                modelInstance.SetActive(i == 0); // 只激活种子阶段
                
                // 创建阶段数据
                StageData stageData = new StageData(
                    (OrangeTreeStage)i,
                    stageNames[i],
                    thresholds[i]
                );
                stageData.stageModel = modelInstance;
                
                controller.stages.Add(stageData);
            }
            
            // 创建环境管理器
            GameObject envObj = new GameObject("EnvironmentManager");
            envObj.AddComponent<EnvironmentManager>();
            
            // 创建UI
            CreateUI(controller);
            
            // 选中创建的对象
            Selection.activeGameObject = treeRoot;
            
            EditorUtility.DisplayDialog(
                "创建成功",
                "橘子树系统已创建！\n\n" +
                "- OrangeTree: 主控制器\n" +
                "- EnvironmentManager: 环境管理器\n" +
                "- Canvas: UI界面\n\n" +
                "点击Play测试生长过程！",
                "确定"
            );
            
            Debug.Log("✅ 橘子树系统创建完成！");
        }
        
        private void CreateUI(OrangeTreeController controller)
        {
            // 创建Canvas
            GameObject canvasObj = new GameObject("GrowthUI");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            canvasObj.AddComponent<GraphicRaycaster>();
            
            // 创建面板
            GameObject panel = new GameObject("Panel");
            panel.transform.SetParent(canvasObj.transform);
            
            RectTransform panelRect = panel.AddComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0, 0.5f);
            panelRect.anchorMax = new Vector2(0, 0.5f);
            panelRect.pivot = new Vector2(0, 0.5f);
            panelRect.anchoredPosition = new Vector2(20, 0);
            panelRect.sizeDelta = new Vector2(300, 400);
            
            Image panelBg = panel.AddComponent<Image>();
            panelBg.color = new Color(0, 0, 0, 0.7f);
            
            // 添加UI控制器
            GrowthUIController uiController = panel.AddComponent<GrowthUIController>();
            uiController.treeController = controller;
            
            // 创建文本元素
            uiController.stageText = CreateText(panel.transform, "StageText", "阶段: 种子", new Vector2(0, 150));
            uiController.growthText = CreateText(panel.transform, "GrowthText", "生长: 0%", new Vector2(0, 100));
            uiController.temperatureText = CreateText(panel.transform, "TempText", "温度: 25°C", new Vector2(0, 0));
            uiController.humidityText = CreateText(panel.transform, "HumidText", "湿度: 60%", new Vector2(0, -50));
            uiController.sunlightText = CreateText(panel.transform, "SunText", "光照: 600 lux", new Vector2(0, -100));
            
            // 创建进度条
            GameObject barBg = new GameObject("GrowthBar_BG");
            barBg.transform.SetParent(panel.transform);
            RectTransform barRect = barBg.AddComponent<RectTransform>();
            barRect.anchorMin = new Vector2(0.1f, 0.5f);
            barRect.anchorMax = new Vector2(0.9f, 0.5f);
            barRect.pivot = new Vector2(0.5f, 0.5f);
            barRect.anchoredPosition = new Vector2(0, 50);
            barRect.sizeDelta = new Vector2(0, 30);
            
            Image barBgImg = barBg.AddComponent<Image>();
            barBgImg.color = new Color(0.3f, 0.3f, 0.3f);
            
            GameObject barFill = new GameObject("Fill");
            barFill.transform.SetParent(barBg.transform);
            RectTransform fillRect = barFill.AddComponent<RectTransform>();
            fillRect.anchorMin = new Vector2(0, 0);
            fillRect.anchorMax = new Vector2(0, 1);
            fillRect.pivot = new Vector2(0, 0.5f);
            fillRect.anchoredPosition = Vector2.zero;
            fillRect.sizeDelta = Vector2.zero;
            
            Image fillImg = barFill.AddComponent<Image>();
            fillImg.color = new Color(0.2f, 0.8f, 0.2f);
            fillImg.type = Image.Type.Filled;
            fillImg.fillMethod = Image.FillMethod.Horizontal;
            
            uiController.growthBar = fillImg;
        }
        
        private Text CreateText(Transform parent, string name, string text, Vector2 position)
        {
            GameObject textObj = new GameObject(name);
            textObj.transform.SetParent(parent);
            
            RectTransform rect = textObj.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = new Vector2(280, 40);
            
            Text txt = textObj.AddComponent<Text>();
            txt.text = text;
            txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            txt.fontSize = 20;
            txt.alignment = TextAnchor.MiddleCenter;
            txt.color = Color.white;
            
            return txt;
        }
    }
}
