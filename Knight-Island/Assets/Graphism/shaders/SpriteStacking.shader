Shader "Custom/SpriteStacking"
{
    Properties
    {
        _Atlas ("Atlas", 2D) = "white" {}
        _SliceCount ("Slice Count", Int) = 8
        _SliceSpread ("Slice Spread", Float) = 0.1
        _SliceHeightUV ("Slice height UV", Float) = 0.1
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
            
            sampler2D _Atlas;
            int _SliceCount;
            float _SliceSpread;
            float _SliceHeightUV;
            
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
                    // Origine V de cette slice dans l'espace quad
                    float sliceOrigin = s * _SliceSpread;

                    // UV local à la slice : on remet entre 0 et 1
                    float localV = (i.uv.y - sliceOrigin) / _SliceHeightUV;
                    float localU = i.uv.x;

                    if (localV >= 0.0 && localV <= 1.0)
                    {
                        float atlasU = (localU + s) / _SliceCount;
                        fixed4 slice = tex2D(_Atlas, float2(atlasU, localV));

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

