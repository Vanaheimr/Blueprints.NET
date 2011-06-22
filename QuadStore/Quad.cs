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
using System.Collections.Generic;

#endregion

namespace de.ahzf.Blueprints.BlueQuad
{

    /// <summary>
    /// A Quad is a little fragment of a graph.
    /// Subject -Predicate-> Object [Context]
    /// Vertex  -Edge->      Vertex [HyperEdge]
    /// </summary>
    /// <typeparam name="T">The type of the subject, predicate, objects and context of a quad.</typeparam>
    public class Quad<T> : IEquatable<Quad<T>>, IComparable, IComparable<Quad<T>>
        where T : IEquatable<T>, IComparable, IComparable<T>
    {

        #region Data

        #region SystemId, TransactionId and QuadId

        /// <summary>
        /// The Id of the QuadStore which created this quad.
        /// </summary>
        public readonly T SystemId;

        /// <summary>
        /// The Id of the transaction this quad was build in.
        /// This Id is just local unique. To get a global unique
        /// Id add the SystemId of the QuadStore.
        /// </summary>
        public readonly T TransactionId;

        /// <summary>
        /// The Id of the quad.
        /// From another point of view this is an EdgeId.
        /// This Id is just local unique. To get a global unique
        /// Id add the SystemId of the QuadStore.
        /// </summary>
        public readonly T QuadId;

        #endregion

        #region Subject, Predicate, Object and Context

        /// <summary>
        /// The Subject of this quad.
        /// From another point of view this is an VertexId.
        /// </summary>
        public readonly T Subject;

        /// <summary>
        /// The Predicate of this quad.
        /// From another point of view this is a PropertyId.
        /// </summary>
        public readonly T Predicate;

        /// <summary>
        /// The Object of this quad.
        /// </summary>
        public readonly T Object;

        /// <summary>
        /// The Context of this quad.
        /// From another point of view this is a HyperEdgeId.
        /// </summary>
        public readonly T Context;

        #endregion

        #region ObjectOnDisc and ObjectReference

        /// <summary>
        /// The on disc position of this quad.
        /// </summary>
        public readonly T ObjectOnDisc;

        /// <summary>
        /// A hashset of references to quads having
        /// the Object of this quad as Subject.
        /// </summary>
        public HashSet<Quad<T>> ObjectReference;

        #endregion

        #endregion

        #region Constructor(s)

        #region Quad(SystemId, TransactionId, QuadId, Subject, Predicate, Object, Context, ObjectOnDisc = default(T))

        /// <summary>
        /// A Quad is a little fragment of a graph.
        /// Subject -Predicate-> Object [Context]
        /// Vertex  -Edge->      Vertex [HyperEdge]
        /// </summary>
        /// <param name="SystemId">The Id of the QuadStore which created this quad.</param>
        /// <param name="TransactionId">The Id of the transaction this quad was build in.</param>
        /// <param name="QuadId">The Id of this quad.</param>
        /// <param name="Subject">The Subject.</param>
        /// <param name="Predicate">The Predicate.</param>
        /// <param name="Object">The Object.</param>
        /// <param name="Context">The Context.</param>
        /// <param name="ObjectOnDisc"></param>
        public Quad(T SystemId, T TransactionId, T QuadId, T Subject, T Predicate, T Object, T Context, T ObjectOnDisc = default(T))
        {

            #region Initial checks

            if (SystemId  == null || SystemId .Equals(default(T)))
                throw new ArgumentNullException("The SystemId must not be null or default(T)!");

            if (QuadId    == null || QuadId   .Equals(default(T)))
                throw new ArgumentNullException("The QuadId must not be null or default(T)!");

            if (Subject   == null || Subject  .Equals(default(T)))
                throw new ArgumentNullException("The Subject must not be null or default(T)!");

            if (Predicate == null || Predicate.Equals(default(T)))
                throw new ArgumentNullException("The Predicate must not be null or default(T)!");

            if (Object    == null || Object   .Equals(default(T)))
                throw new ArgumentNullException("The Object must not be null or default(T)!");

            if (Context   == null || Context.  Equals(default(T)))
                throw new ArgumentNullException("The Context must not be null or default(T)!");

            #endregion

            this.SystemId        = SystemId;
            this.TransactionId   = TransactionId;
            this.QuadId          = QuadId;
            this.Subject         = Subject;
            this.Predicate       = Predicate;
            this.Object          = Object;
            this.Context         = Context;
            this.ObjectOnDisc    = ObjectOnDisc;
            this.ObjectReference = null;

        }

        #endregion

        #endregion


        #region Operator overloading

        #region Operator == (Quad1, Quad2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Quad1">A Quad.</param>
        /// <param name="Quad2">Another Quad.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Quad<T> Quad1, Quad<T> Quad2)
        {

            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(Quad1, Quad2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) Quad1 == null) || ((Object) Quad2 == null))
                return false;

