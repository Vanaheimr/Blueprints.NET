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
    /// A voxel of type T.
    /// </summary>
    /// <typeparam name="T">The internal type of the voxel.</typeparam>
    public class Voxel<T> : AMath<T>, IEquatable<Voxel<T>>
        where T : IEquatable<T>, IComparable<T>, IComparable
    {

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

        #region Voxel(x, y, z)

        /// <summary>
        /// Create a voxel of type T.
        /// </summary>
        /// <param name="x">The X-coordinate.</param>
        /// <param name="y">The Y-coordinate.</param>
        /// <param name="z">The Z-coordinate.</param>
        public Voxel(T x, T y, T z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        #endregion

        #endregion


        #region DistanceTo(Voxel)

        /// <summary>
        /// A method to calculate the distance between
        /// this and another voxel of type T.
        /// </summary>
        /// <param name="Voxel">A voxel of type T</param>
        /// <returns>The distance between this voxel and the given voxel.</returns>
        public T DistanceTo(Voxel<T> Voxel)
        {
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

        #region Equals(VoxelT)

        /// <summary>
        /// Compares two voxels for equality.
        /// </summary>
        /// <param name="VoxelT">A voxel to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Voxel<T> VoxelT)
        {

            if ((Object) VoxelT == null)
                return false;

            return X.Equals(VoxelT.X) &&
                   Y.Equals(VoxelT.Y) &&
                   Z.Equals(VoxelT.Z);

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