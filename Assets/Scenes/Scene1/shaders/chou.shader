Shader "Unlit/chou"
{
    Properties {
        _BendScale("Bend Scale", Range(0.0, 1.0)) = 0.2
        _MainTex("Main Texture", 2D) = "white" {}
    }
    SubShader {
        Tags { "RenderType" = "Transparent" }
        Cull Off Zwrite On
        CGPROGRAM
        #pragma surface surf Lambert alpha vertex:vert
        #define PI 3.14159
        struct Input {
            float2 uv_MainTex;
            float4 color : Color;
        };
        sampler2D _MainTex;
        float _BendScale;
        void vert (inout appdata_full v) {
            float bend = sin(PI * _Time.x * 1000 / 45 + v.vertex.y);
            float x = sin(v.texcoord.x * PI) - 1.0;
            float y = sin(v.texcoord.y * PI) - 1.0;
            v.vertex.y += _BendScale * bend * (x + y);
        }
        void surf (Input IN, inout SurfaceOutput o) {
            float4 tex = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = IN.color.rgb;
            o.Alpha  = IN.color.a * tex.a;
        }
        ENDCG
    }
    Fallback "Diffuse"
}
