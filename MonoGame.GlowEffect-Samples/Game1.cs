using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGame.GlowEffect_Samples;

public class Game1 : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    public List<Drawables.IDrawable> drawList = [];

    SpriteFont
        arialSpriteFont,
        cooperBlackSpriteFont;

    Drawables.Image
        imgObjSurge,
        imgObjDragon,
        imgObjTroll,
        imgObjColorSelected,
        imgObjFpsText;

    Texture2D
        imgPixel,
        imgSurge,
        imgDragon,
        imgTroll;

    Color glowColor = Color.White;

    int lastFpsCount = 0;
    int fpsCount = 0;
    TimeSpan fpsTimeSpan = new();

    #region Shader variables
    int glowWidth = 20;
    float intensity = 30;
    float spread = 0;
    float totalGlowMultiplier = 100;
    bool hideTexture = false;
    #endregion

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        graphics.SynchronizeWithVerticalRetrace = false;
        IsFixedTimeStep = false;

        graphics.PreferredBackBufferHeight = 700;
        graphics.PreferredBackBufferWidth = 900;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        GlowEffect.InitializeAndLoad(Content, GraphicsDevice);

        spriteBatch = new SpriteBatch(GraphicsDevice);

        imgPixel = new Texture2D(GraphicsDevice, 1, 1);
        Color[] _Color = [Color.White];
        imgPixel.SetData(_Color);

        arialSpriteFont = Content.Load<SpriteFont>("Arial");
        cooperBlackSpriteFont = Content.Load<SpriteFont>("CooperBlack");

        imgSurge = Content.Load<Texture2D>("surge");
        imgDragon = Content.Load<Texture2D>("Dragon");
        imgTroll = Content.Load<Texture2D>("Troll");

        var lineSpace = 42;

        var pos = new Vector2(50, 20);

        drawList.Add(new Drawables.Text(arialSpriteFont, "Color:", pos, Color.White));

        var posLeftColor = pos.X + 100;
        pos = new Vector2(posLeftColor, pos.Y);
        CreateSquareColor(pos, Color.White, true);
        pos.X += 45;
        CreateSquareColor(pos, Color.Yellow);
        pos.X += 45;
        CreateSquareColor(pos, Color.LightGreen);
        pos.X += 45;
        CreateSquareColor(pos, Color.MediumVioletRed);
        pos.X += 45;
        CreateSquareColor(pos, Color.LightSkyBlue);
        pos.X += 45;
        CreateSquareColor(pos, Color.GreenYellow);
        pos.X += 45;
        CreateSquareColor(pos, Color.Pink);
        pos.X = posLeftColor;
        pos.Y += 45;
        CreateSquareColor(pos, Color.LightGray);
        pos.X += 45;
        CreateSquareColor(pos, Color.Gray);
        pos.X += 45;
        CreateSquareColor(pos, Color.DarkGray);
        pos.X += 45;
        CreateSquareColor(pos, Color.Olive);
        pos.X += 45;
        CreateSquareColor(pos, Color.Cyan);
        pos.X += 45;
        CreateSquareColor(pos, Color.Orange);
        pos.X += 45;
        CreateSquareColor(pos, Color.Magenta);

        pos = new Vector2(50, 150);
        CreateOptionNumber("Glow Width", pos, glowWidth, 1f, value => glowWidth = (int)value);

        pos.Y += lineSpace;
        CreateOptionNumber("Intensity", pos, intensity, 1f, value => intensity = value);

        pos.Y += lineSpace;
        CreateOptionNumber("Spread", pos, spread, 1f, value => spread = value);

        pos.Y += lineSpace;
        CreateOptionNumber("Total Glow Multiplier", pos, totalGlowMultiplier, 5f, value => totalGlowMultiplier = value);

        pos.Y += lineSpace;
        CreateOptionCheckbox("Hide texture", pos, hideTexture, value => hideTexture = value);

        pos = new Vector2(550, 15);

        CreateAndAddImageObject(new Drawables.Image(imgPixel, new Rectangle(520, 15, 2, 650)))
            .color = Color.Yellow;

        imgObjFpsText = CreateAndAddImageObject(new Drawables.Image(imgPixel, new Vector2(600, 0)));

        imgObjSurge = CreateAndAddImageObject(new Drawables.Image(imgPixel, new Vector2(650, 90)));

        imgObjDragon = CreateAndAddImageObject(new Drawables.Image(imgPixel, new Vector2(600, 210)));

        imgObjTroll = CreateAndAddImageObject(new Drawables.Image(imgPixel, new Vector2(570, 400)));

        UpdateTextureOutlines();
    }

    private void CreateOptionNumber(string text, Vector2 pos, float value, float multiplier, Action<float> onValueChange)
    {
        // Create title
        var imgObjText = new Drawables.Text(arialSpriteFont, text, pos, Color.White);
        drawList.Add(imgObjText);
        pos.X += arialSpriteFont.MeasureString(text).X + 15;

        Drawables.Text imgObjTextNumber = null;

        // Create Minus button
        var imgBtnMinus = Content.Load<Texture2D>("btnMinus");
        CreateAndAddImageObject(new Drawables.Image(imgBtnMinus, pos))
            .OnClick = () =>
            {
                value -= multiplier;
                onValueChange(value);
                imgObjTextNumber.SetText(value.ToString("0.#"));
                UpdateTextureOutlines();
            };
        pos.X += 45;

        // Create the number object
        imgObjTextNumber = new Drawables.Text(arialSpriteFont, value.ToString(), pos, Color.White);
        drawList.Add(imgObjTextNumber);
        pos.X += 60;

        // Create the plus number
        var imgBtnPlus = Content.Load<Texture2D>("btnPlus");
        CreateAndAddImageObject(new Drawables.Image(imgBtnPlus, pos))
            .OnClick = () =>
            {
                value += multiplier;
                onValueChange(value);
                imgObjTextNumber.SetText(value.ToString("0.#"));
                UpdateTextureOutlines();
            };
    }

    private void CreateOptionCheckbox(string text, Vector2 pos, bool value, Action<bool> onValueChange)
    {
        // Create title
        var imgObjText = new Drawables.Text(arialSpriteFont, text, pos, Color.White);
        drawList.Add(imgObjText);
        pos.X += arialSpriteFont.MeasureString(text).X + 15;

        var imgCheckbox = Content.Load<Texture2D>("CheckBox");
        var imgCheckboxChecked = Content.Load<Texture2D>("CheckBox_Checked");

        // Create Checkbox button
        var checkbox = CreateAndAddImageObject(new Drawables.Image(value ? imgCheckboxChecked : imgCheckbox, pos));
        checkbox.OnClick = () =>
            {
                value = !value;
                onValueChange(value);
                checkbox.texture = value ? imgCheckboxChecked : imgCheckbox;
                UpdateTextureOutlines();
            };
        pos.X += 45;
    }

    private void UpdateTextureOutlines()
    {
        CreateGlow(imgObjSurge, imgSurge);
        CreateGlow(imgObjDragon, imgDragon);
        CreateGlow(imgObjTroll, imgTroll);

        UpdateFpsTexture(true);
    }

    private void CreateGlow(Drawables.Image image, Texture2D texture)
    {
        var imageGlow = GlowEffect.CreateGlow(texture, glowColor, glowWidth, intensity, spread, totalGlowMultiplier, hideTexture);

        // Dispose if the previous texture is not pixel
        if (image.texture != imgPixel)
            image.texture.Dispose();

        image.texture = imageGlow;
    }

    private string lastTextFpsCount = "";
    private void UpdateFpsTexture(bool forceUpdate)
    {
        var text = $"FPS: {lastFpsCount}";
        if (!forceUpdate && text == lastTextFpsCount)
            return;
        lastTextFpsCount = text;

        imgObjFpsText.texture = GlowEffect.CreateGlowSpriteFont(cooperBlackSpriteFont, text, Color.DarkRed, new Vector2(0.85f), glowColor, glowWidth, intensity, spread, totalGlowMultiplier, hideTexture);
    }

    private DateTime? lastClick = null;
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        fpsCount++;
        fpsTimeSpan = fpsTimeSpan.Add(gameTime.ElapsedGameTime);
        if (fpsTimeSpan >= new TimeSpan(0, 0, 1))
        {
            lastFpsCount = fpsCount;
            fpsTimeSpan = new TimeSpan(0, 0, 0, 0, fpsTimeSpan.Milliseconds);
            fpsCount = 0;
            UpdateFpsTexture(false);
        }

        var mouse = Mouse.GetState();

        if (mouse.LeftButton == ButtonState.Pressed)
        {
            if (lastClick == null || lastClick.Value.AddMilliseconds(150) < DateTime.Now)
            {
                lastClick = DateTime.Now;
                var pos = new Vector2(mouse.X, mouse.Y);

                var imgObj = drawList.FirstOrDefault(item => item.Collision(pos));
                imgObj?.OnClick?.Invoke();
            }
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        spriteBatch.Begin();
        drawList.ForEach(drawItem => drawItem.Draw(spriteBatch));
        spriteBatch.End();
    }

    private void CreateSquareColor(Vector2 pos, Color color, bool isSelected = false)
    {
        var squareSize = 32;

        var rec = new Rectangle(pos.ToPoint(), new Point(squareSize));

        if (isSelected)
            SelectedColor(rec);

        var imgObj = CreateAndAddImageObject(new Drawables.Image(imgPixel, rec));
        imgObj.color = color;
        imgObj.OnClick = () =>
        {
            glowColor = imgObj.color;
            SelectedColor(rec);
            UpdateTextureOutlines();
        };
    }

    private void SelectedColor(Rectangle rec)
    {
        var squareBorder = 4;

        if (imgObjColorSelected == null)
        {
            imgObjColorSelected = CreateAndAddImageObject(new Drawables.Image(imgPixel, new Rectangle(Point.Zero, new Point(rec.Width + (squareBorder * 2)))));
            imgObjColorSelected.color = Color.GreenYellow;
        }

        imgObjColorSelected.destinationRectangle = new Rectangle(rec.Location - new Point(squareBorder), imgObjColorSelected.destinationRectangle.Size);
    }

    private Drawables.Image CreateAndAddImageObject(Drawables.Image imgObj)
    {
        drawList.Add(imgObj);
        return imgObj;
    }
}
