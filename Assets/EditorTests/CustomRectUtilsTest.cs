using Balloondle;
using NUnit.Framework;
using UnityEngine;

namespace EditorTests
{
    public class CustomRectUtilsTest
    {
        /// <summary>
        /// Check if the calculation of the closest vertex to a certain point is correct.
        /// </summary>
        [Test]
        public void CalculatesCorrectlyCloserVertexToPoint()
        {
            Vector2 pointA = new Vector2(1f, 1f);
            Vector2 pointB = new Vector2(5f, 5f);
            Vector2 pointC = new Vector2(5f, 1f);
            Vector2 pointD = new Vector2(1f, 5f);
            Rect rect = new Rect(2f, 2f, 2f, 2f);
            CustomRectUtils rectUtils = CustomRectUtils.Instance;

            Vector2 vertexA = rectUtils.GetClosestPointFromARectPerimeter(pointA, rect);
            Vector2 vertexB = rectUtils.GetClosestPointFromARectPerimeter(pointB, rect);
            Vector2 vertexC = rectUtils.GetClosestPointFromARectPerimeter(pointC, rect);
            Vector2 vertexD = rectUtils.GetClosestPointFromARectPerimeter(pointD, rect);

            Assert.AreEqual(new Vector2(2f, 2f), vertexA);
            Assert.AreEqual(new Vector2(4f, 4f), vertexB);
            Assert.AreEqual(new Vector2(4f, 2f), vertexC);
            Assert.AreEqual(new Vector2(2f, 4f), vertexD);
        }

        /// <summary>
        /// Check if the closest point, from a segment, to an another point is correct.
        /// </summary>
        [Test]
        public void CalculatesCorrectlyCloserPointFromSegmentToPoint()
        {
            Vector2 pointA = new Vector2(3f, 1f);
            Vector2 pointB = new Vector2(5f, 3f);
            Vector2 pointC = new Vector2(3f, 5f);
            Vector2 pointD = new Vector2(1f, 3f);
            Rect rect = new Rect(2f, 2f, 2f, 2f);
            CustomRectUtils rectUtils = CustomRectUtils.Instance;

            Vector2 closestToPointA = rectUtils.GetClosestPointFromARectPerimeter(pointA, rect);
            Vector2 closestToPointB = rectUtils.GetClosestPointFromARectPerimeter(pointB, rect);
            Vector2 closestToPointC = rectUtils.GetClosestPointFromARectPerimeter(pointC, rect);
            Vector2 closestToPointD = rectUtils.GetClosestPointFromARectPerimeter(pointD, rect);

            Assert.AreEqual(new Vector2(3f, 2f), closestToPointA);
            Assert.AreEqual(new Vector2(4f, 3f), closestToPointB);
            Assert.AreEqual(new Vector2(3f, 4f), closestToPointC);
            Assert.AreEqual(new Vector2(2f, 3f), closestToPointD);
        }
    }   
}