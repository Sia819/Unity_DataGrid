Shader "Custom/BorderShader"
{
    Properties
    {
        _BorderColor("Border Color", Color) = (1,1,1,1)
        _BorderWidth("Border Width", Float) = 0.1
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _BorderColor;
            float _BorderWidth;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float border = _BorderWidth;
                float alpha = step(border, i.uv.x) * step(border, 1-i.uv.x) * step(border, i.uv.y) * step(border, 1-i.uv.y);
                return fixed4(_BorderColor.rgb, 1-alpha);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
