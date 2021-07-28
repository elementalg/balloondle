// Based on Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Modified by ElementalG, 2021.

Shader "Balloondle/Alpha Mask Sprite Shader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        _AlphaMaskTex ("Alpha Mask", 2D) = "white" {}
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
        _EnableOutline ("Enable Outline", Float) = 0
        _OutlineMaskTex ("Outline Mask Tex", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineAngle ("Outline Angle", Vector) = (0,0,0,0)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment SpriteFrag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA

            #ifndef ALPHA_PRECISION
            #define ALPHA_PRECISION 0.000000000000000001
            #endif
        
            #ifndef UNITY_SPRITES_INCLUDED
            #define UNITY_SPRITES_INCLUDED

            #include "UnityCG.cginc"

            #ifdef UNITY_INSTANCING_ENABLED

                UNITY_INSTANCING_BUFFER_START(PerDrawSprite)
                    // SpriteRenderer.Color while Non-Batched/Instanced.
                    UNITY_DEFINE_INSTANCED_PROP(fixed4, unity_SpriteRendererColorArray)
                    // this could be smaller but that's how bit each entry is regardless of type
                    UNITY_DEFINE_INSTANCED_PROP(fixed2, unity_SpriteFlipArray)
                UNITY_INSTANCING_BUFFER_END(PerDrawSprite)

                #define _RendererColor  UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteRendererColorArray)
                #define _Flip           UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteFlipArray)

            #endif // instancing

            CBUFFER_START(UnityPerDrawSprite)
            #ifndef UNITY_INSTANCING_ENABLED
                fixed4 _RendererColor;
                fixed2 _Flip;
            #endif
                float _EnableExternalAlpha;
            CBUFFER_END

            // Material Color.
            fixed4 _Color;

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            inline float4 UnityFlipSprite(in float3 pos, in fixed2 flip)
            {
                return float4(pos.xy * flip, pos.z, 1.0);
            }

            v2f SpriteVert(appdata_t IN)
            {
                v2f OUT;

                UNITY_SETUP_INSTANCE_ID (IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.vertex = UnityFlipSprite(IN.vertex, _Flip);
                OUT.vertex = UnityObjectToClipPos(OUT.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color * _RendererColor;

                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

                return OUT;
            }

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            sampler2D _AlphaMaskTex;
        
            float _EnableOutline;
            sampler2D _OutlineMaskTex;
            float4 _OutlineColor;
            float4 _OutlineAngle;
        
            fixed4 SampleSpriteTexture (float2 uv)
            {
                fixed4 color = tex2D (_MainTex, uv);
                
                // Apply trasparency depending upon the brightness of the alpha mask at the position.
                fixed4 alpha_mask = tex2D (_AlphaMaskTex, uv);
                fixed calculated_alpha = max (alpha_mask.r, max (alpha_mask.g, alpha_mask.b)) / 1.0;
                
				// Avoid messing up the borders' finishing.
				color.a = min (color.a, calculated_alpha);
                
            #if ETC1_EXTERNAL_ALPHA
                fixed4 alpha = tex2D (_AlphaTex, uv);
                color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
            #endif

                return color;
            }

            float IsOutlineWithinAngleLimit(float2 uv)
            {
                float2 segmentStartPoint = float2 (cos (_OutlineAngle.x), sin (_OutlineAngle.x));
                float startSegmentSlope = tan (_OutlineAngle.x);
                
                float2 segmentEndPoint = float2 (cos (_OutlineAngle.y), sin (_OutlineAngle.y));
                float endSegmentSlope = tan (_OutlineAngle.y);

                float minY = min (startSegmentSlope * (uv.x - 0.5), endSegmentSlope * (uv.x - 0.5));
                float maxY = max (startSegmentSlope * (uv.x - 0.5), endSegmentSlope * (uv.x - 0.5));

                return step (minY, (uv.y - 0.5)) * step ((uv.y - 0.5), maxY);
            }
        
            fixed4 SpriteFrag(v2f IN) : SV_Target
            {
                fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
                
                float outlineAlpha = tex2D (_OutlineMaskTex, IN.texcoord).a;
                c.rgb = lerp (c.rgb, _OutlineColor.rgb, _OutlineColor.a * outlineAlpha * _EnableOutline * IsOutlineWithinAngleLimit (IN.texcoord));
                
                c.rgb *= c.a;
                return c;
            }

            #endif // UNITY_SPRITES_INCLUDED
        ENDCG
        }
    }
}
