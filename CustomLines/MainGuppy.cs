using Guppy.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TranslucentInstancedPrimitives.Graphics.Effects;

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

        private LineEffect _effect;

        private VertexBuffer _triangleBuffer;
        private VertexPositionColor[] _triangleVertices;

        private VertexBuffer _lineBuffer;
        private VertexPositionColor[] _lineVertices;


        public MainGuppy(GraphicsDevice graphics, SpriteBatch spriteBatch, ContentManager content)
        {
            _font = content.Load<SpriteFont>("Font");
            _spriteBatch = spriteBatch;
            _graphics = graphics;
            _effect = new LineEffect(graphics);

            Vector3[] lines = new Vector3[]
            {
                new Vector3(100, 50, 0),
                new Vector3(150, 100, 0),
                new Vector3(200, 75, 0),
                new Vector3(250, 150, 0),
                new Vector3(150, 200, 0),
            };

            _lineVertices = lines.Select(x => new VertexPositionColor() { Position = new Vector3(x.X, x.Y, 0), Color = Color.Red }).ToArray();
            _triangleVertices = ToTriangleList(lines).Select(x => new VertexPositionColor() { Position = new Vector3(x.X, x.Y, 0), Color = Color.Green }).ToArray();

            _lineBuffer = new VertexBuffer(graphics, typeof(VertexPositionColor), _lineVertices.Length, BufferUsage.WriteOnly);
            _lineBuffer.SetData(_lineVertices);

            _triangleBuffer = new VertexBuffer(graphics, typeof(VertexPositionColor), _triangleVertices.Length, BufferUsage.WriteOnly);
            _triangleBuffer.SetData(_triangleVertices);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            var world = Matrix.CreateTranslation(0, 0, 0);
            var view = Matrix.CreateScale(2, 2, 1);
            var projection = Matrix.CreateOrthographicOffCenter(
                0, _graphics.Viewport.Width,
                _graphics.Viewport.Height, 0,
                -100, 100);

            _effect.WorldViewProjection = world * view * projection;


            _graphics.Clear(Color.Black);

            _effect.CurrentTechnique.Passes[0].Apply();

            _graphics.SetVertexBuffer(_triangleBuffer);
            _graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, _triangleVertices.Length / 3);

            _graphics.BlendState = BlendState.Additive;
            _graphics.SetVertexBuffer(_lineBuffer);
            // _graphics.DrawPrimitives(PrimitiveType.LineStrip, 0, _lineVertices.Length - 1);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Vector3[] lines = new Vector3[]
            {
                new Vector3(190, 50, 0),
                new Vector3(150, 100, 0),
                new Vector3(200, 75, 0),
                new Vector3(250, 150, 0),
                new Vector3(150, 200, 0),
            };

            _lineVertices = lines.Select(x => new VertexPositionColor() { Position = new Vector3(x.X, x.Y, 0), Color = Color.Red }).ToArray();
            _triangleVertices = ToTriangleList(lines).Select(x => new VertexPositionColor() { Position = new Vector3(x.X, x.Y, 0), Color = new Color(Color.Green, 0.75f) }).ToArray();

            _lineBuffer.SetData(_lineVertices);
            _triangleBuffer.SetData(_triangleVertices);
        }

        private static Vector3[] ToTriangleList(Vector3[] lines)
        {
            List<Vector3> vertices = new List<Vector3>();

            for (int i = 0; i < lines.Length - 1; i++)
            {
                Vector3 p1 = lines[i];
                Vector3 p2 = lines[i + 1];

                Vector3 delta = p2 - p1;
                Vector3 noramal = Normalize(new Vector3(-delta.Y, delta.X, 0));

                vertices.Add(p1 + noramal);
                vertices.Add(p1 - noramal);
                vertices.Add(p2 - noramal);

                vertices.Add(p2 - noramal);
                vertices.Add(p2 + noramal);
                vertices.Add(p1 + noramal);

                if (i > 0)
                {
                    Vector3 p0 = lines[i - 1];

                    Vector3 delta0 = p0 - p1;
                    Vector3 noramal0 = Normalize(new Vector3(-delta0.Y, delta0.X, 0));
                }

                // vertices.Add(p1 + Normalize(-noramal));
                // vertices.Add(p2 + Normalize(-noramal));
                // vertices.Add(p2 + Normalize(noramal));
                // vertices.Add(p1);

                // ertices.Add(antiNormal1);
                // ertices.Add(normal2);
            }

            return vertices.ToArray();
        }

        private static Vector3 Normalize(Vector3 input)
        {
            return (input / input.Length()) * 10;
        }
    }
}
