namespace Ds.Graphs.Repository
{
    using System.Collections.Generic;

    public class UnweightedGraph
    {
        public HashSet<int>[] AdjacencyArray { get; set; }

        public int VertexCount { get; set; }

        public UnweightedGraph(int vertexCount)
        {
            this.VertexCount = vertexCount;
            this.AdjacencyArray = new HashSet<int>[this.VertexCount];
            for (var i = 0; i < this.VertexCount; i++)
            {
                this.AdjacencyArray[i] = new HashSet<int>();
            }
        }

        public void AddUndirectedEdge(int startVertex, int endVertex)
        {
            this.AddDirectedEdge(startVertex, endVertex);
            this.AddDirectedEdge(endVertex, startVertex);
        }

        public void AddDirectedEdge(int startVertex, int endVertex)
        {
            this.AdjacencyArray[startVertex].Add(endVertex);
        }
    }
}
