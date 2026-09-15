Shader "Frog Croaked Team/UI_SpriteBevel"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Bevel ("Bevel", Range(0,1)) = 0.5
        _BevelWidth ("Bevel Width (Texture Pixels)", Range(0.5,16)) = 3
        _LightAngle ("Light Angle (Radians)", Range(0,6.2831853)) = 3.1416
        [HDR] _SpecularColor ("Specular Color", Color) = (1.9399782,2,1.1792452,1)
        _SpecularPower ("Specular Power", Range(0,4)) = 2.44
        _Reflectivity ("Reflectivity", Range(5,15)) = 12.02
        _Diffuse ("Diffuse", Range(0,1)) = 0.474
        _Ambient ("Ambient", Range(0,1)) = 0.568
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend One OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            half4 _TextureSampleAdd, _Color, _SpecularColor;
            float4 _ClipRect;
            float _UIMaskSoftnessX, _UIMaskSoftnessY;
            float _Bevel, _BevelWidth, _LightAngle;
            float _SpecularPower, _Reflectivity, _Diffuse, _Ambient;
            int _UIVertexColorAlwaysGammaSpace;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                half4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                half4 color : COLOR;
                float4 mask : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };
            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                if (_UIVertexColorAlwaysGammaSpace && !IsGammaSpace())
                    v.color.rgb = UIGammaToLinear(v.color.rgb);
                o.color = v.color * _Color;
                float2 pixelSize = o.vertex.w;
                pixelSize /= abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));
                float4 rect = clamp(_ClipRect, -2e10, 2e10);
                o.mask = float4(v.vertex.xy * 2 - rect.xy - rect.zw,
                    0.25 / (0.25 * float2(_UIMaskSoftnessX, _UIMaskSoftnessY) + abs(pixelSize)));
                return o;
            }
            half4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
                half4 color = (tex2D(_MainTex, i.uv) + _TextureSampleAdd) * i.color;
                // A sprite has coverage alpha instead of TMP's signed distance field.
                // Approximate the bevel height from neighboring coverage samples.
                float2 delta = abs(_MainTex_TexelSize.xy) * _BevelWidth;
                float left = tex2D(_MainTex, i.uv - float2(delta.x, 0)).a;
                float right = tex2D(_MainTex, i.uv + float2(delta.x, 0)).a;
                float down = tex2D(_MainTex, i.uv - float2(0, delta.y)).a;
                float up = tex2D(_MainTex, i.uv + float2(0, delta.y)).a;
                float2 slope = float2(right - left, up - down) * (_Bevel * 4);
                float3 n = normalize(float3(-slope, -1));
                float3 light = normalize(float3(sin(_LightAngle), cos(_LightAngle), -1));
                float specular = pow(max(0, dot(n, light)), _Reflectivity) * _SpecularPower;
                half3 lit = color.rgb + _SpecularColor.rgb * specular;
                lit *= 1 - dot(n, light) * _Diffuse;
                lit *= lerp(_Ambient, 1, n.z * n.z);
                // At zero bevel the original image is unchanged.
                color.rgb = lerp(color.rgb, lit, saturate(_Bevel));
                #ifdef UNITY_UI_CLIP_RECT
                half2 mask = saturate((_ClipRect.zw - _ClipRect.xy - abs(i.mask.xy)) * i.mask.zw);
                color.a *= mask.x * mask.y;
                #endif
                #ifdef UNITY_UI_ALPHACLIP
                clip(color.a - 0.001);
                #endif
                color.rgb *= color.a;
                return color;
            }
            ENDCG
        }
    }
}
