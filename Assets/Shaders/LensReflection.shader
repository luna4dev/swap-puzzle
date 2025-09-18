Shader "Unlit/LensReflection"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LensColor ("Lens Color", Color) = (0.8, 0.9, 1.0, 1.0)
        _ReflectionIntensity ("Reflection Intensity", Range(0, 2)) = 0.5
        _LensSize("Lens Size", Range(0.1, 1.0)) = 0.4
        _RimWidth("Rim Width", Range(0.01, 0.1)) = 0.03
        _Center ("Center", Vector) = (0.5, 0.5, 0, 0)
    }
    SubShader
    {
        Tags { 
            "RenderType"="Transparent" 
            "Queue"="Transparent"
            "IgnoreProjector"="True"
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _LensColor;
            float _ReflectionIntensity;
            float _LensSize;
            float _RimWidth;
            float4 _Center;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // calculate distance from center
                float2 center = _Center.xy;
                float2 uv = i.uv - center;
                float distance = length(uv);

                // create circular lens boundary
                float lensEdge = _LensSize;
                float rimInner = lensEdge - _RimWidth;
                float rimOuter = lensEdge + _RimWidth;

                // calculate lens reflection effect
                float reflection = 0.0;

                // Main circular reflection at the rim
                if (distance > rimInner && distance < rimOuter) {
                    float rimFactor = 1.0 - abs(distance - lensEdge) / _RimWidth;
                    rimFactor = smoothstep(0.0, 1.0, rimFactor);
                    reflection = rimFactor;
                }

                // add subtle inner reflection
                float innerReflection = 1.0 - smoothstep(0.0, lensEdge * 0.8, distance);
                innerReflection *= 0.1; // much weaker

                // combine reflections
                reflection = max(reflection, innerReflection);

                // add some angle-based variation for more realistic look
                float angle = atan2(uv.y, uv.x);
                float angleVariation = sin(angle * 6.0) * 0.1 + 1.0; // slight hexagonal hint
                reflection *= angleVariation;

                // apply lens color and intensity
                float3 lensReflection = _LensColor.rgb * reflection * _ReflectionIntensity;

                // blend with base
                col.rgb += lensReflection;

                float alpha = smoothstep(rimOuter + 0.1, rimOuter, distance);
                col.a *= alpha;

                return col;
            }
            ENDCG
        }
    }
}
