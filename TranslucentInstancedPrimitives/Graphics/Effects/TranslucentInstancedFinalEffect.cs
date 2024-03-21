using Microsoft.Xna.Framework.Graphics;

namespace TranslucentInstancedPrimitives.Graphics.Effects
{
    public class TranslucentInstancedFinalEffect : BaseEffect
    {
        public Texture2D AccumTexture
        {
            set => this.Parameters[nameof(AccumTexture)].SetValue(value);
        }

        public TranslucentInstancedFinalEffect(GraphicsDevice graphicsDevice) : base(graphicsDevice, @"Content\Shaders\Compiled\TranslucentInstancedFinal.mgfx")
        {
        }
    }
}
