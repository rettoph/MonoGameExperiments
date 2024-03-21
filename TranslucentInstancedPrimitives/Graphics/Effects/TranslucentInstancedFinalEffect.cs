using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TranslucentInstancedPrimitives.Graphics.Effects
{
    public class TranslucentInstancedFinalEffect : BaseEffect
    {
        public Texture2D AccumTexture
        {
            set => this.Parameters[nameof(AccumTexture)].SetValue(value);
        }

        public Vector4 ViewportBounds
        {
            set => this.Parameters[nameof(ViewportBounds)].SetValue(value);
        }

        public bool RenderAccum
        {
            set => this.Parameters[nameof(RenderAccum)].SetValue(value);
        }

        public TranslucentInstancedFinalEffect(GraphicsDevice graphicsDevice) : base(graphicsDevice, @"Content\Shaders\Compiled\TranslucentInstancedFinal.mgfx")
        {
        }
    }
}
