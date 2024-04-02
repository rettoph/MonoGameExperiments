using Microsoft.Xna.Framework.Graphics;

namespace TranslucentInstancedPrimitives.Graphics.Effects
{
    public class LineEffect : BaseEffect
    {
        public LineEffect(GraphicsDevice graphicsDevice) : base(graphicsDevice, @"Content\Shaders\Compiled\Line.mgfx")
        {
        }
    }
}
