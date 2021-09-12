using System;
using UnityEngine;

namespace Balloondle.UI
{
    /// <summary>
    /// Proceeds to update the size and position of the UI element in order to keep it looking as
    /// in the designed version.
    ///
    /// It is required to have the Canvas as a direct parent for proper operation.
    /// </summary>
    public class UIRelativeSize : MonoBehaviour
    {
        [SerializeField, Tooltip("Resolution used for designing the UI element's size.")]
        private Vector2 m_DesignResolution;
        
        private void Start()
        {
            if (GetComponent<RectTransform>() == null)
            {
                throw new InvalidOperationException("Missing RectTransform component from GameObject.");
            }
            
            RectTransform rectTransform = GetComponent<RectTransform>();

            // Update the size of the element.
            Vector2 originalSize = rectTransform.sizeDelta;
            Vector2 relativeSize = originalSize / m_DesignResolution;

            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                throw new InvalidOperationException("There must be a Canvas in the scene.");
            }
            
            Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;

            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, relativeSize.x * canvasSize.x);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, relativeSize.y * canvasSize.y);

            // Update the position of the element.
            Vector2 initialPos = rectTransform.anchoredPosition;

            Vector2 newPos = new Vector2(
                initialPos.x * (relativeSize.x * canvasSize.x / originalSize.x),
                initialPos.y * (relativeSize.y * canvasSize.y / originalSize.y));
            
            rectTransform.anchoredPosition = newPos;
        }
    }
}