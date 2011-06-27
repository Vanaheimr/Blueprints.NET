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
using System.Collections.Concurrent;

#endregion

namespace de.ahzf.Blueprints.PropertyGraph.InMemory
{

    #region SimplePropertyGraph<TId, TRevisionId, TKey, TValue>

    /// <summary>
    /// A simplified in-memory implementation of a generic property graph.
    /// </summary>
    /// <typeparam name="TId">The type of the graph element identifiers.</typeparam>
    /// <typeparam name="TRevisionId">The type of the graph element revision identifiers.</typeparam>
    /// <typeparam name="TLabel">The type of the (hyper-)edge labels.</typeparam>
    /// <typeparam name="TKey">The type of the graph element property keys.</typeparam>
    /// <typeparam name="TValue">The type of the graph element property values.</typeparam>
    public class SimplePropertyGraph<TId, TRevisionId, TLabel, TKey, TValue>
                     : InMemoryGenericPropertyGraph<// Vertex definition
                                                    TId, TRevisionId,         TKey, TValue, IDictionary<TKey, TValue>,
                                                    ICollection<IPropertyEdge<TId, TRevisionId,         TKey, TValue,
                                                                              TId, TRevisionId, TLabel, TKey, TValue,
                                                                              TId, TRevisionId, TLabel, TKey, TValue>>,

                                                    // Edge definition
                                                    TId, TRevisionId, TLabel, TKey, TValue, IDictionary<TKey, TValue>,

                                                    // Hyperedge definition
                                                    TId, TRevisionId, TLabel, TKey, TValue, IDictionary<TKey, TValue>>

        where TId         : IEquatable<TId>,         IComparable<TId>,         IComparable, TValue
        where TRevisionId : IEquatable<TRevisionId>, IComparable<TRevisionId>, IComparable, TValue
        where TKey        : IEquatable<TKey>,        IComparable<TKey>,        IComparable

        where TLabel      : IEquatable<TLabel>,      IComparable<TLabel>,      IComparable
    {

        #region Delegates

        #region IdCreatorDelegate()

        /// <summary>
        /// A delegate for creating a new TId
        /// (if no one was provided by the user).
        /// </summary>
        /// <returns>A valid TId.</returns>
        public delegate TId IdCreatorDelegate();

        #endregion

        #region RevisionIdCreatorDelegate()

        /// <summary>
        /// A delegate for creating a new TRevisionId
        /// (if no one was provided by the user).
        /// </summary>
        /// <returns>A valid TRevisionId.</returns>
        public delegate TRevisionId RevisionIdCreatorDelegate();

        #endregion

        #endregion

        #region Constructor(s)

        #region InMemoryPropertyGraph()

        /// <summary>
        /// Created a new in-memory property graph.
        /// </summary>
        public SimplePropertyGraph(TId                       GraphId,
                                   TKey                      IdKey,
                                   IdCreatorDelegate         IdCreatorDelegate,
                                   TKey                      RevisionIdKey,
                                   RevisionIdCreatorDelegate RevisionIdCreatorDelegate,
                                   GraphInitializer<TId, TRevisionId,         TKey, TValue,
                                                    TId, TRevisionId, TLabel, TKey, TValue,
                                                    TId, TRevisionId, TLabel, TKey, TValue> GraphInitializer = null)
            : base (GraphId,
                    IdKey,
                    RevisionIdKey,
                    () => new Dictionary<TKey, TValue>(),

                    // Create a new Vertex
                    () => IdCreatorDelegate(),
                    (VertexId, VertexInitializer) =>
                        new PropertyVertex<TId, TRevisionId,         TKey, TValue, IDictionary<TKey, TValue>,
                                           TId, TRevisionId, TLabel, TKey, TValue, IDictionary<TKey, TValue>,
                                           TId, TRevisionId, TLabel, TKey, TValue, IDictionary<TKey, TValue>,
                                           ICollection<IPropertyEdge<TId, TRevisionId,         TKey, TValue,
                                                                     TId, TRevisionId, TLabel, TKey, TValue,
                                                                     TId, TRevisionId, TLabel, TKey, TValue>>>
                            (VertexId, IdKey, RevisionIdKey,
                             () => new Dictionary<TKey, TValue>(),
                             () => new HashSet<IPropertyEdge<TId, TRevisionId,         TKey, TValue,
                                                             TId, TRevisionId, TLabel, TKey, TValue,
                                                             TId, TRevisionId, TLabel, TKey, TValue>>(),
                             VertexInitializer
                            ),

                   // Create a new Edge
                   () => IdCreatorDelegate(),
                   (OutVertex, InVertex, EdgeId, Label, EdgeInitializer) =>
                        new PropertyEdge<TId, TRevisionId,         TKey, TValue, IDictionary<TKey, TValue>,
                                         TId, TRevisionId, TLabel, TKey, TValue, IDictionary<TKey, TValue>,
                                         TId, TRevisionId, TLabel, TKey, TValue, IDictionary<TKey, TValue>>
                            (OutVertex, InVertex, EdgeId, Label, IdKey, RevisionIdKey,
                             () => new Dictionary<TKey, TValue>(),
                             EdgeInitializer
                            ),


                   // Create a new HyperEdge
                   () => IdCreatorDelegate(),
                   (Edges, HyperEdgeId, Label, HyperEdgeInitializer) =>
                       new PropertyHyperEdge<TId, TRevisionId,         TKey, TValue, IDictionary<TKey, TValue>,
                                             TId, TRevisionId, TLabel, TKey, TValue, IDictionary<TKey, TValue>,
                                             TId, TRevisionId, TLabel, TKey, TValue, IDictionary<TKey, TValue>,
                                             ICollection<IPropertyEdge<TId, TRevisionId,         TKey, TValue,
                                                                       TId, TRevisionId, TLabel, TKey, TValue,
                                                                       TId, TRevisionId, TLabel, TKey, TValue>>>
                            (Edges, HyperEdgeId, Label, IdKey, RevisionIdKey,
                             () => new Dictionary<TKey, TValue>(),
                             () => new HashSet<IPropertyEdge<TId, TRevisionId,         TKey, TValue,
                                                             TId, TRevisionId, TLabel, TKey, TValue,
                                                             TId, TRevisionId, TLabel, TKey, TValue>>(),
                             HyperEdgeInitializer
                            ),

                   // The vertices collection
                   new ConcurrentDictionary<TId, IPropertyVertex   <TId, TRevisionId,         TKey, TValue,
                                                                    TId, TRevisionId, TLabel, TKey, TValue,
                                                                    TId, TRevisionId, TLabel, TKey, TValue>>(),

                   // The edges collection
                   new ConcurrentDictionary<TId, IPropertyEdge     <TId, TRevisionId,         TKey, TValue,
                                                                    TId, TRevisionId, TLabel, TKey, TValue,
                                                                    TId, TRevisionId, TLabel, TKey, TValue>>(),

                   // The hyperedges collection
                   new ConcurrentDictionary<TId, IPropertyHyperEdge<TId, TRevisionId,         TKey, TValue,
                                                                    TId, TRevisionId, TLabel, TKey, TValue,
                                                                    TId, TRevisionId, TLabel, TKey, TValue>>(),

                   GraphInitializer)

        { }

        #endregion

        #endregion

    }

