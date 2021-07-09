Shader "Blending/Blend2Textures" {
    Properties{
        _FirstTex("Base (RGB)", 2D) = "white" {}
        _SecondTex("Base (RGB)", 2D) = "white" {}
        _Blend("Blend", Range(0,1)) = 0.0
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 150

        CGPROGRAM
        #pragma surface surf Lambert noforwardadd

        sampler2D _FirstTex;
        sampler2D _SecondTex;
        fixed _Blend;

        struct Input {
            float2 uv_FirstTex;
        };

        void surf(Input IN, inout SurfaceOutput o) {
            fixed4 f = tex2D(_FirstTex, IN.uv_FirstTex);
            fixed4 s = tex2D(_SecondTex, IN.uv_FirstTex);
            o.Albedo = lerp(f, s, _Blend);
        }
        ENDCG
    }
}