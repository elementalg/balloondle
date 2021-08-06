using UnityEngine;

namespace Balloondle.Gameplay
{
    public class CharacterInitializer : MonoBehaviour
    {
        [SerializeField] private RopeCreator m_RopeCreator;

        [SerializeField] private GameObject m_StartGameObject;
        [SerializeField] private Rigidbody2D m_EndBody;
        
        // Start is called before the first frame update
        void Start()
        {
            m_RopeCreator.CreateRopeConnectingTwoRigidBodies2D(m_StartGameObject.GetComponent<Rigidbody2D>(),
                new Vector2(0, -0.5f), m_EndBody, new Vector2(0, -0.32f),
                5f);
        }
    }
}
