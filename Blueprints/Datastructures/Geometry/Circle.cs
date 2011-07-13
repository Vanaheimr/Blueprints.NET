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

using de.ahzf.Blueprints.Maths;

#endregion

namespace de.ahzf.Blueprints
{

    /// <summary>
    /// A circle of type T.
    /// </summary>
    /// <typeparam name="T">The internal type of the circle.</typeparam>
    public class Circle<T> : ICircle<T>
        where T : IEquatable<T>, IComparable<T>, IComparable
    {

        #region Data

        /// <summary>
        /// Mathoperation helpers.
        /// </summary>
        protected readonly IMaths<T> Math;

        #endregion

        #region Properties

        #region X

        /// <summary>
        /// The X-coordinate.
        /// </summary>
        public T X { get; private set; }

        #endregion

        #region Y

        /// <summary>
        /// The Y-coordinate.
        /// </summary>
        public T Y { get; private set; }

        #endregion

        #region Radius

        /// <summary>
        /// The radius of the circle.
        /// </summary>
        public T Radius { get; private set; }

        #endregion


        #region Center

        /// <summary>
        /// The center of the circle.
        /// </summary>
        public IPixel<T> Center { get; private set; }

        #endregion

        #region Diameter

        /// <summary>
        /// The diameter of the circle.
        /// </summary>
        public T Diameter
        {
            get
            {
                return Math.Add(Radius, Radius);
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region Circle(X, Y, Radius)

        /// <summary>
        /// Create a circle of type T.
        /// </summary>
        /// <param name="X">The x-coordinate of the circle.</param>
        /// <param name="Y">The y-coordinate of the circle.</param>
        /// <param name="Radius">The radius of the circle.</param>
        public Circle(T X, T Y, T Radius)
        {

            #region Initial Checks

            if (X      == null)
                throw new ArgumentNullException("The given x-coordinate must not be null!");

            if (Y      == null)
                throw new ArgumentNullException("The given y-coordinate must not be null!");

            if (Radius == null)
                throw new ArgumentNullException("The given radius must not be null!");

            #endregion

            this.Math   = MathsFactory<T>.Instance;

            #region Math Checks

            if (Radius.Equals(Math.Zero))
                throw new ArgumentException("The given radius must not be zero!");

            #endregion

            this.X      = X;
            this.Y      = Y;
            this.Center = new Pixel<T>(X, Y);
            this.Radius = Radius;

        }

        #endregion

        #endregion


        #region Contains(x, y)

        /// <summary>
        /// Checks if the given x- and y-coordinates are
        /// located within this circle.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <returns>True if the coordinates are located within this circle; False otherwise.</returns>
        public Boolean Contains(T x, T y)
        {

            #region Initial Checks

            if (x == null)
                throw new ArgumentNullException("The given x-coordinate must not be null!");

            if (y == null)
                throw new ArgumentNullException("The given y-coordinate must not be null!");

            #endregion

            if (Center.DistanceTo(new Pixel<T>(x, y)).IsLessThanOrEquals(Radius))
                return true;

            return false;

        }

        #endregion

        #region Contains(Pixel)

        /// <summary>
        /// Checks if the given pixel is located
        /// within this circle.
        /// </summary>
        /// <param name="Pixel">A pixel.</param>
        /// <returns>True if the pixel is located within this circle; False otherwise.</returns>
        public Boolean Contains(IPixel<T> Pixel)
        {

            #region Initial Checks

            if (Pixel == null)
                throw new ArgumentNullException("The given pixel must not be null!");

            #endregion

            if (Center.DistanceTo(Pixel).IsLessThanOrEquals(Radius))
                return true;

            return false;

        }

        #endregion

        #region Contains(Circle)

        /// <summary>
        /// Checks if the given circle is located
        /// within this circle.
        /// </summary>
        /// <param name="Circle">A circle of type T.</param>
        /// <returns>True if the circle is located within this circle; False otherwise.</returns>
        public Boolean Contains(ICircle<T> Circle)
        {

            #region Initial Checks

            if (Circle == null)
                throw new ArgumentNullException("The given circle must not be null!");

            #endregion

            if (Center.DistanceTo(Circle.Center).IsLessThanOrEquals(Math.Sub(Radius, Circle.Radius)))
                return true;

            return true;

        }

        #endregion

        #region Overlaps(Circle)

        /// <summary>
        /// Checks if the given circle shares some
        /// area with this circle.
        /// </summary>
        /// <param name="Circle">A circle of type T.</param>
        /// <returns>True if the circle shares some area with this circle; False otherwise.</returns>
        public Boolean Overlaps(ICircle<T> Circle)
        {

            #region Initial Checks

            if (Circle == null)
                throw new ArgumentNullException("The given circle must not be null!");

            #endregion

            if (Center.DistanceTo(Circle.Center).IsLessThanOrEquals(Math.Add(Radius, Circle.Radius)))
                return true;

            return true;

        }

        #endregion


        #region Operator overloadings

        #region Operator == (Circle1, Circle2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Circle1">A Circle&lt;T&gt;.</param>
        /// <param name="Circle2">Another Circle&lt;T&gt;.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Circle<T> Circle1, Circle<T> Circle2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(Circle1, Circle2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) Circle1 == null) || ((Object) Circle2 == null))
                return false;

            return Circle1.Equals(Circle2);

        }

        #endregion

        #region Operator != (Circle1, Circle2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Circle1">A Circle&lt;T&gt;.</param>
        /// <param name="Circle2">Another Circle&lt;T&gt;.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Circle<T> Circle1, Circle<T> Circle2)
        {
            return !(Circle1 == Circle2);
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

            if (Object == null)
                return false;

            // Check if the given object is an Circle<T>.
            var CircleT = (Circle<T>) Object;
            if ((Object) CircleT == null)
                return false;

            return this.Equals(CircleT);

        }

        #endregion

        #region Equals(ICircle)

        /// <summary>
        /// Compares two circles for equality.
        /// </summary>
        /// <param name="ICircle">A circle to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ICircle<T> ICircle)
        {

            if ((Object) ICircle == null)
                return false;

            return this.X.     Equals(ICircle.X) &&
                   this.Y.     Equals(ICircle.Y) &&
                   this.Radius.Equals(ICircle.Radius);

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
            return X.GetHashCode() ^ 1 + Y.GetHashCode() ^ 2 + Radius.GetHashCode();
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Return a string represtentation of this object.
        /// </summary>
        public override String ToString()
        {
            return String.Format("Left={0}, Top={1}, Radius={2}",
                                 X.     ToString(),
                                 Y.     ToString(),
                                 Radius.ToString());
        }

        #endregion

    }

}