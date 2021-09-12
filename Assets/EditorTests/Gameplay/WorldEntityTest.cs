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
        public void CannotRefillArmorWithNegativeAmount()
        {
            float startingArmor = _worldEntity.Armor;
            
            _worldEntity.RefillArmor(-1f);
            
            Assert.AreEqual(startingArmor, _worldEntity.Armor, WorldEntityFloatDelta, 
                "Armor is not equal to the starting one, even though negative armor refilling must be ignored.");
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
        public void ExcessiveArmorRefillingClampsToMaximumRealFloatNumber()
        {
            // Try to overflow armor.
            _worldEntity.RefillArmor(1f);
            _worldEntity.RefillArmor(float.MaxValue);
            
            Assert.False(float.IsPositiveInfinity(_worldEntity.Armor), 
                "Armor refilling does not avoid overflowing, thus the armor value is positive infinity.");
            Assert.AreEqual(float.MaxValue, _worldEntity.Armor, WorldEntityFloatDelta, 
                "RefillArmor does not avoid overflowing.");
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
        public void ArmorRefillClampsPositiveInfinityToMaximumRealFloatNumber()
        {
            _worldEntity.RefillArmor(float.PositiveInfinity);
            
            Assert.False(float.IsPositiveInfinity(_worldEntity.Armor), "ArmorRefill does not clamp positive infinity.");
            Assert.AreEqual(float.MaxValue, _worldEntity.Armor, WorldEntityFloatDelta, 
                "ArmorRefill does not clamp positive infinity to the float maximum existing value.");
        }
    }
}