﻿/*
 * Copyright (c) 2010-2011, Achim 'ahzf' Friedland <code@ahzf.de>
 * This file is part of Blueprints.NET
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
using de.ahzf.blueprints.Datastructures;

#endregion

namespace de.ahzf.blueprints
{

    public interface IPropertyGraph : IPropertyGraph<// Vertex
                                                     VertexId,    RevisionId, String, Object, IDictionary<String, Object>,
                                                     IGenericVertex<VertexId,    RevisionId, IProperties<String, Object>,
                                                                    EdgeId,      RevisionId, IProperties<String, Object>,
                                                                    HyperEdgeId, RevisionId, IProperties<String, Object>>,

                                                     // Edge
                                                     EdgeId,      RevisionId, String, Object, IDictionary<String, Object>,
                                                     IGenericEdge<VertexId,    RevisionId, IProperties<String, Object>,
                                                                  EdgeId,      RevisionId, IProperties<String, Object>,
                                                                  HyperEdgeId, RevisionId, IProperties<String, Object>>,

                                                     // HyperEdge
                                                     HyperEdgeId, RevisionId, String, Object, IDictionary<String, Object>,
                                                     IGenericHyperEdge<VertexId,    RevisionId, IProperties<String, Object>,
                                                                       EdgeId,      RevisionId, IProperties<String, Object>,
                                                                       HyperEdgeId, RevisionId, IProperties<String, Object>>,
                                                     Object>

    { }

    /// <summary>
    /// A property graph is a container object for a collection of vertices and edges.
    /// </summary>
    public interface IPropertyGraph<TIdVertex,    TRevisionIdVertex,    TKeyVertex,    TValueVertex,    TDatastructureVertex,    TVertexExchange,
                                    TIdEdge,      TRevisionIdEdge,      TKeyEdge,      TValueEdge,      TDatastructureEdge,      TEdgeExchange,
                                    TIdHyperEdge, TRevisionIdHyperEdge, TKeyHyperEdge, TValueHyperEdge, TDatastructureHyperEdge, THyperEdgeExchange,                                  
                                    TGraphDatastructure>

                     : IGenericGraph<   // Vertex definition
                                        IGenericVertex<TIdVertex,    TRevisionIdVertex,    IProperties<TKeyVertex,    TValueVertex>,
                                                       TIdEdge,      TRevisionIdEdge,      IProperties<TKeyEdge,      TValueEdge>,
                                                       TIdHyperEdge, TRevisionIdHyperEdge, IProperties<TKeyHyperEdge, TValueHyperEdge>>,                                        
                                        TIdVertex,
                                        TRevisionIdVertex,
                                        IProperties<TKeyVertex, TValueVertex>,
                                        TVertexExchange,

                                        // Edge definition
                                        IGenericEdge<TIdVertex,    TRevisionIdVertex,    IProperties<TKeyVertex,    TValueVertex>,
                                                     TIdEdge,      TRevisionIdEdge,      IProperties<TKeyEdge,      TValueEdge>,
                                                     TIdHyperEdge, TRevisionIdHyperEdge, IProperties<TKeyHyperEdge, TValueHyperEdge>>,
                                        TIdEdge,
                                        TRevisionIdEdge,
                                        IProperties<TKeyEdge, TValueEdge>,
                                        TEdgeExchange,

                                        // Hyperedge definition
                                        IGenericHyperEdge<TIdVertex,    TRevisionIdVertex,    IProperties<TKeyVertex,    TValueVertex>,
                                                          TIdEdge,      TRevisionIdEdge,      IProperties<TKeyEdge,      TValueEdge>,
                                                          TIdHyperEdge, TRevisionIdHyperEdge, IProperties<TKeyHyperEdge, TValueHyperEdge>>,
                                        TIdHyperEdge,
                                        TRevisionIdHyperEdge,
                                        IProperties<TKeyHyperEdge, TValueHyperEdge>,
                                        THyperEdgeExchange,

                                        // Rest...
                                        TGraphDatastructure>

        where TDatastructureVertex    : IDictionary<TKeyVertex,    TValueVertex>
        where TDatastructureEdge      : IDictionary<TKeyEdge,      TValueEdge>
        where TDatastructureHyperEdge : IDictionary<TKeyHyperEdge, TValueHyperEdge>

        where TKeyVertex              : IEquatable<TKeyVertex>,           IComparable<TKeyVertex>,           IComparable
        where TKeyEdge                : IEquatable<TKeyEdge>,             IComparable<TKeyEdge>,             IComparable
        where TKeyHyperEdge           : IEquatable<TKeyHyperEdge>,        IComparable<TKeyHyperEdge>,        IComparable
                                                                                                            
        where TIdVertex               : IEquatable<TIdVertex>,            IComparable<TIdVertex>,            IComparable, TValueVertex
        where TIdEdge                 : IEquatable<TIdEdge>,              IComparable<TIdEdge>,              IComparable, TValueEdge
        where TIdHyperEdge            : IEquatable<TIdHyperEdge>,         IComparable<TIdHyperEdge>,         IComparable, TValueHyperEdge

        where TRevisionIdVertex       : IEquatable<TRevisionIdVertex>,    IComparable<TRevisionIdVertex>,    IComparable, TValueVertex
        where TRevisionIdEdge         : IEquatable<TRevisionIdEdge>,      IComparable<TRevisionIdEdge>,      IComparable, TValueEdge
        where TRevisionIdHyperEdge    : IEquatable<TRevisionIdHyperEdge>, IComparable<TRevisionIdHyperEdge>, IComparable, TValueHyperEdge

        where TVertexExchange         : IGenericVertex   <TIdVertex,    TRevisionIdVertex,    IProperties<TKeyVertex,    TValueVertex>,
                                                          TIdEdge,      TRevisionIdEdge,      IProperties<TKeyEdge,      TValueEdge>,
                                                          TIdHyperEdge, TRevisionIdHyperEdge, IProperties<TKeyHyperEdge, TValueHyperEdge>>

        where TEdgeExchange           : IGenericEdge     <TIdVertex,    TRevisionIdVertex,    IProperties<TKeyVertex,    TValueVertex>,
                                                          TIdEdge,      TRevisionIdEdge,      IProperties<TKeyEdge,      TValueEdge>,
                                                          TIdHyperEdge, TRevisionIdHyperEdge, IProperties<TKeyHyperEdge, TValueHyperEdge>>

        where THyperEdgeExchange      : IGenericHyperEdge<TIdVertex,    TRevisionIdVertex,    IProperties<TKeyVertex,    TValueVertex>,
                                                          TIdEdge,      TRevisionIdEdge,      IProperties<TKeyEdge,      TValueEdge>,
                                                          TIdHyperEdge, TRevisionIdHyperEdge, IProperties<TKeyHyperEdge, TValueHyperEdge>>

    { }

}
