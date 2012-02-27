﻿/*
 * Copyright (c) 2010-2012, Achim 'ahzf' Friedland <code@ahzf.de>
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
using System.Linq;
using System.Linq.Expressions;

using de.ahzf.Illias.Commons;

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
    /// <typeparam name="TRevIdVertex">The type of the vertex revision identifiers.</typeparam>
    /// <typeparam name="TVertexLabel">The type of the vertex type.</typeparam>
    /// <typeparam name="TKeyVertex">The type of the vertex property keys.</typeparam>
    /// <typeparam name="TValueVertex">The type of the vertex property values.</typeparam>
    /// <typeparam name="TPropertiesCollectionVertex">A data structure to store the properties of the vertices.</typeparam>
    /// 
    /// <typeparam name="TIdEdge">The type of the edge identifiers.</typeparam>
    /// <typeparam name="TRevIdEdge">The type of the edge revision identifiers.</typeparam>
    /// <typeparam name="TEdgeLabel">The type of the edge label.</typeparam>
    /// <typeparam name="TKeyEdge">The type of the edge property keys.</typeparam>
    /// <typeparam name="TValueEdge">The type of the edge property values.</typeparam>
    /// <typeparam name="TPropertiesCollectionEdge">A data structure to store the properties of the edges.</typeparam>
    /// 
    /// <typeparam name="TIdMultiEdge">The type of the multiedge identifiers.</typeparam>
    /// <typeparam name="TRevIdMultiEdge">The type of the multiedge revision identifiers.</typeparam>
    /// <typeparam name="TMultiEdgeLabel">The type of the multiedge label.</typeparam>
    /// <typeparam name="TKeyMultiEdge">The type of the multiedge property keys.</typeparam>
    /// <typeparam name="TValueMultiEdge">The type of the multiedge property values.</typeparam>
    /// <typeparam name="TPropertiesCollectionMultiEdge">A data structure to store the properties of the multiedges.</typeparam>
    /// 
    /// <typeparam name="TIdHyperEdge">The type of the multiedge identifiers.</typeparam>
    /// <typeparam name="TRevIdHyperEdge">The type of the multiedge revision identifiers.</typeparam>
    /// <typeparam name="THyperEdgeLabel">The type of the multiedge label.</typeparam>
    /// <typeparam name="TKeyHyperEdge">The type of the multiedge property keys.</typeparam>
    /// <typeparam name="TValueHyperEdge">The type of the multiedge property values.</typeparam>
    /// <typeparam name="TPropertiesCollectionHyperEdge">A data structure to store the properties of the multiedges.</typeparam>
    /// 
    /// <typeparam name="TEdgesCollection">A data structure to store edges.</typeparam>
    public class GenericPropertyMultiEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                          TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                          TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                          TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>

                                          : AGraphElement<TIdMultiEdge, TRevIdMultiEdge, TKeyMultiEdge, TValueMultiEdge>,

                                            IGenericPropertyMultiEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                      TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                      TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                      TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>,

                                            IDynamicGraphElement<GenericPropertyMultiEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                                          TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                                          TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                                          TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>


        where TIdVertex        : IEquatable<TIdVertex>,       IComparable<TIdVertex>,       IComparable, TValueVertex
        where TIdEdge          : IEquatable<TIdEdge>,         IComparable<TIdEdge>,         IComparable, TValueEdge
        where TIdMultiEdge     : IEquatable<TIdMultiEdge>,    IComparable<TIdMultiEdge>,    IComparable, TValueMultiEdge
        where TIdHyperEdge     : IEquatable<TIdHyperEdge>,    IComparable<TIdHyperEdge>,    IComparable, TValueHyperEdge

        where TRevIdVertex     : IEquatable<TRevIdVertex>,    IComparable<TRevIdVertex>,    IComparable, TValueVertex
        where TRevIdEdge       : IEquatable<TRevIdEdge>,      IComparable<TRevIdEdge>,      IComparable, TValueEdge
        where TRevIdMultiEdge  : IEquatable<TRevIdMultiEdge>, IComparable<TRevIdMultiEdge>, IComparable, TValueMultiEdge
        where TRevIdHyperEdge  : IEquatable<TRevIdHyperEdge>, IComparable<TRevIdHyperEdge>, IComparable, TValueHyperEdge

        where TVertexLabel     : IEquatable<TVertexLabel>,    IComparable<TVertexLabel>,    IComparable
        where TEdgeLabel       : IEquatable<TEdgeLabel>,      IComparable<TEdgeLabel>,      IComparable
        where TMultiEdgeLabel  : IEquatable<TMultiEdgeLabel>, IComparable<TMultiEdgeLabel>, IComparable
        where THyperEdgeLabel  : IEquatable<THyperEdgeLabel>, IComparable<THyperEdgeLabel>, IComparable

        where TKeyVertex       : IEquatable<TKeyVertex>,      IComparable<TKeyVertex>,      IComparable
        where TKeyEdge         : IEquatable<TKeyEdge>,        IComparable<TKeyEdge>,        IComparable
        where TKeyMultiEdge    : IEquatable<TKeyMultiEdge>,   IComparable<TKeyMultiEdge>,   IComparable
        where TKeyHyperEdge    : IEquatable<TKeyHyperEdge>,   IComparable<TKeyHyperEdge>,   IComparable

    {

        #region Data

        /// <summary>
        /// The edges wrapped by this multiedge.
        /// </summary>
        protected readonly IGroupedCollection<TEdgeLabel, TIdEdge, IGenericPropertyEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                                        TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                                        TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                                        TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> _Edges;

        #endregion

        #region Properties

        #region Graph

        /// <summary>
        /// The associated property graph.
        /// </summary>
        public IGenericPropertyGraph<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,  
                                     TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,    
                                     TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                     TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> Graph { get; private set; }

        #endregion

        #region Label

        /// <summary>
        /// The label associated with this multiedge.
        /// </summary>
        public TMultiEdgeLabel Label { get; private set; }

        #endregion

        #endregion

        #region Constructor(s)

        #region GenericPropertyMultiEdge(Graph, Edges, MultiEdgeId, Label, IdKey, RevIdKey, DescriptionKey, DatastructureInitializer, EdgesCollectionInitializer, MultiEdgeInitializer = null)

        /// <summary>
        /// Creates a new edge.
        /// </summary>
        /// <param name="Graph">The associated property graph.</param>
        /// <param name="EdgeSelector">An enumeration of edges.</param>
        /// <param name="MultiEdgeId">The identification of this multiedge.</param>
        /// <param name="Label">A label stored within this multiedge.</param>
        /// <param name="IdKey">The key to access the Id of this multiedge.</param>
        /// <param name="RevIdKey">The key to access the RevId of this multiedge.</param>
        /// <param name="DatastructureInitializer">A delegate to initialize the properties datastructure.</param>
        /// <param name="EdgesCollectionInitializer">A delegate to initialize the datastructure for storing the edges.</param>
        /// <param name="MultiEdgeInitializer">A delegate to initialize the newly created multiedge.</param>
        public GenericPropertyMultiEdge(IGenericPropertyGraph<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                              TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                              TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                              TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> Graph,

                                        MultiEdgeEdgeSelector<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                              TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                              TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                              TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeSelector,
                                       
                                        TIdMultiEdge    MultiEdgeId,
                                        TMultiEdgeLabel Label,
                                        TKeyMultiEdge   IdKey,
                                        TKeyMultiEdge   DescriptionKey,
                                        TKeyMultiEdge   RevIdKey,

                                        IDictionaryInitializer<TKeyMultiEdge, TValueMultiEdge> DatastructureInitializer,

                                        Func<IGroupedCollection<TEdgeLabel, TIdEdge, IGenericPropertyEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                                                          TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                                                          TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                                                          TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>> EdgesCollectionInitializer,
                                       
                                        Action<IGenericPropertyMultiEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                         TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                         TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                         TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>> MultiEdgeInitializer = null)

            : base(MultiEdgeId, IdKey, RevIdKey, DescriptionKey, DatastructureInitializer)

        {

            #region Initial checks

            if (Graph == null)
                throw new ArgumentNullException("The given graph must not be null!");

            if (MultiEdgeId == null)
                throw new ArgumentNullException("The given MultiEdgeId must not be null!");

            if (IdKey == null)
                throw new ArgumentNullException("The given IdKey must not be null!");

            if (RevIdKey == null)
                throw new ArgumentNullException("The given RevIdKey must not be null!");

            if (DatastructureInitializer == null)
                throw new ArgumentNullException("The given PropertiesCollectionInitializer must not be null!");

            if (EdgesCollectionInitializer == null)
                throw new ArgumentNullException("The given EdgesCollectionInitializer must not be null!");

            if (EdgeSelector == null)
                throw new ArgumentNullException("The given EdgeSelector delegate must not be null!");

            #endregion

            this.Graph = Graph;

            _Edges = EdgesCollectionInitializer();

            // Add the label
            //_Properties.Add(__Label, myLabel);

            if (MultiEdgeInitializer != null)
                MultiEdgeInitializer(this);

        }

        #endregion

        #endregion


        #region Edges

        #region EdgesByLabel(params EdgeLabels)

        /// <summary>
        /// The enumeration of all edges connected by this multiedge.
        /// </summary>
        public IEnumerable<IGenericPropertyEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>

                   EdgesByLabel(params TEdgeLabel[] EdgeLabels)

        {
            throw new NotImplementedException();
        }

        #endregion

        #region Edges(EdgeFilter = null)

        /// <summary>
        /// The enumeration of all edges connected by this multiedge.
        /// An optional edge filter may be applied for filtering.
        /// </summary>
        public IEnumerable<IGenericPropertyEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>
            
                   Edges(EdgeFilter<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                    TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                    TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                    TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> EdgeFilter = null)

        {
            throw new NotImplementedException();
        }

        #endregion

        #region NumberOfEdges(EdgeFilter = null)

        /// <summary>
        /// Return the current number of edges which match the given optional filter.
        /// When the filter is null, this method should implement an optimized
        /// way to get the currenty number of edges.
        /// </summary>
        /// <param name="EdgeFilter">A delegate for edge filtering.</param>
        public UInt64 NumberOfEdges(EdgeFilter<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                               TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                               TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                               TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>

                   EdgeFilter = null)

        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion



        public Boolean CheckIfMatches(IGenericPropertyEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                           TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                           TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                           TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> Edge)
        {
            throw new NotImplementedException();
        }

        public Boolean AddIfMatches(IGenericPropertyEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                         TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                         TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                         TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> Edge)
        {
            throw new NotImplementedException();
        }


        #region Operator overloading

        #region Operator == (PropertyMultiEdge1, PropertyMultiEdge)

        /// <summary>
        /// Compares two multiedges for equality.
        /// </summary>
        /// <param name="PropertyMultiEdge1">A Edge.</param>
        /// <param name="PropertyMultiEdge2">Another Edge.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (GenericPropertyMultiEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                    TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                    TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                    TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>
                                                                    PropertyMultiEdge1,
                                           GenericPropertyMultiEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                    TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                    TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                    TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>
                                                                    PropertyMultiEdge2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(PropertyMultiEdge1, PropertyMultiEdge2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) PropertyMultiEdge1 == null) || ((Object) PropertyMultiEdge2 == null))
                return false;

            return PropertyMultiEdge1.Equals(PropertyMultiEdge2);

        }

        #endregion

        #region Operator != (PropertyMultiEdge1, PropertyMultiEdge)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PropertyMultiEdge1">A Edge.</param>
        /// <param name="PropertyMultiEdge">Another Edge.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (GenericPropertyMultiEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                    TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                    TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                    TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>
                                                                    PropertyMultiEdge1,
                                           GenericPropertyMultiEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                    TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                    TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                    TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>
                                                                    PropertyMultiEdge2)
        {
            return !(PropertyMultiEdge1 == PropertyMultiEdge2);
        }

        #endregion

        #region Operator <  (PropertyMultiEdge1, PropertyMultiEdge2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PropertyMultiEdge1">A Edge.</param>
        /// <param name="PropertyMultiEdge2">Another Edge.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (GenericPropertyMultiEdge <TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                    TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                    TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                    TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>
                                                                    PropertyMultiEdge1,
                                          GenericPropertyMultiEdge <TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                    TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                    TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                    TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>
                                                                    PropertyMultiEdge2)
        {

            if ((Object) PropertyMultiEdge1 == null)
                throw new ArgumentNullException("The given PropertyMultiEdge1 must not be null!");

            if ((Object) PropertyMultiEdge2 == null)
                throw new ArgumentNullException("The given PropertyMultiEdge must not be null!");

            return PropertyMultiEdge1.CompareTo(PropertyMultiEdge2) < 0;

        }

        #endregion

        #region Operator <= (PropertyMultiEdge1, PropertyMultiEdge2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PropertyMultiEdge1">A Edge.</param>
        /// <param name="PropertyMultiEdge2">Another Edge.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (GenericPropertyMultiEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                    TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                    TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                    TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>
                                                                    PropertyMultiEdge1,
                                           GenericPropertyMultiEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                    TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                    TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                    TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>
                                                                    PropertyMultiEdge2)
        {
            return !(PropertyMultiEdge1 > PropertyMultiEdge2);
        }

        #endregion

        #region Operator >  (PropertyMultiEdge1, PropertyMultiEdge2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PropertyMultiEdge1">A Edge.</param>
        /// <param name="PropertyMultiEdge2">Another Edge.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (GenericPropertyMultiEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                   TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                   TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                   TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>
                                                                   PropertyMultiEdge1,
                                          GenericPropertyMultiEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                   TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                   TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                   TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>
                                                                   PropertyMultiEdge2)
        {

            if ((Object) PropertyMultiEdge1 == null)
                throw new ArgumentNullException("The given PropertyMultiEdge1 must not be null!");

            if ((Object) PropertyMultiEdge2 == null)
                throw new ArgumentNullException("The given PropertyMultiEdge2 must not be null!");

            return PropertyMultiEdge1.CompareTo(PropertyMultiEdge2) > 0;

        }

        #endregion

        #region Operator >= (PropertyMultiEdge1, PropertyMultiEdge2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PropertyMultiEdge1">A Edge.</param>
        /// <param name="PropertyMultiEdge2">Another Edge.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (GenericPropertyMultiEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                    TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                    TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                    TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>
                                                                    PropertyMultiEdge1,
                                           GenericPropertyMultiEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                    TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                    TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                    TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>
                                                                    PropertyMultiEdge2)
        {
            return !(PropertyMultiEdge1 < PropertyMultiEdge2);
        }

        #endregion

        #endregion

        #region IDynamicGraphObject<PropertyMultiEdge> Members

        #region GetMetaObject(myExpression)

        /// <summary>
        /// Return the appropriate DynamicMetaObject.
        /// </summary>
        /// <param name="myExpression">An Expression.</param>
        public DynamicMetaObject GetMetaObject(Expression myExpression)
        {
            return new DynamicGraphElement<GenericPropertyMultiEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                    TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                    TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                    TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>>
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
            return SetProperty((TKeyMultiEdge) (Object) myBinder, (TValueMultiEdge) myObject);
        }

        #endregion

        #region GetMember(myBinder)

        /// <summary>
        /// Returns the value of the given property key.
        /// </summary>
        /// <param name="myBinder">The property key.</param>
        public virtual Object GetMember(String myBinder)
        {
            TValueMultiEdge myObject;
            TryGet((TKeyMultiEdge) (Object) myBinder, out myObject);
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
                PropertyData.Remove((TKeyMultiEdge) (Object) myBinder);
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

            // Check if the given object can be casted to a PropertyMultiEdge
            var PropertyMultiEdge = Object as GenericPropertyMultiEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                       TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                       TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                       TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>;
            if ((Object) PropertyMultiEdge == null)
                throw new ArgumentException("The given object is not a PropertyMultiEdge!");

            return CompareTo(PropertyMultiEdge);

        }

        #endregion

        #region CompareTo(IGenericPropertyMultiEdge)

        /// <summary>
        /// Compares two generic property multiedges.
        /// </summary>
        /// <param name="IGenericPropertyMultiEdge">A generic property multiedge to compare with.</param>
        public Int32 CompareTo(IGenericPropertyMultiEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                         TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                         TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                         TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> IGenericPropertyMultiEdge)
        {

            if ((Object) IGenericPropertyMultiEdge == null)
                throw new ArgumentNullException("The given IGenericPropertyMultiEdge must not be null!");

            return Id.CompareTo(IGenericPropertyMultiEdge[IdKey]);

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

            // Check if the given object can be casted to a PropertyMultiEdge
            var PropertyMultiEdge = Object as GenericPropertyMultiEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                                       TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                                       TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                                       TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge>;
            if ((Object) PropertyMultiEdge == null)
                return false;

            return this.Equals(PropertyMultiEdge);

        }

        #endregion

        #region Equals(IGenericPropertyMultiEdge)

        /// <summary>
        /// Compares two generic property multiedges for equality.
        /// </summary>
        /// <param name="IGenericPropertyMultiEdge">A generic property multiedge to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(IGenericPropertyMultiEdge<TIdVertex,    TRevIdVertex,    TVertexLabel,    TKeyVertex,    TValueVertex,
                                                        TIdEdge,      TRevIdEdge,      TEdgeLabel,      TKeyEdge,      TValueEdge,
                                                        TIdMultiEdge, TRevIdMultiEdge, TMultiEdgeLabel, TKeyMultiEdge, TValueMultiEdge,
                                                        TIdHyperEdge, TRevIdHyperEdge, THyperEdgeLabel, TKeyHyperEdge, TValueHyperEdge> IGenericPropertyMultiEdge)
        {

            if ((Object) IGenericPropertyMultiEdge == null)
                return false;

            //TODO: Here it might be good to check all attributes of the UNIQUE constraint!
            return Id.Equals(IGenericPropertyMultiEdge[IdKey]);

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
            return "GenericPropertyMultiEdge [Id: " + Id.ToString() + "']";
        }

        #endregion

    }

}


