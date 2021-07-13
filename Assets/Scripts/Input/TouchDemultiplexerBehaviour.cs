using UnityEngine;

namespace Balloondle.UI
{
    public class TouchDemultiplexerBehaviour : MonoBehaviour
    {
        [SerializeField, Tooltip("TouchListener to which the demultiplexer will be connected.")]
        private TouchListener m_TouchListener;

        public TouchDemultiplexer Demultiplexer { get; } = new TouchDemultiplexer();
        
        void Start()
        {
            m_TouchListener.OnTouchUpdate += Demultiplexer.OnTouchUpdate;
        }
    }
}