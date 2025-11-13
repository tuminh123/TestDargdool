
using System.Collections;
using UnityEngine;
public enum RightAttack { right_punch = 0, right_kick = 1, right_onto_pillow = 2, right_elbow_strike = 3 }
public enum LeftAttack { left_punch = 0, left_kick = 1, left_onto_pillow = 2, left_elbow_strike = 3 }
public class Attack : MonoBehaviour
{
    [Header("attack weapon")]
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject leftFoot;
    [SerializeField] private GameObject rightFoot;
    [SerializeField] private GameObject leftElbow;
    [SerializeField] private GameObject rightElbow;
    [SerializeField] private GameObject leftPillow;
    [SerializeField] private GameObject rightPillow;
    [Space]
    [Header("body")]
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject leftLeg;
    [SerializeField] private GameObject rightLeg;
    [Space]
    [Header("punch action index")]
    [SerializeField] float leftHandForce = 7f;
    [SerializeField] float rightHandForce = 7f;
    [Space]
    [Header("kick action index")]
    [SerializeField] float leftLegForce = 10;
    [SerializeField] float leftPillowForce = 10;
    [SerializeField] float leftFootForce = 10;
    [SerializeField] float rightLegForce = 10;
    [SerializeField] float rightPillowForce = 10;
    [SerializeField] float rightFootForce = 10;

    private Rigidbody2D leftLegRB;
    private Rigidbody2D leftHandRB;
    private Rigidbody2D leftFootRB;
    private Rigidbody2D leftElbowRB;
    private Rigidbody2D leftPillowRB;

    private Rigidbody2D rightLegRB;
    private Rigidbody2D rightHandRB;
    private Rigidbody2D rightFootRB;
    private Rigidbody2D rightElbowRB;
    private Rigidbody2D rightPillowRB;
    

    private Vector2 attackDir;
    private Animator ani;
    private void Awake()
    {
        leftHandRB = leftHand.GetComponent<Rigidbody2D>();
        leftFootRB = leftFoot.GetComponent<Rigidbody2D>();
        leftElbowRB = leftElbow.GetComponent<Rigidbody2D>();
        leftPillowRB = leftPillow.GetComponent<Rigidbody2D>();
        leftLegRB = leftLeg.GetComponent<Rigidbody2D>();

        rightHandRB = rightHand.GetComponent<Rigidbody2D>();
        rightFootRB = rightFoot.GetComponent<Rigidbody2D>();
        rightElbowRB = rightElbow.GetComponent<Rigidbody2D>();
        rightPillowRB = rightPillow.GetComponent<Rigidbody2D>();
        rightLegRB = rightLeg.GetComponent<Rigidbody2D>();

        ani = GetComponent<Animator>();
    }
    private void Start()
    {
    }
    private void Update()
    {
        Vector3 camDir = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        camDir.z = 0;
        attackDir = (camDir-body.transform.position).normalized;
        // Debug hướng
        Debug.DrawLine(body.transform.position,camDir, Color.blue);

    }
    private void FixedUpdate()
    {
        if (InputManager.Instance.attackInput)
        {
            if (attackDir.x > 0)
            {
                RightAttackHandle(RightAttack.right_punch);
            }
            else if(attackDir.x < 0)
            {
                LeftAttackHandle(LeftAttack.left_kick);
            }

        } 
    }
    private void RightAttackHandle(RightAttack attackType)
    {
        switch (attackType) 
        {
            case RightAttack.right_punch:
                ani.Play("punch_right");
                rightHandRB.AddForce(attackDir * (rightHandForce * 1000) * Time.fixedDeltaTime);
                break;
            case RightAttack.right_kick:
                Debug.Log("right kick");
                rightLegRB.AddForce(Vector2.up * (rightLegForce * 1000) * Time.fixedDeltaTime);
                rightPillowRB.AddForce(Vector2.up * (rightPillowForce * 1000) * Time.fixedDeltaTime);
                rightFootRB.AddForce(attackDir * (rightFootForce * 1000) * Time.fixedDeltaTime);
                break;
            case RightAttack.right_elbow_strike:
                Debug.Log("right elbow strike");
                rightElbowRB.AddForce(attackDir * (rightHandForce * 1000) * Time.fixedDeltaTime);
                break;
            case RightAttack.right_onto_pillow:
                Debug.Log("right onto pillow");
                rightLegRB.AddForce(Vector2.right * (rightLegForce * 1000) * Time.fixedDeltaTime);
                //rightPillowRB.AddForce(Vector2.right * (rightHandForce * 1000) * Time.fixedDeltaTime);
                break;
            default:
                break;
        }
    }
    private void LeftAttackHandle(LeftAttack attackType)
    {
        switch (attackType)
        {
            case LeftAttack.left_punch:
                ani.Play("punch_left");
                leftHandRB.AddForce(attackDir * (leftHandForce * 1000) * Time.fixedDeltaTime);
                break;
            case LeftAttack.left_kick:
                Debug.Log("left kick");
                leftLegRB.AddForce(Vector2.up * (leftLegForce * 1000) * Time.fixedDeltaTime);
                leftPillowRB.AddForce(Vector2.up * (leftPillowForce * 1000) * Time.fixedDeltaTime);
                leftFootRB.AddForce(attackDir* (leftFootForce * 1000) * Time.fixedDeltaTime);
                break;
            case LeftAttack.left_elbow_strike:
                Debug.Log("left elbow strike");
                leftElbowRB.AddForce(attackDir * (leftHandForce * 1000) * Time.fixedDeltaTime);
                break;
            case LeftAttack.left_onto_pillow:
               leftPillowRB.AddForce(attackDir * (leftHandForce * 1000) * Time.fixedDeltaTime);
                break;
            default:
                break;
        }
    }
    private RightAttack GetRandomRightAttack()
    {
        int rand = Random.Range(0,System.Enum.GetValues(typeof(RightAttack)).Length);
        return (RightAttack)rand;
    }
    private LeftAttack GetRandomLeftAttack()
    {
        int rand = Random.Range(0, System.Enum.GetValues(typeof(LeftAttack)).Length);
        return (LeftAttack)rand;
    }
}
