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
using System.Linq;
using System.Dynamic;
using System.Threading;
using System.Linq.Expressions;
using System.Collections.Generic;

using de.ahzf.Illias.Commons;
using de.ahzf.Illias.Commons.Votes;

#endregion

namespace de.ahzf.Blueprints.PropertyGraphs.InMemory.Mutable
{

    /// <summary>
    /// A vertex maintains pointers to both a set of incoming and outgoing edges.
    /// The outgoing edges are those edges for which the vertex is the tail.
    /// The incoming edges are those edges for which the vertex is the head.
    /// Diagrammatically, ---InEdges---> Vertex ---OutEdges/HyperEdges--->.
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
    /// <typeparam name="TEdgeCollection">A data structure to store edges.</typeparam>
    /// 
    /// <typeparam name="TIdMultiEdge">The type of the multiedge identifiers.</typeparam>
    /// <typeparam name="TRevIdMultiEdge">The type of the multiedge revision identifiers.</typeparam>
    /// <typeparam name="TMultiEdgeLabel">The type of the multiedge label.</typeparam>
    /// <typeparam name="TKeyMultiEdge">The type of the multiedge property keys.</typeparam>
    /// <typeparam name="TValueMultiEdge">The type of the multiedge property values.</typeparam>
    /// <typeparam name="TPropertiesCollectionMultiEdge">A data structure to store the properties of the multiedges.</typeparam>
    /// <typeparam name="TMultiEdgeCollection">A data structure to store multiedges.</typeparam>
    /// 
    /// <typeparam name="TIdHyperEdge">The type of the hyperedge identifiers.</typeparam>
    /// <typeparam name="TRevIdHyperEdge">The type of the hyperedge revision identifiers.</typeparam>
    /// <typeparam name="THyperEdgeLabel">The type of the hyperedge label.</typeparam>
    /// <typeparam name="TKeyHyperEdge">The type of the hyperedge property keys.</typeparam>
    /// <typeparam name="TValueHyperEdge">The type of the hyperedge property values.</typeparam>
    /// <typeparam name="TPropertiesCollectionHyperEdge">A data structure to store the properties of the hyperedges.</typeparam>
    /// <typeparam name="THyperEdgeCollection">A data structure to store hyperedges.</typeparam>
    public class PropertyVertex : GenericPropertyVertex<UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object>,
                                  IPropertyVertex,
                                  IPropertyGraph,
                                  IDynamicGraphElement<PropertyVertex>
    {

        #region Properties

        #region Graph

        /// <summary>
        /// The associated property graph.
        /// </summary>
        IPropertyGraph IPropertyVertex.Graph
        {
            get
            {
                return _IGenericPropertyGraph as IPropertyGraph;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region (internal) PropertyVertex(VertexId, IdKey, RevIdKey, DescriptionKey, DatastructureInitializer)

        /// <summary>
        /// The PropertyVertex constructor for creating a new PropertyGraph.
        /// </summary>
        /// <param name="VertexId"></param>
        /// <param name="IdKey"></param>
        /// <param name="RevIdKey"></param>
        /// <param name="DescriptionKey"></param>
        /// <param name="DatastructureInitializer"></param>
        /// <param name="VertexIdCreatorDelegate"></param>
        /// <param name="VertexCreatorDelegate"></param>
        /// <param name="VerticesCollectionInitializer"></param>
        /// <param name="EdgeIdCreatorDelegate"></param>
        /// <param name="EdgeCreatorDelegate"></param>
        /// <param name="EdgesCollectionInitializer"></param>
        /// <param name="MultiEdgeIdCreatorDelegate"></param>
        /// <param name="MultiEdgeCreatorDelegate"></param>
        /// <param name="MultiEdgesCollectionInitializer"></param>
        /// <param name="HyperEdgeIdCreatorDelegate"></param>
        /// <param name="HyperEdgeCreatorDelegate"></param>
        /// <param name="HyperEdgesCollectionInitializer"></param>
        internal PropertyVertex(UInt64  VertexId,
                                String  IdKey,
                                String  RevIdKey,
                                String  DescriptionKey,
                                IDictionaryInitializer<String, Object> DatastructureInitializer,

                                #region Create a new vertex

                                       VertexIdCreatorDelegate   <UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object> VertexIdCreatorDelegate,

                                       VertexCreatorDelegate     <UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object> VertexCreatorDelegate,

                                       Func<IGroupedCollection<String, UInt64, IGenericPropertyVertex<UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object>>>
                                       VerticesCollectionInitializer,

                                       #endregion

                                #region Create a new edge

                                EdgeIdCreatorDelegate     <UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object> EdgeIdCreatorDelegate,

                                EdgeCreatorDelegate       <UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object> EdgeCreatorDelegate,

                                Func<IGroupedCollection<String, UInt64, IGenericPropertyEdge<UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object>>>
                                EdgesCollectionInitializer,

                                #endregion

                                #region Create a new multiedge

                                MultiEdgeIdCreatorDelegate<UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object> MultiEdgeIdCreatorDelegate,

                                MultiEdgeCreatorDelegate  <UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object> MultiEdgeCreatorDelegate,

                                Func<IGroupedCollection<String, UInt64, IGenericPropertyMultiEdge<UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object>>>
                                MultiEdgesCollectionInitializer,

                                #endregion

                                #region Create a new hyperedge

                                HyperEdgeIdCreatorDelegate<UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object> HyperEdgeIdCreatorDelegate,

                                HyperEdgeCreatorDelegate  <UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object> HyperEdgeCreatorDelegate,

                                Func<IGroupedCollection<String, UInt64, IGenericPropertyHyperEdge<UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object>>>
                                HyperEdgesCollectionInitializer)

                                #endregion

            : base(VertexId, IdKey, RevIdKey, DescriptionKey, DatastructureInitializer,
                   VertexIdCreatorDelegate,    VertexCreatorDelegate,    VerticesCollectionInitializer,
                   EdgeIdCreatorDelegate,      EdgeCreatorDelegate,      EdgesCollectionInitializer,
                   MultiEdgeIdCreatorDelegate, MultiEdgeCreatorDelegate, MultiEdgesCollectionInitializer,
                   HyperEdgeIdCreatorDelegate, HyperEdgeCreatorDelegate, HyperEdgesCollectionInitializer)

        { }

        #endregion

        #region PropertyVertex(Graph, VertexId, IdKey, RevIdKey, DescriptionKey, DatastructureInitializer, EdgeCollectionInitializer, HyperEdgeCollectionInitializer, VertexInitializer = null)

        /// <summary>
        /// Creates a new vertex.
        /// </summary>
        /// <param name="Graph">The associated property graph.</param>
        /// <param name="VertexId">The identification of this vertex.</param>
        /// <param name="IdKey">The key to access the Id of this vertex.</param>
        /// <param name="RevIdKey">The key to access the RevId of this vertex.</param>
        /// <param name="DatastructureInitializer">A delegate to initialize the properties datastructure.</param>
        /// <param name="EdgesCollectionInitializer">A delegate to initialize the datastructure for storing all edges.</param>
        /// <param name="HyperEdgeCollectionInitializer">A delegate to initialize the datastructure for storing all hyperedges.</param>
        /// <param name="VertexInitializer">A delegate to initialize the newly created vertex.</param>
        public PropertyVertex(IPropertyGraph Graph,
                              UInt64         VertexId,
                              String         IdKey,
                              String         RevIdKey,
                              String         DescriptionKey,

                              IDictionaryInitializer<String, Object> DatastructureInitializer,

                              Func<IGroupedCollection<String, UInt64, IGenericPropertyVertex   <UInt64, Int64, String, String, Object,
                                                                                                UInt64, Int64, String, String, Object,
                                                                                                UInt64, Int64, String, String, Object,
                                                                                                UInt64, Int64, String, String, Object>>> VerticesCollectionInitializer,

                              Func<IGroupedCollection<String, UInt64, IGenericPropertyEdge     <UInt64, Int64, String, String, Object,
                                                                                                UInt64, Int64, String, String, Object,
                                                                                                UInt64, Int64, String, String, Object,
                                                                                                UInt64, Int64, String, String, Object>>> EdgesCollectionInitializer,

                              Func<IGroupedCollection<String, UInt64, IGenericPropertyMultiEdge<UInt64, Int64, String, String, Object,
                                                                                                UInt64, Int64, String, String, Object,
                                                                                                UInt64, Int64, String, String, Object,
                                                                                                UInt64, Int64, String, String, Object>>> MultiEdgesCollectionInitializer,

                              Func<IGroupedCollection<String, UInt64, IGenericPropertyHyperEdge<UInt64, Int64, String, String, Object,
                                                                                                UInt64, Int64, String, String, Object,
                                                                                                UInt64, Int64, String, String, Object,
                                                                                                UInt64, Int64, String, String, Object>>> HyperEdgesCollectionInitializer,

                              VertexInitializer<UInt64, Int64, String, String, Object,
                                                UInt64, Int64, String, String, Object,
                                                UInt64, Int64, String, String, Object,
                                                UInt64, Int64, String, String, Object> VertexInitializer = null)

            : base(Graph, VertexId, IdKey, RevIdKey, DescriptionKey, DatastructureInitializer,
                   VerticesCollectionInitializer, EdgesCollectionInitializer, MultiEdgesCollectionInitializer, HyperEdgesCollectionInitializer,
                   VertexInitializer)

        { }

        #endregion

        #endregion


        // IPropertyVertex

        #region OutEdge methods [IPropertyVertex]

        #region AddOutEdge(Edge)

        /// <summary>
        /// Add an outgoing edge.
        /// </summary>
        /// <param name="Edge">The edge to add.</param>
        void IPropertyVertex.AddOutEdge(IPropertyEdge Edge)
        {
            _IGenericPropertyVertex.AddOutEdge(Edge);
        }

        #endregion


        #region OutEdges(params EdgeLabels)      // OutEdges()!

        /// <summary>
        /// The edges emanating from, or leaving, this vertex
        /// filtered by their label. If no label was given,
        /// all edges will be returned.
        /// </summary>
        IEnumerable<IPropertyEdge> IPropertyVertex.OutEdges(params String[] EdgeLabels)
        {

            return from   Edge
                   in     _IGenericPropertyVertex.OutEdges(EdgeLabels)
                   select Edge as IPropertyEdge;

        }

        #endregion

        #region OutEdges(EdgeFilter)

        /// <summary>
        /// The edges emanating from, or leaving, this vertex
        /// filtered by the given edge filter delegate.
        /// </summary>
        IEnumerable<IPropertyEdge> IPropertyVertex.OutEdges(EdgeFilter<UInt64, Int64, String, String, Object,
                                                                       UInt64, Int64, String, String, Object,
                                                                       UInt64, Int64, String, String, Object,
                                                                       UInt64, Int64, String, String, Object> EdgeFilter)
        
        {

            return from   Edge
                   in     _OutEdges
                   where  EdgeFilter(Edge)
                   select Edge as IPropertyEdge;

        }

        #endregion

        #region OutDegree(params EdgeLabels)     // OutDegree()!

        /// <summary>
        /// The number of edges emanating from, or leaving, this vertex
        /// filtered by their label. If no label was given,
        /// all edges will be returned.
        /// </summary>
        UInt64 IPropertyVertex.OutDegree(params String[] EdgeLabels)
        {
            return _IGenericPropertyVertex.OutDegree(EdgeLabels);
        }

        #endregion

        #region OutDegree(EdgeFilter)

        /// <summary>
        /// The number of edges emanating from, or leaving, this vertex
        /// filtered by the given edge filter delegate.
        /// </summary>
        UInt64 IPropertyVertex.OutDegree(EdgeFilter<UInt64, Int64, String, String, Object,
                                                    UInt64, Int64, String, String, Object,
                                                    UInt64, Int64, String, String, Object,
                                                    UInt64, Int64, String, String, Object> EdgeFilter)
        {
            return _IGenericPropertyVertex.OutDegree(EdgeFilter);
        }

        #endregion


        #region RemoveOutEdges(params Edges)    // RemoveOutEdges()!

        /// <summary>
        /// Remove outgoing edges.
        /// </summary>
        /// <param name="Edges">An array of outgoing edges to be removed.</param>
        void IPropertyVertex.RemoveOutEdges(params IPropertyEdge[] Edges)
        {
            _IGenericPropertyVertex.RemoveOutEdges(Edges);
        }

        #endregion

        #region RemoveOutEdges(EdgeFilter)

        /// <summary>
        /// Remove any outgoing edge matching the
        /// given edge filter delegate.
        /// </summary>
        /// <param name="EdgeFilter">A delegate for edge filtering.</param>
        void IPropertyVertex.RemoveOutEdges(EdgeFilter<UInt64, Int64, String, String, Object,
                                                       UInt64, Int64, String, String, Object,
                                                       UInt64, Int64, String, String, Object,
                                                       UInt64, Int64, String, String, Object> EdgeFilter)

        {
            _IGenericPropertyVertex.RemoveOutEdges(EdgeFilter);
        }

        #endregion

        #endregion

        #region InEdge methods [IPropertyVertex]

        #region AddInEdge(Edge)

        /// <summary>
        /// Add an incoming edge.
        /// </summary>
        /// <param name="Edge">The edge to add.</param>
        void IPropertyVertex.AddInEdge(IPropertyEdge Edge)
        {
            _IGenericPropertyVertex.AddInEdge(Edge);
        }

        #endregion


        #region InEdges(params EdgeLabels)      // InEdges()!

        /// <summary>
        /// The edges incoming to, or arriving at, this vertex
        /// filtered by their label. If no label was given,
        /// all edges will be returned.
        /// </summary>
        IEnumerable<IPropertyEdge> IPropertyVertex.InEdges(params String[] EdgeLabels)
        {

            return from   Edge
                   in     _IGenericPropertyVertex.InEdges(EdgeLabels)
                   select Edge as IPropertyEdge;

        }

        #endregion

        #region InEdges(EdgeFilter)

        /// <summary>
        /// The edges incoming to, or arriving at, this vertex
        /// filtered by the given edge filter delegate.
        /// </summary>
        IEnumerable<IPropertyEdge> IPropertyVertex.InEdges(EdgeFilter<UInt64, Int64, String, String, Object,
                                                                      UInt64, Int64, String, String, Object,
                                                                      UInt64, Int64, String, String, Object,
                                                                      UInt64, Int64, String, String, Object> EdgeFilter)
        
        {

            return from   Edge
                   in     _InEdges
                   where  EdgeFilter(Edge)
                   select Edge as IPropertyEdge;

        }

        #endregion

        #region InDegree(params EdgeLabels)     // InDegree()!

        /// <summary>
        /// The number of edges incoming to, or arriving at, this vertex
        /// filtered by their label. If no label was given,
        /// all edges will be returned.
        /// </summary>
        UInt64 IPropertyVertex.InDegree(params String[] EdgeLabels)
        {
            return _IGenericPropertyVertex.InDegree(EdgeLabels);
        }

        #endregion

        #region InDegree(EdgeFilter)

        /// <summary>
        /// The number of edges incoming to, or arriving at, this vertex
        /// filtered by the given edge filter delegate.
        /// </summary>
        UInt64 IPropertyVertex.InDegree(EdgeFilter<UInt64, Int64, String, String, Object,
                                                   UInt64, Int64, String, String, Object,
                                                   UInt64, Int64, String, String, Object,
                                                   UInt64, Int64, String, String, Object> EdgeFilter)

        {
            return _IGenericPropertyVertex.InDegree(EdgeFilter);
        }

        #endregion


        #region RemoveInEdges(params Edges)    // RemoveInEdges()!

        /// <summary>
        /// Remove incoming edges.
        /// </summary>
        /// <param name="Edges">An array of incoming edges to be removed.</param>
        void IPropertyVertex.RemoveInEdges(params IPropertyEdge[] Edges)
        {
            _IGenericPropertyVertex.RemoveInEdges(Edges);
        }

        #endregion

        #region RemoveInEdges(EdgeFilter)

        /// <summary>
        /// Remove any incoming edge matching the
        /// given edge filter delegate.
        /// </summary>
        /// <param name="EdgeFilter">A delegate for edge filtering.</param>
        void IPropertyVertex.RemoveInEdges(EdgeFilter<UInt64, Int64, String, String, Object,
                                                      UInt64, Int64, String, String, Object,
                                                      UInt64, Int64, String, String, Object,
                                                      UInt64, Int64, String, String, Object> EdgeFilter)

        {
            _IGenericPropertyVertex.RemoveInEdges(EdgeFilter);
        }

        #endregion

        #endregion

        #region MultiEdge methods [IPropertyVertex]

        #region AddMultiEdge(MultiEdge)

        /// <summary>
        /// Add a multiedge.
        /// </summary>
        /// <param name="MultiEdge">The multiedge to add.</param>
        void IPropertyVertex.AddMultiEdge(IPropertyMultiEdge MultiEdge)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region MultiEdges(params MultiEdgeLabels)      // MultiEdges()!

        /// <summary>
        /// The multiedges emanating from, or leaving, this vertex
        /// filtered by their label. If no label was given,
        /// all multiedges will be returned.
        /// </summary>
        IEnumerable<IPropertyMultiEdge> IPropertyVertex.MultiEdges(params String[] MultiEdgeLabels)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region MultiEdges(MultiEdgeFilter)

        /// <summary>
        /// The multiedges emanating from, or leaving, this vertex
        /// filtered by the given multiedge filter delegate.
        /// </summary>
        /// <param name="MultiEdgeFilter">A delegate for multiedge filtering.</param>
        IEnumerable<IPropertyMultiEdge> IPropertyVertex.MultiEdges(MultiEdgeFilter<UInt64, Int64, String, String, Object,
                                                                                   UInt64, Int64, String, String, Object,
                                                                                   UInt64, Int64, String, String, Object,
                                                                                   UInt64, Int64, String, String, Object> MultiEdgeFilter)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region RemoveMultiEdges(params MultiEdges)    // RemoveMultiEdges()!

        /// <summary>
        /// Remove multiedges.
        /// </summary>
        /// <param name="MultiEdges">An array of outgoing edges to be removed.</param>
        void IPropertyVertex.RemoveMultiEdges(params IPropertyMultiEdge[] MultiEdges)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region RemoveMultiEdges(MultiEdgeFilter = null)

        /// <summary>
        /// Remove any outgoing multiedge matching
        /// the given multiedge filter delegate.
        /// </summary>
        /// <param name="MultiEdgeFilter">A delegate for multiedge filtering.</param>
        void IPropertyVertex.RemoveMultiEdges(MultiEdgeFilter<UInt64, Int64, String, String, Object,
                                                              UInt64, Int64, String, String, Object,
                                                              UInt64, Int64, String, String, Object,
                                                              UInt64, Int64, String, String, Object> MultiEdgeFilter = null)

        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

        #region HyperEdge methods [IPropertyVertex]

        #region AddHyperEdge(HyperEdge)

        /// <summary>
        /// Add a hyperedge.
        /// </summary>
        /// <param name="HyperEdge">The hyperedge to add.</param>
        void IPropertyVertex.AddHyperEdge(IPropertyHyperEdge HyperEdge)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region HyperEdges(params HyperEdgeLabels)      // HyperEdges()!

        /// <summary>
        /// The hyperedges emanating from, or leaving, this vertex
        /// filtered by their label. If no label was given,
        /// all hyperedges will be returned.
        /// </summary>
        IEnumerable<IPropertyHyperEdge> IPropertyVertex.HyperEdges(params String[] HyperEdgeLabels)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region HyperEdges(HyperEdgeFilter)

        /// <summary>
        /// The hyperedges emanating from, or leaving, this vertex
        /// filtered by the given hyperedge filter delegate.
        /// </summary>
        /// <param name="HyperEdgeFilter">A delegate for hyperedge filtering.</param>
        IEnumerable<IPropertyHyperEdge> IPropertyVertex.HyperEdges(HyperEdgeFilter<UInt64, Int64, String, String, Object,
                                                                                   UInt64, Int64, String, String, Object,
                                                                                   UInt64, Int64, String, String, Object,
                                                                                   UInt64, Int64, String, String, Object> HyperEdgeFilter)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region RemoveHyperEdges(params HyperEdges)    // RemoveHyperEdges()!

        /// <summary>
        /// Remove hyperedges.
        /// </summary>
        /// <param name="HyperEdges">An array of outgoing edges to be removed.</param>
        void IPropertyVertex.RemoveHyperEdges(params IPropertyHyperEdge[] HyperEdges)
        {
            _IGenericPropertyVertex.RemoveHyperEdges(HyperEdges);
        }

        #endregion

        #region RemoveHyperEdges(HyperEdgeFilter = null)

        /// <summary>
        /// Remove any outgoing hyperedge matching
        /// the given hyperedge filter delegate.
        /// </summary>
        /// <param name="HyperEdgeFilter">A delegate for hyperedge filtering.</param>
        void IPropertyVertex.RemoveHyperEdges(HyperEdgeFilter<UInt64, Int64, String, String, Object,
                                                              UInt64, Int64, String, String, Object,
                                                              UInt64, Int64, String, String, Object,
                                                              UInt64, Int64, String, String, Object> HyperEdgeFilter)
        {
            _IGenericPropertyVertex.RemoveHyperEdges(HyperEdgeFilter);
        }

        #endregion

        #endregion


        // IPropertyGraph

        #region Vertex methods [IPropertyGraph]

        #region AddVertex(VertexInitializer = null)

        /// <summary>
        /// Adds a vertex to the graph using the given VertexId and initializes
        /// the vertex by invoking the given vertex initializer.
        /// </summary>
        /// <param name="VertexInitializer">A delegate to initialize the new vertex.</param>
        /// <returns>The new vertex</returns>
        IPropertyVertex IPropertyGraph.AddVertex(VertexInitializer<UInt64, Int64, String, String, Object,
                                                                   UInt64, Int64, String, String, Object,
                                                                   UInt64, Int64, String, String, Object,
                                                                   UInt64, Int64, String, String, Object> VertexInitializer = null)
        {
            return _IGenericPropertyGraph.AddVertex(VertexInitializer) as IPropertyVertex;
        }

        #endregion

        #region AddVertex(VertexId, VertexInitializer = null)

        /// <summary>
        /// Adds a vertex to the graph using the given VertexId and initializes
        /// the vertex by invoking the given vertex initializer.
        /// </summary>
        /// <param name="VertexId">A VertexId. If none was given a new one will be generated.</param>
        /// <param name="VertexInitializer">A delegate to initialize the new vertex.</param>
        /// <returns>The new vertex</returns>
        IPropertyVertex IPropertyGraph.AddVertex(UInt64 VertexId, VertexInitializer<UInt64, Int64, String, String, Object,
                                                                                    UInt64, Int64, String, String, Object,
                                                                                    UInt64, Int64, String, String, Object,
                                                                                    UInt64, Int64, String, String, Object> VertexInitializer = null)
        {
            return _IGenericPropertyGraph.AddVertex(VertexId, VertexInitializer) as IPropertyVertex;
        }

        #endregion

        #region AddVertex(IPropertyVertex)

        /// <summary>
        /// Adds the given vertex to the graph.
        /// Will fail if the Id of the vertex is already present in the graph.
        /// </summary>
        /// <param name="IPropertyVertex">An IPropertyVertex.</param>
        /// <returns>The given vertex.</returns>
        IPropertyVertex IPropertyGraph.AddVertex(IPropertyVertex Vertex)
        {
            return _IGenericPropertyGraph.AddVertex(Vertex) as IPropertyVertex;
        }

        #endregion


        #region VertexById(VertexId)

        /// <summary>
        /// Return the vertex referenced by the given vertex identifier.
        /// If no vertex is referenced by the identifier return null.
        /// </summary>
        /// <param name="VertexId">A vertex identifier.</param>
        IPropertyVertex IPropertyGraph.VertexById(UInt64 VertexId)
        {
            return _IGenericPropertyGraph.VertexById(VertexId) as IPropertyVertex;
        }

        #endregion

        #region VerticesById(params VertexIds)

        /// <summary>
        /// Return the vertices referenced by the given array of vertex identifiers.
        /// If no vertex is referenced by a given identifier this value will be
        /// skipped.
        /// </summary>
        /// <param name="VertexIds">An array of vertex identifiers.</param>
        IEnumerable<IPropertyVertex> IPropertyGraph.VerticesById(params UInt64[] VertexIds)
        {

            return from   Vertex
                   in     _IGenericPropertyGraph.VerticesById(VertexIds)
                   select Vertex as IPropertyVertex;

        }

        #endregion

        #region VerticesByLabel(params VertexLabels)

        /// <summary>
        /// Return an enumeration of all vertices having one of the
        /// given vertex labels.
        /// </summary>
        /// <param name="VertexLabels">An array of vertex labels.</param>
        IEnumerable<IPropertyVertex> IPropertyGraph.VerticesByLabel(params String[] VertexLabels)
        {

            return from   Vertex
                   in     _IGenericPropertyGraph.VerticesByLabel(VertexLabels)
                   select Vertex as IPropertyVertex;

        }

        #endregion

        #region Vertices(VertexFilter = null)

        /// <summary>
        /// Get an enumeration of all vertices in the graph.
        /// An optional vertex filter may be applied for filtering.
        /// </summary>
        /// <param name="VertexFilter">A delegate for vertex filtering.</param>
        IEnumerable<IPropertyVertex> IPropertyGraph.Vertices(VertexFilter<UInt64, Int64, String, String, Object,
                                                                          UInt64, Int64, String, String, Object,
                                                                          UInt64, Int64, String, String, Object,
                                                                          UInt64, Int64, String, String, Object> VertexFilter = null)
        {

            return from   Vertex
                   in     _IGenericPropertyGraph.Vertices(VertexFilter)
                   select Vertex as IPropertyVertex;

        }

        #endregion

        #region NumberOfVertices(VertexFilter = null)

        /// <summary>
        /// Return the current number of vertices matching the given optional vertex filter.
        /// When the filter is null, this method should use implement an optimized
        /// way to get the currenty number of vertices.
        /// </summary>
        /// <param name="VertexFilter">A delegate for vertex filtering.</param>
        UInt64 IPropertyGraph.NumberOfVertices(VertexFilter<UInt64, Int64, String, String, Object,
                                                            UInt64, Int64, String, String, Object,
                                                            UInt64, Int64, String, String, Object,
                                                            UInt64, Int64, String, String, Object> VertexFilter = null)
        {
            return _IGenericPropertyGraph.NumberOfVertices(VertexFilter);
        }

        #endregion


        #region RemoveVerticesById(params VertexIds)

        /// <summary>
        /// Remove the vertex identified by the given VertexId from the graph
        /// </summary>
        /// <param name="VertexIds">An array of VertexIds of the vertices to remove.</param>
        void IPropertyGraph.RemoveVerticesById(params UInt64[] VertexIds)
        {
            _IGenericPropertyGraph.RemoveVerticesById(VertexIds);
        }

        #endregion

        #region RemoveVertices(params Vertices)

        /// <summary>
        /// Remove the given array of vertices from the graph.
        /// Upon removing a vertex, all the edges by which the vertex
        /// is connected will be removed as well.
        /// </summary>
        /// <param name="Vertices">An array of vertices to be removed from the graph.</param>
        void IPropertyGraph.RemoveVertices(params IPropertyVertex[] Vertices)
        {
            _IGenericPropertyGraph.RemoveVertices(Vertices);
        }

        #endregion

        #region RemoveVertices(VertexFilter = null)

        /// <summary>
        /// Remove each vertex matching the given filter.
        /// </summary>
        /// <param name="VertexFilter">A delegate for vertex filtering.</param>
        void IPropertyGraph.RemoveVertices(VertexFilter<UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object,
                                                        UInt64, Int64, String, String, Object> VertexFilter = null)
        {
            _IGenericPropertyGraph.RemoveVertices(VertexFilter);
        }

        #endregion

        #endregion

        #region Edge methods [IPropertyGraph]

        #region AddEdge(OutVertex, Label, InVertex, EdgeInitializer = null)

        /// <summary>
        /// Add an edge to the graph. The added edge requires a tail vertex,
        /// a head vertex, a label and initializes the edge
        /// by invoking the given EdgeInitializer.
        /// OutVertex --Label-> InVertex is the "Semantic Web Notation" ;)
        /// </summary>
        /// <param name="OutVertex">The vertex on the tail of the edge.</param>
        /// <param name="Label">The label associated with the edge.</param>
        /// <param name="InVertex">The vertex on the head of the edge.</param>
        /// <param name="EdgeInitializer">A delegate to initialize the new edge.</param>
        /// <returns>The new edge.</returns>
        IPropertyEdge IPropertyGraph.AddEdge(IPropertyVertex OutVertex,
                                             String          Label,
                                             IPropertyVertex InVertex,
                                             EdgeInitializer<UInt64, Int64, String, String, Object,
                                                             UInt64, Int64, String, String, Object,
                                                             UInt64, Int64, String, String, Object,
                                                             UInt64, Int64, String, String, Object> EdgeInitializer = null)

        {
            return _IGenericPropertyGraph.AddEdge(OutVertex, Label, InVertex, EdgeInitializer) as IPropertyEdge;
        }

        #endregion

        #region AddEdge(EdgeId, OutVertex, Label, InVertex, EdgeInitializer = null)

        /// <summary>
        /// Add an edge to the graph. The added edge requires a tail vertex,
        /// a head vertex, an identifier, a label and initializes the edge
        /// by invoking the given EdgeInitializer.
        /// OutVertex --Label-> InVertex is the "Semantic Web Notation" ;)
        /// </summary>
        /// <param name="EdgeId">A EdgeId. If none was given a new one will be generated.</param>
        /// <param name="OutVertex">The vertex on the tail of the edge.</param>
        /// <param name="Label">The label associated with the edge.</param>
        /// <param name="InVertex">The vertex on the head of the edge.</param>
        /// <param name="EdgeInitializer">A delegate to initialize the new edge.</param>
        /// <returns>The new edge.</returns>
        IPropertyEdge IPropertyGraph.AddEdge(UInt64          EdgeId,
                                             IPropertyVertex OutVertex,
                                             String          Label,
                                             IPropertyVertex InVertex,
                                             EdgeInitializer<UInt64, Int64, String, String, Object,
                                                             UInt64, Int64, String, String, Object,
                                                             UInt64, Int64, String, String, Object,
                                                             UInt64, Int64, String, String, Object> EdgeInitializer = null)
        {
            return _IGenericPropertyGraph.AddEdge(EdgeId, OutVertex, Label, InVertex, EdgeInitializer) as IPropertyEdge;
        }

        #endregion

        #region AddEdge(OutVertex, InVertex, Label = default, EdgeInitializer = null)

        /// <summary>
        /// Add an edge to the graph. The added edge requires a tail vertex,
        /// a head vertex, an identifier, a label and initializes the edge
        /// by invoking the given EdgeInitializer.
        /// </summary>
        /// <param name="OutVertex">The vertex on the tail of the edge.</param>
        /// <param name="InVertex">The vertex on the head of the edge.</param>
        /// <param name="Label">The label associated with the edge.</param>
        /// <param name="EdgeInitializer">A delegate to initialize the new edge.</param>
        /// <returns>The new edge.</returns>
        IPropertyEdge IPropertyGraph.AddEdge(IPropertyVertex OutVertex,
                                             IPropertyVertex InVertex,
                                             String          Label  = default(String),
                                             EdgeInitializer<UInt64, Int64, String, String, Object,
                                                             UInt64, Int64, String, String, Object,
                                                             UInt64, Int64, String, String, Object,
                                                             UInt64, Int64, String, String, Object> EdgeInitializer = null)
        {
            return _IGenericPropertyGraph.AddEdge(OutVertex, InVertex, Label, EdgeInitializer) as IPropertyEdge;
        }

        #endregion

        #region AddEdge(OutVertex, InVertex, EdgeId, Label = default, EdgeInitializer = null)

        /// <summary>
        /// Add an edge to the graph. The added edge requires a tail vertex,
        /// a head vertex, an identifier, a label and initializes the edge
        /// by invoking the given EdgeInitializer.
        /// </summary>
        /// <param name="OutVertex">The vertex on the tail of the edge.</param>
        /// <param name="InVertex">The vertex on the head of the edge.</param>
        /// <param name="EdgeId">A EdgeId. If none was given a new one will be generated.</param>
        /// <param name="Label">The label associated with the edge.</param>
        /// <param name="EdgeInitializer">A delegate to initialize the new edge.</param>
        /// <returns>The new edge.</returns>
        IPropertyEdge IPropertyGraph.AddEdge(IPropertyVertex OutVertex,
                                             IPropertyVertex InVertex,
                                             UInt64          EdgeId,
                                             String          Label = default(String),
                                             EdgeInitializer<UInt64, Int64, String, String, Object,
                                                             UInt64, Int64, String, String, Object,
                                                             UInt64, Int64, String, String, Object,
                                                             UInt64, Int64, String, String, Object> EdgeInitializer = null)
        {
            return _IGenericPropertyGraph.AddEdge(OutVertex, InVertex, EdgeId, Label, EdgeInitializer) as IPropertyEdge;
        }

        #endregion


        #region EdgeById(EdgeId)

        /// <summary>
        /// Return the edge referenced by the given edge identifier.
        /// If no edge is referenced by a given identifier return null.
        /// </summary>
        /// <param name="EdgeId">An edge identifier.</param>
        IPropertyEdge IPropertyGraph.EdgeById(UInt64 EdgeId)
        {
            return _IGenericPropertyGraph.EdgeById(EdgeId) as IPropertyEdge;
        }

        #endregion

        #region EdgesById(params EdgeIds)

        /// <summary>
        /// Return the edges referenced by the given array of edge identifiers.
        /// If no edge is referenced by a given identifier this value will be
        /// skipped.
        /// </summary>
        /// <param name="EdgeIds">An array of edge identifiers.</param>
        IEnumerable<IPropertyEdge> IPropertyGraph.EdgesById(params UInt64[] EdgeIds)
        {

            return from   Edge
                   in     _IGenericPropertyGraph.EdgesById(EdgeIds)
                   select Edge as IPropertyEdge;

        }

        #endregion

        #region EdgesByLabel(params EdgeLabels)

        /// <summary>
        /// Return an enumeration of all edges having one of the
        /// given edge labels.
        /// </summary>
        /// <param name="EdgeLabels">An array of edge labels.</param>
        IEnumerable<IPropertyEdge> IPropertyGraph.EdgesByLabel(params String[] EdgeLabels)
        {

            return from   Edge
                   in     _IGenericPropertyGraph.EdgesByLabel(EdgeLabels)
                   select Edge as IPropertyEdge;

        }

        #endregion

        #region Edges(EdgeFilter = null)

        /// <summary>
        /// Get an enumeration of all edges in the graph.
        /// An optional edge filter may be applied for filtering.
        /// </summary>
        /// <param name="EdgeFilter">A delegate for edge filtering.</param>
        IEnumerable<IPropertyEdge> IPropertyGraph.Edges(EdgeFilter<UInt64, Int64, String, String, Object,
                                                                   UInt64, Int64, String, String, Object,
                                                                   UInt64, Int64, String, String, Object,
                                                                   UInt64, Int64, String, String, Object> EdgeFilter = null)
        {

            return from Edge
                   in _IGenericPropertyGraph.Edges(EdgeFilter)
                   select Edge as IPropertyEdge;

        }

        #endregion

        #region NumberOfEdges(EdgeFilter = null)

        /// <summary>
        /// Return the current number of edges matching the given optional edge filter.
        /// When the filter is null, this method should use implement an optimized
        /// way to get the currenty number of edges.
        /// </summary>
        /// <param name="EdgeFilter">A delegate for edge filtering.</param>
        UInt64 IPropertyGraph.NumberOfEdges(EdgeFilter<UInt64, Int64, String, String, Object,
                                                       UInt64, Int64, String, String, Object,
                                                       UInt64, Int64, String, String, Object,
                                                       UInt64, Int64, String, String, Object> EdgeFilter = null)
        {
            return _IGenericPropertyGraph.NumberOfEdges(EdgeFilter);
        }

        #endregion


        #region RemoveEdgesById(params EdgeIds)

        /// <summary>
        /// Remove the given array of edges identified by their EdgeIds.
        /// </summary>
        /// <param name="EdgeIds">An array of EdgeIds of the edges to remove</param>
        void IPropertyGraph.RemoveEdgesById(params UInt64[] EdgeIds)
        {
            _IGenericPropertyGraph.RemoveEdgesById(EdgeIds);
        }

        #endregion

        #region RemoveEdges(params Edges)

        /// <summary>
        /// Remove the given array of edges from the graph.
        /// </summary>
        /// <param name="Edges">An array of edges to be removed from the graph.</param>
        void IPropertyGraph.RemoveEdges(params IPropertyEdge[] Edges)
        {
            _IGenericPropertyGraph.RemoveEdges(Edges);
        }

        #endregion

        #region RemoveEdges(EdgeFilter = null)

        /// <summary>
        /// Remove any edge matching the given edge filter.
        /// </summary>
        /// <param name="EdgeFilter">A delegate for edge filtering.</param>
        void IPropertyGraph.RemoveEdges(EdgeFilter<UInt64, Int64, String, String, Object,
                                                   UInt64, Int64, String, String, Object,
                                                   UInt64, Int64, String, String, Object,
                                                   UInt64, Int64, String, String, Object> EdgeFilter)
        {
            _IGenericPropertyGraph.RemoveEdges(EdgeFilter);
        }

        #endregion

        #endregion



        #region Operator overloading

        #region Operator == (PropertyVertex1, PropertyVertex2)

        /// <summary>
        /// Compares two vertices for equality.
        /// </summary>
        /// <param name="PropertyVertex1">A vertex.</param>
        /// <param name="PropertyVertex2">Another vertex.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (PropertyVertex PropertyVertex1,
                                           PropertyVertex PropertyVertex2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(PropertyVertex1, PropertyVertex2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) PropertyVertex1 == null) || ((Object) PropertyVertex2 == null))
                return false;

            return PropertyVertex1.Equals(PropertyVertex2);

        }

