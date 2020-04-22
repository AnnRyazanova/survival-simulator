Shader "Survival/Flow" {
	Properties {
		_Color ("Base Color", Color) = (1,1,1,1)

        [Header(Spec Layer 1)]
		_SpecsTex1 ("Specs Texture", 2D) = "white" {}
        _SpecColor1 ("Spec Color", Color) = (1,1,1,1)
        _SpecDirection1 ("Spec Direction", Vector) = (0, 1, 0, 0)

        [Header(Spec Layer 2)]
		_SpecsTex2 ("Specs Texture", 2D) = "white" {}
        _SpecColor2 ("Spec Color", Color) = (1,1,1,1)
        _SpecDirection2 ("Spec Direction", Vector) = (0, 1, 0, 0)
	}
	SubShader {
	
	    // Shader with transparency
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "ForceNoShadowCasting"="True"}
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types, then set it to render transparent
		#pragma surface surf Standard fullforwardshadows alpha

		#pragma target 4.0

		struct Input {
			float2 uv_SpecsTex1;
            float2 uv_SpecsTex2;
            float2 uv_FoamNoise;
            float eyeDepth;
            float4 screenPos;
		};

        sampler2D_float _CameraDepthTexture;

		fixed4 _Color;

        sampler2D _SpecsTex1;
        fixed4 _SpecColor1;
        float2 _SpecDirection1;

        sampler2D _SpecsTex2;
        fixed4 _SpecColor2;
        float2 _SpecDirection2;

		void surf (Input IN, inout SurfaceOutputStandard o) {
            //set river base color
			fixed4 col = _Color;
			
            // ============== region Scrolling UVs ==============
            //add first layer of moving specs
            
            // we calculate the spec coordinates by adding the direction of the scrolling multiplied by the time to the base coordinates of the texture. 
            // _Time.y parameter is the unscaled time 
            float2 specCoordinates1 = IN.uv_SpecsTex1 + _SpecDirection1 * _Time.y;
            fixed4 specLayer1 = tex2D(_SpecsTex1, specCoordinates1) * _SpecColor1;
            col.rgb = lerp(col.rgb, specLayer1.rgb, specLayer1.a);
            col.a = lerp(col.a, 1, specLayer1.a);

            //add second layer of moving specs
            float2 specCoordinates2 = IN.uv_SpecsTex2 + _SpecDirection2 * _Time.y;
            fixed4 specLayer2 = tex2D(_SpecsTex2, specCoordinates2) * _SpecColor2;
            col.rgb = lerp(col.rgb, specLayer2.rgb, specLayer2.a);
            col.a = lerp(col.a, 1, specLayer2.a);
            // ============== endregion ==============

            //apply values to output struct
			o.Albedo = col.rgb;
			o.Alpha = col.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}