Shader "Blending/CrossDissolve"
{
    Properties
    {
        _Lerp("Lerp", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
        _Glossiness("Smoothness", Range(0,1)) = 0.5

        _MainTex("Texture 1 (RGB)", 2D) = ""
        _SecondTex("Texture 2 (RGB)", 2D) = ""
        _Dissolve("Dissolve (Grayscale)", 2D) = ""



    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex; 
        sampler2D _SecondTex;
        sampler2D _Dissolve;
        float _Lerp;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 s = tex2D(_SecondTex, IN.uv_MainTex);
            fixed4 d = tex2D(_Dissolve, IN.uv_MainTex);

            c = lerp(c, s, saturate(sign(d.r - _Lerp)));
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
