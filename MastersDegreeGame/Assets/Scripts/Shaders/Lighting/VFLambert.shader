Shader "Survival/Lighting/VFLambert"
{
     Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        
        [Toggle] _ReceiveShadows ("Receive Shadows", Float) = 0
    }
    SubShader
    {
        Pass
        {
            Tags { "LightMode" = "ForwardBase" }
        
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"
            
            // to receive shadows
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            #include "lighting.cginc"
            #include "AutoLight.cginc"
            //
            
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                fixed4 diffuse : COLOR0;
                SHADOW_COORDS(1)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;     
            fixed4 _Color;
            float _ReceiveShadows;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
               
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                
                half nl = max (0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                o.diffuse = nl * _LightColor0;
                
                TRANSFER_SHADOW(o)      
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                fixed shadow = SHADOW_ATTENUATION(i);
                fixed shadowCoeff = _ReceiveShadows > 0 ? shadow : 1;
                col *= i.diffuse * shadowCoeff; 
                return col;
            }
            ENDCG
        }
        
        // To put shadow on the surface
        Pass
        {
            Tags { "LightMode" = "ShadowCaster" }
        
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                V2F_SHADOW_CASTER;
            };

            v2f vert (appdata v)
            {
                v2f o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i);
            }
            ENDCG
        }
    }
}
