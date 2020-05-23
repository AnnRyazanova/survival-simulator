Shader "Survival/Fire"
{
    Properties
    {
        _MainTex ("Fire Noise", 2D) = "white" {}
        _ScrollSpeed ("Speed", Range(0,2)) = 1
        
        _Color1 ("Color1", Color) = (1,1,1,1)
        _Color2 ("Color2", Color) = (1,1,1,1)
        _Color3 ("Color3", Color) = (1,1,1,1)
        
        _Frontier1 ("Frontier 1", Range(0,1)) = .25
        _Frontier2 ("Frontier 2", Range(0,1)) = .5
        
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
        
        Cull off
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            fixed4 _Color1;
            fixed4 _Color2;
            fixed4 _Color3;
            float _ScrollSpeed;
            
            float _Frontier1;
            float _Frontier2;

            sampler2D _MainTex;
            float4 _MainTex_ST;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 position : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv; //TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            //smooth version of step
			float aaStep(float compValue, float gradient){
			    float change = fwidth(gradient) / 2;
			    //base the range of the inverse lerp on the change over two pixels
			    float lowerEdge = compValue - change;
			    float upperEdge = compValue + change;
			    //do the inverse interpolation
			    float stepped = (gradient - lowerEdge) / (upperEdge - lowerEdge);
			    stepped = saturate(stepped);
			    
			    return stepped;
			}

            fixed4 frag (v2f i) : SV_Target
            {
                //To square fireGradient here to make the fire smaller
			    float fireGradient = 1 - i.uv.y;
			    fireGradient = fireGradient * fireGradient;
			    
			    //calculate fire UVs and animate them
			    float2 fireUV = TRANSFORM_TEX(i.uv, _MainTex);
			    fireUV.y -= _Time.y * _ScrollSpeed;
			    //get the noise texture
			    float fireNoise = tex2D(_MainTex, fireUV).x;
			    
			    //calculate whether fire is visibe at all and which colors should be shown
                //float outline = aaStep(fireNoise, fireGradient);
                //float edge1 = aaStep(fireNoise, fireGradient - _Edge1);
                //float edge2 = aaStep(fireNoise, fireGradient - _Edge2);
                
                float outline = step(fireNoise, fireGradient);
                float edge1 = step(fireNoise, fireGradient - _Frontier1);
                float edge2 = step(fireNoise, fireGradient - _Frontier2);
			    
                //define shape of fire
			    fixed4 col = _Color1 * outline;
			    //add other colors
			    col = lerp(col, _Color2, edge1);
			    col = lerp(col, _Color3, edge2);
			    
				return col;
            }
            ENDCG
        }
    }
}
