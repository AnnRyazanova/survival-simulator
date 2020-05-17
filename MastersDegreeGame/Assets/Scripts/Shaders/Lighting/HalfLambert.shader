Shader "Survival/Lighting/HalfLambert"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        //_MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        CGPROGRAM
       
        #pragma surface surf HalfLambert
        
        float4 LightingHalfLambert(SurfaceOutput o, fixed3 lightDir, fixed atten)
        {
            float NdotL = max(0, dot(o.Normal, lightDir));
            float HalfLambert = pow(NdotL * 0.5 + 0.5, 2);
            
            float4 color;
            color.rgb = o.Albedo * _LightColor0.rgb * (HalfLambert * atten);
            color.a = o.Alpha;
            return color;
        }

        sampler2D _MainTex;       
        fixed4 _Color;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            //fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = _Color.rgb;
            o.Alpha = _Color.a;
        }
        ENDCG
    }
}
