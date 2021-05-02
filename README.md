<p align="center">
  <img src="http://daniloperes.com/MonoGame.GlowEffect_Logo_256.png" alt="MonoGame.GlowEffect" width="120" height="120">
</p>

# MonoGame.GlowEffect
<b>MonoGame.GlowEffect</b> is a library to generate glow for Texture2D in MonoGame. We also support Sprite Font.
We use a shader effect to generate the Glow effect.

## Nuget package
There is a nuget package avaliable here https://www.nuget.org/packages/MonoGame.GlowEffect/.

# Examples

<img src="http://daniloperes.com/MonoGame.GlowEffect.Samples.gif?1" alt="MonoGame.GlowEffect" width="640" height="520 ">

# How to use

## Create a new Texture with glow

```csharp
Color color = Color.Yellow;
float blurX = 30f;
float blurY =  30f;
float dist = 0f;
float angle = 0f;
float alpha = 1f;
float strength = 15f;
float inner = 0f;
float knockout = 0.5f;
float circleSamplingTimes = 12f;
float linearSamplingTimes = 7f;
var textureWithGlow = GlowEffect.CreateGlow(myTexture, color, blurX, blurY, dist, angle, alpha, strength, inner, knockout, circleSamplingTimes, linearSamplingTimes, GraphicsDevice);
```

## 

```csharp
Color textColor = Color.Black;
Color glowColor = Color.Yellow;
float blurX = 30f;
float blurY =  30f;
float dist = 0f;
float angle = 0f;
float alpha = 1f;
float strength = 15f;
float inner = 0f;
float knockout = 0.5f;
float circleSamplingTimes = 12f;
float linearSamplingTimes = 7f;
var textGlow = GlowEffect.CreateGlowSpriteFont(arialSpriteFont, "My Text", textColor, glowColor, blurX, blurY, dist, angle, alpha, strength, inner, knockout, circleSamplingTimes, linearSamplingTimes, GraphicsDevice);
```

## License

MIT
