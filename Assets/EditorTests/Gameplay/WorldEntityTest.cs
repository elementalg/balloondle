using System;
using Balloondle.Gameplay;
using NUnit.Framework;
using UnityEngine;

namespace EditorTests.Gameplay
{
    public class WorldEntityTest
    {
        private const float WorldEntityFloatDelta = 0.00001f;
        private WorldEntity _worldEntity;

        [SetUp]
        public void InitializeWorldEntity()
        {
            GameObject gameObject = new GameObject();
            _worldEntity = gameObject.AddComponent<WorldEntity>();
        }

        [Test]
        public void CannotHealWithNegativeAmount()
        {
            float startingHealth = _worldEntity.Health;
            
            _worldEntity.Heal(-1f);
            
            Assert.AreEqual(startingHealth, _worldEntity.Health, WorldEntityFloatDelta, 
                "Health is not equal to the starting one, even though negative healing must be ignored.");
        }

        [Test]
        public void ExcessiveHealingClampsToMaximumRealFloatNumber()
        {
            // Try to overflow health.
            _worldEntity.Heal(1f);
            _worldEntity.Heal(float.MaxValue);
            
            Assert.False(float.IsPositiveInfinity(_worldEntity.Health),
                "Heal does not avoid overflowing, thus the health value is positive infinity.");
            Assert.AreEqual(float.MaxValue, _worldEntity.Health, WorldEntityFloatDelta, 
                "Heal does not avoid overflowing.");
        }

        [Test]
        public void HealClampsPositiveInfinityToMaximumRealFloatNumber()
        {
            _worldEntity.Heal(float.PositiveInfinity);
            
            Assert.False(float.IsPositiveInfinity(_worldEntity.Health), "Heal does not clamp positive infinity.");
            Assert.AreEqual(float.MaxValue, _worldEntity.Health, WorldEntityFloatDelta, 
                "Heal does not clamp positive infinity to the float maximum existing value.");
        }

        [Test]
        public void CannotApplyNegativeOrZeroDamage()
        {
            float startingHealth = _worldEntity.Health;
            
            _worldEntity.Damage(-1f);
            _worldEntity.Damage(0f);
            
            Assert.AreEqual(startingHealth, _worldEntity.Health);
        }

        [Test]
        public void PreHealthActionIsInvokedBeforeApplyingHealingToHealth()
        {
            bool onPreHealingActionExecuted = false;
            bool healthIsNotAffectedOnPreHealth = false;
            float startingHealth = _worldEntity.Health;
            float healingToBeMade = 100f;
            
            _worldEntity.OnHealthPreReceived += (healing) =>
            {
                onPreHealingActionExecuted = true;

                healthIsNotAffectedOnPreHealth = Math.Abs(startingHealth - _worldEntity.Health) < WorldEntityFloatDelta;
            };
            
            _worldEntity.Heal(healingToBeMade);
            
            Assert.True(onPreHealingActionExecuted);
            Assert.True(healthIsNotAffectedOnPreHealth);
            Assert.True(Math.Abs(startingHealth + healingToBeMade - _worldEntity.Health) < WorldEntityFloatDelta);
        }
        
        [Test]
        public void PreDamageActionIsInvokedBeforeApplyingDamageToHealth()
        {
            bool onPreDamageActionExecuted = false;
            bool healthIsNotAffectedOnPreDamage = false;
            float startingHealth = _worldEntity.Health;
            float damageToBeApplied = 99f;
            
            _worldEntity.OnDamagePreReceived += (damage) =>
            {
                onPreDamageActionExecuted = true;

                healthIsNotAffectedOnPreDamage = Math.Abs(startingHealth - _worldEntity.Health) < WorldEntityFloatDelta;
            };
            
            _worldEntity.Damage(damageToBeApplied);
            
            Assert.True(onPreDamageActionExecuted);
            Assert.True(healthIsNotAffectedOnPreDamage);
            Assert.True(Math.Abs(startingHealth - damageToBeApplied - _worldEntity.Health) < WorldEntityFloatDelta);
        }

        [Test]
        public void OnDamageAllHealthPreDestroyPassesLastDamageAmount()
        {
            bool onPreDestroyActionExecuted = false;
            bool isDamageEqualToLastDamageApplied = false;
            float startingHealth = _worldEntity.Health;

            float damageToBeApplied = startingHealth + 5f;
            
            _worldEntity.OnPreDestroy += (damage) =>
            {
                onPreDestroyActionExecuted = true;
                isDamageEqualToLastDamageApplied =
                    Math.Abs(damageToBeApplied - damage) < WorldEntityFloatDelta;
            };
            
            _worldEntity.Damage(damageToBeApplied);
            
            Assert.True(onPreDestroyActionExecuted);
            Assert.True(isDamageEqualToLastDamageApplied);
        }

        [Test]
        public void IndestructibleIgnoresDamage()
        {
            float startingHealth = _worldEntity.Health;
            _worldEntity.m_Indestructible = true;

            _worldEntity.Damage(100f);

            Assert.True(Math.Abs(startingHealth - _worldEntity.Health) < WorldEntityFloatDelta);
        }
    }
}