namespace Ds.Graphs.Repository
{
    using System.Collections.Generic;

    public class WeightedGraph
    {
        public Dictionary<int, double>[] AdjacencyArray { get; set; }

        public int VertexCount { get; set; }

        public WeightedGraph(int vertexCount)
        {
            this.VertexCount = vertexCount;
            this.AdjacencyArray = new Dictionary<int, double>[this.VertexCount];
            for (var i = 0; i < this.VertexCount; i++)
            {
                this.AdjacencyArray[i] = new Dictionary<int, double>();
            }
        }

        public void AddUndirectedEdge(int startVertex, int endVertex, double edgeValue)
        {
            this.AddDirectedEdge(startVertex, endVertex, edgeValue);
            this.AddDirectedEdge(endVertex, startVertex, edgeValue);
        }

        public void AddDirectedEdge(int startVertex, int endVertex, double edgeValue)
        {
            this.AdjacencyArray[startVertex].Add(endVertex, edgeValue);
        }
    }
}