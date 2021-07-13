using UnityEngine;

namespace Balloondle.UI.Controllers
{
    public class Joystick : MonoBehaviourWithBoundsDetector
    {
        private void Start()
        {
            InitializePressDetector();
        }

        public void OnPressed(Vector2 screenPoint)
        {
            
        }

        public void InputUpdate(Vector2 screenPoint)
        {
            
        }
    }
}