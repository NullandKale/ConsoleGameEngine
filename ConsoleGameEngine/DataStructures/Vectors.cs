using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine.DataStructures
{
    public struct Vec2
    {
        public float x;
        public float y;

        public Vec2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static explicit operator Vec2(Vec2i v)
        {
            return new Vec2(v.x, v.y);
        }

        public Vec2i toVec2i()
        {
            return new Vec2i((int)x, (int)y);
        }

        // Existing operators
        public static Vec2 operator *(Vec2 v1, Vec2 v2)
        {
            return new Vec2(v1.x * v2.x, v1.y * v2.y);
        }

        public static Vec2 operator *(Vec2 v1, float scalar)
        {
            return new Vec2(v1.x * scalar, v1.y * scalar);
        }

        public static Vec2 operator +(Vec2 v1, Vec2 v2)
        {
            return new Vec2(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vec2 operator -(Vec2 v1, Vec2 v2)
        {
            return new Vec2(v1.x - v2.x, v1.y - v2.y);
        }

        // New operator: Division by a scalar
        public static Vec2 operator /(Vec2 v1, float scalar)
        {
            return new Vec2(v1.x / scalar, v1.y / scalar);
        }

        // New operator: Negation
        public static Vec2 operator -(Vec2 v)
        {
            return new Vec2(-v.x, -v.y);
        }

        // Implement Length method to calculate the magnitude of the vector
        public float Length()
        {
            return (float)Math.Sqrt(x * x + y * y);
        }

        // Implement Normalized method to return a normalized version of the vector
        public Vec2 Normalized()
        {
            float length = Length();
            if (length == 0)
            {
                return new Vec2(0, 0);
            }
            return this / length;
        }

        public static float Dot(Vec2 v1, Vec2 v2)
        {
            return v1.x * v2.x + v1.y * v2.y;
        }

        public Vec2 Perpendicular()
        {
            // Rotate 90 degrees counterclockwise
            return new Vec2(-y, x);
        }
        public Vec2 PerpendicularClockwise()
        {
            // Rotate 90 degrees clockwise
            return new Vec2(y, -x);
        }

        public float LengthSquared()
        {
            return x * x + y * y;
        }
    }

    public struct Vec2i
    {
        public int x;
        public int y;

        public Vec2i(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Vec2i Translate(Vec2i other)
        {
            return new Vec2i(x + other.x, y + other.y);
        }

        public bool IsWithin(int width, int height) 
        {
            return x < width && y < height && x >= 0 && y >= 0;
        }

        public bool IsWithin(Vec2i other)
        {
            return x < other.x && y < other.y && x >= 0 && y >= 0;
        }

        public void Clamp(Vec2i size)
        {
            if(x < 0)
            {
                x = 0;
            }

            if (y < 0)
            {
                y = 0;
            }

            if (x > size.x)
            {
                x = size.x;
            }

            if (y > size.y)
            {
                y = size.y;
            }
        }

        public static Vec2i operator +(Vec2i v1, Vec2i v2)
        {
            return new Vec2i(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vec2i operator -(Vec2i v1, Vec2i v2)
        {
            return new Vec2i(v1.x - v2.x, v1.y - v2.y);
        }

        public static Vec2i operator -(Vec2i v1)
        {
            return new Vec2i(-v1.x, -v1.y);
        }

        public static explicit operator Vec2i(Vec2 v)
        {
            return new Vec2i((int)v.x, (int)v.y);
        }

        public static Vec2i operator /(Vec2i v1, int v2)
        {
            return new Vec2i(v1.x / v2, v1.y / v2);
        }

        public static bool operator ==(Vec2i v1, Vec2i v2)
        {
            return v1.Equals(v2);
        }
        public static bool operator !=(Vec2i v1, Vec2i v2)
        {
            return !v1.Equals(v2);
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            
            if(obj is Vec2i v)
            {
                return x.Equals(v.x) && y.Equals(v.y);
            }

            return false;
        }

        public override int GetHashCode()
        {
            // Extract the bottom 16 bits of both x and y
            int lower16BitsX = x & 0xFFFF;
            int lower16BitsY = y & 0xFFFF;

            // Shift the bits of x to the top 16 bits and combine with y using bitwise OR
            return (lower16BitsX << 16) | lower16BitsY;
        }

    }

    public struct Vec3 : IEquatable<Vec3>
    {
        public float x;
        public float y;
        public float z;


        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vec3(double x, double y, double z)
        {
            this.x = (float)x;
            this.y = (float)y;
            this.z = (float)z;
        }

        public override string ToString()
        {
            return "{" + string.Format("{0:0.00}", x) + ", " + string.Format("{0:0.00}", y) + ", " + string.Format("{0:0.00}", z) + "}";
        }


        public static Vec3 operator -(Vec3 vec)
        {
            return new Vec3(-vec.x, -vec.y, -vec.z);
        }

        public float length()
        {
            return MathF.Sqrt(x * x + y * y + z * z);
        }

        public float lengthSquared()
        {
            return x * x + y * y + z * z;
        }

        public float getAt(int a)
        {
            switch (a)
            {
                case 0:
                    return x;
                case 1:
                    return y;
                case 2:
                    return z;
                default:
                    return 0;
            }
        }

        public static Vec3 setX(Vec3 v, float x)
        {
            return new Vec3(x, v.y, v.z);
        }

        public static Vec3 setY(Vec3 v, float y)
        {
            return new Vec3(v.x, y, v.z);
        }

        public static Vec3 setZ(Vec3 v, float z)
        {
            return new Vec3(v.x, v.y, z);
        }

        public static float dist(Vec3 v1, Vec3 v2)
        {
            float dx = v1.x - v2.x;
            float dy = v1.y - v2.y;
            float dz = v1.z - v2.z;
            return MathF.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public static Vec3 operator +(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Vec3 operator -(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static Vec3 operator *(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        public static Vec3 operator /(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }

        public static Vec3 operator /(float v, Vec3 v1)
        {
            return new Vec3(v / v1.x, v / v1.y, v / v1.z);
        }

        public static Vec3 operator *(Vec3 v1, float v)
        {
            return new Vec3(v1.x * v, v1.y * v, v1.z * v);
        }

        public static Vec3 operator *(float v, Vec3 v1)
        {
            return new Vec3(v1.x * v, v1.y * v, v1.z * v);
        }

        public static Vec3 operator +(Vec3 v1, float v)
        {
            return new Vec3(v1.x + v, v1.y + v, v1.z + v);
        }

        public static Vec3 operator +(float v, Vec3 v1)
        {
            return new Vec3(v1.x + v, v1.y + v, v1.z + v);
        }

        public static Vec3 operator -(Vec3 v1, float v)
        {
            return new Vec3(v1.x - v, v1.y - v, v1.z - v);
        }

        public static Vec3 operator -(float v, Vec3 v1)
        {
            return new Vec3(v1.x - v, v1.y - v, v1.z - v);
        }

        public static Vec3 operator /(Vec3 v1, float v)
        {
            return v1 * (1.0f / v);
        }

        public static float dot(Vec3 v1, Vec3 v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }

        public static Vec3 cross(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.y * v2.z - v1.z * v2.y,
                          -(v1.x * v2.z - v1.z * v2.x),
                            v1.x * v2.y - v1.y * v2.x);
        }

        public static Vec3 unitVector(Vec3 v)
        {
            return v / MathF.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
        }

        public static Vec3 reflect(Vec3 normal, Vec3 incomming)
        {
            return unitVector(incomming - normal * 2f * dot(incomming, normal));
        }

        public static Vec3 refract(Vec3 v, Vec3 n, float niOverNt)
        {
            Vec3 uv = unitVector(v);
            float dt = dot(uv, n);
            float discriminant = 1.0f - niOverNt * niOverNt * (1f - dt * dt);

            if (discriminant > 0)
            {
                return niOverNt * (uv - (n * dt)) - n * MathF.Sqrt(discriminant);
            }

            return v;
        }

        public static float NormalReflectance(Vec3 normal, Vec3 incomming, float iorFrom, float iorTo)
        {
            float iorRatio = iorFrom / iorTo;
            float cosThetaI = -dot(normal, incomming);
            float sinThetaTSquared = iorRatio * iorRatio * (1 - cosThetaI * cosThetaI);
            if (sinThetaTSquared > 1)
            {
                return 1f;
            }

            float cosThetaT = MathF.Sqrt(1 - sinThetaTSquared);
            float rPerpendicular = (iorFrom * cosThetaI - iorTo * cosThetaT) / (iorFrom * cosThetaI + iorTo * cosThetaT);
            float rParallel = (iorFrom * cosThetaI - iorTo * cosThetaT) / (iorFrom * cosThetaI + iorTo * cosThetaT);
            return (rPerpendicular * rPerpendicular + rParallel * rParallel) / 2f;
        }
        public static Vec3 Clamp(Vec3 v, Vec3 min, Vec3 max)
        {
            return new Vec3(Math.Clamp(v.x, min.x, max.x), Math.Clamp(v.y, min.y, max.y), Math.Clamp(v.z, min.z, max.z));
        }

        public static Vec3 Abs(Vec3 v)
        {
            return new Vec3(Math.Abs(v.x), Math.Abs(v.y), Math.Abs(v.z));
        }

        public static Vec3 Mod(Vec3 v, float f)
        {
            return v - f * new Vec3(Math.Floor(v.x / f), Math.Floor(v.y / f), Math.Floor(v.z / f));
        }

        public static Vec3 Mix(Vec3 x, Vec3 y, float a)
        {
            return x * (1 - a) + y * a;
        }

        public static Vec3 aces_approx(Vec3 v)
        {
            v *= 0.6f;
            float a = 2.51f;
            float b = 0.03f;
            float c = 2.43f;
            float d = 0.59f;
            float e = 0.14f;
            Vec3 working = (v * (a * v + b)) / (v * (c * v + d) + e);
            return new Vec3(Math.Clamp(working.x, 0, 1), Math.Clamp(working.y, 0, 1), Math.Clamp(working.z, 0, 1));
        }

        public static Vec3 reinhard(Vec3 v)
        {
            return v / (1.0f + v);
        }

        public static Vec3 HsvToRgb(int h, float s, float v)
        {
            float hp = h / 60.0f;
            float c = s * v;
            float x = c * (1f - Math.Abs(hp % 2.0f - 1f));
            float m = v - c;
            float r = 0, g = 0, b = 0;
            if (hp <= 1)
            {
                r = c;
                g = x;
            }
            else if (hp <= 2)
            {
                r = x;
                g = c;
            }
            else if (hp <= 3)
            {
                g = c;
                b = x;
            }
            else if (hp <= 4)
            {
                g = x;
                b = c;
            }
            else if (hp <= 5)
            {
                r = x;
                b = c;
            }
            else
            {
                r = c;
                b = x;
            }
            r += m;
            g += m;
            b += m;
            return new Vec3(r, g, b);
        }

        public static bool Equals(Vec3 a, Vec3 b)
        {
            return a.x == b.x &&
                   a.y == b.y &&
                   a.z == b.z;
        }

        public static int CompareTo(Vec3 a, Vec3 b)
        {
            return a.lengthSquared().CompareTo(b.lengthSquared());
        }

        public override bool Equals(object? obj)
        {
            return obj is Vec3 vec && Equals(vec);
        }

        public bool Equals(Vec3 other)
        {
            return x == other.x &&
                   y == other.y &&
                   z == other.z;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z);
        }

        public static Vec3 Lerp(Vec3 color0, Vec3 color1, float v)
        {
            float x = color0.x + (color1.x - color0.x) * v;
            float y = color0.y + (color1.y - color0.y) * v;
            float z = color0.z + (color1.z - color0.z) * v;
            return new Vec3(x, y, z);
        }


        public static implicit operator Vector3(Vec3 d)
        {
            return new Vector3((float)d.x, (float)d.y, (float)d.z);
        }

        public static implicit operator Vec3(Vector3 d)
        {
            return new Vec3(d.X, d.Y, d.Z);
        }

        public static implicit operator Vector4(Vec3 d)
        {
            return new Vector4((float)d.x, (float)d.y, (float)d.z, 0);
        }

        public static implicit operator Vec3(Vector4 d)
        {
            return new Vec3(d.X, d.Y, d.Z);
        }

        public static implicit operator Color(Vec3 v)
        {
            return Color.FromArgb((int)(v.x * 255), (int)(v.y * 255), (int)(v.z * 255f));
        }

        public static bool operator ==(Vec3 left, Vec3 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vec3 left, Vec3 right)
        {
            return !(left == right);
        }

    }

    public struct Vec3i
    {
        public int x;
        public int y;
        public int z;

        public Vec3i(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vec3i(float x, float y, float z)
        {
            this.x = (int)x;
            this.y = (int)y;
            this.z = (int)z;
        }

        public override string ToString()
        {
            return "{" + string.Format("{0:0.00}", x) + ", " + string.Format("{0:0.00}", y) + ", " + string.Format("{0:0.00}", z) + "}";
        }


        public static Vec3i operator -(Vec3i vec)
        {
            return new Vec3i(-vec.x, -vec.y, -vec.z);
        }


        public float length()
        {
            return MathF.Sqrt(x * x + y * y + z * z);
        }


        public float lengthSquared()
        {
            return x * x + y * y + z * z;
        }

        public float getAt(int a)
        {
            switch (a)
            {
                case 0:
                    return x;
                case 1:
                    return y;
                case 2:
                    return z;
                default:
                    return 0;
            }
        }

        public static Vec3i setX(Vec3i v, int x)
        {
            return new Vec3i(x, v.y, v.z);
        }

        public static Vec3i setY(Vec3i v, int y)
        {
            return new Vec3i(v.x, y, v.z);
        }

        public static Vec3i setZ(Vec3i v, int z)
        {
            return new Vec3i(v.x, v.y, z);
        }


        public static float dist(Vec3i v1, Vec3i v2)
        {
            float dx = v1.x - v2.x;
            float dy = v1.y - v2.y;
            float dz = v1.z - v2.z;
            return MathF.Sqrt(dx * dx + dy * dy + dz * dz);
        }


        public static Vec3i operator +(Vec3i v1, Vec3i v2)
        {
            return new Vec3i(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }


        public static Vec3i operator -(Vec3i v1, Vec3i v2)
        {
            return new Vec3i(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }


        public static Vec3i operator *(Vec3i v1, Vec3i v2)
        {
            return new Vec3i(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }


        public static Vec3i operator /(Vec3i v1, Vec3i v2)
        {
            return new Vec3i(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }


        public static Vec3i operator /(int v, Vec3i v1)
        {
            return new Vec3i(v / v1.x, v / v1.y, v / v1.z);
        }


        public static Vec3i operator *(Vec3i v1, int v)
        {
            return new Vec3i(v1.x * v, v1.y * v, v1.z * v);
        }


        public static Vec3i operator *(int v, Vec3i v1)
        {
            return new Vec3i(v1.x * v, v1.y * v, v1.z * v);
        }


        public static Vec3i operator +(Vec3i v1, int v)
        {
            return new Vec3i(v1.x + v, v1.y + v, v1.z + v);
        }


        public static Vec3i operator +(int v, Vec3i v1)
        {
            return new Vec3i(v1.x + v, v1.y + v, v1.z + v);
        }


        public static Vec3i operator /(Vec3i v1, int v)
        {
            return v1 * (1 / v);
        }


        public static float dot(Vec3i v1, Vec3i v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }


        public static Vec3i cross(Vec3i v1, Vec3i v2)
        {
            return new Vec3i(v1.y * v2.z - v1.z * v2.y,
                          -(v1.x * v2.z - v1.z * v2.x),
                            v1.x * v2.y - v1.y * v2.x);
        }


        public static bool Equals(Vec3i a, Vec3i b)
        {
            return a.x == b.x &&
                   a.y == b.y &&
                   a.z == b.z;
        }
    }
}
