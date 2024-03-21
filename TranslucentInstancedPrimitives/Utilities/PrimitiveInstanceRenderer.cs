using Microsoft.Xna.Framework.Graphics;
using TranslucentInstancedPrimitives.Graphics.Vertices;

namespace TranslucentInstancedPrimitives.Utilities
{
    public class PrimitiveInstanceRenderer
    {
        private readonly GraphicsDevice _graphics;
        private readonly VertexBuffer _staticVertices;
        private readonly IndexBuffer _indices;

        private VertexBufferManager<VertexInstance> _instanceVertices;
        private VertexBufferBinding[] _bindings;

        public IndexBuffer Indices => _indices;

        public int StaticTriangleCount { get; }
        public int InstanceCount => _instanceVertices.Count;
        public int TriangleCount => this.StaticTriangleCount * this.InstanceCount;

        public PrimitiveInstanceRenderer(GraphicsDevice graphics, List<VertexStatic> vertices)
        {
            _graphics = graphics;
            _bindings = Array.Empty<VertexBufferBinding>();

            _instanceVertices = new VertexBufferManager<VertexInstance>(graphics, 100);

            PrimitiveInstanceRenderer.BuildBuffers(graphics, vertices, out _staticVertices, out _indices);
            this.StaticTriangleCount = _indices.IndexCount / 3;

            this.SetBindings();
        }

        public void Dispose()
        {
            _indices.Dispose();
            _staticVertices.Dispose();
            _instanceVertices.VertexBuffer.Dispose();
        }

        private void SetBindings()
        {
            _bindings = [
                new VertexBufferBinding(_staticVertices, 0, 0),
                new VertexBufferBinding(_instanceVertices.VertexBuffer, 0, 1)
            ];
        }

        public void Draw(List<VertexInstance> instances, Effect effect)
        {
            if (instances.Count == 0)
            {
                return;
            }

            if (_instanceVertices.Resize(instances.Count) || true)
            {
                this.SetBindings();
            }


            instances.CopyTo(_instanceVertices.Data);
            _instanceVertices.SetData(instances.Count);

            _graphics.SetVertexBuffers(_bindings);
            _graphics.Indices = _indices;

            // effect.CurrentTechnique.Passes[0].Apply();
            _graphics.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, this.TriangleCount, this.InstanceCount);
        }

        private static readonly short[] _indexBuffer = new short[3];
        private static void BuildBuffers(GraphicsDevice graphics, List<VertexStatic> vertices, out VertexBuffer vertexBuffer, out IndexBuffer indexBuffer)
        {
            List<short> indices = new List<short>();

            _indexBuffer[0] = 0;
            _indexBuffer[1] = 1;

            for (short vertex_i = 2; vertex_i < vertices.Count; vertex_i++)
            {
                _indexBuffer[2] = vertex_i;

                indices.AddRange(_indexBuffer[..3]);
                _indexBuffer[1] = _indexBuffer[2];
            }

            vertexBuffer = new VertexBuffer(graphics, typeof(VertexStatic), vertices.Count, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices.ToArray());

            indexBuffer = new IndexBuffer(graphics, IndexElementSize.SixteenBits, indices.Count, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices.ToArray());
        }
    }
}
