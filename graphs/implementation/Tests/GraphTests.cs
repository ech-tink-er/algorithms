using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Utilities;

using Graphs;
using static Graphs.Traversals;
using static Graphs.Forest;
using static Demo.GraphIO;

[TestClass]
public class GraphTests
{
    private const string GraphsDir = "graphs";
    private static readonly char[] Vertices = { 'A', 'B', 'C', 'D', 'E', 'F' };

    private static bool AreShortestPaths(Graph<char> graph, Paths<char> paths, Vertex<char> target = null)
    {
        return graph.Edges
            .Where(edge => target == null || edge.Destination == target)
            .All(edge => !paths.CanRelax(edge));
    }

    public static Graph<char> EdgesToGraph(IEnumerable<Edge<char>> edges)
    {
        var graph = new Graph<char>();

        foreach (var edge in edges)
        {
            graph.AddEdge(edge.Source.Key, edge.Destination.Key, edge.Weight, edge.Directed);
        }

        return graph;
    }

    public static int? MaxEdgeWeightOnPath(Vertex<char> source, Vertex<char> target, Dictionary<Edge<char>, bool> visited = null)
    {
        if (visited == null)
        {
            visited = new Dictionary<Edge<char>, bool>();
        }

        if (source == target)
        {
            if (!visited.Any())
                return null;
            else 
                return visited.Where(p => !p.Value).Max(p => p.Key.Weight);
        }

        foreach (var edge in source.Edges)
        {
            if (visited.Keys.Any(e => e.Source == edge.Destination || e.Destination == edge.Destination))
                continue;

            visited[edge] = false;
            var result = MaxEdgeWeightOnPath(edge.Destination, target, visited);
            visited[edge] = true;

            if (result != null)
                return result;
        }

        return null;
    }

    private Dictionary<string, Graph<char>> graphs;
    private Dictionary<string, Graph<char>> undirected;
    private Dictionary<string, Graph<char>> unweighted;
    private Dictionary<string, Graph<char>> dags;
    private Dictionary<string, Graph<char>> negativeWeightCycles;

    public GraphTests()
    {
        var graphs = LoadGraphs(GraphsDir);

        this.graphs = graphs[0];
        this.undirected = graphs[1];
        this.unweighted = graphs[2];
        this.dags = graphs[3];
        this.negativeWeightCycles = graphs[4];
    }

    [TestMethod]
    public void AddsAndReadsVertices()
    {
        var graph = new Graph<char>();

        Assert.IsFalse(Vertices.Any(v => graph.HasVertex(v)));
        Assert.AreEqual(0, graph.Vertices.Count);

        foreach (var vertex in Vertices)
            graph.AddVertex(vertex);

        Assert.IsTrue(Vertices.All(v => graph.HasVertex(v)));
        Assert.AreEqual(Vertices.Length, graph.Vertices.Count);
    }

    [TestMethod]
    public void RemovesVertices()
    {
        var graph = new Graph<char>();
        foreach (var vertex in Vertices)
            graph.AddVertex(vertex);

        Assert.IsTrue(Vertices.All(v => graph.HasVertex(v)));
        Assert.AreEqual(Vertices.Length, graph.Vertices.Count);

        foreach (var vertex in Vertices)
            graph.RemoveVertex(vertex);

        Assert.IsFalse(Vertices.Any(v => graph.HasVertex(v)));
        Assert.AreEqual(0, graph.Vertices.Count);
    }

    [TestMethod]
    public void ClearRemovesAllVertices()
    {
        var graph = new Graph<char>();
        foreach (var vertex in Vertices)
            graph.AddVertex(vertex);

        Assert.IsTrue(Vertices.All(v => graph.HasVertex(v)));
        Assert.AreEqual(Vertices.Length, graph.Vertices.Count);

        graph.Clear();
            
        Assert.IsFalse(Vertices.Any(v => graph.HasVertex(v)));
        Assert.AreEqual(0, graph.Vertices.Count);
    }

