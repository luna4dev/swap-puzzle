Shader "Unlit/BorderRadius"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BorderRadius ("BorderRadius", Range(0.01, 0.5)) = 0.05
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
            float _BorderRadius;

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
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                float2 folded_uv = float2(
                    i.uv.x > 0.5 ? 1 - i.uv.x : i.uv.x,
                    i.uv.y > 0.5 ? 1 - i.uv.y : i.uv.y
                );

                float ct_dist = length(folded_uv - _BorderRadius);
                float smooth = 0.01;
                float radius = step(ct_dist, _BorderRadius);
                float u_area = step(_BorderRadius, folded_uv.x);
                float v_area = step(_BorderRadius, folded_uv.y);

                col.a = radius + u_area + v_area >= 1 ? 1 : 0;

                return col;
            }
            ENDCG
        }
    }
}
