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

using de.ahzf.blueprints.Datastructures;

#endregion

namespace de.ahzf.blueprints
{

    /// <summary>
    /// An edge links two vertices. Along with its key/value properties,
    /// an edge has both a directionality and a label.
    /// The directionality determines which vertex is the tail vertex
    /// (out vertex) and which vertex is the head vertex (in vertex).
    /// The edge label determines the type of relationship that exists
    /// between the two vertices.
    /// Diagrammatically, outVertex ---label---> inVertex.
    /// </summary>
    public interface IEdge : IEdge<EdgeId, String>
    { }


    /// <summary>
    /// An edge links two vertices. Along with its key/value properties,
    /// an edge has both a directionality and a label.
    /// The directionality determines which vertex is the tail vertex
    /// (out vertex) and which vertex is the head vertex (in vertex).
    /// The edge label determines the type of relationship that exists
    /// between the two vertices.
    /// Diagrammatically, outVertex ---label---> inVertex.
    /// </summary>
    /// <typeparam name="TId">The type of the id.</typeparam>
    /// <typeparam name="TKey">The type of the property keys.</typeparam>
    public interface IEdge<TId, TKey> : IGenericEdge<TId, IProperties<TKey>>, IProperties<TKey>, IElement<TId, TKey>
        where TId : IEquatable<TId>, IComparable<TId>, IComparable
        where TKey : IEquatable<TKey>, IComparable<TKey>, IComparable
    { }

}
