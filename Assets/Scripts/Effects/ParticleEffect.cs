using UnityEngine;

namespace TreePlanQAQ.Effects
{
    public class ParticleEffect : MonoBehaviour
    {
        [Header("粒子设置")]
        public int particleCount = 20;
        public float particleSize = 0.1f;
        public float speed = 2f;
        public float lifetime = 2f;
        public Color particleColor = Color.green;
        
        [Header("运动设置")]
        public bool randomDirection = true;
        public Vector3 direction = Vector3.up;
        public bool spiralMotion = false;
        
        private ParticleSystem particles;
        
        private void Awake()
        {
            SetupParticleSystem();
        }
        
        private void SetupParticleSystem()
        {
            particles = GetComponent<ParticleSystem>();
            if (particles == null)
            {
                particles = gameObject.AddComponent<ParticleSystem>();
            }
            
            var main = particles.main;
            main.startLifetime = lifetime;
            main.startSpeed = speed;
            main.startSize = particleSize;
            main.startColor = particleColor;
            main.maxParticles = particleCount * 10;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            
            var emission = particles.emission;
            emission.rateOverTime = particleCount / lifetime;
            
            var shape = particles.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = 0.2f;
        }
        
        public void SetColor(Color color)
        {
            particleColor = color;
            if (particles != null)
            {
                var main = particles.main;
                main.startColor = color;
            }
        }
        
        public void SetParticleCount(int count)
        {
            particleCount = count;
            if (particles != null)
            {
                var emission = particles.emission;
                emission.rateOverTime = particleCount / lifetime;
            }
        }
    }
    
    public static class ParticleFactory
    {
        public static GameObject CreateSparkleEffect(Color color)
        {
            GameObject effect = new GameObject("SparkleEffect");
            
            ParticleSystem ps = effect.AddComponent<ParticleSystem>();
            var main = ps.main;
            main.startColor = color;
            main.startSize = 0.05f;
            main.startSpeed = 1f;
            main.startLifetime = 1f;
            main.maxParticles = 100;
            
            var emission = ps.emission;
            emission.rateOverTime = 30;
            
            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = 0.3f;
            
            var colorOverLifetime = ps.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(color, 0f), new GradientColorKey(color, 1f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) }
            );
            colorOverLifetime.color = gradient;
            
            return effect;
        }
        
        public static GameObject CreateGrowthEffect()
        {
            GameObject effect = new GameObject("GrowthEffect");
            
            ParticleSystem ps = effect.AddComponent<ParticleSystem>();
            var main = ps.main;
            main.startColor = new Color(0.3f, 1f, 0.3f);
            main.startSize = 0.1f;
            main.startSpeed = 3f;
            main.startLifetime = 0.8f;
            main.maxParticles = 50;
            
            var emission = ps.emission;
            emission.rateOverTime = 40;
            
            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 30f;
            shape.radius = 0.1f;
            
            return effect;
        }
        
        public static GameObject CreateFlowerEffect()
        {
            GameObject effect = new GameObject("FlowerEffect");
            
            ParticleSystem ps = effect.AddComponent<ParticleSystem>();
            var main = ps.main;
            main.startColor = new Color(1f, 0.8f, 0.9f);
            main.startSize = 0.15f;
            main.startSpeed = 2f;
            main.startLifetime = 1.5f;
            main.maxParticles = 80;
            
            var emission = ps.emission;
            emission.rateOverTime = 25;
            
            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = 0.5f;
            
            return effect;
        }
        
        public static GameObject CreateFruitEffect()
        {
            GameObject effect = new GameObject("FruitEffect");
            
            ParticleSystem ps = effect.AddComponent<ParticleSystem>();
            var main = ps.main;
            main.startColor = new Color(1f, 0.5f, 0.2f);
            main.startSize = 0.12f;
            main.startSpeed = 1.5f;
            main.startLifetime = 1.2f;
            main.maxParticles = 60;
            
            var emission = ps.emission;
            emission.rateOverTime = 20;
            
            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = 0.6f;
            
            return effect;
        }
        
        public static GameObject CreateHarvestEffect()
        {
            GameObject effect = new GameObject("HarvestEffect");
            
            ParticleSystem ps = effect.AddComponent<ParticleSystem>();
            var main = ps.main;
            main.startColor = new Color(1f, 0.9f, 0.3f);
            main.startSize = 0.2f;
            main.startSpeed = 4f;
            main.startLifetime = 1f;
            main.maxParticles = 100;
            
            var emission = ps.emission;
            emission.rateOverTime = 60;
            
            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 45f;
            shape.radius = 0.3f;
            
            return effect;
        }
    }
}
