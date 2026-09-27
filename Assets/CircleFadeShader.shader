Shader "UI/CircleFade"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Fade Color", Color) = (0,0,0,1)
        _Radius ("Circle Radius", Range(0.0, 1.5)) = 1.5
        _Softness ("Edge Softness", Range(0.0, 0.5)) = 0.01
    }

    SubShader
    {
        Tags { "Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
        Cull Off Lighting Off ZWrite Off Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                float2 texcoord  : TEXCOORD0;
            };

            fixed4 _Color;
            float _Radius;
            float _Softness;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Shift UV center from (0,0) to the center of the screen (0.5, 0.5)
                float2 centerUV = i.texcoord - float2(0.5, 0.5);
                
                // Account for screen aspect ratio so the circle stays round
                centerUV.x *= (_ScreenParams.x / _ScreenParams.y);

                // Calculate distance from center
                float dist = length(centerUV);

                // Smoothstep controls where the alpha cuts off
                // If dist > radius, it becomes fully opaque fade color.
                float alpha = smoothstep(_Radius - _Softness, _Radius, dist);

                // Multiply the target color's transparency by our calculation
                fixed4 col = _Color;
                col.a *= alpha;

                return col;
            }
            ENDCG
        }
    }
}