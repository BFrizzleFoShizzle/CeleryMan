Shader "Custom/CelShader"
{
	Properties
	{
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Cuts("Cuts", Int) = 2
		_Falloff("Falloff", Float) = 0.5
		_MinimumLight("Minimum light (lowest cut value)", Float) = 0.0
		_InterpolationRange ("Interpolation range (range interpolated at light cut)", Float) = 0.01
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf CelShader

		float _Falloff;
		float _MinimumLight;
		float _InterpolationRange;
		int _Cuts;

		half4 LightingCelShader(SurfaceOutput s, half3 lightDir, half atten) {
			half NdotL = dot(s.Normal, lightDir);

			if (NdotL <= 0.0)
				NdotL = 0;

			// there's a tradeoff here: we can't currently sum up all  the light contributions

			float lightContribution = min(NdotL * atten * 2, 1.0);

			lightContribution = pow(lightContribution, _Falloff);

			float cutLightContribution = lightContribution;

			cutLightContribution = round(cutLightContribution * _Cuts) / _Cuts;
			
			float interpolationVal = lightContribution - (cutLightContribution - (0.5f / _Cuts));
			if (interpolationVal > 0.0f && interpolationVal < _InterpolationRange)
				cutLightContribution -= (1.0 - (interpolationVal / _InterpolationRange)) / _Cuts;
				
			// TODO should this shift the whole range of cuts, or just set the lowest cut?
			// value must be BELOW 1.0/cuts to look right
			cutLightContribution = max(cutLightContribution, _MinimumLight);

			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * cutLightContribution;
			c.a = s.Alpha;
			return c;
		}

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
