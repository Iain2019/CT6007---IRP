Shader "Custom/GnomonicShader"
{
    Properties
    {
        _MainTex("BaseMap", 2D) = "" {}
        _BaseColour("BaseColour", Color) = (0.1, 0.1, 0.1, 1)
        //_Centre("Centre", Vector) = (2.5, 2.5, 0, 0)
        //_Width("Width", float) = 0.0
        //_Height("Height", float) = 0.0
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
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
                float _Width;
                float _Height;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    UNITY_TRANSFER_FOG(o,o.vertex);

                    float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    float3 posToCam = _WorldSpaceCameraPos - worldPos;


                    float xSquared = worldPos.x * worldPos.x;
                    float zSquared = worldPos.z * worldPos.z;

                    float denominator = 1 + xSquared + zSquared;
                    float xPos = (2 * worldPos.x) / denominator;
                    float yPos = (2 * worldPos.z) / denominator;
                    float zPos = (1 - xSquared - zSquared) / denominator;

                    float xPosG = xPos / yPos;
                    float yPosG = yPos;
                    float zPosG = zPos / yPos;

                    o.vertex.x += xPosG;
                    o.vertex.y += yPosG;
                    o.vertex.z += zPosG;
                    o.vertex.w = 1;

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