            return Quad1.Equals(Quad2);

        }

        #endregion

        #region Operator != (Quad1, Quad2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Quad1">A Quad.</param>
        /// <param name="Quad2">Another Quad.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Quad<T> Quad1, Quad<T> Quad2)
        {
            return !(Quad1 == Quad2);
        }

        #endregion

        #endregion

        #region IEquatable<Quad<T>> Members

        #region Equals(myObject)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="myObject">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object myObject)
        {

            // Check if myObject is null
            if (myObject == null)
                throw new ArgumentNullException("Parameter myObject must not be null!");

            // Check if myObject can be cast to EdgeId
            var AnotherQuad = myObject as Quad<T>;
            if ((Object) AnotherQuad == null)
                throw new ArgumentException("Parameter myObject could not be casted to type Quad<T>!");

            return this.Equals(AnotherQuad);

        }

        #endregion

        #region Equals(AnotherQuad)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AnotherQuad">Another quad to compare with.</param>
        /// <returns>true|false</returns>
        public Boolean Equals(Quad<T> AnotherQuad)
        {

            // Check if OtherQuad is null
            if (AnotherQuad == null)
                throw new ArgumentNullException("Parameter AnotherQuad must not be null!");

            return QuadId.Equals(AnotherQuad.QuadId);

        }

        #endregion

        #endregion

        #region IComparable<Quad<T>> Members

        #region CompareTo(myObject)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="myObject">An object to compare with.</param>
        /// <returns>true|false</returns>
        public Int32 CompareTo(Object myObject)
        {

            // Check if myObject is null
            if (myObject == null)
                throw new ArgumentNullException("myObject must not be null!");

            // Check if Object can be casted to an Quad<T> object
            var AnotherQuad = myObject as Quad<T>;
            if ((Object) AnotherQuad == null)
                throw new ArgumentException("myObject is not of type Quad<T>!");

            return CompareTo(AnotherQuad);

        }

        #endregion

        #region CompareTo(AnotherQuad)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AnotherQuad">Another quad to compare with.</param>
        /// <returns>true|false</returns>
        public Int32 CompareTo(Quad<T> AnotherQuad)
        {

            // Check if AnotherQuad is null
            if (AnotherQuad == null)
                throw new ArgumentNullException("AnotherQuad must not be null!");

            return QuadId.CompareTo(AnotherQuad.QuadId);

        }

        #endregion

        #endregion


        #region GetHashCode()

        /// <summary>
        /// Returns the HashCode.
        /// </summary>
        public override Int32 GetHashCode()
        {
            return QuadId.GetHashCode();
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Shows information on this quad.
        /// </summary>
        public override String ToString()
        {
            return String.Format("Id {0}: {1} -{2}-> {3} [{4}]", QuadId.ToString(), Subject.ToString(), Predicate.ToString(), Object.ToString(), Context.ToString());
        }

        #endregion

    }

}
