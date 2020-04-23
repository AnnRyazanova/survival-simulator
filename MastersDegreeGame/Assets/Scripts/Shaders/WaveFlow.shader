Shader "Survival/WaveFlow"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _TintAmount("Tint amount", Range(0,1)) = .5
        _ColorA ("Color A", Color) = (1,1,1,1)
        _ColorB ("Color B", Color) = (1,1,1,1)
        _Speed("Wave Speed", Range(0.1, 80)) = 5
        _Frequency("Wave Frequency", Range(0, 5)) = 2
        _Amplitude("Wave Amplitude", Range(-1, 1)) = 1 
        
        _BumpTex ("Bump Texture", 2D) = "bump" {}
        _BumpRange ("Bump Amount", Range(0, 10)) = 1
        _BumpScaleRange ("Texture Bump Scale", Range(0, 2)) = 1
    }
    SubShader
    {
		Tags { "ForceNoShadowCasting"="True"}
    
        CGPROGRAM
        
        // vertex:vert - указание, что используем вершинную функцию vert
        #pragma surface surf Lambert vertex:vert

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpTex;
            float3 vertColor;
        };
        
        sampler2D _MainTex;
        float4 _ColorA;
        float4 _ColorB;
        float _TintAmount;
        float _Speed;
        float _Frequency;
        float _Amplitude;
        
        sampler2D _BumpTex;
        half _BumpRange;
        half _BumpScaleRange;
        
        void vert(inout appdata_full v, out Input o)
        {
            // указание, чтобы шейдер компилировался под DirectX 11
            UNITY_INITIALIZE_OUTPUT(Input, o);
            float time = _Time.x * _Speed;
            // Вычисление волны по синусоиде
            float waveValue = sin(time + v.vertex.x * _Frequency) * _Amplitude;
            v.vertex.xyz = float3(v.vertex.x, v.vertex.y + waveValue, v.vertex.z);
            v.normal = normalize(float3(v.normal.x, v.normal.y + waveValue, v.normal.z));
            o.vertColor = float3(waveValue, waveValue, waveValue);
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            half4 color = tex2D(_MainTex, IN.uv_MainTex * _BumpScaleRange);
            float3 tintColor = lerp(_ColorA, _ColorB, IN.vertColor).rgb;
            
            o.Albedo = color.rgb * (tintColor * _TintAmount);
            o.Alpha = color.a;
            
            o.Normal = UnpackNormal(tex2D (_BumpTex, IN.uv_BumpTex * _BumpScaleRange)); 
            o.Normal *= float3(_BumpRange, _BumpRange, 1);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
