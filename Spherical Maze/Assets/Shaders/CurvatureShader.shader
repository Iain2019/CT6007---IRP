Shader "Custom/CurvatureShader"
{
    Properties
    {
        _MainTex("BaseMap", 2D) = "" {}
        _BaseColour ("BaseColour", Color) = (0.1, 0.1, 0.1, 1)
        _Curvature("Curvature", Range(-0.2, 0.2)) = 0.0
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _BaseColour;
            float _Curvature;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 posToCam = _WorldSpaceCameraPos - worldPos;

                float distance = sqrt((posToCam.x * posToCam.x) + (posToCam.z * posToCam.z));
                float result = (distance * distance) * _Curvature;
                
                o.vertex.y += result;

                o.vertex = normalize(o.vertex);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // sample the texture
                half4 color = tex2D(_MainTex, i.uv) * _BaseColour;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, color);

                return color;
            }
            ENDCG
        }
    }
}
