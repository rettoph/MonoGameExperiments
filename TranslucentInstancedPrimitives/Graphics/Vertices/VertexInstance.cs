using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TranslucentInstancedPrimitives.Graphics.Vertices
{
    public struct VertexInstance : IVertexType
    {
        private static readonly VertexDeclaration _vertexDeclaration = new VertexDeclaration([
            new VertexElement(00, VertexElementFormat.Vector3, VertexElementUsage.BlendWeight, 0),
        ]);

        public VertexDeclaration VertexDeclaration => _vertexDeclaration;

        public Vector3 Position { get; set; }
    }
}
