using UnityEngine;

namespace QuickUnity.Extensions.Unity
{
    public static class ExVector
    {
        /// <summary>
        /// Multiply the vectors by each element one-to-one.
        /// </summary>
        /// <returns>The one to one.</returns>
        /// <param name="origin">Origin.</param>
        /// <param name="factor">Factor.</param>
        public static Vector3 ProductOneToOne(Vector3 origin, Vector3 factor)
        {
            return new Vector3(origin.x * factor.x, origin.y * factor.y, origin.z * factor.z);
        }

        /// <summary>
        /// Multiply the vectors by each element one-to-one.
        /// </summary>
        /// <returns>The one to one.</returns>
        /// <param name="origin">Origin.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        public static Vector3 ProductOneToOne(Vector3 origin, float x, float y, float z)
        {
            return ProductOneToOne(origin, new Vector3(x, y, z));
        }

        /// <summary>
        /// Returns the absolute value of the distance to the target.
        /// </summary>
        /// <returns>The distance.</returns>
        /// <param name="origin">Origin.</param>
        /// <param name="target">Target.</param>
        public static float GetDistance(Vector3 origin, Vector3 target)
        {
            return Mathf.Abs((origin - target).sqrMagnitude);
        }

        /// <summary>
        /// Returns the absolute value of the distance to the target, ignoring the height.
        /// </summary>
        /// <returns>The distance2 d.</returns>
        /// <param name="origin">Origin.</param>
        /// <param name="target">Target.</param>
        public static float GetDistance2D(Vector3 origin, Vector3 target)
        {
            return Mathf.Abs((SetY(origin, 0) - SetY(target, 0)).sqrMagnitude);
        }

        /// <summary>
        /// Computes a directional vector that points to the target.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        public static Vector3 GetTowardNormalizedVector(Vector3 origin, Vector3 factor)
        {
            return (factor - origin).normalized;
        }

        /// <summary>
        /// Returns a vector that overrides the x.
        /// </summary>
        /// <returns>The x.</returns>
        /// <param name="origin">Origin.</param>
        /// <param name="x">The x coordinate.</param>
        public static Vector3 SetX(Vector3 origin, float x)
        {
            return new Vector3(x, origin.y, origin.z);
        }

        /// <summary>
        /// Returns a vector that overrides y.
        /// </summary>
        /// <returns>The y.</returns>
        /// <param name="origin">Origin.</param>
        /// <param name="y">The y coordinate.</param>
        public static Vector3 SetY(Vector3 origin, float y)
        {
            return new Vector3(origin.x, y, origin.z);
        }

        /// <summary>
        /// Returns a vector that overrides z.
        /// </summary>
        /// <returns>The z.</returns>
        /// <param name="origin">Origin.</param>
        /// <param name="z">The z coordinate.</param>
        public static Vector3 SetZ(Vector3 origin, float z)
        {
            return new Vector3(origin.x, origin.y, z);
        }

        /// <summary>
        /// Returns a normalized vector with x set to 0.
        /// </summary>
        /// <returns>The x.</returns>
        /// <param name="origin">Origin.</param>
        public static Vector3 SuppressX(Vector3 origin)
        {
            return SetX(origin, 0).normalized;
        }

        /// <summary>
        /// Returns a normalized vector with y set to 0.
        /// </summary>
        /// <returns>The x.</returns>
        /// <param name="origin">Origin.</param>
        public static Vector3 SuppressY(Vector3 origin)
        {
            return SetY(origin, 0).normalized;
        }

        /// <summary>
        /// Returns a normalized vector with z set to 0.
        /// </summary>
        /// <returns>The x.</returns>
        /// <param name="origin">Origin.</param>
        public static Vector3 SuppressZ(Vector3 origin)
        {
            return SetZ(origin, 0).normalized;
        }

        /// <summary>
        /// Returns vectors around the specified coordinates at random.
        /// </summary>
        /// <returns>The around2 d.</returns>
        /// <param name="origin">Origin.</param>
        /// <param name="radius">Radius.</param>
        public static Vector3 GetAround(Vector3 origin, float radius)
        {
            var rad = global::UnityEngine.Random.Range(-180, 180);
            var vec = Quaternion.AngleAxis(rad, Vector3.up) * Vector3.forward;
            return origin + vec * radius;
        }

        /// <summary>
        /// Returns the result of a vector addition.
        /// </summary>
        /// <param name="origin">Origin.</param>
        /// <param name="target">Target.</param>
        public static Vector3 Add(Vector3 origin, Vector3 target)
        {
            return origin + target;
        }

        /// <summary>
        /// Returns the result of subtraction of a vector.
        /// </summary>
        /// <param name="origin">Origin.</param>
        /// <param name="target">Target.</param>
        public static Vector3 Subtraction(Vector3 origin, Vector3 target)
        {
            return origin - target;
        }

