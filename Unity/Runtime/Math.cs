using UnityEngine;

namespace Utils.Unity.Runtime
{
    public static class Math
    {
        public const float Tolerance = 0.1f;
        public static Vector3 ToV3(this Vector2 @this, float z = 0) => new Vector3(@this.x, @this.y, z);

        public static Vector3 ToV3(this Vector2Int @this, float z = 0) => @this
            .ToV2().ToV3();

        public static Vector3Int ToV3I(this Vector3 @this) =>
            new Vector3Int((int)@this.x, (int)@this.y, (int)@this.z);

        public static Vector3Int SetY(this Vector3Int @this, int y)
        {
            @this.y = y;
            return @this;
        }

        public static Vector3Int SetX(this Vector3Int @this, int x)
        {
            @this.x = x;
            return @this;
        }

        public static Vector3Int SetZ(this Vector3Int @this, int z)
        {
            @this.z = z;
            return @this;
        }

        public static Vector3 SetX(this Vector3 @this, float x)
        {
            @this.x = x;
            return @this;
        }

        public static Vector3 SetY(this Vector3 @this, float y)
        {
            @this.y = y;
            return @this;
        }

        public static Vector3 SetZ(this Vector3 @this, float z)
        {
            @this.z = z;
            return @this;
        }

        public static Vector2 ToV2(this Vector3 @this) => new
            Vector2(@this.x, @this.y);

        public static Vector2 ToV2(this Vector2Int @this) => new
            Vector2(@this.x, @this.y);

        public static Vector2Int ToV2I(this Vector3 @this) =>
            ToV2I((Vector2)@this);

        public static Vector2Int ToV2I(this Vector2 @this) => new
            Vector2Int((int)@this.x, (int)@this.y);

        public static Vector2Int MoveTowards(
                this Vector2Int @this,
                Vector2Int other,
                float a)
        {
            var m = Vector2.MoveTowards(@this, other, a);
            return new Vector2Int((int)m.x, (int)m.y);
        }

        public static Vector2 MoveTowards(
                this Vector2 @this,
                Vector2 other,
                float a)
        {
            return Vector2.MoveTowards(@this, other, a);
        }

        public static float Remap(
                float value,
                float oldMin,
                float oldMax,
                float newMin,
                float newMax)
        {
            return (value - oldMin) / (oldMax - oldMin) *
                (newMax - newMin) + newMin;
        }

        public static Vector2 RadianToVector2(float radian) => new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

        public static Vector2 DgreeToVector2(float degree) => RadianToVector2(degree * Mathf.Deg2Rad);

        public static float AngleFloat(this Vector2 @this) => Mathf.Atan2(@this.y, @this.x);

        public static bool IsInRange(float value, float low, float high) => value >= low && value < high;

        public static int Octant(float value) => (int)Mathf.Round(8 * value / (2 * Mathf.PI) + 8) % 8;

        public static bool ContainsInRotatedRect(Rect rect, float rectAngle, Vector2 point)
        {
            // rotate around rectangle center by -rectAngle
            var sin = Mathf.Sin(-rectAngle);
            var cos = Mathf.Cos(-rectAngle);

            // set newPoint to rect center
            var newPoint = point - rect.center;
            // rotate
            newPoint = new Vector2(newPoint.x * cos - newPoint.y * sin, newPoint.x * sin + newPoint.y * cos);
            // put newPoint back
            newPoint += rect.center;

            // ceck if our transformed point is in the rectangle, which is no longer rotated relative to the point
            return newPoint.x >= rect.xMin && newPoint.x
                <= rect.xMax && newPoint.y >= rect.yMin && newPoint.y <= rect.yMax;
        }
    }
}
