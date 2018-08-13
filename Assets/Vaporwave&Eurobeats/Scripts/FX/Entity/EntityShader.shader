// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/EntityShader" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _OcclusionColor("Occlusion Color", Color) = (0.25, 0.32, 0.71, 1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
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
        
        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

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

        void surf (Input IN, inout SurfaceOutputStandard o) {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            fixed3 e = tex2D(_Emission, IN.uv_MainTex);
            o.Emission = e.rbg * e.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}