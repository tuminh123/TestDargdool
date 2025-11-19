using UnityEngine;

[CreateAssetMenu(fileName = "Attack Rotation SO",menuName = "Data SO/Attack Rotation SO")]
public class AttackRotSO : ScriptableObject
{
        [SerializeField]   private float rot_1,rot_2;
        private float initRot1, initRot2;
        private float initLinearDamping_1, initLinearDamping_2;
        private float initAngularDamping_1, initAngularDamping_2;

        public float Rot_1 => rot_1;
        public float Rot_2 => rot_2;

        public float CaculateRot_1()
        {
                return initRot1 + rot_1;
        }
        public float CaculateRot_2()
        {
                return initRot2 + rot_2;
        }

        public void InitData(Balance bodyPart_1, Balance bodyPart_2)
        {
                initRot1 = bodyPart_1.TargetRotation;
                initRot2 = bodyPart_2.TargetRotation;

                initLinearDamping_1 = bodyPart_1.Rb.linearDamping;
                initLinearDamping_2 = bodyPart_2.Rb.linearDamping;

                initAngularDamping_1 = bodyPart_1.Rb.angularDamping;
                initAngularDamping_2 = bodyPart_2.Rb.angularDamping;
        }
        
        public void ResetData(Balance bodyPart_1, Balance bodyPart_2)
        {
                bodyPart_1.SetTargetRotation(initRot1);
                bodyPart_2.SetTargetRotation(initRot2);
        
                // Restore damping
                bodyPart_1.Rb.linearDamping = initLinearDamping_1;
                bodyPart_2.Rb.linearDamping = initLinearDamping_2;
                bodyPart_1.Rb.angularDamping = initAngularDamping_1;
                bodyPart_2.Rb.angularDamping = initAngularDamping_2;
        }

}