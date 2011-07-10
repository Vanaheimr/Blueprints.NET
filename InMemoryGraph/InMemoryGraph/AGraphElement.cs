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
using System.ComponentModel;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Collections.Specialized;

#endregion

namespace de.ahzf.Blueprints.PropertyGraph.InMemory
{

    /// <summary>
    /// The abstract graph element is the base class for all property graph elements:
    /// The vertices, edges, hyperedges and the property graph itself.
    /// </summary>
    /// <typeparam name="TId">The graph element identification.</typeparam>
    /// <typeparam name="TRevisionId">The graph element revision identification.</typeparam>
    /// <typeparam name="TKey">The type of the graph element property keys.</typeparam>
    /// <typeparam name="TValue">The type of the graph element property values.</typeparam>
    /// <typeparam name="TDatastructure">A datastructure for storing all properties.</typeparam>
    public abstract class AGraphElement<TId, TRevisionId, TKey, TValue, TDatastructure>
                              : IProperties<TKey, TValue>

        where TKey           : IEquatable<TKey>,        IComparable<TKey>,        IComparable
        where TId            : IEquatable<TId>,         IComparable<TId>,         IComparable, TValue
        where TRevisionId    : IEquatable<TRevisionId>, IComparable<TRevisionId>, IComparable, TValue
        where TDatastructure : IDictionary<TKey, TValue>

