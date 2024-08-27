using System.Collections.Generic;

namespace PathFinding
{
    /// <summary>
    ///     <para>Finds a path by A* algorithm.</para> 
    /// </summary>
    public abstract class PathFinder<T> where T : class
    {
        private readonly Dictionary<T, Node> openList;

        private readonly HashSet<T> closedList;

        private T start;

        private T end;

        public PathFinder()
        {
            openList = new Dictionary<T, Node>();
            closedList = new HashSet<T>();
        }

        public bool FindPath(T start, T end, List<T> result)
        {
            result.Clear();

            if (ReferenceEquals(start, null) || ReferenceEquals(end, null))
            {
                return false;
            }

            if (ReferenceEquals(start, end))
            {
                return true;
            }

            this.start = start;
            this.end = end;

            openList.Clear();
            closedList.Clear();

            return FindPath(result);
        }

        protected virtual bool IsAvailable(T point)
        {
            return true;
        }

        protected abstract IEnumerable<T> GetNeighbours(T point);

        protected abstract float GetDistance(T point1, T point2);

        protected abstract float GetHeuristic(T point1, T point2);

        protected virtual float GetCost(Node node)
        {
            return node.cost + node.heuristic;
        }

        private bool FindPath(List<T> result)
        {
            var next = start;
            var nextNode = new Node(
                point: next,
                baseNode: null,
                cost: 0,
                heuristic: GetHeuristic(start, end)
            );

            while (true)
            {
                closedList.Add(next);
                ProcessNeighbours(nextNode);

                if (FindFinish(out var endNode))
                {
                    CreatePath(endNode, result);
                    return true;
                }

                if (!SelectNext(out nextNode))
                {
                    return false;
                }

                next = nextNode.point;
                openList.Remove(next);
            }
        }

        private void ProcessNeighbours(Node node)
        {
            var neighbours = GetNeighbours(node.point);
            foreach (var point in neighbours)
            {
                ProcessNeighbour(point, node);
            }
        }

        private void ProcessNeighbour(T neighbour, Node baseNode)
        {
            if (closedList.Contains(neighbour))
            {
                return;
            }

            if (!IsAvailable(neighbour))
            {
                closedList.Add(neighbour);
                return;
            }

            var distance = GetDistance(neighbour, baseNode.point);
            var distanceToStart = baseNode.cost + distance;
            var neighbourAlreadyExists = openList.TryGetValue(neighbour, out var node);
            if (neighbourAlreadyExists)
            {
                if (node.cost > distanceToStart)
                {
                    node.baseNode = baseNode;
                    node.cost = distanceToStart;
                }
            }
            else
            {
                node = new Node(
                    point: neighbour,
                    baseNode: baseNode,
                    cost: distanceToStart,
                    heuristic: GetHeuristic(neighbour, end)
                );

                openList.Add(neighbour, node);
            }
        }

        private bool FindFinish(out Node node)
        {
            return openList.TryGetValue(end, out node);
        }

        private void CreatePath(Node endNode, List<T> result)
        {
            var currentNode = endNode;
            while (!ReferenceEquals(currentNode.point, start))
            {
                result.Add(currentNode.point);
                currentNode = currentNode.baseNode;
            }

            result.Add(start);
            result.Reverse();
        }

        private bool SelectNext(out Node result)
        {
            result = null;
            float resultWeight = -1;
            foreach (var nodeKV in openList)
            {
                var node = nodeKV.Value;
                var weight = GetCost(node);

                if (result == null || resultWeight > weight)
                {
                    result = node;
                    resultWeight = weight;
                }
            }

            return result != null;
        }

        protected sealed class Node
        {
            public readonly T point;

            public Node baseNode;
            
            public float cost;

            public readonly float heuristic;

            public Node(T point, Node baseNode, float cost, float heuristic)
            {
                this.point = point;
                this.baseNode = baseNode;
                this.cost = cost;
                this.heuristic = heuristic;
            }
        }
    }
}