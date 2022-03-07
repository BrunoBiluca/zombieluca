using Assets.UnityFoundation.Code.Common;
using UnityEngine;

public interface IAIBrain
{
    bool IsWalking { get; }
    bool IsRunning { get; }
    bool IsAttacking { get; }
    bool IsWandering { get; }
    bool IsChasing { get; }
    Optional<Vector3> TargetPosition { get; }
    void SetPlayer(GameObject player);
    void Update();
}
