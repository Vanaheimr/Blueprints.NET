﻿/*
 * Copyright (c) 2010-2012, Achim 'ahzf' Friedland <code@ahzf.de>
 * This file is part of Blueprints.NET <http://www.github.com/Vanaheimr/Blueprints.NET>
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
using System.Linq;
using System.Collections.Generic;

using de.ahzf.Blueprints.PropertyGraphs;
using de.ahzf.Hermod.HTTP;
using de.ahzf.Hermod.Datastructures;
using de.ahzf.Blueprints.PropertyGraphs.InMemory.Mutable;

#endregion

namespace de.ahzf.Blueprints.HTTP.Server
{

    /// <summary>
    /// Simple PropertyGraph HTTP/REST access (server).
    /// </summary>
    public class GraphServer : HTTPServer<IGraphService>,
                               IGraphServer
    {

        #region Data

        private readonly IDictionary<UInt64, IGenericPropertyGraph<UInt64, Int64, String, String, Object,
                                                                   UInt64, Int64, String, String, Object,
                                                                   UInt64, Int64, String, String, Object,
                                                                   UInt64, Int64, String, String, Object>> _PropertyGraphs;

        private readonly IDictionary<String, IGenericPropertyVertex<UInt64, Int64, String, String, Object,
                                                                    UInt64, Int64, String, String, Object,
                                                                    UInt64, Int64, String, String, Object,
                                                                    UInt64, Int64, String, String, Object>> VertexLookup;

        #endregion
        
        #region Properties

        #region ServerName

        /// <summary>
        /// The HTTP server name.
        /// </summary>
        public String ServerName
        {

            get
            {
                return base.ServerName;
            }

            set
            {
                base.ServerName = value;
            }

        }

        #endregion

        #endregion

        #region Constructor(s)

        #region GraphServer(PropertyGraph)

        /// <summary>
        /// Initialize the GraphServer using IPAddress.Any, http port 8182 and start the server.
        /// </summary>
        public GraphServer(IGenericPropertyGraph<UInt64, Int64, String, String, Object,
                                                 UInt64, Int64, String, String, Object,
                                                 UInt64, Int64, String, String, Object,
                                                 UInt64, Int64, String, String, Object> PropertyGraph)
            : base(IPv4Address.Any, new IPPort(80), Autostart: true)
        {

            #region Initial Checks

            if (PropertyGraph == null)
                throw new ArgumentNullException("PropertyGraph", "The given PropertyGraph must not be null!");

            #endregion

            _PropertyGraphs = new Dictionary<UInt64, IGenericPropertyGraph<UInt64, Int64, String, String, Object,
                                                                           UInt64, Int64, String, String, Object,
                                                                           UInt64, Int64, String, String, Object,
                                                                           UInt64, Int64, String, String, Object>>();
            _PropertyGraphs.Add(PropertyGraph.Id, PropertyGraph);

            this.ServerName    = DefaultServerName;
            this.VertexLookup  = new Dictionary<String, IGenericPropertyVertex<UInt64, Int64, String, String, Object,
                                                                               UInt64, Int64, String, String, Object,
                                                                               UInt64, Int64, String, String, Object,
                                                                               UInt64, Int64, String, String, Object>>();

            base.OnNewHTTPService += CentralService => { CentralService.GraphServer = this; };

        }

        #endregion

        #region GraphServer(PropertyGraph, Port, AutoStart = false)

        /// <summary>
        /// Initialize the GraphServer using IPAddress.Any and the given parameters.
        /// </summary>
        /// <param name="Port">The listening port</param>
        /// <param name="Autostart"></param>
        public GraphServer(IGenericPropertyGraph<UInt64, Int64, String, String, Object,
                                                 UInt64, Int64, String, String, Object,
                                                 UInt64, Int64, String, String, Object,
                                                 UInt64, Int64, String, String, Object> PropertyGraph, IPPort Port, Boolean Autostart = false)
            : base(IPv4Address.Any, Port, Autostart: true)
        {

            #region Initial Checks

            if (PropertyGraph == null)
                throw new ArgumentNullException("PropertyGraph", "The given PropertyGraph must not be null!");

            #endregion

            _PropertyGraphs = new Dictionary<UInt64, IGenericPropertyGraph<UInt64, Int64, String, String, Object,
                                                                           UInt64, Int64, String, String, Object,
                                                                           UInt64, Int64, String, String, Object,
                                                                           UInt64, Int64, String, String, Object>>();
            _PropertyGraphs.Add(PropertyGraph.Id, PropertyGraph);

            this.ServerName   = DefaultServerName;
            this.VertexLookup = new Dictionary<String, IGenericPropertyVertex<UInt64, Int64, String, String, Object,
                                                                              UInt64, Int64, String, String, Object,
                                                                              UInt64, Int64, String, String, Object,
                                                                              UInt64, Int64, String, String, Object>>();

            base.OnNewHTTPService += CentralService => { CentralService.GraphServer = this; };

        }

        #endregion

        #region GraphServer(PropertyGraph, IIPAddress, Port, AutoStart = false)

        /// <summary>
        /// Initialize the GraphServer using the given parameters.
        /// </summary>
        /// <param name="IIPAddress">The listening IP address(es)</param>
        /// <param name="Port">The listening port</param>
        /// <param name="Autostart"></param>
        public GraphServer(IGenericPropertyGraph<UInt64, Int64, String, String, Object,
                                                 UInt64, Int64, String, String, Object,
                                                 UInt64, Int64, String, String, Object,
                                                 UInt64, Int64, String, String, Object> PropertyGraph, IIPAddress IIPAddress, IPPort Port, Boolean Autostart = false)
            : base(IIPAddress, Port, Autostart: true)
        {

            #region Initial Checks

            if (PropertyGraph == null)
                throw new ArgumentNullException("PropertyGraph", "The given PropertyGraph must not be null!");

            #endregion

            _PropertyGraphs = new Dictionary<UInt64, IGenericPropertyGraph<UInt64, Int64, String, String, Object,
                                                                           UInt64, Int64, String, String, Object,
                                                                           UInt64, Int64, String, String, Object,
                                                                           UInt64, Int64, String, String, Object>>();
            _PropertyGraphs.Add(PropertyGraph.Id, PropertyGraph);

            this.ServerName   = DefaultServerName;
            this.VertexLookup = new Dictionary<String, IGenericPropertyVertex<UInt64, Int64, String, String, Object,
                                                                              UInt64, Int64, String, String, Object,
                                                                              UInt64, Int64, String, String, Object,
                                                                              UInt64, Int64, String, String, Object>>();

            base.OnNewHTTPService += CentralService => { CentralService.GraphServer = this; };

        }

        #endregion

        #region GraphServer(PropertyGraph, IPSocket, Autostart = false)

        /// <summary>
        /// Initialize the GraphServer using the given parameters.
        /// </summary>
        /// <param name="IPSocket">The listening IPSocket.</param>
        /// <param name="Autostart"></param>
        public GraphServer(IGenericPropertyGraph<UInt64, Int64, String, String, Object,
                                                 UInt64, Int64, String, String, Object,
                                                 UInt64, Int64, String, String, Object,
                                                 UInt64, Int64, String, String, Object> PropertyGraph, IPSocket IPSocket, Boolean Autostart = false)
            : base(IPSocket.IPAddress, IPSocket.Port, Autostart: true)
        {

            #region Initial Checks

            if (PropertyGraph == null)
                throw new ArgumentNullException("PropertyGraph", "The given PropertyGraph must not be null!");

            #endregion

            _PropertyGraphs = new Dictionary<UInt64, IGenericPropertyGraph<UInt64, Int64, String, String, Object,
                                                                           UInt64, Int64, String, String, Object,
                                                                           UInt64, Int64, String, String, Object,
                                                                           UInt64, Int64, String, String, Object>>();
            _PropertyGraphs.Add(PropertyGraph.Id, PropertyGraph);

            this.ServerName   = DefaultServerName;
            this.VertexLookup = new Dictionary<String, IGenericPropertyVertex<UInt64, Int64, String, String, Object,
                                                                              UInt64, Int64, String, String, Object,
                                                                              UInt64, Int64, String, String, Object,
                                                                              UInt64, Int64, String, String, Object>>();

            base.OnNewHTTPService += CentralService => { CentralService.GraphServer = this; };

        }

        #endregion

        #endregion


        #region IGraphServer Members

        #region AddPropertyGraph(PropertyGraph)

        /// <summary>
        /// Adds the given property graph to the server.
        /// </summary>
        /// <param name="PropertyGraph">An object implementing the IPropertyGraph interface.</param>
        public IGenericPropertyGraph<UInt64, Int64, String, String, Object,
                                     UInt64, Int64, String, String, Object,
                                     UInt64, Int64, String, String, Object,
                                     UInt64, Int64, String, String, Object>

            AddPropertyGraph(IGenericPropertyGraph<UInt64, Int64, String, String, Object,
                                                   UInt64, Int64, String, String, Object,
                                                   UInt64, Int64, String, String, Object,
                                                   UInt64, Int64, String, String, Object> PropertyGraph)

        {

            _PropertyGraphs.Add(PropertyGraph.Id, PropertyGraph);

            return PropertyGraph;

        }

        #endregion

        #region NewPropertyGraph(GraphId, GraphInitializer = null)

        /// <summary>
        /// Creates a new class-based in-memory implementation of a property graph
        /// and adds it to the server.
        /// </summary>
        /// <param name="GraphId">A unique identification for this graph (which is also a vertex!).</param>
        /// <param name="GraphInitializer">A delegate to initialize the new property graph.</param>
        public IGenericPropertyGraph<UInt64, Int64, String, String, Object,
                                     UInt64, Int64, String, String, Object,
                                     UInt64, Int64, String, String, Object,
                                     UInt64, Int64, String, String, Object>

            NewPropertyGraph(UInt64 GraphId,
                             String Description = null,
                             GraphInitializer<UInt64, Int64, String, String, Object,
                                              UInt64, Int64, String, String, Object,
                                              UInt64, Int64, String, String, Object,
                                              UInt64, Int64, String, String, Object> GraphInitializer = null)

        {

            return AddPropertyGraph(GraphFactory.CreateGenericPropertyGraph(GraphId, Description, GraphInitializer));

        }

        #endregion


        #region GetPropertyGraph(GraphId)

        /// <summary>
        /// Return the property graph identified by the given GraphId.
        /// </summary>
        /// <param name="GraphId">A property graph identifier.</param>
        public IGenericPropertyGraph<UInt64, Int64, String, String, Object,
                                     UInt64, Int64, String, String, Object,
                                     UInt64, Int64, String, String, Object,
                                     UInt64, Int64, String, String, Object> GetPropertyGraph(UInt64 GraphId)
        {
            return _PropertyGraphs[GraphId];
        }

        #endregion

        #region TryGetPropertyGraph(GraphId, out PropertyGraph)

        /// <summary>
        /// Return the property graph identified by the given GraphId.
        /// </summary>
        /// <param name="GraphId">A property graph identifier.</param>
        public Boolean TryGetPropertyGraph(UInt64 GraphId, out IGenericPropertyGraph<UInt64, Int64, String, String, Object,
                                                                                     UInt64, Int64, String, String, Object,
                                                                                     UInt64, Int64, String, String, Object,
                                                                                     UInt64, Int64, String, String, Object> PropertyGraph)
        {
            return _PropertyGraphs.TryGetValue(GraphId, out PropertyGraph);
        }

        #endregion

        #region AllGraphs()

        public IEnumerable<IGenericPropertyGraph<UInt64, Int64, String, String, Object,
                                                 UInt64, Int64, String, String, Object,
                                                 UInt64, Int64, String, String, Object,
                                                 UInt64, Int64, String, String, Object>> AllGraphs()
        {
            return _PropertyGraphs.Values;
        }

        #endregion


        #region RemovePropertyGraph(GraphId)

        public Boolean RemovePropertyGraph(UInt64 GraphId)
        {
            return _PropertyGraphs.Remove(GraphId);
        }

        #endregion

        #endregion

        #region IEnumerable<PropertyGraph> Members

        public IEnumerator<IGenericPropertyGraph<UInt64, Int64, String, String, Object,
                                                 UInt64, Int64, String, String, Object,
                                                 UInt64, Int64, String, String, Object,
                                                 UInt64, Int64, String, String, Object>> GetEnumerator()
        {
            return _PropertyGraphs.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _PropertyGraphs.Values.GetEnumerator();
        }

        #endregion

    }

}