using UnityEngine;

namespace Balloondle.Light
{
    public class BalloonLightHandler : MonoBehaviour
    {
        private const string OutlineAngleShaderProperty = "_OutlineAngle";
        
        [SerializeField, Tooltip("Material containing the dynamic outline shader.")]
        private Material m_DynamicOutline;

        [SerializeField, Tooltip("Angle, in radians, which will be outlined for each raycast.")]
        private float m_OutlineAngleRadius = 0.3f;

        [SerializeField, Tooltip("Space, in radians, in between the outlined angles.")]
        private float m_OutlineAngleSpacing = 0.66f;
        
        [SerializeField, Tooltip("Angle, in radians, which is outlined next to the outline.")]
        private float m_SecondaryOutlineAngleDiameter = 0.36f;

        [SerializeField, Tooltip("Transform of the GameObject which acts as a light source.")]
        private Vector3 m_LightSource;
        
        private int _frameHitCount;
        private int _outlineBiSegmentPropertyId;
        private Vector4 _outlineBiSegmentAnglesRange;

        private void Start()
        {
            _outlineBiSegmentPropertyId = Shader.PropertyToID(OutlineAngleShaderProperty);
            _outlineBiSegmentAnglesRange = new Vector4();

            m_DynamicOutline.SetVector(_outlineBiSegmentPropertyId, _outlineBiSegmentAnglesRange);
        }
        
        private void Update()
        {
            // Obtain the lighting incision angle by calculating the direction from the balloon's center to the
            // center of the GameObject acting as light source.
            Vector3 worldDirection = m_LightSource - transform.position;

            Vector2 circumferencePoint = transform.InverseTransformDirection(worldDirection).normalized;
            float angle = Mathf.Atan2(circumferencePoint.y, circumferencePoint.x) / Mathf.PI;

            _outlineBiSegmentAnglesRange.x = (angle - m_OutlineAngleRadius) % 2f;
            _outlineBiSegmentAnglesRange.y = (_outlineBiSegmentAnglesRange.x + m_OutlineAngleRadius) % 2f;
            _outlineBiSegmentAnglesRange.z = (_outlineBiSegmentAnglesRange.y + m_OutlineAngleSpacing) % 2f;
            _outlineBiSegmentAnglesRange.w = (_outlineBiSegmentAnglesRange.z + m_SecondaryOutlineAngleDiameter) % 2f;
            
            // Pass the updated angles to the shader.
            m_DynamicOutline.SetVector(_outlineBiSegmentPropertyId, _outlineBiSegmentAnglesRange);
        }
    }
}
