# MonoGame.GlowEffect
<b>MonoGame.GlowEffect</b> is a library to generate glow for Texture2D in MonoGame. We also support Sprite Font.
We use a shader effect to generate the Glow effect.

# How to use

## Initialize and Load the GlowEffect

```csharp
protected override void LoadContent()
{
    GlowEffect.InitializeAndLoad(Content, GraphicsDevice);

    // ...
}
```

## Create a new Texture with glow

```csharp
Color color = Color.Yellow;
int glowWidth = 20;
float intensity = 30;
float spread = 0;
float totalGlowMultiplier = 100;
var textureWithGlow = GlowEffect.CreateGlow(myTexture, color, glowWidth, intensity, spread, totalGlowMultiplier);
```

## 

```csharp
Color textColor = Color.Black;
Color glowColor = Color.Yellow;
int glowWidth = 20;
float intensity = 30;
float spread = 0;
float totalGlowMultiplier = 100;
var textGlow = GlowEffect.CreateGlowSpriteFont(arialSpriteFont, "My Text", textColor, glowColor, glowWidth, intensity, spread, totalGlowMultiplier);
```

## License

MIT
