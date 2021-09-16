using System;
using Balloondle.Gameplay.Physics2D;
using UnityEngine;

namespace Balloondle.Gameplay
{
    [CreateAssetMenu(fileName = "Rope2DEntityAttacher", menuName = "WorldEntities/Rope2DEntityAttacher", order = 1)]
    public class Rope2DEntityAttacher : WorldEntityAttacher
    {
        [SerializeField, Tooltip("Rope2DSpawner prefab.")]
        private Rope2DSpawner m_Rope2DSpawnerPrefab;

        [SerializeField, Tooltip("Limits applied to the Rope2D. Maximum distance is calculated by the spawner.")]
        private Rope2DLimits m_Rope2DLimits;

        /// <summary>
        /// Spawns a <see cref="Rope2D"/> which attaches the start and end entities.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="startAnchor"></param>
        /// <param name="end"></param>
        /// <param name="endAnchor"></param>
        /// <exception cref="ArgumentException">if <paramref name="start"/> or <paramref name="end"/>
        /// do not contain <see cref="Rigidbody2D"/> components.</exception>
        public override void Attach(WorldEntity start, Vector3 startAnchor, WorldEntity end, Vector3 endAnchor)
        {
            if (start.GetComponent<Rigidbody2D>() == null)
            {
                throw new ArgumentException("WorldEntity must contain a Rigidbody2D component.", nameof(start));
            }

            if (end.GetComponent<Rigidbody2D>() == null)
            {
                throw new ArgumentException("WorldEntity must contain a Rigidbody2D component.", nameof(end));
            }
            
            m_Rope2DSpawnerPrefab.CreateRopeConnectingTwoRigidBodies2D(start.GetComponent<Rigidbody2D>(), startAnchor,
                end.GetComponent<Rigidbody2D>(), endAnchor, m_Rope2DLimits);
        }

        /// <summary>
        /// Checks whether or not the entity is attached by checking if it contains a <see cref="HingeJoint2D"/>
        /// and a <see cref="DistanceJoint2D"/>.
        ///
        /// NOTE: It may return true even if there's no rope attaching the entity, since it checks
        /// if there's a <see cref="HingeJoint2D"/> and a <see cref="DistanceJoint2D"/>.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>True if contains a <see cref="HingeJoint2D"/> and a <see cref="DistanceJoint2D"/>,
        /// false otherwise.</returns>
        public override bool IsAttached(WorldEntity entity)
        {
            return entity.GetComponent<HingeJoint2D>() != null && entity.GetComponent<DistanceJoint2D>() != null;
        }
    }
}