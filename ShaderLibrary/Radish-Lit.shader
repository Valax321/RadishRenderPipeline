Shader "Radish Pipeline/Lit"
{
    Properties
    {
        _BaseMap ("Albedo", 2D) = "white" {}
        _Color ("Tint", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="Radish"
            "LightMode"="RadishLit"
        }
        
        HLSLINCLUDE

        #include "Packages/com.radish.render-pipeline/ShaderLibrary/Common.hlsl"

        struct Attributes
        {
            float4 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float2 texcoord0 : TEXCOORD0;
        };

        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float3 normalWS : NORMAL0;
            float2 texcoord0 : TEXCOORD0;
        };

        struct MaterialProperties
        {
            float4 color;
        };

        RADISH_DECLARE_TEX2D_SAMPLER(_BaseMap);

        CBUFFER_START(UnityPerMaterial)
            float4 _Color;
            float4 _BaseMap_ST;
        CBUFFER_END

        Varyings RadishLitVert(in Attributes IN)
        {
            Varyings OUT;
            
            OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
            OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
            OUT.texcoord0 = TRANSFORM_TEX(IN.texcoord0, _BaseMap);

            return OUT;
        }

        MaterialProperties CalcMaterialProperties(in Varyings IN)
        {
            MaterialProperties props;

            props.color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.texcoord0);

            return props;
        }

        float4 CalcLighting(in MaterialProperties materialProps, in Varyings IN)
        {
            const float3 lightDir = normalize(float3(1, 1, 1));
            float3 lightColor = saturate(dot(lightDir, IN.normalWS));
            
            return float4(lightColor, materialProps.color.a);
        }

        float4 RadishLitFrag(in Varyings IN) : SV_TARGET
        {
            MaterialProperties materialProps = CalcMaterialProperties(IN);
            return CalcLighting(materialProps, IN);
        }
        
        ENDHLSL

        Pass
        {
            Name "Lit"
            ZWrite On
            Cull Back
            
            HLSLPROGRAM

            #pragma vertex RadishLitVert
            #pragma fragment RadishLitFrag
            
            ENDHLSL
        }
    }
}