Shader "Unlit/Procedural Arrow"
{
    Properties
    {
        _Lerp("Arrow Extension", Range(0,1)) = 0.5
        _Color("Main Color", Color) = (1,1,1,1)
        _ArrowTex ("Arrow Head Texture (RGB)", 2D) = "white" {}
        _TailTex ("Arrow Tail Texture (RGB)", 2D) = "white" {}
        _FadeTex ("Fade Texture Gradient (Grayscale)", 2D) = "white" {}
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
        LOD 200
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha


        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            //#pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uvComplete : TEXCOORD0;
                float2 uvHead : TEXCOORD1;
                float2 uvTail : TEXCOORD2;
            };

            struct v2f
            {
                float2 uvComplete : TEXCOORD0;
                float2 uvHead : TEXCOORD1;
                float2 uvTail : TEXCOORD2;
                //UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _ArrowTex;
            sampler2D _TailTex;
            sampler2D _FadeTex;
            float4 _ArrowTex_ST;
            float4 _TailTex_ST;
            float4 _FadeTex_ST;
            fixed4 _Color;
            float _Lerp;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uvHead = TRANSFORM_TEX(v.uvHead, _ArrowTex);
                o.uvTail = TRANSFORM_TEX(v.uvTail, _TailTex);
                o.uvComplete = TRANSFORM_TEX(v.uvComplete, _FadeTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 head = tex2D(_ArrowTex, i.uvHead);
                fixed4 tail = tex2D(_TailTex, i.uvTail);
                fixed4 fade = tex2D(_FadeTex, i.uvComplete);

                fixed4 col = (head + tail);
                col.a = col.a - saturate(sign(fade.r - _Lerp));
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                return col * _Color;
            }
            ENDCG
        }
    }
}
