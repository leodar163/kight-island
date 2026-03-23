Shader "Custom/SpriteStacking"
{
    Properties
    {
        _Atlas ("Atlas", 2D) = "white" {}
        _SliceCount ("Slice Count", Int) = 8
        _Angle ("Angle", Float) = 0
        _SliceSizePx ("Slice Size in Pixels", Vector) = (16, 16, 0, 0)
        _PaddingPx ("Padding in Pixels", Vector) = (0, 0, 0, 0)
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

            Texture2D _Atlas;
            SamplerState sampler_point_clamp_Atlas;
            int _SliceCount;
            float _Angle;
            float2 _SliceSizePx;
            float2 _PaddingPx;

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
                fixed4 result = fixed4(0, 0, 0, 0);

                float2 quadSizePx = float2(
                    _SliceSizePx.x + _PaddingPx.x,
                    _SliceSizePx.y + _SliceCount - 1 + _PaddingPx.y
                );

                // Pixel destination sur le quad
                float2 destPx = i.uv * quadSizePx;

                // Retire le padding pour que Y=0 soit la base de la slice 0
                destPx.y -= _PaddingPx.y * 0.5;
                destPx.x -= _PaddingPx.x * 0.5;

                float cosA = cos(-_Angle);
                float sinA = sin(-_Angle);
                float2 center = _SliceSizePx * 0.5;

                for (int s = 0; s < _SliceCount; s++)
                {
                    // Pixel local à cette slice
                    float2 localPx = float2(destPx.x, destPx.y - s);

                    // Rotation inverse autour du centre
                    float2 centered = localPx - center;
                    float2 srcPx = float2(
                        centered.x * cosA - centered.y * sinA,
                        centered.x * sinA + centered.y * cosA
                    ) + center;

                    // Bounds check
                    if (srcPx.x >= 0 && srcPx.x < _SliceSizePx.x &&
                        srcPx.y >= 0 && srcPx.y < _SliceSizePx.y)
                    {
                        float2 srcUV = float2(
                            (srcPx.x + s * _SliceSizePx.x) / (_SliceCount * _SliceSizePx.x),
                            srcPx.y / _SliceSizePx.y
                        );          

                        fixed4 slice = _Atlas.Sample(sampler_point_clamp_Atlas, srcUV);
                        if (slice.a > 0.5)
                        {
                            result = slice;
                        }
                    }
                }

                return result;
            }
            ENDHLSL
        }
    }
}