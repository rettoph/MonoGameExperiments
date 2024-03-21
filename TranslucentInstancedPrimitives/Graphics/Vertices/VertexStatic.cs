using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TranslucentInstancedPrimitives.Graphics.Vertices
{
    public struct VertexStatic : IVertexType
    {
        private static readonly VertexDeclaration _vertexDeclaration = new VertexDeclaration([
            new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0)
        ]);

        public VertexDeclaration VertexDeclaration => _vertexDeclaration;

        public Vector2 Position { get; set; }
    }
}
