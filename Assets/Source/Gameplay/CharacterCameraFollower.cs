using UnityEngine;

public class CharacterCameraFollower : MonoBehaviour
{
    [SerializeField, Tooltip("GameObject which will be followed")]
    private GameObject m_FollowedObject;

    void Update()
    {
        Vector3 currentPosition = m_FollowedObject.transform.position;
        Vector3 newPosition = new Vector3(currentPosition.x,
            currentPosition.y,
            transform.position.z);

        transform.position = newPosition;
    }
}
