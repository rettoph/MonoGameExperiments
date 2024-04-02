using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PixelShader
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Effect _effect;
        private Texture2D _image;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _image = this.Content.Load<Texture2D>("Image");
            _effect = this.Content.Load<Effect>("File");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);

            _effect.Parameters["MatrixTransform"]?.SetValue(Matrix.CreateOrthographicOffCenter(0, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height, 0, -100, 100));

            _spriteBatch.Begin(effect: _effect, transformMatrix: Matrix.CreateTranslation(100, 100, 0));
            _spriteBatch.Draw(_image, Vector2.Zero, Color.White);
            _spriteBatch.End();
        }
    }
}
