using System;
using UnityEngine;

namespace Balloondle.Light
{
    public class BalloonLightHandler : LightHandler
    {
        private const string OutlineAngleShaderProperty = "_OutlineAngle";
        
        [SerializeField, Tooltip("Material containing the dynamic outline shader.")]
        private Material m_DynamicOutline;

        [SerializeField, Tooltip("Angle, in radians, which will be outlined for each raycast.")]
        private float m_OutlineAngleForEachRaycast = 3.14f;
        
        private int _frameHitCount;
        private int _outlineBiSegmentPropertyId;
        private Vector4 _outlineBiSegmentAnglesRange;

        private void Start()
        {
            _outlineBiSegmentPropertyId = Shader.PropertyToID(OutlineAngleShaderProperty);
            _outlineBiSegmentAnglesRange = new Vector4();
            
            m_DynamicOutline.SetVector(_outlineBiSegmentPropertyId, _outlineBiSegmentAnglesRange);
        }

        public override void OnLightRaycastHit(RaycastHit2D hit2D)
        {
            Vector2 normalizedCircumferencePoint = 1 * hit2D.normal.normalized;

            float angle = Mathf.Atan2(normalizedCircumferencePoint.y, normalizedCircumferencePoint.x) - hit2D.transform.eulerAngles.z % 360 * Mathf.Deg2Rad;
            UpdateOutlineSegment(angle);
            
            _frameHitCount++;
        }

        private void UpdateOutlineSegment(float lightRaycastHitAngle)
        {
            float startingAngle = ((lightRaycastHitAngle - m_OutlineAngleForEachRaycast / 2f) / Mathf.PI) % 2f;
            float endingAngle = ((lightRaycastHitAngle + m_OutlineAngleForEachRaycast) / Mathf.PI) % 2f;

            if (_frameHitCount == 0)
            {
                _outlineBiSegmentAnglesRange.x = startingAngle;
            }
            
            _outlineBiSegmentAnglesRange.y += endingAngle / Mathf.Pow(2f, _frameHitCount);

            m_DynamicOutline.SetVector(_outlineBiSegmentPropertyId, _outlineBiSegmentAnglesRange);
        }
        
        private void Update()
        {
            Debug.Log($"Update: {_frameHitCount}");
            _frameHitCount = 0;
        }
    }
}