    [TestMethod]
    public void AddsReversableEdgesAlongWithVertices()
    {
        var graph = new Graph<char>();
        char[] vertices = { 'A', 'B', 'C', 'D', 'E', 'F' };

        graph.AddEdge('A', 'B', 7, directed: false);
        graph.AddEdge('A', 'C', 9, directed: false);
        graph.AddEdge('A', 'D', 14, directed: false);

        graph.AddEdge('B', 'C', 10, directed: false);
        graph.AddEdge('B', 'E', 15, directed: false);

        graph.AddEdge('C', 'D', 2, directed: false);
        graph.AddEdge('C', 'E', 11, directed: false);

        graph.AddEdge('D', 'F', 9, directed: false);
        graph.AddEdge('E', 'F', 6, directed: false);

        Assert.IsTrue(vertices.All(v => graph.HasVertex(v)));
        Assert.AreEqual(vertices.Length, graph.Vertices.Count);

        foreach (var edge in graph.Edges)
        {
            Assert.IsTrue(edge.Destination.Edges.Any(e =>
            {
                return e.Destination == edge.Source && e.Weight == edge.Weight;
            }));       
        }
    }

    [TestMethod]
    public void RemovesEdgesWithoutTheVertices()
    {
        var graph = new Graph<char>();

        graph.AddEdge('A', 'B', 5);
        graph.AddEdge('B', 'C', 13);

        graph.RemoveEdge(graph['A'].Edges.First(e => e.Destination.Key == 'B'));
        graph.RemoveEdge(graph['B'].Edges.First(e => e.Destination.Key == 'C'));

        Assert.IsTrue(graph.HasVertex('A'));
        Assert.IsTrue(graph.HasVertex('B'));

        Assert.AreEqual(0, graph['A'].Edges.Count());
        Assert.AreEqual(0, graph['B'].Edges.Count());
        Assert.AreEqual(0, graph.Edges.Count());
    }

    [TestMethod]
    public void CopiesAllVerticesAndEdges()
    {
        var graph = this.graphs.Values.First();
        var copy = graph.Copy();

        Assert.IsTrue(graph.Vertices.All(v => copy.HasVertex(v.Key)));
        Assert.AreEqual(graph.Vertices.Count, copy.Vertices.Count);

        Assert.IsTrue(graph.Edges.All(graphEdge => copy.Edges.Any(copyEdge =>
        {
            return graphEdge.Source.Key.Equals(copyEdge.Source.Key) &&
                graphEdge.Destination.Key.Equals(copyEdge.Destination.Key) &&
                graphEdge.Weight == copyEdge.Weight;
        })));
        Assert.AreEqual(graph.Edges.Count(), copy.Edges.Count());
    }

    [TestMethod]
    public void ShortestPathsThrowsOnNegativeWeights()
    {
        var graph = this.dags["dag2.txt"];

        Assert.ThrowsException<InvalidOperationException>(() => graph.ShortestPaths(graph.Vertices.First()));
    }

    [TestMethod]
    public void FindsShortestPaths()
    {
        foreach (var graph in this.graphs.Values)
        {
            var paths = graph.ShortestPaths(graph.Vertices.First());

            Assert.IsTrue(AreShortestPaths(graph, paths));
        }
    }

    [TestMethod]
    public void FindsEdgewiseShortestPaths()
    {
        foreach (var graph in this.unweighted.Values)
        {
            var paths = graph.ShortestPaths(graph.Vertices.First(), edgewise: true);

            Assert.IsTrue(AreShortestPaths(graph, paths));
        }
    }

    [TestMethod]
    public void FindsDAGShortestPaths()
    {
        foreach (var graph in this.dags)
        {
            var source = graph.Value.Vertices.First();
            var paths = graph.Value.ShortestPaths(source, dag: true);

            Assert.IsTrue(AreShortestPaths(graph.Value, paths));
        }
    }

    [TestMethod]
    public void DAGShortestPathsThrowsOnCycle()
    {
        foreach (var graph in this.negativeWeightCycles)
        {
            var source = graph.Value.Vertices.First();

            Assert.ThrowsException<CycleException>(() => graph.Value.ShortestPaths(source, dag: true));
        }
    }

    [TestMethod]
    public void FindsShortestPathsWithSafe()
    {
        foreach (var graph in this.graphs.Values)
        {
            var paths = graph.ShortestPaths(graph.Vertices.First(), safe: true);

            Assert.IsTrue(AreShortestPaths(graph, paths));
        }
    }

