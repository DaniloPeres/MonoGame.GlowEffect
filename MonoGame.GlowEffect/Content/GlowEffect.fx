#if SM4
	#define PS_PROFILE ps_4_0
	#define VS_PROFILE vs_4_0
#else
	#define PS_PROFILE ps_3_0
	#define VS_PROFILE vs_3_0
#endif

float2 pixelSize;

const float dist = 0.0;
const float angle = 0.0;
const float4 color = float4(0.7, 0.0, 0.4, 0.0);
const float alpha = 1.0;
const float blurX = 30.0;
const float blurY = 30.0;
const float strength = 15.0;
const float inner = 0.0;
const float knockout = 0.5;
const float hideObject = 0.0;

const float linearSamplingTimes = 7.0;
const float circleSamplingTimes = 12.0;
const float PI = 3.14159265358979323846264;

sampler s0;

float calc_alpha(float4 color){
    if (length(color)>0.7){ return 1.0;}
	return 0.0;
}

float random(float2 fragCoord, float2 scale)
{
    return frac(sin(dot(fragCoord.xy, scale)) * 43758.5453);
}

float4 processBloomPS( float4 inPosition : SV_Position,
			    float4 inColor : COLOR0,
			    float2 coords : TEXCOORD0 ) : COLOR0
{
    float2 px = pixelSize;
    float4 ownColor = tex2D(s0, coords);
    
    if (calc_alpha(ownColor)==0.0f) {
        ownColor = float4(0,0,0,0);
    }

    float4 curColor;
    float totalAlpha = 0.0f;
    float maxTotalAlpha = 0.0f;
    float curDistanceX = 0.0f;
    float curDistanceY = 0.0f;
    float offsetX = dist*px.x * cos(angle);
    float offsetY = dist*px.y* sin(angle);

    float cosAngle;
    float sinAngle;
    float offset = PI * 2.0 / circleSamplingTimes * random(coords * pixelSize, float2(12.9898, 78.233));

    float stepX = blurX * px.x / linearSamplingTimes;
    float stepY = blurY * px.y / linearSamplingTimes;

    if (ownColor.a>0.5){
        return ownColor;
    }

    // Cyclic angle sampling
#if OPENGL
    [unroll(7)]
#else
    [loop]
#endif
    for (float a = 0.0; a <= PI * 2.0; a += PI * 2.0 / circleSamplingTimes)
    {
        cosAngle = cos(a + offset);
        sinAngle = sin(a + offset);
        // Linear sampling
#if OPENGL
        [unroll(7)]
#else
        [loop]
#endif
        for (float i = 1.0; i <= linearSamplingTimes; i++) {
            curDistanceX = i * stepX * cosAngle;
            curDistanceY = i * stepY * sinAngle;
            float2 uv = float2(coords.x + curDistanceX - offsetX, coords.y + curDistanceY + offsetY);
            if (uv.x >= 0.0 && uv.x <= 1.0 && uv.y >= 0.0 && uv.y <= 1.0){
                curColor = tex2D(s0, uv);
                totalAlpha += (linearSamplingTimes - i) * calc_alpha(curColor);
            }
            maxTotalAlpha += (linearSamplingTimes - i);
        }
    }

    // Calculate alpha average
    totalAlpha = totalAlpha/maxTotalAlpha;
   
    if (totalAlpha<0.1){
        return ownColor;
    }

    return color*totalAlpha*totalAlpha * strength;
}

technique Technique1
{
    pass P1
    {
        PixelShader = compile PS_PROFILE processBloomPS();
    }
}