using Guppy.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TranslucentInstancedPrimitives.Entities;
using TranslucentInstancedPrimitives.Graphics.Effects;
using TranslucentInstancedPrimitives.Graphics.Vertices;
using TranslucentInstancedPrimitives.Utilities;

namespace TranslucentInstancedPrimitives
{
    internal class MainGuppy : GameGuppy
    {
        private readonly GraphicsDevice _graphics;
        private readonly TranslucentInstancedAccumEffect _effect_accum;
        private readonly TranslucentInstancedFinalEffect _effect_final;
        private readonly PrimitiveInstanceRenderer _squareRenderer;
        private readonly PrimitiveInstanceRenderer _triangleRenderer;
        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteFont _font;

        private Matrix _world;
        private Matrix _view;
        private Matrix _projection;

        private List<VertexInstance> _squareVertices;
        private List<Entity> _squares;

        private List<VertexInstance> _triangleVertices;
        private List<Entity> _triangles;

        private System.Drawing.RectangleF _bounds;

        private static int Entities = 50;
        private static float Scale = 100;
        private static float MaxVelocity = 10;

        private RenderTarget2D _target_accum;
        private RenderTarget2D _target_final;

        private BlendState _bs;

        private Vector4[] _data_accum;
        private Vector4[] _data_final;


