using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using YLib;
using Y86SEQEmulator;

namespace SEQRenderer
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        PixelRaster EmuRenderer;
        Y86SEQEmulator.Processor YProcessor;

        public void RenderPixelInterrupt(Y86SEQEmulator.Processor processor)
        {
            EmuRenderer.SetPixel( (int)processor.R8, (int)processor.R9, Color.White);
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            YProcessor = new Processor();
            EmuRenderer = new PixelRaster(4,4,20,20);

            InterruptHandler.InterruptMapping.Add(3, RenderPixelInterrupt);
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            EmuRenderer.SetPixelScale(6, 6);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            EmuRenderer.LoadContent(Content.Load<Texture2D>("1Pixel"));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            YProcessor.Tick();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            EmuRenderer.DrawRaster(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
