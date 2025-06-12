// このシェーダー、何をやっているのかよくわからないです
// このときはUnityのシェーダー触るの初めてだったので、思い出として取っておきます。
Shader "Unlit/FluidShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        _Alpha ("Alpha", Range(0,1)) = 0.5
        _Fill("Fill", Range(-1, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
        Blend One OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 alpha : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed _Alpha;
            fixed4 _Color;
            float _FillAmount;

            v2f vert (appdata v)
            {
                v2f o;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.alpha = v.vertex.z < _FillAmount * 0.01 ? (1.0, 1.0, 1.0, 1.0) : (1.0, 1.0, 1.0, 0.0);
                
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _Color;
                col.a = i.alpha;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}