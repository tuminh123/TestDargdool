using UnityEngine;

[CreateAssetMenu (fileName ="Attack Data Config",menuName ="Data SO/Action Data SO/Attack Data/ Config")]
public class AttackDataConfigSO : ScriptableObject
{
    [SerializeField] private string attackName;
    [SerializeField] private float attackForce = 15f;
    [SerializeField] private float attackDuration = 0.5f;
    [SerializeField] private float maxSpeed = 5f;                 // Max speed tay/chân
    [SerializeField] private float attackReach = 1f;            // Khoảng cách tay bay trước
    [SerializeField] private float bodyMomentumFactor = 0.5f;   // Tỷ lệ momentum cơ thể kéo tay
    [SerializeField] private float decelDistance = 0.4f;        // Khoảng cách giảm tốc khi gần target
    [SerializeField] private float maxForce = 50f;              // Lực tối đa áp dụng mỗi frame
    [SerializeField] private float maxAngularSpeed = 200f;      // Tốc độ xoay tối đa mỗi frame
    [SerializeField] private float linearDrag = 2f;             // Damping tuyến tính của khớp
    [SerializeField] private float angularDrag = 5f;            // Damping xoay của khớp
    [SerializeField] private float rotateSmoothSpeed = 10f;     // Tốc độ xoay mượt
    [SerializeField] private float launchTime = 0.3f; // 10% duration dùng lực chính
    [SerializeField] private float proceduralOffset = 0.2f;
    //get
    public string AttackName => attackName;
    public float AttackForce => attackForce;
    public float AttackDuration => attackDuration;
    public float AttackReach => attackReach;
    public float BodyMomentumFactor => bodyMomentumFactor;
    public float DecelDistance => decelDistance;
    public float MaxForce => maxForce;
    public float MaxAngularSpeed => maxAngularSpeed;
    public float LinearDrag => linearDrag;
    public float AngularDrag => angularDrag;
    public float RotateSmoothSpeed => rotateSmoothSpeed;
    public float LaunchTime => launchTime;
    public float ProceduralOffset => proceduralOffset;
    public float MaxSpeed => maxSpeed;
}
