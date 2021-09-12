using UnityEngine;

namespace Balloondle.Input
{
    public class PointerDemultiplexerBehaviour : MonoBehaviour
    {
        [SerializeField, Tooltip("PointerListener to which the demultiplexer will be connected.")]
        private PointerListener m_PointerListener;

        public PointerDemultiplexer Demultiplexer { get; } = new PointerDemultiplexer();
        
        void Start()
        {
            m_PointerListener.OnPointerUpdate += Demultiplexer.OnPointerUpdate;
        }

        private void OnApplicationFocus(bool focus)
        {
            if (!focus)
            {
                Demultiplexer.DeselectSelectedPointers();
            }
        }
    }
}