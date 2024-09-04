<p align="center">
  <img src="https://raw.githubusercontent.com/DaniloPeres/MonoGame.GlowEffect/main/Logo.png" alt="MonoGame.GlowEffect" width="120" height="120">
</p>

# MonoGame.GlowEffect
<b>MonoGame.GlowEffect</b> is a library to generate glow for Texture2D in MonoGame. We also support Sprite Font.
We use a shader effect to generate the Glow effect.

## Nuget package
There is a nuget package avaliable here https://www.nuget.org/packages/MonoGame.GlowEffect/.

# Examples

<img src="https://raw.githubusercontent.com/DaniloPeres/MonoGame.GlowEffect/main/MonoGame.GlowEffect-Samples/Sample.gif" alt="MonoGame.GlowEffect" width="640" height="520 ">

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
bool hideTexture = false;
var textureWithGlow = GlowEffect.CreateGlow(myTexture, color, glowWidth, intensity, spread, totalGlowMultiplier, hideTexture);
```

## 

```csharp
Color textColor = Color.Black;
Color glowColor = Color.Yellow;
int glowWidth = 20;
float intensity = 30;
float spread = 0;
float totalGlowMultiplier = 100;
bool hideTexture = false;
var textGlow = GlowEffect.CreateGlowSpriteFont(arialSpriteFont, "My Text", textColor, glowColor, glowWidth, intensity, spread, totalGlowMultiplier, hideTexture);
```

## License

MIT
