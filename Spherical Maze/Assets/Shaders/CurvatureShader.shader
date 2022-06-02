Shader "Custom/CurvatureShader"
{
    Properties
    {
        //inputs for unity 
        _MainTex("_BaseMap", 2D) = "" {}
        _BaseColour ("_BaseColour", Color) = (0.1, 0.1, 0.1, 1)
        _Curvature("_Curvature", Float) = 0.0
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

            //inputs for shader
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

                //get world pos of vertex
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                //get pos of vertex from camera
                float3 posToCam = _WorldSpaceCameraPos - worldPos;

                //get distance to vertex from eye
                float distance = sqrt((posToCam.x * posToCam.x) + (posToCam.z * posToCam.z));
                //apply distance into quadratic for chanage in vertex psoition
                float result = (distance * distance) * _Curvature;
                
                //move vertex
                o.vertex.y += result;

                //normalize
                o.vertex = normalize(o.vertex);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // sample the texture
                half4 color = tex2D(_MainTex, i.uv) * _BaseColour;
                //apply fog
                UNITY_APPLY_FOG(i.fogCoord, color);

                return color;
            }
            ENDCG
        }
    }
}