        /// <summary>
        /// Returns the angular difference between the two vectors.
        /// The returned value is 0 ~ 180.
        /// </summary>
        /// <returns>The angle.</returns>
        /// <param name="origin">Origin.</param>
        /// <param name="target">Target.</param>
        public static float GetAngle(Vector3 origin, Vector3 target)
        {
            return Vector3.Angle(origin, target);
        }

        /// <summary>
        /// Rotate the vector at a specified angle.
        /// </summary>
        /// <param name="origin">Origin.</param>
        /// <param name="xAngle">X angle is 0f ~ 360f</param>
        /// <param name="yAngle">Y angle is 0f ~ 360f</param>
        /// <param name="zAngle">Z angle is 0f ~ 360f</param>
        public static Vector3 Rotation(Vector3 origin, float xAngle, float yAngle, float zAngle)
        {
            return Quaternion.Euler(xAngle, yAngle, zAngle) * origin;
        }

        /// <summary>
        /// Rotate a vector toward a specified vector by a specified angle.
        /// </summary>
        /// <param name="origin">Origin.</param>
        /// <param name="target">Target.</param>
        /// <param name="angle">Angle.</param>
        public static Vector3 Rotation(Vector3 origin, Vector3 target, float angle)
        {
            // Specified angle rotation toward the specified vector
            var axis = Vector3.Cross(target, origin);
            var res = Quaternion.AngleAxis(angle, axis) * target;
            return res;
        }

        /// <summary>
        /// Convert Vector3 to Vector2.
        /// </summary>
        /// <returns>The vector2.</returns>
        /// <param name="origin">Origin.</param>
        public static Vector2 ToVectorXY(Vector3 origin)
        {
            return new Vector2(origin.x, origin.y);
        }

        /// <summary>
        /// Convert Vector3 to Vector2.
        /// </summary>
        /// <returns>The vector2.</returns>
        /// <param name="origin">Origin.</param>
        public static Vector2 ToVectorXZ(Vector3 origin)
        {
            return new Vector2(origin.x, origin.z);
        }

        /// <summary>
        /// Convert Vector3 to Vector2.
        /// </summary>
        /// <returns>The vector2.</returns>
        /// <param name="origin">Origin.</param>
        public static Vector2 ToVectorYZ(Vector3 origin)
        {
            return new Vector2(origin.y, origin.z);
        }

        /// <summary>
        /// Rotate the vector only ANGLE (giving positive numbers and tilting inward)
        /// ANGLE Frequency Method
        /// </summary>
        /// <returns>The vector.</returns>
        /// <param name="vector">Vector.</param>
        /// <param name="ANGLE">ANGLE.</param>
        public static Vector2 RotateVector(Vector2 origin, float angle)
        {
            float rad = angle.ToRadian();
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);

            float x = origin.x;
            float y = origin.y;

            return new Vector2(x * cos + y * sin, -x * sin + y * cos);
        }

        /// <summary>
        /// Returns if all the values of Vector3 are the same.
        /// </summary>
        /// <returns><c>true</c> if is uniform the specified self; otherwise, <c>false</c>.</returns>
        /// <param name="self">Self.</param>
        public static bool IsUniform(Vector3 self)
        {
            return Mathf.Approximately(self.x, self.y) && Mathf.Approximately(self.x, self.z);
        }

        public static Vector3 NearestPointOnAxis(Vector3 axisDirection, Vector3 point, bool isNormalized = false)
        {
            if (!isNormalized) axisDirection.Normalize();

            // Find the inner product.
            var d = Vector3.Dot(point, axisDirection);
            return axisDirection * d;
        }

        public static Vector3 NearestPointOnLine(
            this Vector3 lineDirection, Vector3 point, Vector3 pointOnLine, bool isNormalized = false
        )
        {
            if (!isNormalized) lineDirection.Normalize();

            // Find the inner product.
            var d = Vector3.Dot(point - pointOnLine, lineDirection);
            return pointOnLine + lineDirection * d;
        }

        /// <summary>
        /// Line Segment Crossing (2D Version).
        /// </summary>
        /// <param name="a">Line segment a-b a.</param>
        /// <param name="b">Line segment a-b b.</param>
        /// <param name="c">Line segment c-d c.</param>
        /// <param name="d">Line segment c-d d.</param>
        public static bool CrossLineCheck2D(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            float ta = (a.x - b.x) * (c.y - a.y) + (a.y - b.y) * (a.x - c.x);
            float tb = (a.x - b.x) * (d.y - a.y) + (a.y - b.y) * (a.x - d.x);
            float tc = (c.x - d.x) * (a.y - c.y) + (c.y - d.y) * (c.x - a.x);
            float td = (c.x - d.x) * (b.y - c.y) + (c.y - d.y) * (c.x - b.x);

            if (ta * tb < 0f && (tc * td) < 0f)
            {
                return true;
            }

            return false;
        }

        public static Vector2 RandomPointOnCircle(float radius)
        {
            return Random.insideUnitCircle.normalized * radius;
        }

        public static Vector3 RandomPointOnSphere(float radius)
        {
            return Random.onUnitSphere * radius;
        }
    }
}