    {

        #region Properties

        #region IdKey

        /// <summary>
        /// The property key of the identification.
        /// </summary>
        public TKey IdKey
        {
            get
            {
                return PropertyData.IdKey;
            }
        }

        #endregion

        #region Id

        /// <summary>
        /// The Identification of this property graph element.
        /// </summary>
        public TId Id
        {
            get
            {
                return (TId) PropertyData[IdKey];
            }
        }

        #endregion

        #region RevisionIdKey

        /// <summary>
        /// The property key of the revision identification.
        /// </summary>
        public TKey RevisionIdKey
        {
            get
            {
                return PropertyData.RevisionIdKey;
            }
        }

        #endregion

        #region RevisionId

        /// <summary>
        /// The revision identification of this property graph element.
        /// </summary>
        public TRevisionId RevisionId
        {
            get
            {
                return (TRevisionId) PropertyData[RevisionIdKey];
            }
        }

        #endregion

        #region PropertyData

        /// <summary>
        /// The properties of this graph element.
        /// </summary>
        public IProperties<TKey, TValue> PropertyData { get; private set; }

        #endregion

        #endregion

        #region Events

        #region CollectionChanged/OnCollectionChanged(...)

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {

            add
            {
                PropertyData.CollectionChanged += value;
            }

            remove
            {
                PropertyData.CollectionChanged -= value;
            }

        }

        public void OnCollectionChanged(NotifyCollectionChangedEventArgs myNotifyCollectionChangedEventArgs)
        {
            PropertyData.OnCollectionChanged(myNotifyCollectionChangedEventArgs);
        }

        #endregion

        #region PropertyChanging/OnPropertyChanging(...)

        public event PropertyChangingEventHandler PropertyChanging
        {

            add
            {
                PropertyData.PropertyChanging += value;
            }

            remove
            {
                PropertyData.PropertyChanging -= value;
            }

        }

        public void OnPropertyChanging(String myPropertyName)
        {
            PropertyData.OnPropertyChanging(myPropertyName);
        }

        public void OnPropertyChanging<TResult>(Expression<Func<TResult>> myPropertyExpression)
        {
            PropertyData.OnPropertyChanging(myPropertyExpression);
        }

        #endregion

        #region PropertyChanged/OnPropertyChanged(...)

        public event PropertyChangedEventHandler PropertyChanged
        {

            add
            {
                PropertyData.PropertyChanged += value;
            }

            remove
            {
                PropertyData.PropertyChanged -= value;
            }

        }

        public void OnPropertyChanged(String myPropertyName)
        {
            PropertyData.OnPropertyChanged(myPropertyName);
        }

        public void OnPropertyChanged<TResult>(Expression<Func<TResult>> myPropertyExpression)
        {
            PropertyData.OnPropertyChanged(myPropertyExpression);
        }

        #endregion

        #endregion

        #region (protected) Constructor(s)

        #region (protected) AGraphElement(Id, IdKey, RevisonIdKey, DatastructureInitializer, GraphElementInitializer = null)

        /// <summary>
        /// Creates a new abstract graph element.
        /// </summary>
        /// <param name="Id">The Id of this graph element.</param>
        /// <param name="IdKey">The key to access the Id of this graph element.</param>
        /// <param name="RevisonIdKey">The key to access the RevisionId of this graph element.</param>
        /// <param name="DatastructureInitializer">A delegate to initialize the datastructure of the this graph element.</param>
        /// <param name="GraphElementInitializer">An delegate to do some initial operations like adding some properties.</param>
        internal protected AGraphElement(TId                               Id,
                                         TKey                              IdKey,
                                         TKey                              RevisonIdKey,
                                         Func<TDatastructure>              DatastructureInitializer,
                                         Action<IProperties<TKey, TValue>> GraphElementInitializer = null)
        {

            this.PropertyData  = new Properties<TId, TRevisionId, TKey, TValue, TDatastructure>
                                               (Id, IdKey, RevisonIdKey, DatastructureInitializer);

            if (GraphElementInitializer != null)
                GraphElementInitializer(PropertyData);

        }

        #endregion

        #endregion


        #region IProperties Members

        #region Keys

        /// <summary>
        /// An enumeration of all property keys.
        /// </summary>
        public IEnumerable<TKey> Keys
        {
            get
            {
                return PropertyData.Keys;
            }
        }

        #endregion

        #region Values

        /// <summary>
        /// An enumeration of all property values.
        /// </summary>
        public IEnumerable<TValue> Values
        {
            get
            {
                return PropertyData.Values;
            }
        }

        #endregion


        #region SetProperty(Key, Value)

        /// <summary>
        /// Add a KeyValuePair to the graph element.
        /// If a value already exists for the given key, then the previous value is overwritten.
        /// </summary>
        /// <param name="Key">A key.</param>
        /// <param name="Value">A value.</param>
        public IProperties<TKey, TValue> Set(TKey Key, TValue Value)
        {
            return PropertyData.Set(Key, Value);
        }

        #endregion


        #region ContainsKey(Key)

        /// <summary>
        /// Determines if the specified key exists.
        /// </summary>
        /// <param name="Key">A key.</param>
        public Boolean ContainsKey(TKey Key)
        {
            return PropertyData.ContainsKey(Key);
        }

        #endregion

        #region ContainsValue(Value)

        /// <summary>
        /// Determines if the specified value exists.
        /// </summary>
        /// <param name="Value">A value.</param>
        public Boolean ContainsValue(TValue Value)
        {
            return PropertyData.ContainsValue(Value);
        }

        #endregion

        #region Contains(Key, Value)

        /// <summary>
        /// Determines if an KeyValuePair with the specified key and value exists.
        /// </summary>
        /// <param name="Key">A key.</param>
        /// <param name="Value">A value.</param>
        public Boolean Contains(TKey Key, TValue Value)
        {
            return PropertyData.Contains(Key, Value);
        }

        #endregion


        #region this[Key]

        /// <summary>
        /// Return the value associated with the given key.
        /// </summary>
        /// <param name="Key">A key.</param>
        public TValue this[TKey Key]
        {
            get
            {
                return PropertyData[Key];
            }
        }

        #endregion

        #region Get(Key, out Value)

        /// <summary>
        /// Return the value associated with the given key.
        /// </summary>
        /// <param name="Key">A key.</param>
        /// <param name="Value">The associated value.</param>
        /// <returns>True if the returned value is valid. False otherwise.</returns>
        public Boolean Get(TKey Key, out TValue Value)
        {
            return PropertyData.Get(Key, out Value);
        }

        #endregion

        #region Get(KeyValueFilter = null)

        /// <summary>
        /// Return a filtered enumeration of all KeyValuePairs.
        /// </summary>
        /// <param name="KeyValueFilter">A delegate to filter properties based on their keys and values.</param>
        /// <returns>A enumeration of all key/value pairs matching the given KeyValueFilter.</returns>
        public IEnumerable<KeyValuePair<TKey, TValue>> Get(KeyValueFilter<TKey, TValue> KeyValueFilter = null)
        {
            return PropertyData.Get(KeyValueFilter);
        }

        #endregion


        #region Remove(Key)

        /// <summary>
        /// Removes all KeyValuePairs associated with the given key.
        /// </summary>
        /// <param name="Key">A key.</param>
        /// <returns>The value associated with that key prior to the removal.</returns>
        public TValue Remove(TKey Key)
        {
            return PropertyData.Remove(Key);
        }

        #endregion

        #region Remove(Key, Value)

        /// <summary>
        /// Remove the given KeyValuePair.
        /// </summary>
        /// <param name="Key">A key.</param>
        /// <param name="Value">A value.</param>
        public Boolean Remove(TKey Key, TValue Value)
        {
            return PropertyData.Remove(Key, Value);
        }

        #endregion

        #region Remove(KeyValueFilter = null)

        /// <summary>
        /// Remove all KeyValuePairs specified by the given KeyValueFilter.
        /// </summary>
        /// <param name="KeyValueFilter">A delegate to remove properties based on their keys and values.</param>
        /// <returns>A enumeration of all key/value pairs removed by the given KeyValueFilter before their removal.</returns>
        public IEnumerable<KeyValuePair<TKey, TValue>> Remove(KeyValueFilter<TKey, TValue> KeyValueFilter = null)
        {
            return PropertyData.Remove(KeyValueFilter);
        }

        #endregion

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// An enumerator of all key-value pairs stored.
        /// </summary>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return PropertyData.GetEnumerator();
        }

        /// <summary>
        /// An enumerator of all key-value pairs stored.
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return PropertyData.GetEnumerator();
        }

        #endregion


        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Returns a string representation of this object.
        /// </summary>
        public override String ToString()
        {

            var IdString = (Id == null) ? "<null>" : Id.ToString();

            return this.GetType().Name + "(Id = " + IdString + ")";

        }

        #endregion

    }

}