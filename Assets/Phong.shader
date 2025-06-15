Shader "Custom/PhongURPShader"
{
    Properties
    {
        // Main texture
        _MainTex ("Texture", 2D) = "white" {}
        
        // Diffuse color
        _Color ("Color", Color) = (1,1,1,1)
        
        // Specular properties
        _SpecColor ("Specular Color", Color) = (1,1,1,1)
        _Shininess ("Shininess", Range(0.01, 128)) = 32
        
        // Ambient color
        _AmbientColor ("Ambient Color", Color) = (0.2,0.2,0.2,1)
    }

    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
            "RenderPipeline"="UniversalPipeline"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Include URP library for lighting functions
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // Struct for vertex input
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            // Struct for vertex output / fragment input
            struct V2F
            {
                float4 positionCS : SV_POSITION;
                float3 normalWS : NORMAL;
                float2 uv : TEXCOORD0;
                float3 viewDirWS : TEXCOORD1;
                float3 positionWS : TEXCOORD2;
            };

            // Shader properties
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _Color;
                float4 _SpecColor;
                float _Shininess;
                float4 _AmbientColor;
            CBUFFER_END

            // Vertex shader
            V2F vert(Attributes input)
            {
                V2F output;
                
                // Transform position to world space
                VertexPositionInputs positionInputs = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = positionInputs.positionCS;
                output.positionWS = positionInputs.positionWS;

                // Transform normal to world space
                VertexNormalInputs normalInputs = GetVertexNormalInputs(input.normalOS);
                output.normalWS = normalInputs.normalWS;

                // Calculate view direction
                output.viewDirWS = GetWorldSpaceViewDir(positionInputs.positionWS);

                // Pass through UV
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);

                return output;
            }

            // Fragment shader
            half4 frag(V2F input) : SV_Target
            {
                // Sample main texture
                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);

                // Get main light
                Light mainLight = GetMainLight();
                float3 lightDirWS = normalize(mainLight.direction);
                float3 viewDirWS = normalize(input.viewDirWS);
                float3 normalWS = normalize(input.normalWS);

                // Ambient component
                float3 ambient = _AmbientColor.rgb;

                // Diffuse component (Lambert)
                float NdotL = max(dot(normalWS, lightDirWS), 0);
                float3 diffuse = NdotL * mainLight.color * _Color.rgb;

                // Specular component (Phong)
                float3 reflectDir = reflect(-lightDirWS, normalWS);
                float spec = pow(max(dot(viewDirWS, reflectDir), 0), _Shininess);
                float3 specular = spec * _SpecColor.rgb * mainLight.color;

                // Combine components
                float3 finalColor = (ambient + diffuse + specular) * texColor.rgb;

                return half4(finalColor, 1);
            }
            ENDHLSL
        }
    }
    Fallback "Universal Render Pipeline/Lit"
}