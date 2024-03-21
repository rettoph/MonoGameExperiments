using Microsoft.Xna.Framework;
using TranslucentInstancedPrimitives.Graphics.Vertices;

namespace TranslucentInstancedPrimitives.Entities
{
    public class Entity
    {
        public Vector3 Velocity;
        public Vector3 Position;
        public Color Color;

        public VertexInstance Vertex => new VertexInstance()
        {
            Position = this.Position,
            Color = this.Color
        };
    }
}
