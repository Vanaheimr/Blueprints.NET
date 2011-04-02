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
using de.ahzf.blueprints.InMemory.PropertyGraph.Generic;

#endregion

namespace de.ahzf.blueprints.InMemory.PropertyGraph
{

    /// <summary>
    /// An in-memory implementation of a property graph.
    /// </summary>
    public class InMemoryPropertyGraph : InMemoryGenericPropertyGraph<// Vertices
                                                                      //IPropertyVertex<VertexId,    RevisionId, String, Object, IDictionary<String, Object>,
                                                                      //                EdgeId,      RevisionId, String, Object, IDictionary<String, Object>,
                                                                      //                HyperEdgeId, RevisionId, String, Object, IDictionary<String, Object>>,
                                                                      IGenericVertex<VertexId,    RevisionId, IProperties<String, Object>,
                                                                                     EdgeId,      RevisionId, IProperties<String, Object>,
                                                                                     HyperEdgeId, RevisionId, IProperties<String, Object>>,

                                                                      VertexId,    RevisionId, String, Object, IDictionary<String, Object>,

                                                                      ICollection<IGenericEdge<VertexId,    RevisionId, IProperties<String, Object>,
                                                                                               EdgeId,      RevisionId, IProperties<String, Object>,
                                                                                               HyperEdgeId, RevisionId, IProperties<String, Object>>>,

                                                                      // Edges
                                                                      EdgeId,      RevisionId, String, Object, IDictionary<String, Object>,

                                                                      // Hyperedges
                                                                      HyperEdgeId, RevisionId, String, Object, IDictionary<String, Object>,

                                                                      Object>

    {

        #region Constructor(s)

        #region InMemoryPropertyGraph()

        /// <summary>
        /// Created a new in-memory property graph.
        /// </summary>
        public InMemoryPropertyGraph()
            : base ((myVertexId, myVertexPropertyInitializer) =>
                        new PropertyVertex<VertexId,    RevisionId, String, Object, IDictionary<String, Object>,
                                           EdgeId,      RevisionId, String, Object, IDictionary<String, Object>,
                                           HyperEdgeId, RevisionId, String, Object, IDictionary<String, Object>,
                                           ICollection<IGenericEdge<VertexId,    RevisionId, IProperties<String, Object>,
                                                                    EdgeId,      RevisionId, IProperties<String, Object>,
                                                                    HyperEdgeId, RevisionId, IProperties<String, Object>>>>    
                            (myVertexId, "Id", "RevId",
                             () => new Dictionary<String, Object>(),
                             () => new HashSet<IGenericEdge<VertexId,    RevisionId, IProperties<String, Object>,
                                                            EdgeId,      RevisionId, IProperties<String, Object>,
                                                            HyperEdgeId, RevisionId, IProperties<String, Object>>>(),
                             myVertexPropertyInitializer
                            ),

                   
                   "Id", "RevId", () => new Dictionary<String, Object>(),
                   "Id", "RevId", () => new Dictionary<String, Object>())

        { }

        #endregion

        #endregion

    }

}
