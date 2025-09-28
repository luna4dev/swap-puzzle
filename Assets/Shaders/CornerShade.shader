Shader "Unlit/BorderRadius"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _UOffset ("UOffset", Range(0, 1)) = 0.1
        _VOffset ("VOffset", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags { 
            "RenderType"="Transparent" 
            "Queue"="Transparent"
        }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _UOffset;
            float _VOffset;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float u_shade = step(i.uv.x - _UOffset, float2(0, 0));
                float v_shade = step(i.uv.y - _VOffset, float2(0, 0));

                float shade = saturate(u_shade + v_shade);

                shade *= tex2D(_MainTex, float2(
                    i.uv.x + _UOffset,
                    i.uv.y + _VOffset
                )).a;

                return lerp(col, col * i.color, shade);
            }
            ENDCG
        }
    }
}