        public MainGuppy(GraphicsDevice graphics, SpriteBatch spriteBatch, ContentManager content)
        {
            _font = content.Load<SpriteFont>("Font");
            _spriteBatch = spriteBatch;
            _graphics = graphics;
            _effect_final = new TranslucentInstancedFinalEffect(graphics);
            _effect_accum = new TranslucentInstancedAccumEffect(graphics);
            _squareRenderer = new PrimitiveInstanceRenderer(graphics, [
                new VertexStatic() { Position = new Vector2(0, 0) },
                new VertexStatic() { Position = new Vector2(0, 1) },
                new VertexStatic() { Position = new Vector2(1, 1) },
                new VertexStatic() { Position = new Vector2(1, 0) }
            ]);
            _triangleRenderer = new PrimitiveInstanceRenderer(graphics, [
                new VertexStatic() { Position = new Vector2(0, 0) },
                new VertexStatic() { Position = new Vector2(0, 1) },
                new VertexStatic() { Position = new Vector2(0.5f, 0.5f) },
            ]);

            _bounds = new System.Drawing.RectangleF(0, 0, _graphics.Viewport.Width / Scale, _graphics.Viewport.Height / Scale);

            _squareVertices = new List<VertexInstance>();
            _squares = new List<Entity>();
            for (int i = 0; i < Entities / 2; i++)
            {
                _squares.Add(new Entity()
                {
                    Position = new Vector3(Random.Shared.NextSingle() * _bounds.Width, Random.Shared.NextSingle() * _bounds.Height, Random.Shared.NextSingle() * 10),
                    Velocity = new Vector3((Random.Shared.NextSingle() * MaxVelocity) - (MaxVelocity / 2), (Random.Shared.NextSingle() * MaxVelocity) - (MaxVelocity / 2), 0),
                    Color = new Color(Random.Shared.NextSingle(), Random.Shared.NextSingle(), 0, 0.75f)
                });
            }

            _triangleVertices = new List<VertexInstance>();
            _triangles = new List<Entity>();
            for (int i = 0; i < Entities / 2; i++)
            {
                _triangles.Add(new Entity()
                {
                    Position = new Vector3(Random.Shared.NextSingle() * _bounds.Width, Random.Shared.NextSingle() * _bounds.Height, Random.Shared.NextSingle() * 10),
                    Velocity = new Vector3((Random.Shared.NextSingle() * MaxVelocity) - (MaxVelocity / 2), (Random.Shared.NextSingle() * MaxVelocity) - (MaxVelocity / 2), 0),
                    Color = new Color(Random.Shared.NextSingle(), 0, Random.Shared.NextSingle(), 0.75f)
                });
            }

            _target_final = new RenderTarget2D(_graphics, _graphics.Viewport.Width, _graphics.Viewport.Height, false, SurfaceFormat.Vector4, DepthFormat.None);
            _target_accum = new RenderTarget2D(_graphics, _graphics.Viewport.Width, _graphics.Viewport.Height, false, SurfaceFormat.Vector4, DepthFormat.None);

            _data_accum = new Vector4[_graphics.Viewport.Width * _graphics.Viewport.Height];
            _data_final = new Vector4[_graphics.Viewport.Width * _graphics.Viewport.Height];

            _bs = new BlendState()
            {
                ColorBlendFunction = BlendFunction.Add,
                AlphaSourceBlend = Blend.One,
                ColorSourceBlend = Blend.One,
                AlphaDestinationBlend = Blend.One,
                ColorDestinationBlend = Blend.One
            };
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _world = Matrix.CreateTranslation(0, 0, 0);
            _view = Matrix.CreateScale(Scale, Scale, 1);
            _projection = Matrix.CreateOrthographicOffCenter(
                0, _graphics.Viewport.Width,
                _graphics.Viewport.Height, 0,
                -100, 100);

            // Accum
            _graphics.SetRenderTarget(_target_accum);
            _graphics.Clear(Color.Transparent);
            _graphics.BlendState = _bs;
            _graphics.DepthStencilState = DepthStencilState.None;
            _graphics.RasterizerState = RasterizerState.CullNone;

            _effect_accum.WorldViewProjection = _world * _view * _projection;
            _effect_accum.CurrentTechnique.Passes[0].Apply();
            _triangleRenderer.Draw(_triangleVertices, _effect_accum);
            _squareRenderer.Draw(_squareVertices, _effect_accum);
            _target_accum.GetData(_data_accum);

            // Final
            _graphics.SetRenderTarget(_target_final);
            _graphics.Clear(Color.Transparent);
            _graphics.BlendState = BlendState.Opaque;
            _graphics.DepthStencilState = DepthStencilState.Default;
            _graphics.RasterizerState = RasterizerState.CullNone;

            _effect_final.WorldViewProjection = _world * _view * _projection;
            _effect_final.AccumTexture = _target_accum;
            _effect_final.RenderAccum = Keyboard.GetState().IsKeyDown(Keys.LeftControl);
            // _effect_final.ViewportBounds = new Vector4(0, 0, _graphics.Viewport.Width, _graphics.Viewport.Height);
            _effect_final.CurrentTechnique.Passes[0].Apply();
            _triangleRenderer.Draw(_triangleVertices, _effect_final);
            _squareRenderer.Draw(_squareVertices, _effect_final);
            _target_final.GetData(_data_final);

            // Render Target
            _graphics.SetRenderTarget(null);
            _graphics.BlendState = BlendState.AlphaBlend;
            _graphics.DepthStencilState = DepthStencilState.None;
            _graphics.RasterizerState = RasterizerState.CullNone;

            _spriteBatch.Begin();
            _spriteBatch.Draw(_target_final, Vector2.Zero, Color.White);

            Point mouse = Mouse.GetState().Position;
            mouse.X = mouse.X < 0 ? 0 : mouse.X >= _graphics.Viewport.Width ? _graphics.Viewport.Width - 1 : mouse.X;
            mouse.Y = mouse.Y < 0 ? 0 : mouse.Y >= _graphics.Viewport.Height ? _graphics.Viewport.Height - 1 : mouse.Y;

            int index = mouse.X + (mouse.Y * _graphics.Viewport.Width);



            string[] lines = [
                $"{mouse}",
                $"{index}",
                $"{_data_accum[index]}",
                $"{_data_final [index]}",
            ];

            for (int i = 0; i < lines.Length; i++)
            {
                _spriteBatch.DrawString(_font, lines[i], new Vector2(15, 15 + (20 * i)), Color.White);
            }


            _spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                foreach (Entity entity in _squares)
                {
                    this.Update(gameTime, entity);
                }

                foreach (Entity entity in _triangles)
                {
                    this.Update(gameTime, entity);
                }
            }


            _squareVertices.Clear();
            _squareVertices.AddRange(_squares.Select(x => x.Vertex));

            _triangleVertices.Clear();
            _triangleVertices.AddRange(_triangles.Select(x => x.Vertex));
        }

        private void Update(GameTime gameTime, Entity entity)
        {
            entity.Position += entity.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (entity.Position.X <= 0)
            {
                entity.Position.X = 0;
                entity.Velocity.X *= -1;
            }

            if (entity.Position.X >= _bounds.Width)
            {
                entity.Position.X = _bounds.Width;
                entity.Velocity.X *= -1;
            }

            if (entity.Position.Y <= 0)
            {
                entity.Position.Y = 0;
                entity.Velocity.Y *= -1;
            }

            if (entity.Position.Y >= _bounds.Height)
            {
                entity.Position.Y = _bounds.Height;
                entity.Velocity.Y *= -1;
            }
        }
    }
}
