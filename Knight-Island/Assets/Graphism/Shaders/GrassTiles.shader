Shader "Custom/Tilemap_ColorReplace"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _TargetColor ("Target Color", Color) = (1,0,1,1)

        _ReplaceTex ("Replace Texture", 2D) = "white" {}
        _ReplaceTexPPU ("Replace Texture PPU", Float) = 16.0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
            "IgnoreProjector" = "True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
                float4 color      : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float2 worldPos    : TEXCOORD1;
                float4 color       : COLOR;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_ReplaceTex);
            SAMPLER(sampler_ReplaceTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _Color;
                float4 _TargetColor;
                float4 _ReplaceTex_ST;
                float4 _ReplaceTex_TexelSize;  // (1/width, 1/height, width, height)
                float  _ReplaceTexPPU;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.color = IN.color * _Color;

                // Position world-space XY, passée au fragment
                float3 worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.worldPos = worldPos.xy;

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 spriteColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                spriteColor *= IN.color;

                int4 spriteInt = (int4)(spriteColor  * 255.0 + 0.5);
                int4 targetInt = (int4)(_TargetColor * 255.0 + 0.5);
                bool isTarget  = all(spriteInt.rgb == targetInt.rgb);

                if (!isTarget)
                    return spriteColor;

                // UV global : position world en unités Unity → pixels → frac
                // _ReplaceTexPPU permet d'aligner la texture sur la grille world
                float2 replaceUV = frac(IN.worldPos * _ReplaceTexPPU / _ReplaceTex_TexelSize.zw);

                half4 replaceColor = SAMPLE_TEXTURE2D(_ReplaceTex, sampler_ReplaceTex, replaceUV);
                replaceColor.a = spriteColor.a;
                return replaceColor;
            }
            ENDHLSL
        }
    }
}