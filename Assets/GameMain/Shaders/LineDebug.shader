Shader "Custom/LaserBeamShader"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (1, 0, 0, 1)  // 红色激光颜色
        _GlowColor ("Glow Color", Color) = (1, 0.5, 0, 1) // 橙色边缘光
        _GlowIntensity ("Glow Intensity", Float) = 2.0    // 发光强度
        _Tiling ("Tiling", Float) = 5.0                   // 纹理平铺系数
        _Speed ("Animation Speed", Float) = 1.0           // 激光动画速度
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha One  // Additive blending for glow effect
            ZWrite Off          // Disable depth writing for transparency
            Cull Off            // Render both sides of the beam
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float _Speed;
            float _Tiling;
            float4 _MainColor;
            float4 _GlowColor;
            float _GlowIntensity;

            // 顶点着色器：计算屏幕空间位置
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv ;//* _Tiling + float2(0 * _Speed, 0);  // 动态平移UV
                return o;
            }

            // 片段着色器：应用颜色和发光效果
            fixed4 frag (v2f i) : SV_Target
            {
                // 激光主颜色
                float4 laserColor = _MainColor;

                // 发光效果：颜色渐变模拟// 使用正弦波模拟渐变
                float4 glowColor = _GlowColor  * _GlowIntensity;

                // 组合主颜色和发光效果
                return laserColor + glowColor;
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}
