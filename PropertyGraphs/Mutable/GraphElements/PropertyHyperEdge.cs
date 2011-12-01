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
using System.Dynamic;
using System.Linq.Expressions;

#endregion

namespace de.ahzf.Blueprints.PropertyGraphs.InMemory.Mutable
{

    /// <summary>
    /// A vertex maintains pointers to both a set of incoming and outgoing edges.
    /// The outgoing edges are those edges for which the vertex is the tail.
    /// The incoming edges are those edges for which the vertex is the head.
    /// Diagrammatically, ---inEdges---> vertex ---outEdges--->.
    /// </summary>
    /// <typeparam name="TIdVertex">The type of the vertex identifiers.</typeparam>
    /// <typeparam name="TRevisionIdVertex">The type of the vertex revision identifiers.</typeparam>
    /// <typeparam name="TVertexLabel">The type of the vertex type.</typeparam>
    /// <typeparam name="TKeyVertex">The type of the vertex property keys.</typeparam>
    /// <typeparam name="TValueVertex">The type of the vertex property values.</typeparam>
    /// <typeparam name="TPropertiesCollectionVertex">A data structure to store the properties of the vertices.</typeparam>
    /// 
    /// <typeparam name="TIdEdge">The type of the edge identifiers.</typeparam>
    /// <typeparam name="TRevisionIdEdge">The type of the edge revision identifiers.</typeparam>
    /// <typeparam name="TEdgeLabel">The type of the edge label.</typeparam>
    /// <typeparam name="TKeyEdge">The type of the edge property keys.</typeparam>
    /// <typeparam name="TValueEdge">The type of the edge property values.</typeparam>
    /// <typeparam name="TPropertiesCollectionEdge">A data structure to store the properties of the edges.</typeparam>
    /// 
    /// <typeparam name="TIdMultiEdge">The type of the multiedge identifiers.</typeparam>
    /// <typeparam name="TRevisionIdMultiEdge">The type of the multiedge revision identifiers.</typeparam>
    /// <typeparam name="TMultiEdgeLabel">The type of the multiedge label.</typeparam>
    /// <typeparam name="TKeyMultiEdge">The type of the multiedge property keys.</typeparam>
    /// <typeparam name="TValueMultiEdge">The type of the multiedge property values.</typeparam>
    /// <typeparam name="TPropertiesCollectionMultiEdge">A data structure to store the properties of the multiedges.</typeparam>
    /// 
    /// <typeparam name="TIdHyperEdge">The type of the hyperedge identifiers.</typeparam>
    /// <typeparam name="TRevisionIdHyperEdge">The type of the hyperedge revision identifiers.</typeparam>
    /// <typeparam name="THyperEdgeLabel">The type of the hyperedge label.</typeparam>
    /// <typeparam name="TKeyHyperEdge">The type of the hyperedge property keys.</typeparam>
    /// <typeparam name="TValueHyperEdge">The type of the hyperedge property values.</typeparam>
    /// <typeparam name="TPropertiesCollectionHyperEdge">A data structure to store the properties of the hyperedges.</typeparam>
    /// 
    /// <typeparam name="TVerticesCollection">A data structure to store vertices.</typeparam>
    public class PropertyHyperEdge<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                   TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                   TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                   TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>

                                   : AGraphElement<TIdHyperEdge, TRevisionIdHyperEdge, TKeyHyperEdge, TValueHyperEdge>,

                                     IGenericPropertyHyperEdge<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                        TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                        TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                        TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>,

                                     IDynamicGraphElement<PropertyHyperEdge<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                            TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                            TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                            TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>


        where TIdVertex                      : IEquatable<TIdVertex>,            IComparable<TIdVertex>,            IComparable, TValueVertex
        where TIdEdge                        : IEquatable<TIdEdge>,              IComparable<TIdEdge>,              IComparable, TValueEdge
        where TIdMultiEdge                   : IEquatable<TIdMultiEdge>,         IComparable<TIdMultiEdge>,         IComparable, TValueMultiEdge
        where TIdHyperEdge                   : IEquatable<TIdHyperEdge>,         IComparable<TIdHyperEdge>,         IComparable, TValueHyperEdge

