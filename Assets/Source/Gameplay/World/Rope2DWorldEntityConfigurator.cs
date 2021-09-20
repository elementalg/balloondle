using System;
using UnityEngine;
using Balloondle.Gameplay.Physics2D;

namespace Balloondle.Gameplay
{
    /// <summary>
    /// Creates a link between the <see cref="WorldEntity"/> component, and the <see cref="Rope2D"/> component,
    /// through the usage of an instance of <see cref="Rope2DDestructorForWorldEntity"/>.
    ///
    /// The effect lasts as long as this component is enabled. When disabled, the custom destructor is removed.
    /// 
    /// NOTE: The WorldEntity's DestroyAfterTime is set to 0 by the configurator in order to make
    /// an instantaneous destruction.
    /// </summary>
    public class Rope2DWorldEntityConfigurator : MonoBehaviour
    {
        private void OnEnable()
        {
            if (GetComponent<WorldEntity>() == null)
            {
                throw new InvalidOperationException("A WorldEntity component is required.");
            }

            if (GetComponent<Rope2D>() == null)
            {
                throw new InvalidOperationException("A Rope2D component is required.");
            }
            
            WorldEntity worldEntity = GetComponent<WorldEntity>();
            
            // Make destruction immediate.
            worldEntity.m_DestroyAfterTime = 0f;
            Rope2DDestructorForWorldEntity destructor = new Rope2DDestructorForWorldEntity(worldEntity);

            Rope2D rope2D = GetComponent<Rope2D>();
            
            // Apply the custom destructor to the Rope2D component.
            rope2D.CustomDestructor = destructor;
        }

        private void OnDisable()
        {
            Rope2D rope2D = GetComponent<Rope2D>();

            if (rope2D != null)
            {
                // Disable custom destructor if it *looks like* it is our destructor.
                if (rope2D.CustomDestructor is Rope2DDestructorForWorldEntity)
                {
                    rope2D.CustomDestructor = null;
                }
            }
        }
    }
}