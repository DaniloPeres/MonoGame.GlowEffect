#if SM4
    #define PS_PROFILE ps_4_0
    #define VS_PROFILE vs_4_0
#else
    #define PS_PROFILE ps_3_0
    #define VS_PROFILE vs_3_0
#endif

const int glowWidth = 50;
const float intensity = 10.0; // Reduced intensity for smoother effect
const float spread = 10; // Adjusted for better spread control
const float totalGlowMultiplier = 5.0; // Reduced to enhance transparency
const float4 glowColor = float4(1.0f, 1.0f, 1.0f, 1.0f); // Base glow color
const float alpha = 8; // Adjusted alpha for better transparency
const float2 textureSize : VPOS;

Texture2D SpriteTexture;
sampler2D InputSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float2 pos = input.TextureCoordinates;
    float2 uvPix = float2(1.0, 1.0) / textureSize;

    float4 originalColor = tex2D(InputSampler, pos);

    // If the pixel is not transparent, no need to apply glow
    if (originalColor.a == 1.0)
    {
        return originalColor;
    }

    float glowFactor = 0;
    float totalGlow = 0;
    
    int samples = 50; // Number of samples around the circle for each radius
    float angleStep = 2.0 * 3.14159265359 / samples; // Step size for each angle in radians

    
    [unroll(50)]
    for (int i = 1; i <= glowWidth; i++)
    {
        float weight = exp(-pow(i * (spread / 100), 2)); // Adjusted weight for smoother spread
        
        for (int j = 0; j < samples; j++)
        {           
            float angle = j * angleStep;
            float2 offset = float2(cos(angle) * i, sin(angle) * i) * uvPix; // Calculate circular offset

            float distance = length(offset);
            float distanceWeight = 1.0 / (distance + 1.0);

            float4 sampleColor = tex2D(InputSampler, pos + offset);
            glowFactor += (sampleColor.a * weight * distanceWeight);
        }

        totalGlow += weight * samples * (totalGlowMultiplier / 100); // Account for multiple samples per radius
    }

    glowFactor = saturate(glowFactor / totalGlow); // Normalize the glow factor

    // Enhance the glow effect by scaling the color
    float4 glowEffect = glowColor * (glowFactor * (intensity / 10)); // Increase scaling factor for more intensity

    glowEffect.a = glowFactor;

    // Blend the glow with the original color
    return max(originalColor, glowEffect);
}

technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile PS_PROFILE MainPS();
    }
};
