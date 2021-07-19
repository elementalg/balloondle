using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [SerializeField, Tooltip("Physics body to which the movement will be applied")]
    private Rigidbody2D m_BodyToBeMoved;

    [SerializeField, Tooltip("Maximum velocity")]
    private Vector2 m_MaximumVelocity = new Vector2(5f, 5f);
    
    public void OnMove(InputAction.CallbackContext input)
    {
        ApplyMovement(input.ReadValue<Vector2>());
    }

    // TODO: Add on press, on no longer press handlers.
    private void ApplyMovement(Vector2 inputVelocity)
    {
        inputVelocity = inputVelocity * m_MaximumVelocity;

        Vector2 currentVelocity = m_BodyToBeMoved.velocity;

        Vector2 movementForce = new Vector2();

        if (Mathf.Sign(inputVelocity.x) != Mathf.Sign(currentVelocity.x))
        {
            movementForce.x = inputVelocity.x + (currentVelocity.x * -1f);
        }
        else
        {
            movementForce.x = inputVelocity.x - currentVelocity.x;
        }

        if (Mathf.Sign(inputVelocity.y) != Mathf.Sign(currentVelocity.y))
        {
            movementForce.y = inputVelocity.y + (currentVelocity.y * -1f);
        }
        else
        {
            movementForce.y = inputVelocity.y - currentVelocity.y;
        }

        // Calculate the force required to obtain the previously calculated velocities.
        movementForce = movementForce * m_BodyToBeMoved.mass;

        m_BodyToBeMoved.AddForce(movementForce, ForceMode2D.Impulse);
    }
}