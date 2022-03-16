using UnityFoundation.Code;
using UnityEngine;

public interface IAIBrain
{
    bool DebugMode { get; set; }
    bool IsWalking { get; }
    bool IsRunning { get; }
    bool IsAttacking { get; }
    bool IsWandering { get; }
    bool IsChasing { get; }
    Optional<Vector3> TargetPosition { get; }
    void SetPlayer(GameObject player);
    void Update();
    void Enabled();
    void Disabled();
}
