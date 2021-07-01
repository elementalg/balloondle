using System;
using UnityEngine;

namespace Balloondle
{
    /// <summary>
    /// Utilities for <see cref="Rect"/>.
    /// </summary>
    public class CustomRectUtils
    {
        private enum RectSegments
        {
            Top,
            Right,
            Bottom,
            Left,
        }

        /// <summary>
        /// Retrieves the closest point , from the rect's perimeter, to the passed point.
        ///
        /// NOTE: POINT AND RECT MUST USE THE SAME ORIGIN POINT.
        /// </summary>
        /// <param name="point">Point whose closest projection to the rect's perimeter is required.</param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public Vector2 GetClosestPointFromARectPerimeter(Vector2 point, Rect rect)
        {
            if (rect.xMin > point.x || point.x > rect.xMax)
            {
                if (rect.yMin > point.y || point.y > rect.yMax)
                {
                    // Detect closest vertex.
                    Vector2 closestPointOnPerimeter = new Vector2();
                    closestPointOnPerimeter.x = (Mathf.Abs(rect.xMin - point.x)
                                                 < Mathf.Abs(rect.xMax - point.x)) ?
                        rect.xMin :
                        rect.xMax;

                    closestPointOnPerimeter.y = (Mathf.Abs(rect.yMin - point.y)
                                                 < Mathf.Abs(rect.yMax - point.y)) ?
                        rect.yMin :
                        rect.yMax;

                    return closestPointOnPerimeter;
                }
                else
                {
                    float rightSegmentDistance = GetDistanceFromRectSegment(point, 
                        rect, RectSegments.Right);
                    float leftSegmentDistance = GetDistanceFromRectSegment(point,
                        rect, RectSegments.Left);

                    if (rightSegmentDistance < leftSegmentDistance)
                    {
                        return GetIntersectionBetweenPointAndRectSegment(point, rect, RectSegments.Right);
                    }
                    else
                    {
                        return GetIntersectionBetweenPointAndRectSegment(point, rect, RectSegments.Left);
                    }
                }
            }       
            else
            {
                float topSegmentDistance = GetDistanceFromRectSegment(point, rect, RectSegments.Top);
                float bottomSegmentDistance = GetDistanceFromRectSegment(point,
                    rect, RectSegments.Bottom);

                if (topSegmentDistance < bottomSegmentDistance)
                {
                    return GetIntersectionBetweenPointAndRectSegment(point, rect, RectSegments.Top);
                }
                else
                {
                    return GetIntersectionBetweenPointAndRectSegment(point, rect, RectSegments.Bottom);
                }
            }
        }
    
        private float GetDistanceFromRectSegment(Vector2 point, Rect rect, RectSegments segment)
        {
            Tuple<Vector2, Vector2> segmentVertices = GetSegmentVertices(rect, segment);

            return GetDistanceBetweenPointAndLineDefinedByTwoPoints(point,
                segmentVertices.Item1, segmentVertices.Item2);
        }

        private Tuple<Vector2, Vector2> GetSegmentVertices(Rect rect, RectSegments segment)
        {
            Vector2 segmentAPoint = new Vector2();
            Vector2 segmentBPoint = new Vector2();

            switch (segment)
            {
                case RectSegments.Top:
                    segmentAPoint.x = rect.xMin;
                    segmentAPoint.y = rect.yMax;

                    segmentBPoint.x = rect.xMax;
                    segmentBPoint.y = rect.yMax;
                    break;
                case RectSegments.Right:
                    segmentAPoint.x = rect.xMax;
                    segmentAPoint.y = rect.yMax;

                    segmentBPoint.x = rect.xMax;
                    segmentBPoint.y = rect.yMin;
                    break;
                case RectSegments.Bottom:
                    segmentAPoint.x = rect.xMin;
                    segmentAPoint.y = rect.yMin;

                    segmentBPoint.x = rect.xMax;
                    segmentBPoint.y = rect.yMin;
                    break;
                case RectSegments.Left:
                    segmentAPoint.x = rect.xMin;
                    segmentAPoint.y = rect.yMax;

                    segmentBPoint.x = rect.xMin;
                    segmentBPoint.y = rect.yMin;
                    break;
            }

            return new Tuple<Vector2, Vector2>(segmentAPoint, segmentBPoint);
        }

        private float GetDistanceBetweenPointAndLineDefinedByTwoPoints(Vector2 point,
            Vector2 linePointA, Vector2 linePointB)
        {
            float numerator = Mathf.Abs((linePointB.x - linePointA.x) * (linePointA.y - point.y) - 
                                        (linePointA.x - point.x) * (linePointB.y - linePointA.y));
            float denominator = Mathf.Sqrt(Mathf.Pow(linePointB.x - linePointA.x, 2f) +
                                           Mathf.Pow(linePointB.y - linePointA.y, 2f));

            return numerator / denominator;
        }

        private Vector2 GetIntersectionBetweenPointAndRectSegment(Vector2 point,
            Rect rect, RectSegments segment)
        {
            Tuple<Vector2, Vector2> segmentVertices = GetSegmentVertices(rect, segment);

            // Handle Vertical Line and Horizontal Line.
            float intersectionX;
            float intersectionY;

            if (segmentVertices.Item1.x == segmentVertices.Item2.x) // Vertical Segment
            {
                intersectionX = segmentVertices.Item1.x;
                intersectionY = point.y;
            }
            else if (segmentVertices.Item1.y == segmentVertices.Item2.y) // Horizontal Segment
            {
                intersectionX = point.x;
                intersectionY = segmentVertices.Item1.y;
            }
            else
            {
                float segmentSlope = (segmentVertices.Item1.y - segmentVertices.Item2.y) /
                                     (segmentVertices.Item1.x - segmentVertices.Item2.x);

                float collisionLineSlope = -1f / segmentSlope;

                intersectionX = (segmentSlope * segmentVertices.Item1.x - collisionLineSlope * point.x -
                    segmentVertices.Item1.y + point.y) / (segmentSlope - collisionLineSlope);
                intersectionY = segmentSlope * (intersectionX - segmentVertices.Item1.x) +
                                segmentVertices.Item1.y;
            }

            return new Vector2(intersectionX, intersectionY);
        }

        public Vector2 LocalPointInRectangleToScreenPoint(
            RectTransform rect,
            Vector2 localPoint,
            Camera cam)
        {
            Vector3 worldPoint = rect.TransformPoint(localPoint);
            return RectTransformUtility.WorldToScreenPoint(cam, worldPoint);
        }
    }
}