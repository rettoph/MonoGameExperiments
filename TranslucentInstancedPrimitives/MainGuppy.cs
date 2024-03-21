using Guppy.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TranslucentInstancedPrimitives.Graphics.Effects;
using TranslucentInstancedPrimitives.Graphics.Vertices;
using TranslucentInstancedPrimitives.Utilities;

namespace TranslucentInstancedPrimitives
{
    internal class MainGuppy : GameGuppy
    {
        private readonly GraphicsDevice _graphics;
        private readonly TranslucentInstancedAccumEffect _effect_accum;
        private readonly PrimitiveInstanceRenderer _squares;

        private Matrix _world;
        private Matrix _view;
        private Matrix _projection;

        private List<VertexInstance> _squareInstances;


        public MainGuppy(GraphicsDevice graphics)
        {
            _graphics = graphics;
            _effect_accum = new TranslucentInstancedAccumEffect(graphics);
            _squares = new PrimitiveInstanceRenderer(graphics, [
                new VertexStatic() { Position = new Vector2(0, 0) },
                new VertexStatic() { Position = new Vector2(0, 1) },
                new VertexStatic() { Position = new Vector2(1, 1) },
                new VertexStatic() { Position = new Vector2(1, 0) }
            ]);

            _squareInstances = new List<VertexInstance>([
                new VertexInstance() { Position = new Vector3(0, 0, 0) },
                new VertexInstance() { Position = new Vector3(1, 1, 0) },
                new VertexInstance() { Position = new Vector3(2, 2, 0) }
            ]);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // _squareInstances[0] = new VertexInstance() { Position = new Vector3(-4, 0, 0) };

            _world = Matrix.CreateTranslation(0, 0, 0);
            _view = Matrix.CreateScale(100, 100, 0);
            _projection = Matrix.CreateOrthographicOffCenter(
                0, _graphics.Viewport.Width,
                _graphics.Viewport.Height, 0,
                0, 10);

            _effect_accum.WorldViewProjection = _world * _view * _projection;
            _squares.Draw(_squareInstances, _effect_accum);
        }
    }
}
