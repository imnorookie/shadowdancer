Shader"Custom/WaterShader"
{
    Properties{
        _Color("Tint", Color) = (0,0,0,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Amplitude("Wave Size", Range(0,1)) = 0.4
        _Frequency("Wave Frequency", Range(1,10)) = 2
        _SecondaryFrequency("Wave Second Frequency", Range(1,10)) = 2
        _AnimationSpeed ("Animation Speed", Range(0,20)) = 1
	    _Smoothness ("Smoothness", float) = 0
        _Metallic("Metallic", Range(0,100)) = 1

        [HDR] _Emission ("Emission", color) = (0,0,0)
        _FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
        [PowerSlider(4)] _FresnelExponent ("Fresnel Exponent", Range(0.25, 20)) = 1
    }    

    SubShader{
        Tags{ "Queue" = "Transparent" "RenderType"="Transparent" "RenderPipeline" = "UniversalPipeline"}

        HLSLPROGRAM

        #pragma multi_compile_fog
        #pragma multi_compile_instancing
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        // Declaring the type of shader this will be to Unity.
        // Declaring the Vertex shader in this format vertex:VertexShaderName
        #pragma surface surf Standard fullforwardshadows vertex:vert addshadow alpha:fade

        static const float PI = 3.14159265f;

        sampler2D _MainTex;
        fixed4 _Color;

        float _Amplitude;
        float _Frequency;
        float _SecondaryFrequency;
        float _AnimationSpeed;
        float _Smoothness;
        half _Metallic;

        half3 _Emission;
        float3 _FresnelColor;
        float _FresnelExponent;

        struct Input
        {
            // uv_MainTex is specifically named such that it will already have the tiling and offset of the MainText Texture.
            float2 uv_MainTex;
    
            float3 worldNormal;
            float3 viewDir;
            INTERNAL_DATA
};

        // The Vertex Shader Function in Standard Surface Shaders is defined to: Accept 1 arguement of type; appdata_full. We then call it "data"
        void vert(inout appdata_full data)
        {
            // Note that if we manipulate the scale, we need to tell Unity to recalculate an extra shadow pass, addshadow (in pragma line).
            
            float4 modifiedPos = data.vertex;
    modifiedPos.y += (2 * _Amplitude * cos(((1 / _Frequency) - (1 / _SecondaryFrequency)) / 2 * data.vertex.x - ((2 * PI * _Frequency) - (2 * PI * (_SecondaryFrequency))) / 2 * _Time.y * _AnimationSpeed)) * sin(((1 / _Frequency) + (_SecondaryFrequency)) / 2 * data.vertex.x - ((2 * PI * _Frequency) + (2 * PI * (_SecondaryFrequency))) / 2 * _Time.y * _AnimationSpeed);

    
            float3 posPlusTangent = data.vertex + data.tangent * 0.01;
    posPlusTangent.y += (2 * _Amplitude * cos(((1 / _Frequency) - (1 / _SecondaryFrequency)) / 2 * posPlusTangent.x - ((2 * PI * _Frequency) - (2 * PI * (_SecondaryFrequency))) / 2 * _Time.y * _AnimationSpeed)) * sin(((1 / _Frequency) + (_SecondaryFrequency)) / 2 * posPlusTangent.x - ((2 * PI * _Frequency) + (2 * PI * (_SecondaryFrequency))) / 2 * _Time.y * _AnimationSpeed);

            float3 bitangent = cross(data.normal, data.tangent);
            float3 posPlusBitangent = data.vertex + bitangent * 0.01;
    posPlusBitangent.y += (2 * _Amplitude * cos(((1 / _Frequency) - (1 / _SecondaryFrequency)) / 2 * posPlusBitangent.x - ((2 * PI * _Frequency) - (2 * PI * (_SecondaryFrequency))) / 2 * _Time.y * _AnimationSpeed)) * sin(((1 / _Frequency) + (_SecondaryFrequency)) / 2 * posPlusBitangent.x - ((2 * PI * _Frequency) + (2 * PI * (_SecondaryFrequency))) / 2 * _Time.y * _AnimationSpeed);

            float3 modifiedTangent = posPlusTangent - modifiedPos;
            float3 modifiedBitangent = posPlusBitangent - modifiedPos;

            float3 modifiedNormal = cross(modifiedTangent, modifiedBitangent);
            data.normal = normalize(modifiedNormal);
            data.vertex = modifiedPos;
    
        }
// (2 * _Amplitude * cos( ((1/_Frequency)-(1/_Frequency*2))/2 * posPlusBitangent.x - ((2*PI*_Frequency) - (2*PI*(_Frequency/2)))/2 * _Time )) * sin(((1/_Frequency)+(1/_Frequency*2))/2 * posPlusBitangent.x - ((2*PI*_Frequency) + (2*PI*(_Frequency/2)))/2 * _Time );

        void surf(Input i, inout SurfaceOutputStandard o)
        {
            // Samples the 2D Texture of _MainTex at the given tiling and offset (From Input Struct)
            fixed4 col = tex2D(_MainTex, i.uv_MainTex);
            
            // 'Tint' the Sampled Color.
            col *= _Color;
    
            //get the dot product between the normal and the view direction
            float fresnel = dot(i.worldNormal, i.viewDir);
            //invert the fresnel so the big values are on the outside
            fresnel = saturate(1 - fresnel);
            //raise the fresnel value to the exponents power to be able to adjust it
            fresnel = pow(fresnel, _FresnelExponent);
            //combine the fresnel value with a color
            float3 fresnelColor = fresnel * _FresnelColor;
            //apply the fresnel value to the emission
            o.Emission = _Emission + fresnelColor;
    
            // Assign the Albedo to 'col'.
            o.Albedo = col.rgba;
            o.Alpha = col.a;
            o.Smoothness = _Smoothness;
            o.Metallic = _Metallic;
    
}

		ENDCG

    }
}