    [TestMethod]
    public void FindsTargetedShortestPaths()
    {
        foreach (var graph in this.graphs)
        {
            var source = graph.Value.Vertices.First();
            var target = graph.Value.Vertices.Last();

            var paths = graph.Value.ShortestPaths(source, target);

            Assert.IsTrue(AreShortestPaths(graph.Value, paths, target));
        }
    }

    [TestMethod]
    public void FindsTargetedEdgewiseShortestPaths()
    {
        foreach (var graph in this.unweighted.Values)
        {
            var source = graph.Vertices.First();
            var target = graph.Vertices.Last();

            var paths = graph.ShortestPaths(source, target, edgewise: true);

            Assert.IsTrue(AreShortestPaths(graph, paths, target));
        }
    }

    [TestMethod]
    public void FindsTargetedDAGShortestPaths()
    {
        foreach (var graph in this.dags)
        {
            var source = graph.Value.Vertices.First();
            var target = graph.Value.Vertices.Last();

            var paths = graph.Value.ShortestPaths(source, dag: true);

            Assert.IsTrue(AreShortestPaths(graph.Value, paths, target));
        }
    }

    [TestMethod]
    public void FindsTargetedShortestPathsWithSafe()
    {
        foreach (var graph in this.graphs.Values)
        {
            var source = graph.Vertices.First();
            var target = graph.Vertices.Last();

            var paths = graph.ShortestPaths(source, target, safe: true);

            Assert.IsTrue(AreShortestPaths(graph, paths, target));
        }
    }

    [TestMethod]
    public void ThrowsFindingShortestPathsSafelyWithNegativeWeightCycles()
    {
        foreach (var graph in this.negativeWeightCycles.Values)
        {
            Assert.ThrowsException<CycleException>(() => 
            {
                var paths = graph.ShortestPaths(graph.Vertices.First(), safe: true);
            });
        }
    }

    [TestMethod]
    public void BreadthTraverseExploresInTheCorrectOrder()
    {
        var graph = this.graphs["treeish.txt"];
        char[] expected = { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };

        var traversal = new List<char>();
        BreadthTraverse(graph['A'], v => traversal.Add(v.Key));

        Assert.IsTrue(traversal.ValuesEqual(expected)); 
    }

    [TestMethod]
    public void BreadthTraverseExploresTheEntireGraph()
    {
        var graph = this.graphs["treeish.txt"];
        char[] expected = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I' };

        var traversal = new List<char>();
        BreadthTraverse(graph, v => traversal.Add(v.Key));

        Assert.IsTrue(traversal.ValuesEqual(expected));
    }

    [TestMethod]
    public void DepthTraverseExploresInTheCorrectOrder()
    {
        var graph = this.graphs["treeish.txt"];
        char[] expected = { 'G', 'D', 'E', 'B', 'F', 'C', 'A' };

        var traversal = new List<char>();
        DepthTraverse(graph['A'], v => traversal.Add(v.Key));

        Assert.IsTrue(traversal.ValuesEqual(expected)); 
    }

    [TestMethod]
    public void DepthTraverseExploresTheEntireGraph()
    {
        var graph = this.graphs["treeish.txt"];
        char[] expected = { 'G', 'D', 'E', 'B', 'F', 'C', 'A', 'I', 'H'};

        var traversal = new List<char>();
        DepthTraverse(graph, v => traversal.Add(v.Key));

        Assert.IsTrue(traversal.ValuesEqual(expected));
    }

    [TestMethod]
    public void TopoSortOrdersDAGsCorrectly()
    {
        foreach (var graph in this.dags)
        {
            var sorted = graph.Value.TopoSort();

            for (int i = sorted.Length - 1; i >= 1; i--)
            {
                var paths = graph.Value.ShortestPaths(sorted[i], edgewise: true);

                for (int j = i - 1; i >= 0; i--)
                {
                    Assert.AreEqual(Paths<char>.Unreachable, paths.Lengths[sorted[j]]);
                }
            }
        }
    }

    [TestMethod]
    public void TopoSortThrowsOnCycle()
    {
        foreach (var pair in this.negativeWeightCycles)
        {
            var graph = pair.Value;

            Assert.ThrowsException<CycleException>(() => graph.TopoSort());
        }
    }

    [TestMethod]
    public void MinSpanningTreeThorwsOnDirectedGraph()
    {
        foreach (var pair in this.dags)
        {
            var graph = pair.Value;

            Assert.ThrowsException<InvalidOperationException>(() => graph.MinSpanningTree(graph.Vertices.First()));
        }
    }

