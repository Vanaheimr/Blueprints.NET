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
    /// A rectangle of type Int32.
    /// </summary>
    public class RectangleInt32 : Rectangle<Int32>
    {

        #region Constructor(s)

        #region RectangleInt32(left, top, right, bottom)

        /// <summary>
        /// Create a rectangle of type Int32.
        /// </summary>
        /// <param name="left">The left parameter.</param>
        /// <param name="top">The top parameter.</param>
        /// <param name="right">The right parameter.</param>
        /// <param name="bottom">The bottom parameter.</param>
        public RectangleInt32(Int32 left, Int32 top, Int32 right, Int32 bottom)
            : base(left, top, right, bottom)
        { }

        #endregion

        #endregion


        #region Abstract Math Operations

        #region Add(a, b)

        /// <summary>
        /// A method to add two internal datatypes.
        /// </summary>
        /// <param name="a">An object of type T</param>
        /// <param name="b">An object of type T</param>
        /// <returns>The addition of a and b: a + b</returns>
        protected override Int32 Add(Int32 a, Int32 b)
        {
            return a + b;
        }

        #endregion

        #region Sub(a, b)

        /// <summary>
        /// A method to sub two internal datatypes.
        /// </summary>
        /// <param name="a">An object of type T</param>
        /// <param name="b">An object of type T</param>
        /// <returns>The subtraction of b from a: a - b</returns>
        protected override Int32 Sub(Int32 a, Int32 b)
        {
            return a - b;
        }

        #endregion

        #region Mul(a, b)

        /// <summary>
        /// A method to multiply two internal datatypes.
        /// </summary>
        /// <param name="a">An object of type T</param>
        /// <param name="b">An object of type T</param>
        /// <returns>The multiplication of a and b: a * b</returns>
        protected override Int32 Mul(Int32 a, Int32 b)
        {
            return a * b;
        }

        #endregion

        #region Div(a, b)

        /// <summary>
        /// A method to divide two internal datatypes.
        /// </summary>
        /// <param name="a">An object of type T</param>
        /// <param name="b">An object of type T</param>
        /// <returns>The division of a by b: a / b</returns>
        protected override Int32 Div(Int32 a, Int32 b)
        {
            return a / b;
        }

        #endregion

        #endregion


        #region Operator overloadings

        #region Operator == (Rectangle1, Rectangle2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Rectangle1">A Rectangle&lt;Int32&gt;.</param>
        /// <param name="Rectangle2">Another Rectangle&lt;Int32&gt;.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (RectangleInt32 Rectangle1, RectangleInt32 Rectangle2)
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
        /// <param name="Rectangle1">A Rectangle&lt;Int32&gt;.</param>
        /// <param name="Rectangle2">Another Rectangle&lt;Int32&gt;.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (RectangleInt32 Rectangle1, RectangleInt32 Rectangle2)
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
