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
            float startingHealth = _worldEntity.health;
            
            _worldEntity.Heal(-1f);
            
            Assert.AreEqual(startingHealth, _worldEntity.health, WorldEntityFloatDelta, 
                "Health is not equal to the starting one, even though negative healing must be ignored.");
        }

        [Test]
        public void CannotRefillArmorWithNegativeAmount()
        {
            float startingArmor = _worldEntity.armor;
            
            _worldEntity.RefillArmor(-1f);
            
            Assert.AreEqual(startingArmor, _worldEntity.armor, WorldEntityFloatDelta, 
                "Armor is not equal to the starting one, even though negative armor refilling must be ignored.");
        }

        [Test]
        public void ExcessiveHealingClampsToMaximumRealFloatNumber()
        {
            // Try to overflow health.
            _worldEntity.Heal(1f);
            _worldEntity.Heal(float.MaxValue);
            
            Assert.False(float.IsPositiveInfinity(_worldEntity.health),
                "Heal does not avoid overflowing, thus the health value is positive infinity.");
            Assert.AreEqual(float.MaxValue, _worldEntity.health, WorldEntityFloatDelta, 
                "Heal does not avoid overflowing.");
        }

        [Test]
        public void ExcessiveArmorRefillingClampsToMaximumRealFloatNumber()
        {
            // Try to overflow armor.
            _worldEntity.RefillArmor(1f);
            _worldEntity.RefillArmor(float.MaxValue);
            
            Assert.False(float.IsPositiveInfinity(_worldEntity.armor), 
                "Armor refilling does not avoid overflowing, thus the armor value is positive infinity.");
            Assert.AreEqual(float.MaxValue, _worldEntity.armor, WorldEntityFloatDelta, 
                "RefillArmor does not avoid overflowing.");
        }

        [Test]
        public void HealClampsPositiveInfinityToMaximumRealFloatNumber()
        {
            _worldEntity.Heal(float.PositiveInfinity);
            
            Assert.False(float.IsPositiveInfinity(_worldEntity.health), "Heal does not clamp positive infinity.");
            Assert.AreEqual(float.MaxValue, _worldEntity.health, WorldEntityFloatDelta, 
                "Heal does not clamp positive infinity to the float maximum existing value.");
        }

        [Test]
        public void ArmorRefillClampsPositiveInfinityToMaximumRealFloatNumber()
        {
            _worldEntity.RefillArmor(float.PositiveInfinity);
            
            Assert.False(float.IsPositiveInfinity(_worldEntity.armor), "ArmorRefill does not clamp positive infinity.");
            Assert.AreEqual(float.MaxValue, _worldEntity.armor, WorldEntityFloatDelta, 
                "ArmorRefill does not clamp positive infinity to the float maximum existing value.");
        }
    }
}