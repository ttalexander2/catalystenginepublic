using System;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Xna.Framework;



//Taken from: https://github.com/FNA-XNA/FNA/tree/master/src

namespace Chroma.Engine.Physics
{
    /// <summary>
	/// Describes a 2D-vector.
	/// </summary>
	[Serializable]
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    public struct Vec2 : IEquatable<Vec2>
    {
        #region Public Static Properties

        /// <summary>
        /// Returns a <see cref="Vec2"/> with components 0, 0.
        /// </summary>
        public static Vec2 Zero
        {
            get
            {
                return zeroVector;
            }
        }

        /// <summary>
        /// Returns a <see cref="Vec2"/> with components 1, 1.
        /// </summary>
        public static Vec2 One
        {
            get
            {
                return unitVector;
            }
        }

        /// <summary>
        /// Returns a <see cref="Vec2"/> with components 1, 0.
        /// </summary>
        public static Vec2 UnitX
        {
            get
            {
                return unitXVector;
            }
        }

        /// <summary>
        /// Returns a <see cref="Vec2"/> with components 0, 1.
        /// </summary>
        public static Vec2 UnitY
        {
            get
            {
                return unitYVector;
            }
        }

        #endregion

        #region Internal Properties

        internal string DebugDisplayString
        {
            get
            {
                return string.Concat(
                    X.ToString(), " ",
                    Y.ToString()
                );
            }
        }

        #endregion

        #region Public Fields

        /// <summary>
        /// The x coordinate of this <see cref="Vec2"/>.
        /// </summary>
        public float X;

        /// <summary>
        /// The y coordinate of this <see cref="Vec2"/>.
        /// </summary>
        public float Y;

        #endregion

        #region Private Static Fields

        private static readonly Vec2 zeroVector = new Vec2(0f, 0f);
        private static readonly Vec2 unitVector = new Vec2(1f, 1f);
        private static readonly Vec2 unitXVector = new Vec2(1f, 0f);
        private static readonly Vec2 unitYVector = new Vec2(0f, 1f);

        #endregion

        #region Public Constructors

        /// <summary>
        /// Constructs a 2d vector with X and Y from two values.
        /// </summary>
        /// <param name="x">The x coordinate in 2d-space.</param>
        /// <param name="y">The y coordinate in 2d-space.</param>
        public Vec2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Constructs a 2d vector with X and Y set to the same value.
        /// </summary>
        /// <param name="value">The x and y coordinates in 2d-space.</param>
        public Vec2(float value)
        {
            this.X = value;
            this.Y = value;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object obj)
        {
            return (obj is Vec2) && Equals((Vec2)obj);
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Vec2"/>.
        /// </summary>
        /// <param name="other">The <see cref="Vec2"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(Vec2 other)
        {
            return (X == other.X &&
                    Y == other.Y);
        }

        /// <summary>
        /// Gets the hash code of this <see cref="Vec2"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="Vec2"/>.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }

        /// <summary>
        /// Returns the length of this <see cref="Vec2"/>.
        /// </summary>
        /// <returns>The length of this <see cref="Vec2"/>.</returns>
        public float Length()
        {
            return (float)Math.Sqrt((X * X) + (Y * Y));
        }

        /// <summary>
        /// Returns the squared length of this <see cref="Vec2"/>.
        /// </summary>
        /// <returns>The squared length of this <see cref="Vec2"/>.</returns>
        public float LengthSquared()
        {
            return (X * X) + (Y * Y);
        }

        /// <summary>
        /// Turns this <see cref="Vec2"/> to a unit vector with the same direction.
        /// </summary>
        public void Normalize()
        {
            float val = 1.0f / (float)Math.Sqrt((X * X) + (Y * Y));
            X *= val;
            Y *= val;
        }

        /// <summary>
        /// Returns a <see cref="String"/> representation of this <see cref="Vec2"/> in the format:
        /// {X:[<see cref="X"/>] Y:[<see cref="Y"/>]}
        /// </summary>
        /// <returns>A <see cref="String"/> representation of this <see cref="Vec2"/>.</returns>
        public override string ToString()
        {
            return (
                "{X:" + X.ToString() +
                " Y:" + Y.ToString() +
                "}"
            );
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Performs vector addition on <paramref name="value1"/> and <paramref name="value2"/>.
        /// </summary>
        /// <param name="value1">The first vector to add.</param>
        /// <param name="value2">The second vector to add.</param>
        /// <returns>The result of the vector addition.</returns>
        public static Vec2 Add(Vec2 value1, Vec2 value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }

        /// <summary>
        /// Performs vector addition on <paramref name="value1"/> and
        /// <paramref name="value2"/>, storing the result of the
        /// addition in <paramref name="result"/>.
        /// </summary>
        /// <param name="value1">The first vector to add.</param>
        /// <param name="value2">The second vector to add.</param>
        /// <param name="result">The result of the vector addition.</param>
        public static void Add(ref Vec2 value1, ref Vec2 value2, out Vec2 result)
        {
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains the cartesian coordinates of a vector specified in barycentric coordinates and relative to 2d-triangle.
        /// </summary>
        /// <param name="value1">The first vector of 2d-triangle.</param>
        /// <param name="value2">The second vector of 2d-triangle.</param>
        /// <param name="value3">The third vector of 2d-triangle.</param>
        /// <param name="amount1">Barycentric scalar <c>b2</c> which represents a weighting factor towards second vector of 2d-triangle.</param>
        /// <param name="amount2">Barycentric scalar <c>b3</c> which represents a weighting factor towards third vector of 2d-triangle.</param>
        /// <returns>The cartesian translation of barycentric coordinates.</returns>
        public static Vec2 Barycentric(
            Vec2 value1,
            Vec2 value2,
            Vec2 value3,
            float amount1,
            float amount2
        )
        {
            return new Vec2(
                MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2),
                MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains the cartesian coordinates of a vector specified in barycentric coordinates and relative to 2d-triangle.
        /// </summary>
        /// <param name="value1">The first vector of 2d-triangle.</param>
        /// <param name="value2">The second vector of 2d-triangle.</param>
        /// <param name="value3">The third vector of 2d-triangle.</param>
        /// <param name="amount1">Barycentric scalar <c>b2</c> which represents a weighting factor towards second vector of 2d-triangle.</param>
        /// <param name="amount2">Barycentric scalar <c>b3</c> which represents a weighting factor towards third vector of 2d-triangle.</param>
        /// <param name="result">The cartesian translation of barycentric coordinates as an output parameter.</param>
        public static void Barycentric(
            ref Vec2 value1,
            ref Vec2 value2,
            ref Vec2 value3,
            float amount1,
            float amount2,
            out Vec2 result
        )
        {
            result.X = MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2);
            result.Y = MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2);
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains CatmullRom interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector in interpolation.</param>
        /// <param name="value2">The second vector in interpolation.</param>
        /// <param name="value3">The third vector in interpolation.</param>
        /// <param name="value4">The fourth vector in interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>The result of CatmullRom interpolation.</returns>
        public static Vec2 CatmullRom(
            Vec2 value1,
            Vec2 value2,
            Vec2 value3,
            Vec2 value4,
            float amount
        )
        {
            return new Vec2(
                MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount),
                MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains CatmullRom interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector in interpolation.</param>
        /// <param name="value2">The second vector in interpolation.</param>
        /// <param name="value3">The third vector in interpolation.</param>
        /// <param name="value4">The fourth vector in interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <param name="result">The result of CatmullRom interpolation as an output parameter.</param>
        public static void CatmullRom(
            ref Vec2 value1,
            ref Vec2 value2,
            ref Vec2 value3,
            ref Vec2 value4,
            float amount,
            out Vec2 result
        )
        {
            result.X = MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount);
            result.Y = MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount);
        }

        /// <summary>
        /// Clamps the specified value within a range.
        /// </summary>
        /// <param name="value1">The value to clamp.</param>
        /// <param name="min">The min value.</param>
        /// <param name="max">The max value.</param>
        /// <returns>The clamped value.</returns>
        public static Vec2 Clamp(Vec2 value1, Vec2 min, Vec2 max)
        {
            return new Vec2(
                MathHelper.Clamp(value1.X, min.X, max.X),
                MathHelper.Clamp(value1.Y, min.Y, max.Y)
            );
        }

        /// <summary>
        /// Clamps the specified value within a range.
        /// </summary>
        /// <param name="value1">The value to clamp.</param>
        /// <param name="min">The min value.</param>
        /// <param name="max">The max value.</param>
        /// <param name="result">The clamped value as an output parameter.</param>
        public static void Clamp(
            ref Vec2 value1,
            ref Vec2 min,
            ref Vec2 max,
            out Vec2 result
        )
        {
            result.X = MathHelper.Clamp(value1.X, min.X, max.X);
            result.Y = MathHelper.Clamp(value1.Y, min.Y, max.Y);
        }

        /// <summary>
        /// Returns the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The distance between two vectors.</returns>
        public static float Distance(Vec2 value1, Vec2 value2)
        {
            float v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            return (float)Math.Sqrt((v1 * v1) + (v2 * v2));
        }

        /// <summary>
        /// Returns the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The distance between two vectors as an output parameter.</param>
        public static void Distance(ref Vec2 value1, ref Vec2 value2, out float result)
        {
            float v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            result = (float)Math.Sqrt((v1 * v1) + (v2 * v2));
        }

        /// <summary>
        /// Returns the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The squared distance between two vectors.</returns>
        public static float DistanceSquared(Vec2 value1, Vec2 value2)
        {
            float v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            return (v1 * v1) + (v2 * v2);
        }

        /// <summary>
        /// Returns the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The squared distance between two vectors as an output parameter.</param>
        public static void DistanceSquared(
            ref Vec2 value1,
            ref Vec2 value2,
            out float result
        )
        {
            float v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            result = (v1 * v1) + (v2 * v2);
        }

        /// <summary>
        /// Divides the components of a <see cref="Vec2"/> by the components of another <see cref="Vec2"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2"/>.</param>
        /// <param name="value2">Divisor <see cref="Vec2"/>.</param>
        /// <returns>The result of dividing the vectors.</returns>
        public static Vec2 Divide(Vec2 value1, Vec2 value2)
        {
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            return value1;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vec2"/> by the components of another <see cref="Vec2"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2"/>.</param>
        /// <param name="value2">Divisor <see cref="Vec2"/>.</param>
        /// <param name="result">The result of dividing the vectors as an output parameter.</param>
        public static void Divide(ref Vec2 value1, ref Vec2 value2, out Vec2 result)
        {
            result.X = value1.X / value2.X;
            result.Y = value1.Y / value2.Y;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vec2"/> by a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2"/>.</param>
        /// <param name="divider">Divisor scalar.</param>
        /// <returns>The result of dividing a vector by a scalar.</returns>
        public static Vec2 Divide(Vec2 value1, float divider)
        {
            float factor = 1 / divider;
            value1.X *= factor;
            value1.Y *= factor;
            return value1;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vec2"/> by a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2"/>.</param>
        /// <param name="divider">Divisor scalar.</param>
        /// <param name="result">The result of dividing a vector by a scalar as an output parameter.</param>
        public static void Divide(ref Vec2 value1, float divider, out Vec2 result)
        {
            float factor = 1 / divider;
            result.X = value1.X * factor;
            result.Y = value1.Y * factor;
        }

        /// <summary>
        /// Returns a dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The dot product of two vectors.</returns>
        public static float Dot(Vec2 value1, Vec2 value2)
        {
            return (value1.X * value2.X) + (value1.Y * value2.Y);
        }

        /// <summary>
        /// Returns a dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The dot product of two vectors as an output parameter.</param>
        public static void Dot(ref Vec2 value1, ref Vec2 value2, out float result)
        {
            result = (value1.X * value2.X) + (value1.Y * value2.Y);
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains hermite spline interpolation.
        /// </summary>
        /// <param name="value1">The first position vector.</param>
        /// <param name="tangent1">The first tangent vector.</param>
        /// <param name="value2">The second position vector.</param>
        /// <param name="tangent2">The second tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>The hermite spline interpolation vector.</returns>
        public static Vec2 Hermite(
            Vec2 value1,
            Vec2 tangent1,
            Vec2 value2,
            Vec2 tangent2,
            float amount
        )
        {
            Vec2 result = new Vec2();
            Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains hermite spline interpolation.
        /// </summary>
        /// <param name="value1">The first position vector.</param>
        /// <param name="tangent1">The first tangent vector.</param>
        /// <param name="value2">The second position vector.</param>
        /// <param name="tangent2">The second tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <param name="result">The hermite spline interpolation vector as an output parameter.</param>
        public static void Hermite(
            ref Vec2 value1,
            ref Vec2 tangent1,
            ref Vec2 value2,
            ref Vec2 tangent2,
            float amount,
            out Vec2 result
        )
        {
            result.X = MathHelper.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount);
            result.Y = MathHelper.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount);
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains linear interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <returns>The result of linear interpolation of the specified vectors.</returns>
        public static Vec2 Lerp(Vec2 value1, Vec2 value2, float amount)
        {
            return new Vec2(
                MathHelper.Lerp(value1.X, value2.X, amount),
                MathHelper.Lerp(value1.Y, value2.Y, amount)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains linear interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <param name="result">The result of linear interpolation of the specified vectors as an output parameter.</param>
        public static void Lerp(
            ref Vec2 value1,
            ref Vec2 value2,
            float amount,
            out Vec2 result
        )
        {
            result.X = MathHelper.Lerp(value1.X, value2.X, amount);
            result.Y = MathHelper.Lerp(value1.Y, value2.Y, amount);
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains a maximal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The <see cref="Vec2"/> with maximal values from the two vectors.</returns>
        public static Vec2 Max(Vec2 value1, Vec2 value2)
        {
            return new Vec2(
                value1.X > value2.X ? value1.X : value2.X,
                value1.Y > value2.Y ? value1.Y : value2.Y
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains a maximal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The <see cref="Vec2"/> with maximal values from the two vectors as an output parameter.</param>
        public static void Max(ref Vec2 value1, ref Vec2 value2, out Vec2 result)
        {
            result.X = value1.X > value2.X ? value1.X : value2.X;
            result.Y = value1.Y > value2.Y ? value1.Y : value2.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains a minimal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The <see cref="Vec2"/> with minimal values from the two vectors.</returns>
        public static Vec2 Min(Vec2 value1, Vec2 value2)
        {
            return new Vec2(
                value1.X < value2.X ? value1.X : value2.X,
                value1.Y < value2.Y ? value1.Y : value2.Y
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains a minimal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The <see cref="Vec2"/> with minimal values from the two vectors as an output parameter.</param>
        public static void Min(ref Vec2 value1, ref Vec2 value2, out Vec2 result)
        {
            result.X = value1.X < value2.X ? value1.X : value2.X;
            result.Y = value1.Y < value2.Y ? value1.Y : value2.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains a multiplication of two vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2"/>.</param>
        /// <param name="value2">Source <see cref="Vec2"/>.</param>
        /// <returns>The result of the vector multiplication.</returns>
        public static Vec2 Multiply(Vec2 value1, Vec2 value2)
        {
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            return value1;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains a multiplication of <see cref="Vec2"/> and a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2"/>.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <returns>The result of the vector multiplication with a scalar.</returns>
        public static Vec2 Multiply(Vec2 value1, float scaleFactor)
        {
            value1.X *= scaleFactor;
            value1.Y *= scaleFactor;
            return value1;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains a multiplication of <see cref="Vec2"/> and a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2"/>.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <param name="result">The result of the multiplication with a scalar as an output parameter.</param>
        public static void Multiply(ref Vec2 value1, float scaleFactor, out Vec2 result)
        {
            result.X = value1.X * scaleFactor;
            result.Y = value1.Y * scaleFactor;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains a multiplication of two vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2"/>.</param>
        /// <param name="value2">Source <see cref="Vec2"/>.</param>
        /// <param name="result">The result of the vector multiplication as an output parameter.</param>
        public static void Multiply(ref Vec2 value1, ref Vec2 value2, out Vec2 result)
        {
            result.X = value1.X * value2.X;
            result.Y = value1.Y * value2.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains the specified vector inversion.
        /// direction of <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Source <see cref="Vec2"/>.</param>
        /// <returns>The result of the vector inversion.</returns>
        public static Vec2 Negate(Vec2 value)
        {
            value.X = -value.X;
            value.Y = -value.Y;
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains the specified vector inversion.
        /// direction of <paramref name="value"/> in <paramref name="result"/>.
        /// </summary>
        /// <param name="value">Source <see cref="Vec2"/>.</param>
        /// <param name="result">The result of the vector inversion as an output parameter.</param>
        public static void Negate(ref Vec2 value, out Vec2 result)
        {
            result.X = -value.X;
            result.Y = -value.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains a normalized values from another vector.
        /// </summary>
        /// <param name="value">Source <see cref="Vec2"/>.</param>
        /// <returns>Unit vector.</returns>
        public static Vec2 Normalize(Vec2 value)
        {
            float val = 1.0f / (float)Math.Sqrt((value.X * value.X) + (value.Y * value.Y));
            value.X *= val;
            value.Y *= val;
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains a normalized values from another vector.
        /// </summary>
        /// <param name="value">Source <see cref="Vec2"/>.</param>
        /// <param name="result">Unit vector as an output parameter.</param>
        public static void Normalize(ref Vec2 value, out Vec2 result)
        {
            float val = 1.0f / (float)Math.Sqrt((value.X * value.X) + (value.Y * value.Y));
            result.X = value.X * val;
            result.Y = value.Y * val;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains reflect vector of the given vector and normal.
        /// </summary>
        /// <param name="vector">Source <see cref="Vec2"/>.</param>
        /// <param name="normal">Reflection normal.</param>
        /// <returns>Reflected vector.</returns>
        public static Vec2 Reflect(Vec2 vector, Vec2 normal)
        {
            Vec2 result;
            float val = 2.0f * ((vector.X * normal.X) + (vector.Y * normal.Y));
            result.X = vector.X - (normal.X * val);
            result.Y = vector.Y - (normal.Y * val);
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains reflect vector of the given vector and normal.
        /// </summary>
        /// <param name="vector">Source <see cref="Vec2"/>.</param>
        /// <param name="normal">Reflection normal.</param>
        /// <param name="result">Reflected vector as an output parameter.</param>
        public static void Reflect(ref Vec2 vector, ref Vec2 normal, out Vec2 result)
        {
            float val = 2.0f * ((vector.X * normal.X) + (vector.Y * normal.Y));
            result.X = vector.X - (normal.X * val);
            result.Y = vector.Y - (normal.Y * val);
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains cubic interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2"/>.</param>
        /// <param name="value2">Source <see cref="Vec2"/>.</param>
        /// <param name="amount">Weighting value.</param>
        /// <returns>Cubic interpolation of the specified vectors.</returns>
        public static Vec2 SmoothStep(Vec2 value1, Vec2 value2, float amount)
        {
            return new Vec2(
                MathHelper.SmoothStep(value1.X, value2.X, amount),
                MathHelper.SmoothStep(value1.Y, value2.Y, amount)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains cubic interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2"/>.</param>
        /// <param name="value2">Source <see cref="Vec2"/>.</param>
        /// <param name="amount">Weighting value.</param>
        /// <param name="result">Cubic interpolation of the specified vectors as an output parameter.</param>
        public static void SmoothStep(
            ref Vec2 value1,
            ref Vec2 value2,
            float amount,
            out Vec2 result
        )
        {
            result.X = MathHelper.SmoothStep(value1.X, value2.X, amount);
            result.Y = MathHelper.SmoothStep(value1.Y, value2.Y, amount);
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains subtraction of on <see cref="Vec2"/> from a another.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2"/>.</param>
        /// <param name="value2">Source <see cref="Vec2"/>.</param>
        /// <returns>The result of the vector subtraction.</returns>
        public static Vec2 Subtract(Vec2 value1, Vec2 value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains subtraction of on <see cref="Vec2"/> from a another.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2"/>.</param>
        /// <param name="value2">Source <see cref="Vec2"/>.</param>
        /// <param name="result">The result of the vector subtraction as an output parameter.</param>
        public static void Subtract(ref Vec2 value1, ref Vec2 value2, out Vec2 result)
        {
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains a transformation of 2d-vector by the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="position">Source <see cref="Vec2"/>.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <returns>Transformed <see cref="Vec2"/>.</returns>
        public static Vec2 Transform(Vec2 position, Matrix matrix)
        {
            return new Vec2(
                (position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41,
                (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains a transformation of 2d-vector by the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="position">Source <see cref="Vec2"/>.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="result">Transformed <see cref="Vec2"/> as an output parameter.</param>
        public static void Transform(
            ref Vec2 position,
            ref Matrix matrix,
            out Vec2 result
        )
        {
            float x = (position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41;
            float y = (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42;
            result.X = x;
            result.Y = y;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains a transformation of 2d-vector by the specified <see cref="Quaternion"/>, representing the rotation.
        /// </summary>
        /// <param name="value">Source <see cref="Vec2"/>.</param>
        /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
        /// <returns>Transformed <see cref="Vec2"/>.</returns>
        public static Vec2 Transform(Vec2 value, Quaternion rotation)
        {
            Transform(ref value, ref rotation, out value);
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains a transformation of 2d-vector by the specified <see cref="Quaternion"/>, representing the rotation.
        /// </summary>
        /// <param name="value">Source <see cref="Vec2"/>.</param>
        /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
        /// <param name="result">Transformed <see cref="Vec2"/> as an output parameter.</param>
        public static void Transform(
            ref Vec2 value,
            ref Quaternion rotation,
            out Vec2 result
        )
        {
            float x = 2 * -(rotation.Z * value.Y);
            float y = 2 * (rotation.Z * value.X);
            float z = 2 * (rotation.X * value.Y - rotation.Y * value.X);

            result.X = value.X + x * rotation.W + (rotation.Y * z - rotation.Z * y);
            result.Y = value.Y + y * rotation.W + (rotation.Z * x - rotation.X * z);
        }

        /// <summary>
        /// Apply transformation on all vectors within array of <see cref="Vec2"/> by the specified <see cref="Matrix"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        public static void Transform(
            Vec2[] sourceArray,
            ref Matrix matrix,
            Vec2[] destinationArray
        )
        {
            Transform(sourceArray, 0, ref matrix, destinationArray, 0, sourceArray.Length);
        }

        /// <summary>
        /// Apply transformation on vectors within array of <see cref="Vec2"/> by the specified <see cref="Matrix"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vec2"/> should be written.</param>
        /// <param name="length">The number of vectors to be transformed.</param>
        public static void Transform(
            Vec2[] sourceArray,
            int sourceIndex,
            ref Matrix matrix,
            Vec2[] destinationArray,
            int destinationIndex,
            int length
        )
        {
            for (int x = 0; x < length; x += 1)
            {
                Vec2 position = sourceArray[sourceIndex + x];
                Vec2 destination = destinationArray[destinationIndex + x];
                destination.X = (position.X * matrix.M11) + (position.Y * matrix.M21)
                        + matrix.M41;
                destination.Y = (position.X * matrix.M12) + (position.Y * matrix.M22)
                        + matrix.M42;
                destinationArray[destinationIndex + x] = destination;
            }
        }

        /// <summary>
        /// Apply transformation on all vectors within array of <see cref="Vec2"/> by the specified <see cref="Quaternion"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
        /// <param name="destinationArray">Destination array.</param>
        public static void Transform(
            Vec2[] sourceArray,
            ref Quaternion rotation,
            Vec2[] destinationArray
        )
        {
            Transform(
                sourceArray,
                0,
                ref rotation,
                destinationArray,
                0,
                sourceArray.Length
            );
        }

        /// <summary>
        /// Apply transformation on vectors within array of <see cref="Vec2"/> by the specified <see cref="Quaternion"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
        /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
        /// <param name="destinationArray">Destination array.</param>
        /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vec2"/> should be written.</param>
        /// <param name="length">The number of vectors to be transformed.</param>
        public static void Transform(
            Vec2[] sourceArray,
            int sourceIndex,
            ref Quaternion rotation,
            Vec2[] destinationArray,
            int destinationIndex,
            int length
        )
        {
            for (int i = 0; i < length; i += 1)
            {
                Vec2 position = sourceArray[sourceIndex + i];
                Vec2 v;
                Transform(ref position, ref rotation, out v);
                destinationArray[destinationIndex + i] = v;
            }
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains a transformation of the specified normal by the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="normal">Source <see cref="Vec2"/> which represents a normal vector.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <returns>Transformed normal.</returns>
        public static Vec2 TransformNormal(Vec2 normal, Matrix matrix)
        {
            return new Vec2(
                (normal.X * matrix.M11) + (normal.Y * matrix.M21),
                (normal.X * matrix.M12) + (normal.Y * matrix.M22)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vec2"/> that contains a transformation of the specified normal by the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="normal">Source <see cref="Vec2"/> which represents a normal vector.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="result">Transformed normal as an output parameter.</param>
        public static void TransformNormal(
            ref Vec2 normal,
            ref Matrix matrix,
            out Vec2 result
        )
        {
            float x = (normal.X * matrix.M11) + (normal.Y * matrix.M21);
            float y = (normal.X * matrix.M12) + (normal.Y * matrix.M22);
            result.X = x;
            result.Y = y;
        }

        /// <summary>
        /// Apply transformation on all normals within array of <see cref="Vec2"/> by the specified <see cref="Matrix"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        public static void TransformNormal(
            Vec2[] sourceArray,
            ref Matrix matrix,
            Vec2[] destinationArray
        )
        {
            TransformNormal(
                sourceArray,
                0,
                ref matrix,
                destinationArray,
                0,
                sourceArray.Length
            );
        }

        /// <summary>
        /// Apply transformation on normals within array of <see cref="Vec2"/> by the specified <see cref="Matrix"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vec2"/> should be written.</param>
        /// <param name="length">The number of normals to be transformed.</param>
        public static void TransformNormal(
            Vec2[] sourceArray,
            int sourceIndex,
            ref Matrix matrix,
            Vec2[] destinationArray,
            int destinationIndex,
            int length
        )
        {
            for (int i = 0; i < length; i += 1)
            {
                Vec2 position = sourceArray[sourceIndex + i];
                Vec2 result;
                result.X = (position.X * matrix.M11) + (position.Y * matrix.M21);
                result.Y = (position.X * matrix.M12) + (position.Y * matrix.M22);
                destinationArray[destinationIndex + i] = result;
            }
        }

        #endregion

        #region Public Static Operators

        /// <summary>
        /// Inverts values in the specified <see cref="Vec2"/>.
        /// </summary>
        /// <param name="value">Source <see cref="Vec2"/> on the right of the sub sign.</param>
        /// <returns>Result of the inversion.</returns>
        public static Vec2 operator -(Vec2 value)
        {
            value.X = -value.X;
            value.Y = -value.Y;
            return value;
        }

        /// <summary>
        /// Compares whether two <see cref="Vec2"/> instances are equal.
        /// </summary>
        /// <param name="value1"><see cref="Vec2"/> instance on the left of the equal sign.</param>
        /// <param name="value2"><see cref="Vec2"/> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(Vec2 value1, Vec2 value2)
        {
            return (value1.X == value2.X &&
                    value1.Y == value2.Y);
        }

        /// <summary>
        /// Compares whether two <see cref="Vec2"/> instances are equal.
        /// </summary>
        /// <param name="value1"><see cref="Vec2"/> instance on the left of the equal sign.</param>
        /// <param name="value2"><see cref="Vec2"/> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator !=(Vec2 value1, Vec2 value2)
        {
            return !(value1 == value2);
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2"/> on the left of the add sign.</param>
        /// <param name="value2">Source <see cref="Vec2"/> on the right of the add sign.</param>
        /// <returns>Sum of the vectors.</returns>
        public static Vec2 operator +(Vec2 value1, Vec2 value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }

        /// <summary>
        /// Subtracts a <see cref="Vec2"/> from a <see cref="Vec2"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2"/> on the left of the sub sign.</param>
        /// <param name="value2">Source <see cref="Vec2"/> on the right of the sub sign.</param>
        /// <returns>Result of the vector subtraction.</returns>
        public static Vec2 operator -(Vec2 value1, Vec2 value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
        }

        /// <summary>
        /// Multiplies the components of two vectors by each other.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2"/> on the left of the mul sign.</param>
        /// <param name="value2">Source <see cref="Vec2"/> on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication.</returns>
        public static Vec2 operator *(Vec2 value1, Vec2 value2)
        {
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            return value1;
        }

        /// <summary>
        /// Multiplies the components of vector by a scalar.
        /// </summary>
        /// <param name="value">Source <see cref="Vec2"/> on the left of the mul sign.</param>
        /// <param name="scaleFactor">Scalar value on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication with a scalar.</returns>
        public static Vec2 operator *(Vec2 value, float scaleFactor)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            return value;
        }

        /// <summary>
        /// Multiplies the components of vector by a scalar.
        /// </summary>
        /// <param name="scaleFactor">Scalar value on the left of the mul sign.</param>
        /// <param name="value">Source <see cref="Vec2"/> on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication with a scalar.</returns>
        public static Vec2 operator *(float scaleFactor, Vec2 value)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            return value;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vec2"/> by the components of another <see cref="Vec2"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2"/> on the left of the div sign.</param>
        /// <param name="value2">Divisor <see cref="Vec2"/> on the right of the div sign.</param>
        /// <returns>The result of dividing the vectors.</returns>
        public static Vec2 operator /(Vec2 value1, Vec2 value2)
        {
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            return value1;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vec2"/> by a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vec2"/> on the left of the div sign.</param>
        /// <param name="divider">Divisor scalar on the right of the div sign.</param>
        /// <returns>The result of dividing a vector by a scalar.</returns>
        public static Vec2 operator /(Vec2 value1, float divider)
        {
            float factor = 1 / divider;
            value1.X *= factor;
            value1.Y *= factor;
            return value1;
        }

        #endregion
    }
}
