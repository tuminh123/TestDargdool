using UnityEngine;

[CreateAssetMenu(fileName = "Attack Action SO",menuName = "Data SO/Action Data SO/Attack Data/ Action SO")]
public class AttackRotSO : ScriptableObject
{
        [SerializeField]   private float rot_1,rot_2;
        [SerializeField]   private float force_1,force_2;
        private float initRot1, initRot2;
        private float initForce1, initForce2;
        private float initLinearDamping_1, initLinearDamping_2;
        private float initAngularDamping_1, initAngularDamping_2;

        public float Rot_1 => rot_1;
        public float Rot_2 => rot_2;
        public float Force_1 => force_1;
        public float Force_2 => force_2;

        public float CaculateRot_1()
        {
                return initRot1 + rot_1;
        }
        public float CaculateRot_2()
        {
                return initRot2 + rot_2;
        }
    public float CaculateForce_1()
    {
        return initForce1 + force_1 ;
    }
    public float CaculateForce_2()
    {
        return initForce2 + force_2;
    }

    public void InitData(Balance bodyPart_1, Balance bodyPart_2)
    {
        initRot1 = bodyPart_1.Rotation;
        initRot2 = bodyPart_2.Rotation;

        initForce1 = bodyPart_1.Force;
        initForce2 = bodyPart_2.Force;

        initLinearDamping_1 = bodyPart_1.Rb.linearDamping;
        initLinearDamping_2 = bodyPart_2.Rb.linearDamping;

        initAngularDamping_1 = bodyPart_1.Rb.angularDamping;
        initAngularDamping_2 = bodyPart_2.Rb.angularDamping;
    }
        
    public void ResetData(Balance bodyPart_1, Balance bodyPart_2)
    {

        bodyPart_1.Rb.linearVelocity = Vector2.zero;
        bodyPart_2.Rb.linearVelocity = Vector2.zero;
        bodyPart_1.Rb.angularVelocity = 0f;
        bodyPart_2.Rb.angularVelocity = 0f;


        bodyPart_1.SetRotation(initRot1);
        bodyPart_2.SetRotation(initRot2);
        bodyPart_1.SetForce(initForce1);
        bodyPart_2.SetForce(initForce2);
        
        // Restore damping
        bodyPart_1.Rb.linearDamping = initLinearDamping_1;
        bodyPart_2.Rb.linearDamping = initLinearDamping_2;
        bodyPart_1.Rb.angularDamping = initAngularDamping_1;
        bodyPart_2.Rb.angularDamping = initAngularDamping_2;
    }

}