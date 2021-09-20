using System;
using System.Collections.Generic;
using Balloondle.Gameplay.Physics2D;
using UnityEngine;
using UnityEngine.Serialization;

namespace Balloondle.Gameplay
{
    [CreateAssetMenu(fileName = "Rope2DEntityAttacher", menuName = "WorldEntities/Rope2DEntityAttacher", order = 1)]
    public class Rope2DEntityAttacher : WorldEntityAttacher
    {
        [SerializeField, Tooltip("Rope2DSpawner prefab.")]
        private Rope2DSpawner m_Rope2DSpawnerPrefab;

        [FormerlySerializedAs("m_Rope2DLimits")] [SerializeField, Tooltip("Limits applied to the Rope2D. Maximum distance is calculated by the spawner.")]
        private Rope2DArgs m_Rope2DArgs;

        private readonly Dictionary<WorldEntity, Tuple<WorldEntity, WorldEntity>> _attachments =
            new Dictionary<WorldEntity, Tuple<WorldEntity, WorldEntity>>();
        
        /// <summary>
        /// Spawns a <see cref="Rope2D"/> which attaches the start and end entities.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="startAnchor"></param>
        /// <param name="end"></param>
        /// <param name="endAnchor"></param>
        /// <returns>WorldEntity of the attacher.</returns>
        /// <exception cref="ArgumentException">if <paramref name="start"/> or <paramref name="end"/>
        /// do not contain <see cref="Rigidbody2D"/> components.</exception>
        public override WorldEntity Attach(WorldEntity start, Vector3 startAnchor, WorldEntity end, Vector3 endAnchor)
        {
            ExceptionIfWorldEntitiesMissRigidBody2D(start, end);
            
            WorldEntity ropeEntity = m_Rope2DSpawnerPrefab.CreateRopeConnectingTwoRigidBodies2D(
                start.GetComponent<Rigidbody2D>(),
                startAnchor,
                end.GetComponent<Rigidbody2D>(),
                endAnchor, m_Rope2DArgs);

            StoreAndConfigureRopeEntity(start, ropeEntity, end);

            return ropeEntity;
        }

        private void ExceptionIfWorldEntitiesMissRigidBody2D(WorldEntity start, WorldEntity end)
        {
            if (start.GetComponent<Rigidbody2D>() == null)
            {
                throw new ArgumentException("WorldEntity must contain a Rigidbody2D component.", nameof(start));
            }

            if (end.GetComponent<Rigidbody2D>() == null)
            {
                throw new ArgumentException("WorldEntity must contain a Rigidbody2D component.", nameof(end));
            }
        }

        private void StoreAndConfigureRopeEntity(WorldEntity start, WorldEntity ropeEntity, WorldEntity end)
        {
            _attachments.Add(ropeEntity, new Tuple<WorldEntity, WorldEntity>(start, end));
            
            ropeEntity.OnPreDestroy += (damage) =>
            {
                if (_attachments.ContainsKey(ropeEntity))
                {
                    var (entity, otherEntity) = _attachments[ropeEntity];
                    
                    entity.RemoveAttachmentTo(otherEntity);
                    
                    _attachments.Remove(ropeEntity);
                }
            };
        }

        public override WorldEntity Attach(WorldEntity start, Vector3 startAnchor, WorldEntity end, Vector3 endAnchor,
            float distance)
        {
            ExceptionIfWorldEntitiesMissRigidBody2D(start, end);

            m_Rope2DArgs.Length = distance;
            m_Rope2DArgs.MaximumDistanceBetweenBodies = distance;
            WorldEntity ropeEntity = m_Rope2DSpawnerPrefab.CreateRopeConnectingTwoRigidBodies2D(
                start.GetComponent<Rigidbody2D>(),
                startAnchor,
                end.GetComponent<Rigidbody2D>(),
                endAnchor, m_Rope2DArgs, true);

            StoreAndConfigureRopeEntity(start, ropeEntity, end);

            return ropeEntity;
        }

        public override void Detach(WorldEntity attacher)
        {
            // Kill it. (:
            attacher.Damage(attacher.Health);
        }
    }
}