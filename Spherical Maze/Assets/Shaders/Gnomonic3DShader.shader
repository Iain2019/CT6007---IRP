Shader "Custom/Gnomonic3DShader"
{
    Properties
    {
        //inputs for unity 
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

                //inputs for shader
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

                    //get world pos of vertex
                    float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    //get pos of vertex from camera
                    float3 posToCam = _WorldSpaceCameraPos - worldPos;

                    //sqaure values ahead of time to save on amount of times it needs to be calcuated
                    //due to the large amounts of time calculation is made little makes a difference
                    float xSquared = posToCam.x * posToCam.x;
                    float zSquared = posToCam.z * posToCam.z;

                    //calculate demoninator ahead of time to save on amount of times it needs to be calcuated
                    //due to the large amounts of time calculation is made little makes a difference                    
                    float denominator = 1 + xSquared + zSquared;
                    //Sereographically project to sphere of radius 1
                    float xPos = (2 * posToCam.x) / denominator;
                    float yPos = (2 * posToCam.z) / denominator;
                    float zPos = (1 - xSquared - zSquared) / denominator;

                    //gnomically project back to the flat plane
                    float xPosG = xPos / yPos;
                    float yPosG = yPos / yPos;
                    float zPosG = zPos / yPos;

                    //apply to verticies
                    o.vertex.x += xPosG;
                    o.vertex.y += yPosG;
                    o.vertex.z += zPosG;
                    //o.vertex.w = 1;

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
