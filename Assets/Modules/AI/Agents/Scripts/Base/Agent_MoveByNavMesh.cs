using System;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Agents
{
    public abstract class Agent_MoveByNavMesh : Agent
    {
        public event Action OnPositionReached
        {
            add { MoveAgent.OnPathFinished += value; }
            remove { MoveAgent.OnPathFinished -= value; }
        }

        public bool IsPathFinished
        {
            get { return MoveAgent.IsPathFinished; }
        }

        protected abstract Agent_MoveByPoints<Vector3> MoveAgent { get; }

        private readonly NavMeshPath currentPath = new();

        private int navMeshAreas;

        public void SetNavMeshAreas(int navMeshAreas)
        {
            this.navMeshAreas = navMeshAreas;
        }

        public void SetTargetPosition(Vector3 targetPosition)
        {
            var currentPosition = EvaluateCurrentPosition();
            if (NavMesh.CalculatePath(
                    currentPosition,
                    targetPosition,
                    navMeshAreas,
                    currentPath
                ))
            {
                MoveAgent.SetPath(currentPath.corners);
            }
            else
            {
                Debug.LogWarning($"Can not calculate path to {targetPosition}");
            }
        }

        protected override void OnStart()
        {
            MoveAgent.Play();
        }

        protected override void OnStop()
        {
            MoveAgent.Stop();
        }

        protected abstract Vector3 EvaluateCurrentPosition();
    }
}