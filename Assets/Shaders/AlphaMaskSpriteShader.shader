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
        _EnableInnerOutline ("Enable Inner Outline", Float) = 0
        _InnerOutlineThickness ("Inner Outline Thickness", Int) = 6
        _InnerOutlineColor ("Inner Outline Color", Color) = (1, 1, 1, 1)
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
            sampler2D _AlphaTex;

            float _EnableInnerOutline;
            uniform int _InnerOutlineThickness;
            float4 _InnerOutlineColor;

            float IsUVWithinLimits(float2 uv)
            {
                float2 minimumLimit = float2 (0.0, 0.0);
                float2 maximumLimit = float2 (1.0, 1.0);

                float2 isOffsetWithinLimits = min (step (minimumLimit, uv), step (uv, maximumLimit));

                return isOffsetWithinLimits.x * isOffsetWithinLimits.y;
            }
        
            float IsUVWithinInnerOutline(float2 uv)
            {
                float2 size = float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y) * _InnerOutlineThickness;

                // Check the texel itself.
                float2 textureOffset = float2 (uv.x, uv.y);
                float isPixelNotBorder = step (ALPHA_PRECISION, tex2D (_MainTex, textureOffset).a * IsUVWithinLimits (textureOffset));
                
                // Left texel.
                textureOffset.x = uv.x - size.x;
                textureOffset.y = uv.y;
                float isNoNearBorder = step (ALPHA_PRECISION, tex2D (_MainTex, textureOffset).a * IsUVWithinLimits (textureOffset));

                // Top left texel.
                textureOffset.x = (-1.0 * size.x) + uv.x;
                textureOffset.y = size.y + uv.y;
                isNoNearBorder *= step (ALPHA_PRECISION, tex2D (_MainTex, textureOffset).a * IsUVWithinLimits (textureOffset));

                // Top middle texel.
                textureOffset.x = 0.0 + uv.x;
                textureOffset.y = size.y + uv.y;
                isNoNearBorder *= step (ALPHA_PRECISION, tex2D (_MainTex, textureOffset).a * IsUVWithinLimits (textureOffset));

                // Top right texel.
                textureOffset.x = size.x + uv.x;
                textureOffset.y = size.y + uv.y;
                isNoNearBorder *= step (ALPHA_PRECISION, tex2D (_MainTex, textureOffset).a * IsUVWithinLimits (textureOffset));

                // Right texel.
                textureOffset.x = size.x + uv.x;
                textureOffset.y = 0.0 + uv.y;
                isNoNearBorder *= step (ALPHA_PRECISION, tex2D (_MainTex, textureOffset).a * IsUVWithinLimits (textureOffset));

                // Bottom right texel.
                textureOffset.x = size.x + uv.x;
                textureOffset.y = (-1.0 * size.y) + uv.y;
                isNoNearBorder *= step (ALPHA_PRECISION, tex2D (_MainTex, textureOffset).a * IsUVWithinLimits (textureOffset));

                // Bottom middle texel.
                textureOffset.x = 0.0 + uv.x;
                textureOffset.y = (-1.0 * size.y) + uv.y;
                isNoNearBorder *= step (ALPHA_PRECISION, tex2D (_MainTex, textureOffset).a * IsUVWithinLimits (textureOffset));

                // Bottom left texel.
                textureOffset.x = (-1.0 * size.x) + uv.x;
                textureOffset.y = (-1.0 * size.y) + uv.y;
                isNoNearBorder *= step (ALPHA_PRECISION, tex2D (_MainTex, textureOffset).a * IsUVWithinLimits (textureOffset));

                float isPixelWithinOutline = (1.0 - isNoNearBorder) * _EnableInnerOutline * isPixelNotBorder;
                
                return isPixelWithinOutline;
            }

            int CalculateDistanceFromBorder(float2 uv)
            {
                float2 centerPoint = float2 (0.5, 0.5);
                float lineSlope = (uv.y - centerPoint.y) / (uv.x - centerPoint.x);
                float2 uvLookingForBorder = float2 (uv.x, uv.y);
                float4 detectionColor = float4 (1.0, 1.0, 1.0, 1.0);
                
                [unroll]
                for (int distanceFromBorder = 0; distanceFromBorder < 16; distanceFromBorder++)
                {
                    uvLookingForBorder.x = uv.x + (_MainTex_TexelSize.x * distanceFromBorder * sign (uv.x - centerPoint.x));
                    uvLookingForBorder.y = uv.y + (_MainTex_TexelSize.y * distanceFromBorder * sign (uv.y - centerPoint.y));

                    detectionColor = tex2D (_MainTex, uvLookingForBorder);

                    if (step (ALPHA_PRECISION, detectionColor.a * IsUVWithinLimits (uvLookingForBorder)) == 0.0)
                    {
                        return distanceFromBorder;
                    }
                }

                return -1;
            }

            float AlphaOfOutlineUV(float2 uv)
            {
                float2 centerPoint = float2 (0.5, 0.5);
                float lineSlope = (uv.y - centerPoint.y) / (uv.x - centerPoint.x);
                float2 uvLookingForBorder = float2 (0.0, 0.0);
                float4 detectionColor = float4 (1.0, 1.0, 1.0, 1.0);
                int distanceFromBorder = CalculateDistanceFromBorder (uv);
                float alpha = 1.0;

                if (distanceFromBorder > (_InnerOutlineThickness / 1.5))
                {
                    uvLookingForBorder.x = uv.x + (_MainTex_TexelSize.x * (distanceFromBorder - (_InnerOutlineThickness - distanceFromBorder)) * sign (uv.x - centerPoint.x));
                    uvLookingForBorder.y = uv.y + (_MainTex_TexelSize.y * (distanceFromBorder - (_InnerOutlineThickness - distanceFromBorder)) * sign (uv.y - centerPoint.y));

                    alpha = 1.0 / pow (1.5, distanceFromBorder) * IsUVWithinInnerOutline (uvLookingForBorder);
                }
                else
                {
                    alpha = 1.0;
                }

                return alpha;
            }
        
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

            fixed4 SpriteFrag(v2f IN) : SV_Target
            {
                fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;

                // Inner outline.
                float colorAlpha = c.a;
                float innerOutline = IsUVWithinInnerOutline (IN.texcoord);
                float4 outlineColor = float4 (_InnerOutlineColor.r, _InnerOutlineColor.g, _InnerOutlineColor.b, colorAlpha);
                float outlineSmoothingAlpha = AlphaOfOutlineUV (IN.texcoord);
                c = lerp (c, _InnerOutlineColor, _InnerOutlineColor.a * innerOutline * AlphaOfOutlineUV (IN.texcoord));
                c.a = colorAlpha;
                
                /*float alphaOn = AlphaOfOutlineUV (IN.texcoord);
                if (innerOutline > 0.0)
                {
                    int distance = CalculateDistanceFromBorder (IN.texcoord);

                    if (distance < 0)
                    {
                        c = float4(1.0, 1.0, 1.0, alphaOn);
                    }
                    else if (distance == 0)
                    {
                        c = float4 (0.75, 0.5, 0.5, alphaOn);
                    }
                    else if (distance == 1)
                    {
                        c = float4 (1.0, 0, 0, alphaOn);
                    }
                    else if (distance == 2)
                    {
                        c = float4 (0, 1.0, 0, alphaOn);
                    }
                    else if (distance == 3)
                    {
                        c = float4 (0, 0, 1.0, alphaOn);
                    }
                    else if (distance == 4)
                    {
                        c = float4 (0, 1.0, 1.0, alphaOn);
                    }
                    else if (distance == 5)
                    {
                        c = float4 (1.0, 1.0, 0.0, alphaOn);
                    }
                   else if (distance == 6)
                   {
                       c = float4 (1.0, 0.0, 1.0, alphaOn);
                   }
                   else
                   {
                       c = float4 (0.0, 0.0, 0.0, alphaOn);
                   }
                }*/
                
                c.rgb *= c.a;

                

                /*if (innerOutline > 0.0)
                {
                    if (alphaOn < 0.25)
                    {
                        c = float4( 1.0, 1.0, 1.0, 1.0 );
                    }
                    else if (alphaOn < 0.5)
                    {
                        c = float4( 1.0, 0.0, 0.0, 1.0);
                    }
                    else if (alphaOn < 0.75)
                    {
                        c = float4( 0.0, 1.0, 0.0, 1.0);
                    }
                    else if (alphaOn <= 1.0)
                    {
                        c = float4(0.0, 0.0, 1.0, 1.0);
                    }
                    else
                    {
                        c = float4(0.0, 0.0, 0.0, 0.0);
                    }
                }*/

                
                return c;
            }

            #endif // UNITY_SPRITES_INCLUDED
        ENDCG
        }
    }
}
