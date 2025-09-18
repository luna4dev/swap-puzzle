Shader "Unlit/LensFlare"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1, 1, 1, 1)
        _FlareIntensity ("Flare Intensity", Range(0, 2)) = 0.5
        _FlareColor ("Flare Color", Color) = (1, 1, 1, 1)
        _FlareSize ("Flare Size", Range(0.1, 5)) = 1.0
        _FlareCenter ("Flare Center", Vector) = (0.5, 0.5, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _FlareIntensity;
            float4 _FlareColor;
            float _FlareSize;
            float4 _FlareCenter;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // calculate lens flare effect
                float2 flareUV = i.uv - _FlareCenter.xy;
                float dist = length(flareUV);

                // create multiple flare rings
                float flare1 = 1.0 - smoothstep(0.0, 0.2 / _FlareSize, dist);
                float flare2 = 1.0 - smoothstep(0.1 / _FlareSize, 0.4 / _FlareSize, dist);
                float flare3 = 1.0 - smoothstep(0.3 / _FlareSize, 0.6 / _FlareSize, dist);

                // combine flares with different intensities
                float flareEffect = (flare1 * 0.8 + flare2 * 0.4 + flare3 * 0.2) * _FlareIntensity;

                // add directional streaks
                float2 dir = normalize(flareUV);
                float streak = abs(dot(dir, float2(1, 0))) + abs(dot(dir, float2(0, 1)));
                streak = pow(streak, 8.0) * _FlareIntensity * 0.3;

                // combine flare with streaks
                flareEffect += streak;

                // apply flare to color
                fixed4 flareCol = _FlareColor * flareEffect;
                col.rgb += flareCol.rgb;

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
