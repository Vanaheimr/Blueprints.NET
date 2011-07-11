﻿/*
 * Copyright (c) 2010-2011, Achim 'ahzf' Friedland <code@ahzf.de>
 * This file is part of Blueprints.NET <http://www.github.com/ahzf/Blueprints.NET>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;

#endregion

namespace de.ahzf.Blueprints
{

    /// <summary>
    /// A rectangle of type Double.
    /// </summary>
    public class RectangleDouble : Rectangle<Double>
    {

        #region Constructor(s)

        #region RectangleDouble(Left, Top, Right, Bottom)

        /// <summary>
        /// Create a rectangle of type Double.
        /// </summary>
        /// <param name="Left">The left parameter.</param>
        /// <param name="Top">The top parameter.</param>
        /// <param name="Right">The right parameter.</param>
        /// <param name="Bottom">The bottom parameter.</param>
        public RectangleDouble(Double Left, Double Top, Double Right, Double Bottom)
            : base(Left, Top, Right, Bottom)
        { }

        #endregion

        #region RectangleDouble(Pixel1, Pixel2)

        /// <summary>
        /// Create a rectangle of type Double.
        /// </summary>
        /// <param name="Pixel1">A pixel of type Double.</param>
        /// <param name="Pixel2">A pixel of type Double.</param>
        public RectangleDouble(Pixel<Double> Pixel1, Pixel<Double> Pixel2)
            : base(Pixel1, Pixel2)
        { }

        #endregion

        #region RectangleDouble(Pixel, Width, Height)

        /// <summary>
        /// Create a rectangle of type Double.
        /// </summary>
        /// <param name="Pixel">A pixel of type Double in the upper left corner of the rectangle.</param>
        /// <param name="Width">The width of the rectangle.</param>
        /// <param name="Height">The height of the rectangle.</param>
        public RectangleDouble(Pixel<Double> Pixel, Double Width, Double Height)
            : base(Pixel, Width, Height)
        { }

        #endregion

        #endregion


        #region Abstract Math Operations

        #region Min(a, b)

        /// <summary>
        /// A method to get the minimum of two internal datatypes.
        /// </summary>
        /// <param name="a">A Double.</param>
        /// <param name="b">A Double.</param>
        /// <returns>The minimum of a and b: Min(a, b)</returns>
        protected override Double Min(Double a, Double b)
        {
            return Math.Min(a, b);
        }

        #endregion

        #region Max(a, b)

        /// <summary>
        /// A method to get the maximum of two internal datatypes.
        /// </summary>
        /// <param name="a">A Double.</param>
        /// <param name="b">A Double.</param>
        /// <returns>The maximum of a and b: Max(a, b)</returns>
        protected override Double Max(Double a, Double b)
        {
            return Math.Max(a, b);
        }

        #endregion


        #region Add(a, b)

        /// <summary>
        /// A method to add two internal datatypes.
        /// </summary>
        /// <param name="a">A Double.</param>
        /// <param name="b">A Double.</param>
        /// <returns>The addition of a and b: a + b</returns>
        protected override Double Add(Double a, Double b)
        {
            return a + b;
        }

        #endregion

        #region Sub(a, b)

        /// <summary>
        /// A method to sub two internal datatypes.
        /// </summary>
        /// <param name="a">A Double.</param>
        /// <param name="b">A Double.</param>
        /// <returns>The subtraction of b from a: a - b</returns>
        protected override Double Sub(Double a, Double b)
        {
            return a - b;
        }

        #endregion

        #region Mul(a, b)

        /// <summary>
        /// A method to multiply two internal datatypes.
        /// </summary>
        /// <param name="a">A Double.</param>
        /// <param name="b">A Double.</param>
        /// <returns>The multiplication of a and b: a * b</returns>
        protected override Double Mul(Double a, Double b)
        {
            return a * b;
        }

        #endregion

        #region Div(a, b)

        /// <summary>
        /// A method to divide two internal datatypes.
        /// </summary>
        /// <param name="a">A Double.</param>
        /// <param name="b">A Double.</param>
        /// <returns>The division of a by b: a / b</returns>
        protected override Double Div(Double a, Double b)
        {
            return a / b;
        }

        #endregion

        #region Pow(a, b)

        /// <summary>
        /// A method to calculate an internal datatype raised to the specified power.
        /// </summary>
        /// <param name="a">A Double.</param>
        /// <param name="b">A Double.</param>
        /// <returns>The value a raised to the specified power of b: a^b</returns>
        protected override Double Pow(Double a, Double b)
        {
            return Math.Pow(a, b);
        }

        #endregion


        #region Abs(a)

        /// <summary>
        /// A method to calculate the absolute value of the internal datatype.
        /// </summary>
        /// <param name="a">An Double.</param>
        /// <returns>The absolute value of a: Abs(a)</returns>
        protected override Double Abs(Double a)
        {
            return Math.Abs(a);
        }

        #endregion

        #region Sqrt(a)

        /// <summary>
        /// A method to calculate the square root of the internal datatype.
        /// </summary>
        /// <param name="a">A Double.</param>
        /// <returns>The square root of a: Sqrt(a)</returns>
        protected override Double Sqrt(Double a)
        {
            return Math.Sqrt(a);
        }

        #endregion

        #endregion


        #region Operator overloadings

        #region Operator == (Rectangle1, Rectangle2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Rectangle1">A Rectangle&lt;Double&gt;.</param>
        /// <param name="Rectangle2">Another Rectangle&lt;Double&gt;.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (RectangleDouble Rectangle1, RectangleDouble Rectangle2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(Rectangle1, Rectangle2))
                return true;

            // If one is null, but not both, return false.
            if (((Object)Rectangle1 == null) || ((Object)Rectangle2 == null))
                return false;

            return Rectangle1.Equals(Rectangle2);

        }

        #endregion

        #region Operator != (Rectangle1, Rectangle2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Rectangle1">A Rectangle&lt;Double&gt;.</param>
        /// <param name="Rectangle2">Another Rectangle&lt;Double&gt;.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (RectangleDouble Rectangle1, RectangleDouble Rectangle2)
        {
            return !(Rectangle1.Equals(Rectangle2));
        }

        #endregion

        #endregion

        #region IEquatable Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {
            return base.Equals(Object);
        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

    }

}
