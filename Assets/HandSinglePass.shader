Shader "Custom/HandSinglePass"
{
    Properties
    {
        _MainTex       ("Main Texture",   2D)   = "white" {}
        _Color         ("Tint Color",     Color)= (1,1,1,1)
        _EdgeColor     ("Edge Color",     Color)= (0,0,0,1)
        _ThumbColor    ("Thumb Color",    Color)= (1,1,1,1)
        _FingerColor_1   ("Finger Color",   Color)= (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "FORWARD"
            Tags { "LightMode"="Always" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma multi_compile _ UNITY_STEREO_INSTANCING_ENABLED

            #include "UnityCG.cginc"
            #include "UnityInstancing.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv      : TEXCOORD0;
                float4 pos     : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4   _Color;
            float4   _EdgeColor;
            float4   _ThumbColor;
            float4   _FingerColor;

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv  = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

                // sample base texture and tint
                fixed4 texcol = tex2D(_MainTex, i.uv) * _Color;

                // NOTE: we don’t actually use _EdgeColor/_ThumbColor/_FingerColor
                // in this simple pass, but declaring them satisfies the toolkit.

                return texcol;
            }
            ENDCG
        }
    }
}

