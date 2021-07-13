using UnityEngine;

namespace Balloondle.UI.Controllers
{
    public class JoystickRange : MonoBehaviourWithBoundsDetector
    {
        private void Start()
        {
            InitializePressDetector();
        }
    }
}