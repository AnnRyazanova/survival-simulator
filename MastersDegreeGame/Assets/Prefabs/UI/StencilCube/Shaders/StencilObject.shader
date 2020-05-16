Shader "Survival/UI/StencilObject"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Range ("Range", Range(0, 5)) = 1.5
        
        _SRef ("Stencil Ref", Float) = 1
        [Enum(UnityEngine.Rendering.CompareFunction)]	_SComp("Stencil Comp", Float)	= 8
		[Enum(UnityEngine.Rendering.StencilOp)]	_SOp("Stencil Op", Float)		= 2
    }
    SubShader
    {
        Stencil 
        {
            Ref [_SRef]
            Comp [_SComp]
            Pass [_SOp]
        }
    
        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        fixed4 _Color;
        half _Range;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color * _Range;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
