Shader "VE/CharacterPreviewShader"
{
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _HueShift("Hue Shift", Float) = 0
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            
            #include "UnityCG.cginc"
            float3 HUEtoRGB( float H) {
                float R = abs(H * 6 - 3) - 1;
                float G = 2 - abs(H * 6 - 2);
                float B = 2 - abs(H * 6 - 4);
                return saturate(float3(R,G,B));
            }
            
            float Epsilon = 1e-10;
 
            float3 RGBtoHCV( float3 RGB) {
                float4 P = (RGB.g < RGB.b) ? float4(RGB.bg, -1.0, 2.0/3.0) : float4(RGB.gb, 0.0, -1.0/3.0);
                float4 Q = (RGB.r < P.x) ? float4(P.xyw, RGB.r) : float4(RGB.r, P.yzx);
                float C = Q.x - min(Q.w, Q.y);
                float H = abs((Q.w - Q.y) / (6 * C + Epsilon) + Q.z);
                return float3(H, C, Q.x);
            }
            float3 HSVtoRGB( float3 HSV) {
                float3 RGB = HUEtoRGB(HSV.x);
                return ((RGB - 1) * HSV.y + 1) * HSV.z;
            }
            
            float3 RGBtoHSV(float3 RGB) {
                float3 HCV = RGBtoHCV(RGB);
                float S = HCV.y / (HCV.z + Epsilon);
                return float3(HCV.x, S, HCV.z);
            }
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _HueShift;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target {
                fixed4 original = tex2D(_MainTex, i.uv);
                float3 col = RGBtoHSV(original);
                fixed4 hsv = fixed4(col.r, col.g, col.b, 1);
                hsv.r += _HueShift;
                float3 final = HSVtoRGB(hsv);
                return float4(final.r, final.g, final.b, original.a);
            }
            ENDCG
        }
    }
}