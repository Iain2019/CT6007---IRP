Shader "Custom/SphericalGeom" {
    Properties{
       _MainTex("texture", 2D) = "white" {}
       _Center("the given center", Vector) = (0,0,0)
       _Height("the given coefficient", Range(1, 1000) = 10
    }
    Subshader{
        CGPROGRAM
        #pragma surface surf Standard vertex:vert
        sampler2D _MainTex;
        float3 _Center;
        float _Height;

        struct Input { float2 uv_MainTex; }

        // IMPORTANT STUFF
        void vert(inout appdata_full v) {
           float3 world_vertex = mul(unity_ObjectToWorld, v.vertex) - _Center;
           world_vertex = normalize(world_vertex) * _Height;
           v.vertex = mul(unity_WorldToObject, float4(world_vertex, 1));
        }
        // END OF IMPORTANT STUFF

        void surf(Input IN, inout SurfaceOutputStandard o) {
           o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
        }
        ENDCG
    }
}