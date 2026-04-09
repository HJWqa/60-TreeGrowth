Shader "TreePlanQAQ/PlantEnvironmental"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Humidity ("Humidity", Range(0, 1)) = 0
        _Temperature ("Temperature", Range(0, 1)) = 0
        _Health ("Health", Range(0, 1)) = 1
        
        [Header("Base Colors")]
        _HealthyColor ("Healthy Color", Color) = (0.2, 0.8, 0.2, 1)
        _DryColor ("Dry Color", Color) = (0.6, 0.5, 0.2, 1)
        _WitheredColor ("Withered Color", Color) = (0.4, 0.3, 0.1, 1)
        
        [Header("Effects")]
        _WetnessGloss ("Wetness Gloss", Range(0, 1)) = 0
        _WitherIntensity ("Wither Intensity", Range(0, 1)) = 0
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            float _Humidity;
            float _Temperature;
            float _Health;
            float _WetnessGloss;
            float _WitherIntensity;
            
            float4 _HealthyColor;
            float4 _DryColor;
            float4 _WitheredColor;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                float3 normal = normalize(i.normal);
                float3 viewDir = normalize(i.viewDir);
                
                float3 healthyColor = _HealthyColor.rgb;
                float3 dryColor = _DryColor.rgb;
                float3 witheredColor = _WitheredColor.rgb;
                
                float healthFactor = _Health * (1.0 - _WitherIntensity);
                
                float3 baseColor = lerp(witheredColor, dryColor, _WitherIntensity);
                baseColor = lerp(baseColor, healthyColor, healthFactor);
                
                float NdotV = max(0, dot(normal, viewDir));
                float fresnel = pow(1.0 - NdotV, 2.0);
                
                float wetness = _Humidity * _WetnessGloss;
                float3 wetColor = baseColor + fresnel * wetness * 0.3;
                
                float3 finalColor = baseColor * col.rgb;
                finalColor = lerp(finalColor, wetColor, wetness);
                
                return fixed4(finalColor, 1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
