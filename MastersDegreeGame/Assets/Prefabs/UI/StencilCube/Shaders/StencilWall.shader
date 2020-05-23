Shader "Survival/UI/StencilWall"
{
    Properties
    {
        //_Color ("Color", Color) = (1,1,1,1)
       
       _SRef ("Stencil Ref", Float) = 1
        
        [Enum(UnityEngine.Rendering.CompareFunction)]	_SComp("Stencil Comp", Float)	= 8
		[Enum(UnityEngine.Rendering.StencilOp)]	_SOp("Stencil Op", Float)		= 2
    }
    SubShader
    {
        Tags { "Queue"="Geometry-1" }
        
        ZWrite off
        ColorMask 0
        
         Stencil 
        {
            Ref [_SRef]
            Comp [_SComp]
            Pass [_SOp]
        }

        CGPROGRAM
        
        #pragma surface surf Lambert 

        //sampler2D _MainTex;
        
        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            //fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            //o.Albedo = c.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
