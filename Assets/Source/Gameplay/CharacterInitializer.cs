using Balloondle.Gameplay.Physics2D;
using UnityEngine;

namespace Balloondle.Gameplay
{
    public class CharacterInitializer : MonoBehaviour
    {
        [SerializeField] private Rope2DSpawner m_Rope2DSpawner;

        [SerializeField] private GameObject m_StartGameObject;
        [SerializeField] private Rigidbody2D m_EndBody;
        
        [SerializeField] private float m_EndPointJointBreakForce = 10f;
        [SerializeField] private float m_EndPointJointBreakTorque = 10f;
        [SerializeField] private float m_RopeCellJointBreakForce = 2f;
        [SerializeField] private float m_RopeCellJointBreakTorque = 2f;
        [SerializeField] private float m_JointBetweenEndsBreakForce = 1000f;
        
        // Start is called before the first frame update
        void Start()
        {
            Rope2DLimits ropeLimits = new Rope2DLimits(5f, m_EndPointJointBreakForce, 
                m_EndPointJointBreakTorque, m_RopeCellJointBreakForce,
                m_RopeCellJointBreakTorque, m_JointBetweenEndsBreakForce, 100f);
            
            m_Rope2DSpawner.CreateRopeConnectingTwoRigidBodies2D(m_StartGameObject.GetComponent<Rigidbody2D>(),
                new Vector2(0, -0.5f), m_EndBody, new Vector2(0, -0.32f), ropeLimits);
        }
    }
}
