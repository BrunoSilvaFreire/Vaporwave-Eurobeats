// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/ParticleShader" {
    Properties {
        _Emission ("Emission", Color) = (1,0,1,1)
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha:blend

        #pragma target 3.0

        sampler2D _MainTex;
        fixed4 _Emission;
        
        struct Input {
            float2 uv_MainTex;
            float4 color : COLOR;
        };

        void surf (Input IN, inout SurfaceOutputStandard o) {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * IN.color;
            o.Albedo = c.rgb;
            o.Alpha = IN.color.a;
            o.Emission = _Emission * _Emission.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}