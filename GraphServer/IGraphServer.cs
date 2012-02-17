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

using de.ahzf.Hermod;
using de.ahzf.Hermod.HTTP;
using de.ahzf.Hermod.Datastructures;
using de.ahzf.Blueprints.PropertyGraphs;

#endregion

namespace de.ahzf.Blueprints.HTTP.Server
{

    /// <summary>
    /// Simple PropertyGraph HTTP/REST access.
    /// </summary>
    public interface IGraphServer : IHTTPServer,
                                    IEnumerable<IPropertyGraph>
    {

        /// <summary>
        /// Adds the given property graph to the server.
        /// </summary>
        /// <param name="PropertyGraph">An object implementing the IPropertyGraph interface.</param>
        IPropertyGraph AddPropertyGraph(IPropertyGraph PropertyGraph);

        /// <summary>
        /// Creates a new class-based in-memory implementation of a property graph
        /// and adds it to the server.
        /// </summary>
        /// <param name="GraphId">A unique identification for this graph (which is also a vertex!).</param>
        /// <param name="GraphInitializer">A delegate to initialize the new property graph.</param>
        IPropertyGraph NewPropertyGraph(UInt64 GraphId, GraphInitializer<UInt64, Int64, String, String, Object,
                                                                         UInt64, Int64, String, String, Object,
                                                                         UInt64, Int64, String, String, Object,
                                                                         UInt64, Int64, String, String, Object> GraphInitializer = null);


        /// <summary>
        /// Return the property graph identified by the given GraphId.
        /// </summary>
        /// <param name="GraphId">A property graph identifier.</param>
        IPropertyGraph GetPropertyGraph(UInt64 GraphId);

        /// <summary>
        /// Return the property graph identified by the given GraphId.
        /// </summary>
        /// <param name="GraphId">A property graph identifier.</param>
        Boolean TryGetPropertyGraph(UInt64 GraphId, out IPropertyGraph PropertyGraph);

        IEnumerable<IPropertyGraph> AllGraphs();


        /// <summary>
        /// Removes the property graph identified by the given Id.
        /// </summary>
        /// <param name="GraphId">The Id of the property graph to remove.</param>
        /// <returns>true on success, false </returns>
        Boolean RemovePropertyGraph(UInt64 GraphId);


        

    }

}