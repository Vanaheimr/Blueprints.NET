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
    /// A voxel of type T.
    /// </summary>
    /// <typeparam name="T">The internal type of the voxel.</typeparam>
    public class Voxel<T> : IVoxel<T>
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

        #region Z

        /// <summary>
        /// The Z-coordinate.
        /// </summary>
        public T Z { get; private set; }

        #endregion

        #endregion

        #region Constructor(s)

        #region Voxel(X, Y, Z)

        /// <summary>
        /// Create a voxel of type T.
        /// </summary>
        /// <param name="X">The X-coordinate.</param>
        /// <param name="Y">The Y-coordinate.</param>
        /// <param name="Z">The Z-coordinate.</param>
        public Voxel(T X, T Y, T Z)
        {

            #region Initial Checks

            if (X == null)
                throw new ArgumentNullException("The given x-coordinate must not be null!");

            if (Y == null)
                throw new ArgumentNullException("The given y-coordinate must not be null!");

            if (Z == null)
                throw new ArgumentNullException("The given z-coordinate must not be null!");

            #endregion

            this.Math = MathsFactory<T>.Instance;
            this.X    = X;
            this.Y    = Y;
            this.Z    = Z;

        }

        #endregion

        #endregion


        #region DistanceTo(x, y)

        /// <summary>
        /// A method to calculate the distance between this
        /// voxel and the given coordinates of type T.
        /// </summary>
        /// <param name="x">A x-coordinate of type T</param>
        /// <param name="y">A y-coordinate of type T</param>
        /// <param name="z">A z-coordinate of type T</param>
        /// <returns>The distance between this voxel and the given coordinates.</returns>
        public T DistanceTo(T x, T y, T z)
        {

            #region Initial Checks

            if (x == null)
                throw new ArgumentNullException("The given x-coordinate must not be null!");

            if (y == null)
                throw new ArgumentNullException("The given y-coordinate must not be null!");

            if (z == null)
                throw new ArgumentNullException("The given z-coordinate must not be null!");

            #endregion

            var dX = Math.Distance(X, x);
            var dY = Math.Distance(Y, y);
            var dZ = Math.Distance(Z, z);

            return Math.Sqrt(Math.Add(Math.Mul(dX, dX), Math.Mul(dY, dY), Math.Mul(dZ, dZ)));

        }

        #endregion
        
        #region DistanceTo(Voxel)

        /// <summary>
        /// A method to calculate the distance between
        /// this and another voxel of type T.
        /// </summary>
        /// <param name="Voxel">A voxel of type T</param>
        /// <returns>The distance between this voxel and the given voxel.</returns>
        public T DistanceTo(IVoxel<T> Voxel)
        {

            #region Initial Checks

            if (Voxel == null)
                throw new ArgumentNullException("The given voxel must not be null!");

            #endregion
            
            var dX = Math.Distance(X, Voxel.X);
            var dY = Math.Distance(Y, Voxel.Y);
            var dZ = Math.Distance(Z, Voxel.Z);
            
            return Math.Sqrt(Math.Add(Math.Mul(dX, dX), Math.Mul(dY, dY), Math.Mul(dZ, dZ)));

        }

        #endregion


        #region Operator overloadings

        #region Operator == (Voxel1, Voxel2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Voxel1">A Voxel&lt;T&gt;.</param>
        /// <param name="Voxel2">Another Voxel&lt;T&gt;.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Voxel<T> Voxel1, Voxel<T> Voxel2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(Voxel1, Voxel2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) Voxel1 == null) || ((Object) Voxel2 == null))
                return false;

            return Voxel1.Equals(Voxel2);

        }

        #endregion

        #region Operator != (Voxel1, Voxel2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Voxel1">A Voxel&lt;T&gt;.</param>
        /// <param name="Voxel2">Another Voxel&lt;T&gt;.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Voxel<T> Voxel1, Voxel<T> Voxel2)
        {
            return !(Voxel1 == Voxel2);
        }

        #endregion

        #endregion

        #region IComparable Members

        public int CompareTo(Object obj)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(IVoxel<T> other)
        {
            throw new NotImplementedException();
        }

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

            // Check if the given object is an Voxel<T>.
            var VoxelT = (Voxel<T>) Object;
            if ((Object) VoxelT == null)
                return false;

            return this.Equals(VoxelT);

        }

        #endregion

        #region Equals(Voxel)

        /// <summary>
        /// Compares two voxels for equality.
        /// </summary>
        /// <param name="Voxel">A voxel to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(IVoxel<T> Voxel)
        {

            if ((Object) Voxel == null)
                return false;

            return X.Equals(Voxel.X) &&
                   Y.Equals(Voxel.Y) &&
                   Z.Equals(Voxel.Z);

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
            return X.GetHashCode() ^ 1 + Y.GetHashCode() ^ 2 + Z.GetHashCode();
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Return a string represtentation of this object.
        /// </summary>
        public override String ToString()
        {
            return String.Format("X={0}, Y={1}, Z={2}",
                                 X.ToString(),
                                 Y.ToString(),
                                 Z.ToString());
        }

        #endregion

    }

}
