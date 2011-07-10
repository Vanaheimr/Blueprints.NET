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
    /// A rectangle of type T.
    /// </summary>
    /// <typeparam name="T">The internal type of the rectangle.</typeparam>
    public abstract class Rectangle<T> : IEquatable<Rectangle<T>>, IComparable<Rectangle<T>>, IComparable
        where T : IEquatable<T>, IComparable<T>, IComparable
    {

        #region Data

        /// <summary>
        /// Left
        /// </summary>
        public readonly T Left;

        /// <summary>
        /// Top
        /// </summary>
        public readonly T Top;

        /// <summary>
        /// Right
        /// </summary>
        public readonly T Right;

        /// <summary>
        /// Bottom
        /// </summary>
        public readonly T Bottom;

        #endregion

        #region Constructor(s)

        #region Rectangle(left, top, right, bottom)

        /// <summary>
        /// Create a rectangle of type T.
        /// </summary>
        /// <param name="left">The left parameter.</param>
        /// <param name="top">The top parameter.</param>
        /// <param name="right">The right parameter.</param>
        /// <param name="bottom">The bottom parameter.</param>
        public Rectangle(T left, T top, T right, T bottom)
        {
            this.Left   = left;
            this.Top    = top;
            this.Right  = right;
            this.Bottom = bottom;
        }

        #endregion

        #endregion


        #region Abstract Math Operations

        /// <summary>
        /// A method to add two internal datatypes.
        /// </summary>
        /// <param name="a">An object of type T</param>
        /// <param name="b">An object of type T</param>
        /// <returns>The addition of a and b: a + b</returns>
        protected abstract T Add(T a, T b);

        /// <summary>
        /// A method to sub two internal datatypes.
        /// </summary>
        /// <param name="a">An object of type T</param>
        /// <param name="b">An object of type T</param>
        /// <returns>The subtraction of b from a: a - b</returns>
        protected abstract T Sub(T a, T b);

        /// <summary>
        /// A method to multiply two internal datatypes.
        /// </summary>
        /// <param name="a">An object of type T</param>
        /// <param name="b">An object of type T</param>
        /// <returns>The multiplication of a and b: a * b</returns>
        protected abstract T Mul(T a, T b);

        /// <summary>
        /// A method to divide two internal datatypes.
        /// </summary>
        /// <param name="a">An object of type T</param>
        /// <param name="b">An object of type T</param>
        /// <returns>The division of a by b: a / b</returns>
        protected abstract T Div(T a, T b);

        #endregion



        public bool Contains(T x, T y)
        {
            throw new NotImplementedException();
        }

        public bool Contains(Pixel<T> pt)
        {
            throw new NotImplementedException();
        }

        public bool Contains(Rectangle<T> rect)
        {
            throw new NotImplementedException();
        }




        public bool Equals(Rectangle<T> other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(Rectangle<T> other)
        {
            throw new NotImplementedException();
        }



        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            return Left.GetHashCode() ^ 1 + Top.GetHashCode() ^ 2 + Right.GetHashCode() ^ 3 + Bottom.GetHashCode();
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Return a string represtentation of this object.
        /// </summary>
        public override String ToString()
        {
            return String.Format("{{Left={0}, Top={1}, Right={0}, Bottom={1}}}", Left.ToString(), Top.ToString(), Right.ToString(), Bottom.ToString());
        }

        #endregion

    }

}