        #endregion

        #region Operator != (PropertyVertex1, PropertyVertex2)

        /// <summary>
        /// Compares two vertices for inequality.
        /// </summary>
        /// <param name="PropertyVertex1">A vertex.</param>
        /// <param name="PropertyVertex2">Another vertex.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (PropertyVertex PropertyVertex1,
                                           PropertyVertex PropertyVertex2)
        {
            return !(PropertyVertex1 == PropertyVertex2);
        }

        #endregion

        #region Operator <  (PropertyVertex1, PropertyVertex2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PropertyVertex1">A Vertex.</param>
        /// <param name="PropertyVertex2">Another Vertex.</param>
        /// <returns>true|false</returns>
        public static Boolean operator  < (PropertyVertex PropertyVertex1,
                                           PropertyVertex PropertyVertex2)
        {

            if ((Object) PropertyVertex1 == null)
                throw new ArgumentNullException("The given PropertyVertex1 must not be null!");

            if ((Object) PropertyVertex2 == null)
                throw new ArgumentNullException("The given PropertyVertex2 must not be null!");

            return PropertyVertex1.CompareTo(PropertyVertex2) < 0;

        }

        #endregion

        #region Operator <= (PropertyVertex1, PropertyVertex2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PropertyVertex1">A Vertex.</param>
        /// <param name="PropertyVertex2">Another Vertex.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (PropertyVertex PropertyVertex1,
                                           PropertyVertex PropertyVertex2)
        {
            return !(PropertyVertex1 > PropertyVertex2);
        }

        #endregion

        #region Operator >  (PropertyVertex1, PropertyVertex2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PropertyVertex1">A Vertex.</param>
        /// <param name="PropertyVertex2">Another Vertex.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >  (PropertyVertex PropertyVertex1,
                                           PropertyVertex PropertyVertex2)
        {

            if ((Object) PropertyVertex1 == null)
                throw new ArgumentNullException("The given PropertyVertex1 must not be null!");

            if ((Object) PropertyVertex2 == null)
                throw new ArgumentNullException("The given PropertyVertex2 must not be null!");

            return PropertyVertex1.CompareTo(PropertyVertex2) > 0;

        }

        #endregion

        #region Operator >= (PropertyVertex1, PropertyVertex2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PropertyVertex1">A Vertex.</param>
        /// <param name="PropertyVertex2">Another Vertex.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (PropertyVertex PropertyVertex1,
                                           PropertyVertex PropertyVertex2)
        {
            return !(PropertyVertex1 < PropertyVertex2);
        }

        #endregion

        #endregion

        #region IDynamicGraphObject<PropertyVertex> Members

        #region GetMetaObject(myExpression)

        /// <summary>
        /// Return the appropriate DynamicMetaObject.
        /// </summary>
        /// <param name="myExpression">An Expression.</param>
        public DynamicMetaObject GetMetaObject(Expression myExpression)
        {
            return new DynamicGraphElement<PropertyVertex> (myExpression, this);
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
            return SetProperty((String) (Object) myBinder, (Object) myObject);
        }

        #endregion

        #region GetMember(myBinder)

        /// <summary>
        /// Returns the value of the given property key.
        /// </summary>
        /// <param name="myBinder">The property key.</param>
        public virtual Object GetMember(String myBinder)
        {
            Object myObject;
            TryGetProperty((String) (Object) myBinder, out myObject);
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
                PropertyData.Remove((String) (Object) myBinder);
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

            if ((Object) Object == null)
                throw new ArgumentNullException("The given Object must not be null!");

            return CompareTo((UInt64) Object);

        }

        #endregion

        #region CompareTo(IPropertyVertex)

        /// <summary>
        /// Compares two generic property vertices.
        /// </summary>
        /// <param name="IGenericPropertyVertex">A generic property vertex to compare with.</param>
        public Int32 CompareTo(IPropertyVertex IPropertyVertex)
        {
            
            if ((Object) IPropertyVertex == null)
                throw new ArgumentNullException("The given IPropertyVertex must not be null!");

            return Id.CompareTo(IPropertyVertex[IdKey]);

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

            // Check if the given object can be casted to a PropertyVertex
            var PropertyVertex = Object as PropertyVertex;

            if ((Object) PropertyVertex == null)
                return false;

            return this.Equals(PropertyVertex);

        }

        #endregion

        #region Equals(IPropertyVertex)

        /// <summary>
        /// Compares two generic property vertices for equality.
        /// </summary>
        /// <param name="IGenericPropertyVertex">A generic property vertex to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(IPropertyVertex IPropertyVertex)
        {
            
            if ((Object) IPropertyVertex == null)
                return false;

            //TODO: Here it might be good to check all attributes of the UNIQUE constraint!
            return Id.Equals(IPropertyVertex[IdKey]);

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
            return "IPropertyVertex [Id: " + Id.ToString() + ", " + _OutEdges.Count() + " OutEdges, " + _InEdges.Count() + " InEdges]";
        }

        #endregion

    }

}