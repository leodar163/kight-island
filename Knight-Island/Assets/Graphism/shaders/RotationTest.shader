Shader "Custom/RotationTest"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Angle ("Angle", Float) = 0
        _SizePx ("Size in Pixels", Vector) = (16, 16, 0, 0)
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            Texture2D _MainTex;
            SamplerState sampler_point_clamp_MainTex;
            float _Angle;
            float2 _SizePx;

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; };

            v2f vert(appdata IN)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(IN.vertex);
                o.uv = IN.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float cosA = cos(-_Angle);
                float sinA = sin(-_Angle);

                // Pixel destination en coordonnées entières
                float2 destPx = floor(i.uv * _SizePx);

                // Centrage
                float2 center = floor(_SizePx * 0.5);
                float2 centered = destPx - center;

                // Rotation inverse
                float2 srcPx = float2(
                    centered.x * cosA - centered.y * sinA,
                    centered.x * sinA + centered.y * cosA
                ) + center;

                // On snap au pixel entier
                srcPx = floor(srcPx);

                // Hors bounds
                if (srcPx.x < 0 || srcPx.x >= _SizePx.x || srcPx.y < 0 || srcPx.y >= _SizePx.y)
                    return fixed4(0, 0, 0, 0);

                float2 srcUV = (srcPx + 0.5) / _SizePx;
                return _MainTex.Sample(sampler_point_clamp_MainTex, srcUV);
            }
            ENDHLSL
        }
    }
}