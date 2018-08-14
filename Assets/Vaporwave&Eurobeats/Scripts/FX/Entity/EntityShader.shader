Shader "Custom/EntityShader" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _OcclusionColor("Occlusion Color", Color) = (0.25, 0.32, 0.71, 1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Emission("Emission", 2D) ="white" {}
        _EmissionScale("EmissionScale", Float) = 3
        _HueShift ("Hue shift", Float) = 0
        _Glossiness ("Glossiness", Float) = 0
        _Metallic ("_Metallic", Float) = 1.25
    }
    SubShader {
        LOD 200
        Tags { 
	        "Queue"="Transparent" 
	        "RenderType"="Opaque"
	        "PreviewType"="Plane"
	     }    
        Pass {
		    ZWrite Off        
            Blend SrcAlpha OneMinusSrcAlpha
            ZTest Greater
            Cull Off
            CGPROGRAM
		        #pragma vertex vert
			    #pragma fragment frag
    			
    			#include "UnityCG.cginc"

	    		struct appdata
		    	{
			    	float4 vertex : POSITION;
				    float2 uv : TEXCOORD;
			    };

			    struct v2f {
				    float2 uv : TEXCOORD;
				    float4 vertex : SV_POSITION;
			    };

			    v2f vert (appdata v) {
				    v2f o;
				    o.vertex = UnityObjectToClipPos(v.vertex);
				    o.uv = v.uv;
				    return o;
			    }

			sampler2D _MainTex;
			fixed4 _OcclusionColor;
			fixed4 frag (v2f i) : SV_Target {
			    fixed4 rawCol = tex2D(_MainTex, i.uv);
				fixed4 col = _OcclusionColor;
				col.a = rawCol.a;
				return col;
			}
        ENDCG
        
        }
        Pass {
		    ZWrite Off        
            Blend SrcAlpha OneMinusSrcAlpha
            ZTest Greater
            Cull Off
            CGPROGRAM
		        #pragma vertex vert
			    #pragma fragment frag
    			
    			#include "UnityCG.cginc"

	    		struct appdata
		    	{
			    	float4 vertex : POSITION;
				    float2 uv : TEXCOORD;
			    };

			    struct v2f {
				    float2 uv : TEXCOORD;
				    float4 vertex : SV_POSITION;
			    };

			    v2f vert (appdata v) {
				    v2f o;
				    o.vertex = UnityObjectToClipPos(v.vertex);
				    o.uv = v.uv;
				    return o;
			    }

			sampler2D _MainTex;
			fixed4 _OcclusionColor;
			fixed4 frag (v2f i) : SV_Target {
			    fixed4 rawCol = tex2D(_MainTex, i.uv);
				fixed4 col = _OcclusionColor;
				col.a = rawCol.a;
				return col;
			}
        ENDCG
        
        }
Cull Off
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha:blend

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _Emission;
        struct Input {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
			
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
            
        float _HueShift;
        float _EmissionScale;
        void surf (Input IN, inout SurfaceOutputStandard o) {
            // Albedo comes from a texture tinted by color
            fixed4 original = tex2D(_MainTex, IN.uv_MainTex);
            float3 col = RGBtoHSV(original);
            fixed4 hsv = fixed4(col.r, col.g, col.b, 1);
            hsv.r += _HueShift;
            float3 final = HSVtoRGB(hsv);
            o.Albedo = final;
            // Metallic and smoothness come from slider variables
            o.Normal = float3(0,0, 1);
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = original.a;
            fixed4 e = tex2D(_Emission, IN.uv_MainTex);
            o.Emission = e.rbg * e.a * _EmissionScale;
        }
        ENDCG
    }
    FallBack "Diffuse"
}