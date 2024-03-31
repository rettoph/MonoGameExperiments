using Guppy.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TranslucentInstancedPrimitives
{
    internal class MainGuppy : GameGuppy
    {
        private readonly GraphicsDevice _graphics;
        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteFont _font;

        private Matrix _world;
        private Matrix _view;
        private Matrix _projection;

        private BasicEffect _effect;

        private VertexBuffer _buffer;
        private VertexPositionColor[] _vertices;


        public MainGuppy(GraphicsDevice graphics, SpriteBatch spriteBatch, ContentManager content)
        {
            _font = content.Load<SpriteFont>("Font");
            _spriteBatch = spriteBatch;
            _graphics = graphics;
            _effect = new BasicEffect(graphics) { VertexColorEnabled = true };

            Vector3[] line = new Vector3[]
            {
                new Vector3(100, 100, 0),
                new Vector3(150, 100, 0),
                new Vector3(200, 75, 0),
                new Vector3(50, 150, 0),
            };

            _buffer = new VertexBuffer(graphics, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
            _vertices = new VertexPositionColor[3];
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _buffer.SetData(_vertices);
            _effect.World = Matrix.CreateTranslation(0, 0, 0);
            _effect.View = Matrix.CreateScale(100, 100, 1);
            _effect.Projection = Matrix.CreateOrthographicOffCenter(
                0, _graphics.Viewport.Width,
                _graphics.Viewport.Height, 0,
                -100, 100);

            _graphics.SetVertexBuffer(_buffer);
            _graphics.Clear(Color.Red);

            _effect.CurrentTechnique.Passes[0].Apply();
            _graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _vertices[0] = new VertexPositionColor() { Position = new Vector3(0, 0, 0), Color = Color.Green };
            _vertices[1] = new VertexPositionColor() { Position = new Vector3(1, 0, 0), Color = Color.Green };
            _vertices[2] = new VertexPositionColor() { Position = new Vector3(1, 1, 0), Color = Color.Green };
        }

        private static Vector3[] ToTriangleList(Vector3[] lines)
        {
            Vector3[] triangles = new Vector3[(lines.Length - 1) * 6];
        }
    }
}
