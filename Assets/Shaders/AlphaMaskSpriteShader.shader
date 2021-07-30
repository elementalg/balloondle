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

            #ifndef ZERO_WITH_MARGIN_ERROR
            #define ZERO_WITH_MARGIN_ERROR 0.000000000000000001
            #endif

            #ifndef ANGLE_MARGIN_ERROR
            #define ANGLE_MARGIN_ERROR 0.001
            #endif

            #ifndef MATH_PI
            #define MATH_PI 3.14159265
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

            /*
             * Updates the vector '_OutlineAngle' so the angles held are radians.
             */
            void ConvertRelativeOutlineAnglesToRadians()
            {
                _OutlineAngle.x = (_OutlineAngle.x < 0) ? 2.0 + _OutlineAngle.x : _OutlineAngle.x;
                _OutlineAngle.x = (_OutlineAngle.x % 2.0) * MATH_PI; 
                
                _OutlineAngle.y = (_OutlineAngle.y < 0) ? 2.0 + _OutlineAngle.y : _OutlineAngle.y;
                _OutlineAngle.y = (_OutlineAngle.y % 2.0) * MATH_PI;

                _OutlineAngle.z = (_OutlineAngle.z < 0) ? 2.0 + _OutlineAngle.z : _OutlineAngle.z;
                _OutlineAngle.z = (_OutlineAngle.z % 2.0) * MATH_PI; 
                
                _OutlineAngle.w = (_OutlineAngle.w < 0) ? 2.0 + _OutlineAngle.w : _OutlineAngle.w;
                _OutlineAngle.w = (_OutlineAngle.w % 2.0) * MATH_PI;
            }

            bool IsAngleWithinAngleRange(float checkedAngle, float2 angleRange)
            {
                float isAngleRangeALap = step (angleRange.y, angleRange.x);
                
                float strictAngleRange = step (angleRange.x, checkedAngle) *
                    step (checkedAngle, angleRange.y + isAngleRangeALap * (2 * MATH_PI));
                float lapAngleRange = step (0.0, checkedAngle) * step (checkedAngle, angleRange.y);
                return min (1.0, strictAngleRange + lapAngleRange * isAngleRangeALap) > 0;
            }
        
            /*
             * Detect whether or not a texture coordinate is located within the boundaries established for the outline.
             *
             * Returns: 1.0 if the texture coordinate is within the boundaries, 0.0 otherwise. 
             */
            float IsOutlineWithinAngleLimit(float2 uv)
            {
                // Translate the texture coordinate, so we can use (0,0) as center point.
                float2 displacedUV = uv - 0.5;
                float2 circumferencePoint = float2 (0.0, 0.0);

                // Return always 1.0 if the point is located in the center.
                if (1.0 - step (ZERO_WITH_MARGIN_ERROR, abs (displacedUV.x)) *
                    step (ZERO_WITH_MARGIN_ERROR, abs (displacedUV.y)))
                {
                    return 1.0;
                }

                // Vertical line on x = 0.
                if (step (ANGLE_MARGIN_ERROR, abs (displacedUV.x)) == 0.0)
                {
                    circumferencePoint.x = 0.0;
                    circumferencePoint.y = sign (displacedUV.y);
                }
                else
                {
                    float lineSlope = (displacedUV.y / displacedUV.x);
                    float n = displacedUV.y - lineSlope * displacedUV.x;

                    // 2nd grade equation for retrieving the desired x value within the unitary circle's circumference.
                    float a = pow (lineSlope, 2.0) + 1;
                    float b = 2.0 * lineSlope * n;
                    float c = pow (n, 2.0) - 1;
                    float dividend = -1.0 * b + (sign (displacedUV.x) * sqrt (pow (b, 2.0) - 4 * a * c));
                    float divisor = 2 * a;
                    
                    circumferencePoint.x = dividend / divisor;
                    circumferencePoint.y = lineSlope * circumferencePoint.x + n;
                }

                // Angle calculation based upon the point within the unitary circle's circumference.
                float xAngle = acos (circumferencePoint.x);
                float yAngle = asin (circumferencePoint.y);
                float uvAngle = (yAngle < 0.0) ? (MATH_PI * 2.0) - xAngle : xAngle;
                uvAngle %= 2.0 * MATH_PI;
                
                ConvertRelativeOutlineAnglesToRadians();
                
                return min (1, IsAngleWithinAngleRange(uvAngle, _OutlineAngle.xy)
                    + IsAngleWithinAngleRange(uvAngle, _OutlineAngle.zw));
            }
        
            fixed4 SpriteFrag(v2f IN) : SV_Target
            {
                fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;

                // Apply outline as long as it is enabled and the texture coordinate is within the defined angle limit.
                float outlineAlpha = tex2D (_OutlineMaskTex, IN.texcoord).a;
                c.rgb = lerp (c.rgb, _OutlineColor.rgb,
                    _OutlineColor.a * outlineAlpha * _EnableOutline * IsOutlineWithinAngleLimit (IN.texcoord));
                
                c.rgb *= c.a;
                return c;
            }

            #endif // UNITY_SPRITES_INCLUDED
        ENDCG
        }
    }
}
