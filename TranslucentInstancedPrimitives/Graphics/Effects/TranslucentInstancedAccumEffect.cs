using Microsoft.Xna.Framework.Graphics;

namespace TranslucentInstancedPrimitives.Graphics.Effects
{
    public class TranslucentInstancedAccumEffect : BaseEffect
    {
        public TranslucentInstancedAccumEffect(GraphicsDevice graphicsDevice) : base(graphicsDevice, @"Content\Shaders\Compiled\TranslucentInstancedAccum.mgfx")
        {
        }
    }
}
