using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TranslucentInstancedPrimitives.Graphics.Effects
{
    public abstract class BaseEffect : Effect
    {
        public Matrix WorldViewProjection
        {
            set => this.Parameters[nameof(WorldViewProjection)].SetValue(value);
        }

        protected BaseEffect(GraphicsDevice graphicsDevice, string mgfxPath) : base(graphicsDevice, File.ReadAllBytes(mgfxPath))
        {
        }
    }
}
