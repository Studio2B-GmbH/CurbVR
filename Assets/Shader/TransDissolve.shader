Shader "Legacy Shaders/Transparent/Cutout/Dissolve" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
		_DissolveTex("Dissolve (Grayscale)", 2D) = "white" {}
	}

		SubShader{
			Tags {"Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout"}
			LOD 200

		CGPROGRAM
		#pragma surface surf Lambert alphatest:_Cutoff

		sampler2D _MainTex;
	sampler2D _DissolveTex;
		fixed4 _Color;

		struct Input {
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			fixed4 d = tex2D(_DissolveTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = d.r;
		}
		ENDCG
	}
}