        where TRevisionIdVertex              : IEquatable<TRevisionIdVertex>,    IComparable<TRevisionIdVertex>,    IComparable, TValueVertex
        where TRevisionIdEdge                : IEquatable<TRevisionIdEdge>,      IComparable<TRevisionIdEdge>,      IComparable, TValueEdge
        where TRevisionIdMultiEdge           : IEquatable<TRevisionIdMultiEdge>, IComparable<TRevisionIdMultiEdge>, IComparable, TValueMultiEdge
        where TRevisionIdHyperEdge           : IEquatable<TRevisionIdHyperEdge>, IComparable<TRevisionIdHyperEdge>, IComparable, TValueHyperEdge

        where TVertexLabel                   : IEquatable<TVertexLabel>,         IComparable<TVertexLabel>,         IComparable
        where TEdgeLabel                     : IEquatable<TEdgeLabel>,           IComparable<TEdgeLabel>,           IComparable
        where TMultiEdgeLabel                : IEquatable<TMultiEdgeLabel>,      IComparable<TMultiEdgeLabel>,      IComparable
        where THyperEdgeLabel                : IEquatable<THyperEdgeLabel>,      IComparable<THyperEdgeLabel>,      IComparable

        where TKeyVertex                     : IEquatable<TKeyVertex>,           IComparable<TKeyVertex>,           IComparable
        where TKeyEdge                       : IEquatable<TKeyEdge>,             IComparable<TKeyEdge>,             IComparable
        where TKeyMultiEdge                  : IEquatable<TKeyMultiEdge>,        IComparable<TKeyMultiEdge>,        IComparable
        where TKeyHyperEdge                  : IEquatable<TKeyHyperEdge>,        IComparable<TKeyHyperEdge>,        IComparable