    [TestMethod]
    public void MinSpanningTreeIsATree()
    {
        foreach (var pair in this.undirected)
        {
            var graph = pair.Value;

            var root = graph.Vertices.First();
            var mst = graph.MinSpanningTree(root);

            foreach (var vertex in mst.Keys)
            {
                Assert.IsTrue(FindRoot(mst, vertex) == root);
            }
        }
    }

    [TestMethod]
    public void MinSpanningTreeSpansEntireComponent()
    {
        foreach (var pair in this.undirected)
        {
            var graph = pair.Value;

            var source = graph.Vertices.First();

            var component = new HashSet<Vertex<char>>();
            BreadthTraverse(source, v => component.Add(v));

            var mst = graph.MinSpanningTree(source);
            var mstGraph = EdgesToGraph(mst.Values);

            Assert.IsTrue(component.All(v => mstGraph.Vertices.Any(tv => v.Key == tv.Key)));
            Assert.AreEqual(component.Count, mstGraph.Vertices.Count);
        }
    }

    [TestMethod]
    public void MinSpanningTreeIsMinimum()
    {
        foreach (var pair in this.undirected)
        {
            var graph = pair.Value;
            var source = graph.Vertices.First();

            var component = new HashSet<Vertex<char>>();
            BreadthTraverse(source, v => component.Add(v));
            var componentEdges = component.SelectMany(v => v.Edges)
                .Distinct();

            var mst = graph.MinSpanningTree(source);
            var mstEdges = mst.Values;
            var nonMstEdges = componentEdges.Where(e => !mstEdges.Contains(e));

            var mstGraph = EdgesToGraph(mstEdges);

            foreach (var edge in nonMstEdges)
            {
                int maxWeight = (int)MaxEdgeWeightOnPath(mstGraph[edge.Source.Key], mstGraph[edge.Destination.Key]);

                Assert.IsFalse(edge.Weight < maxWeight);
            }
        }
    }

    [TestMethod]
    public void MinSpanningForestThorwsOnDirectedGraph()
    {
        foreach (var pair in this.dags)
        {
            var graph = pair.Value;

            Assert.ThrowsException<InvalidOperationException>(() => graph.MinSpanningForest());
        }
    }

    [TestMethod]
    public void MinSpanningForestHasNoCycles()
    {
        foreach (var pair in this.undirected)
        {
            var graph = pair.Value;

            var msf = graph.MinSpanningForest();

            foreach (var vertex in graph.Vertices)
            {
                var root = FindRoot(msf, vertex);
                Assert.IsTrue(msf[root].Destination == root);
            }
        }
    }

    [TestMethod]
    public void MinSpanningForestContainsAllVertices()
    {
        foreach (var pair in this.undirected)
        {
            var graph = pair.Value;

            var msf = graph.MinSpanningForest();

            var vertices = new HashSet<Vertex<char>>();
            foreach (var edge in msf.Values)
            {
                vertices.Add(edge.Source);
                vertices.Add(edge.Destination);
            }

            Assert.AreEqual(graph.Vertices.Count, vertices.Count);
            Assert.IsTrue(vertices.All(v => graph.HasVertex(v)));
        }
    }

    [TestMethod]
    public void MinSpanningForestSpansAllComponents()
    {
        foreach (var pair in this.undirected)
        {
            var graph = pair.Value;

            var msf = graph.MinSpanningForest();

            foreach (var edge in graph.Edges)
            {
                Assert.IsTrue(FindRoot(msf, edge.Source) == FindRoot(msf, edge.Destination));
            }
        }
    }

    [TestMethod]
    public void MinSpanningForestIsMinimum()
    {
        foreach (var pair in this.undirected)
        {
            var graph = pair.Value;

            var msf = graph.MinSpanningForest();
            var msfEdges = msf.Values;
            var nonMsfEdges = graph.Edges.Distinct()
                .Where(e => !msf.Values.Contains(e));

            var msfGraph = EdgesToGraph(msf.Values);

            foreach (var edge in nonMsfEdges)
            {
                int maxWeight = (int)MaxEdgeWeightOnPath(msfGraph[edge.Source.Key], msfGraph[edge.Destination.Key]);

                Assert.IsFalse(edge.Weight < maxWeight);
            }
        }
    }
}