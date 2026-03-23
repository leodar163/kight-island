Shader "Custom/SpriteStacking"
{
    Properties
    {
        _Atlas ("Atlas", 2D) = "white" {}
        _SliceCount ("Slice Count", Int) = 8
        _SliceSpread ("Slice Spread", Float) = 0.1
        _SliceHeightUV ("Slice height UV", Float) = 0.1
        _Angle ("Angle", Float) = 0
        _SliceSizePx ("Slice Size in Pixel", Vector) = (0,0,0,0)
        _PaddingRatioX ("Padding Ratio X", Float) = 1
        _PaddingRatioY ("Padding Ratio Y", Float) = 1
        _PaddingYUV ("", Float) = 0
    }

    SubShader
    {
       Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }

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
            float _SliceSpread;
            float _SliceHeightUV;
            float _Angle;
            float _PaddingRatioX;
            float _PaddingRatioY;
            float2 _SliceSizePx;
            float _PaddingYUV;
            
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

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _BaseMap_ST;
            CBUFFER_END
            
            float2 PixelPerfectRotateUV(float2 localUV, float angle, float2 sliceSizePx)
            {
                // On passe en espace pixel
                float2 pixelCoord = localUV * sliceSizePx;
                
                // Centre en pixels
                float2 center = sliceSizePx * 0.5;
                float2 centered = pixelCoord - center;
                
                // Rotation et arrondi au pixel le plus proche
                float cosA = cos(angle);
                float sinA = sin(angle);
                
                float2 rotated = float2(
                    centered.x * cosA - centered.y * sinA,
                    centered.x * sinA + centered.y * cosA
                );
                
                // Arrondi : c'est ici que la magie pixel art opère
                float2 snapped = floor(rotated + 0.5) + center;
                
                // On repasse en UV normalisés
                return snapped / sliceSizePx;
            }

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

                for (int s = 0; s < _SliceCount; s++)
                {
                    float localU = (i.uv.x - 0.5) * _PaddingRatioX + 0.5;

                    float sliceOrigin = s * _SliceSpread;
                    float localV = (i.uv.y - sliceOrigin - _PaddingYUV) / _SliceHeightUV;

                    // On rejette tout ce qui est hors de la slice
                    if (localU >= 0.0 && localU <= 1.0 && localV >= 0.0 && localV <= 1.0)
                    {
                        float atlasU = (localU + s) / _SliceCount;
                        fixed4 slice = _Atlas.Sample(sampler_point_clamp_Atlas, float2(atlasU, localV));
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