    #endregion

    #region SimplePropertyGraph

    /// <summary>
    /// A simplified in-memory implementation of a generic property graph.
    /// </summary>
    public class SimplePropertyGraph : SimplePropertyGraph<UInt64, Int64, String, String, Object>
    {

        #region Constructor(s)

        #region SimplePropertyGraph(GraphInitializer = null)

        /// <summary>
        /// Created a new simple in-memory property graph.
        /// </summary>
        /// <param name="GraphInitializer">A delegate to initialize the graph.</param>
        public SimplePropertyGraph(GraphInitializer<UInt64, Int64,         String, Object,
                                                    UInt64, Int64, String, String, Object,
                                                    UInt64, Int64, String, String, Object> GraphInitializer = null)
            : this(SimplePropertyGraph.NewId, GraphInitializer)
        { }

        #endregion

        #region SimplePropertyGraph(GraphId, GraphInitializer = null)

        /// <summary>
        /// Created a new simple in-memory property graph.
        /// </summary>
        /// <param name="GraphId">A unique identification for this graph.</param>
        /// <param name="GraphInitializer">A delegate to initialize the graph.</param>
        public SimplePropertyGraph(UInt64 GraphId,
                                   GraphInitializer<UInt64, Int64,         String, Object,
                                                    UInt64, Int64, String, String, Object,
                                                    UInt64, Int64, String, String, Object> GraphInitializer = null)
            : base (GraphId,

                    // TId key
                    "Id",

                    // TId creator delegate
                    () => SimplePropertyGraph.NewId,

                    // RevisionId key
                    "RevId",

                    // RevisionId creator delegate
                    () => SimplePropertyGraph.NewRevisionId,

                    GraphInitializer)

        { }

        #endregion

        #endregion

        #region (private) NewId

        /// <summary>
        /// Return a new random Id.
        /// </summary>
        private static UInt64 NewId
        {
            get
            {
                return (UInt64) new Random().Next(Int32.MaxValue);
            }
        }

        #endregion

        #region (private) NewRevisionId

        /// <summary>
        /// Return a new random RevisionId.
        /// </summary>
        private static Int64 NewRevisionId
        {
            get
            {
                return DateTime.Now.Ticks;
            }
        }

        #endregion

    }

    #endregion

}