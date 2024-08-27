using System.Collections.Generic;
using System.Linq;

namespace AI.GOAP
{
    public sealed class GreedyPlanner : IPlanner
    {
        private IActor[] actions;
        private IFactState worldState;
        
        public bool MakePlan(IFactState worldState, IFactState goal, IActor[] actions, out List<IActor> plan)
        {
            if (goal.EqualsTo(worldState))
            {
                plan = new List<IActor>();
                return true;
            }

            this.worldState = worldState;
            this.actions = actions;

            return MakePlanRecursively(goal, baseNode: null, out plan);
        }

        private bool MakePlanRecursively(IFactState goal, Node baseNode, out List<IActor> plan)
        {
            var neighbours = FindNeighbours(goal);
            var orderedNeighbours = neighbours.OrderBy(it => it.EvaluateCost());

            foreach (var action in orderedNeighbours)
            {
                var node = new Node
                {
                    baseNode = baseNode,
                    action = action
                };
                
                var requiredState = action.RequiredState;
                if (requiredState.EqualsTo(worldState))
                {
                    plan = CreatePlan(node);
                    return true;
                }

                if (MakePlanRecursively(requiredState, node, out plan))
                {
                    return true;
                }
            }

            plan = null;
            return false;
        }

        private List<IActor> FindNeighbours(IFactState goal)
        {
            var result = new List<IActor>();
            
            foreach (var action in actions)
            {
                if (PlannerUtils.MatchesAction(action, goal, worldState))
                {
                    result.Add(action);
                }
            }

            return result;
        }

        private List<IActor> CreatePlan(Node endNode)
        {
            var plan = new List<IActor>();

            while (endNode != null)
            {
                plan.Add(endNode.action);
                endNode = endNode.baseNode;
            }
            
            return plan;
        }

        private sealed class Node
        {
            public Node baseNode;
            public IActor action;
        }
    }
}
