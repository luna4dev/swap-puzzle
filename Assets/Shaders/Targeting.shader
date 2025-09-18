Shader "Unlit/Targeting"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Center ("Center", Vector) = (0.5, 0.5, 0, 0)
        _Intensity ("Intensity", Range(0, 1)) = 1
        _Radius ("Radius", Range(0, 0.5)) = 0.3
        _Thickness ("Thickness", Range(0, 1)) = 1
        _PulseSpeed ("PulseSpeed", Range(0, 1)) = 0.2
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
            float4 _Center;
            float _Intensity;
            float _Radius;
            float _Thickness;
            float _PulseSpeed;
            static const float PI = 3.14159265f;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // pulse
                float pulse_factor = (sin(_Time.y * _PulseSpeed * 10) + 1) * 0.2 + 0.5;

                // create circle from the center
                float distance = length(i.uv - _Center.xy);
                float offset = (_Radius * pulse_factor) - distance;

                float tickness_adjusted = _Thickness * 0.05;
                float factor = (sin(((offset + tickness_adjusted) * PI) / (2 * tickness_adjusted)) + 1) / 2;
                fixed4 additive = i.color * (abs(offset) > tickness_adjusted * 2 ? 0 : factor);

                return lerp(col, col + additive, _Intensity);
            }
            ENDCG
        }
    }
}
