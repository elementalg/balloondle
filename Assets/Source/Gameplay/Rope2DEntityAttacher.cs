using System;
using System.Collections.Generic;
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
            if (start.GetComponent<Rigidbody2D>() == null)
            {
                throw new ArgumentException("WorldEntity must contain a Rigidbody2D component.", nameof(start));
            }

            if (end.GetComponent<Rigidbody2D>() == null)
            {
                throw new ArgumentException("WorldEntity must contain a Rigidbody2D component.", nameof(end));
            }
            
            WorldEntity ropeEntity = m_Rope2DSpawnerPrefab.CreateRopeConnectingTwoRigidBodies2D(
                start.GetComponent<Rigidbody2D>(),
                startAnchor,
                end.GetComponent<Rigidbody2D>(),
                endAnchor, m_Rope2DLimits);
            
            _attachments.Add(ropeEntity, new Tuple<WorldEntity, WorldEntity>(start, end));
            
            ropeEntity.OnPreDestroy += (damage) =>
            {
                if (_attachments.ContainsKey(ropeEntity))
                {
                    var (entity, otherEntity) = _attachments[ropeEntity];
                    
                    entity.RemoveAttachmentTo(otherEntity);
                    otherEntity.RemoveAttachmentTo(entity);
                    
                    _attachments.Remove(ropeEntity);
                }
            };

            return ropeEntity;
        }

        public override void Detach(WorldEntity attacher)
        {
            // Kill it. (:
            attacher.Damage(attacher.Health);
        }
    }
}