    {

        #region Properties

        #region Graph

        /// <summary>
        /// The associated property graph.
        /// </summary>
        public IGenericPropertyGraph<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                     TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                     TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                     TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> Graph { get; private set; }

        #endregion

        #region Label

        /// <summary>
        /// The label associated with this hyperedge.
        /// </summary>
        public THyperEdgeLabel Label { get; private set; }

        #endregion

        #region Vertices

        /// <summary>
        /// The vertex at the tail of this edge.
        /// </summary>
        private IGroupedCollection<TVertexLabel, TIdVertex, IGenericPropertyVertex<TIdVertex, TRevisionIdVertex, TVertexLabel, TKeyVertex, TValueVertex,
                                                                                                                    TIdEdge, TRevisionIdEdge, TEdgeLabel, TKeyEdge, TValueEdge,
                                                                                                                    TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                                                                    TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> _Vertices;

        ///// <summary>
        ///// The vertex at the head of this edge.
        ///// </summary>
        //public IEnumerable<IPropertyVertex<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
        //                                   TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
        //                                   TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
        //                                   TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>
        //                                   Vertices
        //{
        //    get
        //    {
        //        return _Vertices;
        //    }
        //}

        #endregion

        #endregion

        #region Constructor(s)

        #region PropertyHyperEdge(Graph, OutVertex, InVertex, EdgeId, Label, IdKey, RevisionIdKey, PropertiesCollectionInitializer, EdgeInitializer = null)

        /// <summary>
        /// Creates a new hyperedge.
        /// </summary>
        /// <param name="Graph">The associated property graph.</param>
        /// <param name="Vertices">The vertices connected by this multiedge.</param>
        /// <param name="MultiEdgeId">The identification of this multiedge.</param>
        /// <param name="Label">A label stored within this multiedge.</param>
        /// <param name="IdKey">The key of the multiedge identifier.</param>
        /// <param name="RevisonIdKey">The key of the multiedge revision identifier.</param>
        /// <param name="PropertiesCollectionInitializer">A delegate to initialize the properties datastructure.</param>
        /// <param name="VerticesCollectionInitializer">A delegate to initialize the datastructure for storing the vertices.</param>
        /// <param name="MultiEdgeInitializer">A delegate to initialize the newly created multiedge.</param>
        public PropertyHyperEdge(IGenericPropertyGraph<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                       TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                       TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                       TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> Graph,

                                 IEnumerable<IGenericPropertyVertex<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                             TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                             TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                             TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>
                                                             Vertices,

                                 TIdHyperEdge                         HyperEdgeId,
                                 THyperEdgeLabel                      Label,
                                 TKeyHyperEdge                        IdKey,
                                 TKeyHyperEdge                        RevisonIdKey,
                                 Func<IDictionary<TKeyHyperEdge, TValueHyperEdge>> PropertiesCollectionInitializer,
                                 Func<IGroupedCollection<TVertexLabel, TIdVertex, IGenericPropertyVertex<TIdVertex, TRevisionIdVertex, TVertexLabel, TKeyVertex, TValueVertex,
                                                                                                                    TIdEdge, TRevisionIdEdge, TEdgeLabel, TKeyEdge, TValueEdge,
                                                                                                                    TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                                                                    TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>> VerticesCollectionInitializer,

                                 HyperEdgeInitializer<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                      TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                      TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                      TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> HyperEdgeInitializer = null)

            : base(HyperEdgeId, IdKey, RevisonIdKey, PropertiesCollectionInitializer)

        {

            #region Initial checks

            if (Graph == null)
                throw new ArgumentNullException("The given graph must not be null!");

            if (HyperEdgeId == null)
                throw new ArgumentNullException("The given HyperEdgeId must not be null!");

            if (IdKey == null)
                throw new ArgumentNullException("The given IdKey must not be null!");

            if (RevisonIdKey == null)
                throw new ArgumentNullException("The given RevisonIdKey must not be null!");

            if (PropertiesCollectionInitializer == null)
                throw new ArgumentNullException("The given PropertiesCollectionInitializer must not be null!");

            if (VerticesCollectionInitializer == null)
                throw new ArgumentNullException("The given VerticesCollectionInitializer must not be null!");

            #endregion

            this.Graph     = Graph;
            this._Vertices = VerticesCollectionInitializer();
            this.Label     = Label;

            if (HyperEdgeInitializer != null)
                HyperEdgeInitializer(this);

        }

        #endregion

        #endregion


        public IEnumerable<IGenericPropertyVertex<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                           TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                           TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                           TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>

                                           VerticesByLabel(params TVertexLabel[] VertexTypes)

        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGenericPropertyVertex<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                           TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                           TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                           TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>

                                           Vertices(VertexFilter<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                 TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                 TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                 TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexFilter = null)

        {
            throw new NotImplementedException();
        }

        public UInt64 NumberOfVertices(VertexFilter<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                    TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                    TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                    TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> VertexFilter = null)
        {
            throw new NotImplementedException();
        }


        #region Operator overloading

        #region Operator == (PropertyHyperEdge1, PropertyHyperEdge2)

        /// <summary>
        /// Compares two edges for equality.
        /// </summary>
        /// <param name="PropertyHyperEdge1">A Edge.</param>
        /// <param name="PropertyHyperEdge2">Another Edge.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PropertyHyperEdge<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                             TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                             TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                             TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>
                                                             PropertyHyperEdge1,
                                           PropertyHyperEdge<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                             TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                             TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                             TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>
                                                             PropertyHyperEdge2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(PropertyHyperEdge1, PropertyHyperEdge2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) PropertyHyperEdge1 == null) || ((Object) PropertyHyperEdge2 == null))
                return false;

            return PropertyHyperEdge1.Equals(PropertyHyperEdge2);

        }

        #endregion

        #region Operator != (PropertyHyperEdge1, PropertyHyperEdge2)

        /// <summary>
        /// Compares two edges for equality.
        /// </summary>
        /// <param name="PropertyHyperEdge1">A Edge.</param>
        /// <param name="PropertyHyperEdge2">Another Edge.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PropertyHyperEdge<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                             TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                             TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                             TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>
                                                             PropertyHyperEdge1,
                                           PropertyHyperEdge<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                             TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                             TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                             TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>
                                                             PropertyHyperEdge2)
        {
            return !(PropertyHyperEdge1 == PropertyHyperEdge2);
        }

        #endregion

        #region Operator <  (PropertyHyperEdge1, PropertyHyperEdge2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PropertyHyperEdge1">A Edge.</param>
        /// <param name="PropertyHyperEdge2">Another Edge.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <  (PropertyHyperEdge<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                             TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                             TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                             TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>
                                                             PropertyHyperEdge1,
                                           PropertyHyperEdge<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                             TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                             TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                             TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>
                                                             PropertyHyperEdge2)
        {

            if ((Object) PropertyHyperEdge1 == null)
                throw new ArgumentNullException("The given PropertyHyperEdge1 must not be null!");

            if ((Object) PropertyHyperEdge2 == null)
                throw new ArgumentNullException("The given PropertyHyperEdge2 must not be null!");

            return PropertyHyperEdge1.CompareTo(PropertyHyperEdge2) < 0;

        }

        #endregion

        #region Operator <= (PropertyHyperEdge1, PropertyHyperEdge2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PropertyHyperEdge1">A Edge.</param>
        /// <param name="PropertyHyperEdge2">Another Edge.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (PropertyHyperEdge<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                             TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                             TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                             TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>
                                                             PropertyHyperEdge1,
                                           PropertyHyperEdge<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                             TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                             TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                             TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>
                                                             PropertyHyperEdge2)
        {
            return !(PropertyHyperEdge1 > PropertyHyperEdge2);
        }

        #endregion

        #region Operator >  (PropertyHyperEdge1, PropertyHyperEdge2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PropertyHyperEdge1">A Edge.</param>
        /// <param name="PropertyHyperEdge2">Another Edge.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >  (PropertyHyperEdge<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                             TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                             TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                             TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>
                                                             PropertyHyperEdge1,
                                           PropertyHyperEdge<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                             TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                             TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                             TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>
                                                             PropertyHyperEdge2)
        {

            if ((Object) PropertyHyperEdge1 == null)
                throw new ArgumentNullException("The given PropertyHyperEdge1 must not be null!");

            if ((Object) PropertyHyperEdge2 == null)
                throw new ArgumentNullException("The given PropertyHyperEdge2 must not be null!");

            return PropertyHyperEdge1.CompareTo(PropertyHyperEdge2) > 0;

        }

        #endregion

        #region Operator >= (PropertyHyperEdge1, PropertyHyperEdge2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PropertyHyperEdge1">A Edge.</param>
        /// <param name="PropertyHyperEdge2">Another Edge.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (PropertyHyperEdge<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                             TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                             TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                             TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>
                                                             PropertyHyperEdge1,
                                           PropertyHyperEdge<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                             TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                             TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                             TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>
                                                             PropertyHyperEdge2)
        {
            return !(PropertyHyperEdge1 < PropertyHyperEdge2);
        }

        #endregion

        #endregion

        #region IDynamicGraphObject<PropertyHyperEdge> Members

        #region GetMetaObject(myExpression)

        /// <summary>
        /// Return the appropriate DynamicMetaObject.
        /// </summary>
        /// <param name="myExpression">An Expression.</param>
        public DynamicMetaObject GetMetaObject(Expression myExpression)
        {
            return new DynamicGraphElement<PropertyHyperEdge<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                             TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                             TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                             TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>
                                                             (myExpression, this);
        }

        #endregion

        #region GetDynamicMemberNames()

        /// <summary>
        /// Returns an enumeration of all property keys.
        /// </summary>
        public virtual IEnumerable<String> GetDynamicMemberNames()
        {
            foreach (var _PropertyKey in PropertyData.Keys)
                yield return _PropertyKey.ToString();
        }

        #endregion


        #region SetMember(myBinder, myObject)

        /// <summary>
        /// Sets a new property or overwrites an existing.
        /// </summary>
        /// <param name="myBinder">The property key</param>
        /// <param name="myObject">The property value</param>
        public virtual Object SetMember(String myBinder, Object myObject)
        {
            return PropertyData.SetProperty((TKeyHyperEdge) (Object) myBinder, (TValueHyperEdge) myObject);
        }

        #endregion

        #region GetMember(myBinder)

        /// <summary>
        /// Returns the value of the given property key.
        /// </summary>
        /// <param name="myBinder">The property key.</param>
        public virtual Object GetMember(String myBinder)
        {
            TValueHyperEdge myObject;
            PropertyData.GetProperty((TKeyHyperEdge) (Object) myBinder, out myObject);
            return myObject as Object;
        }

        #endregion

        #region DeleteMember(myBinder)

        /// <summary>
        /// Tries to remove the property identified by the given property key.
        /// </summary>
        /// <param name="myBinder">The property key.</param>
        public virtual Object DeleteMember(String myBinder)
        {

            try
            {
                PropertyData.Remove((TKeyHyperEdge)(Object)myBinder);
                return true;
            }
            catch
            { }

            return false;

        }

        #endregion

        #endregion

        #region IComparable Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given Object must not be null!");

            // Check if the given object can be casted to a PropertyHyperEdge
            var PropertyHyperEdge = Object as PropertyHyperEdge<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>;

            if ((Object) PropertyHyperEdge == null)
                throw new ArgumentException("The given object is not a PropertyHyperEdge!");

            return CompareTo(PropertyHyperEdge);

        }

        #endregion

        #region CompareTo(HyperEdgeId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HyperEdgeId">An object to compare with.</param>
        public Int32 CompareTo(TIdHyperEdge HyperEdgeId)
        {

            if ((Object) HyperEdgeId == null)
                throw new ArgumentNullException("The given HyperEdgeId must not be null!");

            return Id.CompareTo(HyperEdgeId);

        }

        #endregion

        #region CompareTo(IGraphElement)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="IGraphElement">An object to compare with.</param>
        public Int32 CompareTo(IGraphElement<TIdHyperEdge, TRevisionIdHyperEdge, TKeyHyperEdge, TValueHyperEdge> IGraphElement)
        {

            if ((Object) IGraphElement == null)
                throw new ArgumentNullException("The given IGraphElement must not be null!");

            return Id.CompareTo(IGraphElement.PropertyData[IdKey]);

        }

        #endregion

        #region CompareTo(IPropertyHyperEdge)

        /// <summary>
        /// Compares two property multiedges.
        /// </summary>
        /// <param name="IPropertyHyperEdge">A property edge to compare with.</param>
        public Int32 CompareTo(IGenericPropertyHyperEdge<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                  TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                  TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                  TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> IPropertyHyperEdge)
        {

            if ((Object) IPropertyHyperEdge == null)
                throw new ArgumentNullException("The given IPropertyHyperEdge must not be null!");

            return Id.CompareTo(IPropertyHyperEdge.PropertyData[IdKey]);

        }

        #endregion

        #endregion

        #region IEquatable Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            // Check if the given object can be casted to a PropertyHyperEdge
            var PropertyHyperEdge = Object as PropertyHyperEdge<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>;

            if ((Object) PropertyHyperEdge == null)
                return false;

            return this.Equals(PropertyHyperEdge);

        }

        #endregion

        #region Equals(HyperEdgeId)

        /// <summary>
        /// Compares this property edge to an hyperedge identification.
        /// </summary>
        /// <param name="HyperEdgeId">A multiedge identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(TIdHyperEdge HyperEdgeId)
        {

            if ((Object) HyperEdgeId == null)
                return false;

            return Id.Equals(HyperEdgeId);

        }

        #endregion

        #region Equals(IGraphElement)

        /// <summary>
        /// Compares this property edge to another property element.
        /// </summary>
        /// <param name="IGraphElement">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(IGraphElement<TIdHyperEdge, TRevisionIdHyperEdge, TKeyHyperEdge, TValueHyperEdge> IGraphElement)
        {

            if ((Object) IGraphElement == null)
                return false;

            //TODO: Here it might be good to check all attributes of the UNIQUE constraint!
            return Id.Equals(IGraphElement.PropertyData[IdKey]);

        }

        #endregion

        #region Equals(IPropertyHyperEdge)

        /// <summary>
        /// Compares two property multiedges for equality.
        /// </summary>
        /// <param name="IPropertyHyperEdge">A property edge to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(IGenericPropertyHyperEdge<TIdVertex,    TRevisionIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                 TIdEdge,      TRevisionIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                 TIdMultiEdge, TRevisionIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                 TIdHyperEdge, TRevisionIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> IPropertyHyperEdge)
        {

            if ((Object) IPropertyHyperEdge == null)
                return false;

            //TODO: Here it might be good to check all attributes of the UNIQUE constraint!
            return Id.Equals(IPropertyHyperEdge.PropertyData[IdKey]);

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

        #region ToString()

        /// <summary>
        /// Returns a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return "PropertyHyperEdge [Id: '" + Id.ToString() + "']";
        }

        #endregion

    }

}
