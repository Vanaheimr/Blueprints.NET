﻿/*
 * Copyright (c) 2010-2014, Achim 'ahzf' Friedland <achim@graphdefined.org>
 * This file is part of Balder <http://www.github.com/Vanaheimr/Balder>
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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Illias.Collections;

#endregion

namespace org.GraphDefined.Vanaheimr.Balder.InMemory
{

    /// <summary>
    /// The abstract graph element is the base class for all property graph elements:
    /// The vertices, edges, multiedges, hyperedges and the property graph itself.
    /// </summary>
    /// <typeparam name="TId">The graph element identification.</typeparam>
    /// <typeparam name="TRevId">The graph element revision identification.</typeparam>
    /// <typeparam name="TLabel">The type of the graph element label.</typeparam>
    /// <typeparam name="TKey">The type of the graph element property keys.</typeparam>
    /// <typeparam name="TValue">The type of the graph element property values.</typeparam>
    public abstract class AGraphElement<TId, TRevId, TLabel, TKey, TValue>
                              : IGraphElement<TId, TRevId, TLabel, TKey, TValue>

        where TId     : IEquatable<TId>,    IComparable<TId>,    IComparable, TValue
        where TRevId  : IEquatable<TRevId>, IComparable<TRevId>, IComparable, TValue
        where TLabel  : IEquatable<TLabel>, IComparable<TLabel>, IComparable, TValue
        where TKey    : IEquatable<TKey>,   IComparable<TKey>,   IComparable

    {

        #region Data

        /// <summary>
        /// Good for explicit locking of this object
        /// during complex operations.
        /// </summary>
        protected readonly Object LockObject;

        /// <summary>
        /// The datastructure holding all graph properties.
        /// </summary>
        protected readonly IDictionary<TKey, TValue> PropertyData;

        /// <summary>
        /// A delegate to create a new vote.
        /// </summary>
        private readonly Func<IVote<Boolean>> VoteCreator;

        #endregion

        #region Properties

        #region IdKey

        /// <summary>
        /// The property key of the identification.
        /// </summary>
        public TKey IdKey { get; private set; }

        #endregion

        #region Id

        /// <summary>
        /// An identifier that is unique to its inheriting class.
        /// All vertices, edges, multiedges and hyperedges of a graph must have an unique identifier.
        /// </summary>
        public TId Id
        {
            get
            {

                TValue _TValue;

                if (PropertyData.TryGetValue(IdKey, out _TValue))
                    return (TId) _TValue;

                return default(TId);

            }
        }

        //ToDo: Solve ambiguity!
        //TValue IReadOnlyProperties<TKey, TValue>.Id
        //{
        //    get
        //    {

        //        TValue _TValue;

        //        if (PropertyData.TryGetValue(IdKey, out _TValue))
        //            return _TValue;

        //        return default(TId);

        //    }
        //}

        #endregion

        #region RevIdKey

        /// <summary>
        /// The property key of the revision identification.
        /// </summary>
        public TKey RevIdKey { get; private set; }

        #endregion

        #region RevId

        /// <summary>
        /// The RevId extends the Id to identify multiple revisions of
        /// an element during the lifetime of a graph. A RevId should
        /// additionally be unique among all elements of a graph.
        /// </summary>
        public TRevId RevId
        {
            get
            {

                TValue _TValue;

                if (PropertyData.TryGetValue(RevIdKey, out _TValue))
                    return (TRevId) (Object) _TValue;

                return default(TRevId);

            }
        }

        //ToDo: Solve ambiguity!
        //TValue IReadOnlyProperties<TKey, TValue>.RevId
        //{
        //    get
        //    {

        //        TValue _TValue;

        //        if (PropertyData.TryGetValue(RevIdKey, out _TValue))
        //            return _TValue;

        //        return default(TId);

        //    }
        //}

        #endregion

        #region LabelKey

        /// <summary>
        /// The property key of the label.
        /// </summary>
        public TKey LabelKey { get; private set; }

        #endregion

        #region Label

        /// <summary>
        /// Provides a label of something.
        /// </summary>
        public TLabel Label
        {
            get
            {

                TValue _TValue;

                if (PropertyData.TryGetValue(LabelKey, out _TValue))
                    return (TLabel) (Object) _TValue;

                return default(TLabel);

            }
        }

        #endregion

        #endregion

        #region Events

        #region OnPropertyAdding

        /// <summary>
        /// Called when a property value will be added.
        /// </summary>
        public event PropertyAddingEventHandler<TKey, TValue> OnPropertyAdding;

        #endregion

        #region OnPropertyAdded

        /// <summary>
        /// Called whenever a property value was added.
        /// </summary>
        public event PropertyAddedEventHandler<TKey, TValue> OnPropertyAdded;

        #endregion

        #region OnPropertyChanging

        /// <summary>
        /// Called whenever a property value will be changed.
        /// </summary>
        public event PropertyChangingEventHandler<TKey, TValue> OnPropertyChanging;

        #endregion

        #region OnPropertyChanged

        /// <summary>
        /// Called whenever a property value was changed.
        /// </summary>
        public event PropertyChangedEventHandler<TKey, TValue> OnPropertyChanged;

        #endregion

        #region OnPropertyRemoving

        /// <summary>
        /// Called whenever a property value will be removed.
        /// </summary>
        public event PropertyRemovingEventHandler<TKey, TValue> OnPropertyRemoving;

        #endregion

        #region OnPropertyRemoved

        /// <summary>
        /// Called whenever a property value was removed.
        /// </summary>
        public event PropertyRemovedEventHandler<TKey, TValue> OnPropertyRemoved;

        #endregion

        #endregion

        #region (internal) Send[...]Notifications(...)

        #region (internal) SendPropertyAdditionVote(Key, Value)

        /// <summary>
        /// Notify and send a vore about a property to be added.
        /// </summary>
        /// <param name="Key">The key of the property to be added.</param>
        /// <param name="Value">The value of the property to be added.</param>
        internal Boolean SendPropertyAdditionVote(TKey Key, TValue Value)
        {

            var Vote = VoteCreator();

            if (OnPropertyAdding != null)
                OnPropertyAdding(this, Key, Value, Vote);

            return Vote.Result;

        }

        #endregion

        #region (internal) SendPropertyAddedNotification(Key, Value)

        /// <summary>
        /// Notify about an added property.
        /// </summary>
        /// <param name="Key">The key of the added property.</param>
        /// <param name="Value">The value of the added property.</param>
        internal void SendPropertyAddedNotification(TKey Key, TValue Value)
        {
            if (OnPropertyAdded != null)
                OnPropertyAdded(this, Key, Value);
        }

        #endregion

        #region (internal) SendPropertyChangingVote(Key, OldValue, NewValue)

        /// <summary>
        /// Notify and send vote about a property to be changed.
        /// </summary>
        /// <param name="Key">The key of the property to be changed.</param>
        /// <param name="OldValue">The old value of the property to be changed.</param>
        /// <param name="NewValue">The new value of the property to be changed.</param>
        internal Boolean SendPropertyChangingVote(TKey Key, TValue OldValue, TValue NewValue)
        {

            var Vote = VoteCreator();

            if (OnPropertyChanging != null)
                OnPropertyChanging(this, Key, OldValue, NewValue, Vote);

            return Vote.Result;

        }

        #endregion

        #region (internal) SendPropertyChangedNotification(Key, OldValue, NewValue)

        /// <summary>
        /// Notify about a changed property.
        /// </summary>
        /// <param name="Key">The key of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal void SendPropertyChangedNotification(TKey Key, TValue OldValue, TValue NewValue)
        {
            if (OnPropertyChanged != null)
                OnPropertyChanged(this, Key, OldValue, NewValue);
        }

        #endregion

        #region (internal) SendPropertyRemovalVote(Key, Value)

        /// <summary>
        /// Notify and send vote about a property to be removed.
        /// </summary>
        /// <param name="Key">The key of the property to be removed.</param>
        /// <param name="Value">The value of the property to be removed.</param>
        internal Boolean SendPropertyRemovalVote(TKey Key, TValue Value)
        {

            var Vote = VoteCreator();

            if (OnPropertyRemoving != null)
                OnPropertyRemoving(this, Key, Value, Vote);

            return Vote.Result;

        }

        #endregion

        #region (internal) SendPropertyRemovedNotification(Key, Value)

        /// <summary>
        /// Notify about a removed property.
        /// </summary>
        /// <param name="Key">The key of the removed property.</param>
        /// <param name="Value">The value of the removed property.</param>
        internal void SendPropertyRemovedNotification(TKey Key, TValue Value)
        {
            if (OnPropertyRemoved != null)
                OnPropertyRemoved(this, Key, Value);
        }

        #endregion

        #endregion

        #region (internal protected) Constructor(s)

        #region (internal protected) AGraphElement(Id, Label, IdKey, RevIdKey, LabelKey, DatastructureInitializer, PropertiesInitializer = null, VoteCreator = null)

        /// <summary>
        /// Creates a new abstract graph element.
        /// </summary>
        /// <param name="Id">The Id of this graph element.</param>
        /// <param name="Label">The label of the graph element.</param>
        /// <param name="IdKey">The key to access the Id of this graph element.</param>
        /// <param name="RevIdKey">The key to access the RevId of this graph element.</param>
        /// <param name="LabelKey">The key to access the Label of this graph element.</param>
        /// <param name="DatastructureInitializer">A delegate to initialize the properties datastructure of the this graph element.</param>
        /// <param name="PropertiesInitializer">A delegate to do some initial operations like adding some properties.</param>
        /// <param name="VoteCreator">A delegate to create a new vote.</param>
        internal protected AGraphElement(TId                                   Id,
                                         TLabel                                Label,
                                         TKey                                  IdKey,
                                         TKey                                  RevIdKey,
                                         TKey                                  LabelKey,
                                         IDictionaryInitializer<TKey, TValue>  DatastructureInitializer,
                                         IPropertiesInitializer<TKey, TValue>  PropertiesInitializer  = null,
                                         Func<IVote<Boolean>>                  VoteCreator = null)

        {

            #region Initial checks

            if (IdKey    == null)
                throw new ArgumentNullException("IdKey", "The given IdKey must not be null!");

            if (Id       == null)
                throw new ArgumentNullException("Id", "The given Id must not be null!");

            if (RevIdKey == null)
                throw new ArgumentNullException("RevIdKey", "The given RevIdKey must not be null!");

            if (LabelKey == null)
                throw new ArgumentNullException("LabelKey", "The given LabelKey must not be null!");

            if (DatastructureInitializer == null)
                throw new ArgumentNullException("DatastructureInitializer", "The given DatastructureInitializer must not be null!");

            #endregion

            // Good for explicit locking of this object
            // during complex operations...
            this.LockObject    = new Object();

            this.IdKey         = IdKey;
            this.RevIdKey      = RevIdKey;
            this.LabelKey      = LabelKey;
            this.PropertyData  = DatastructureInitializer();
            this.PropertyData.Add(IdKey,    Id);
            this.PropertyData.Add(RevIdKey, RevId);
            this.PropertyData.Add(LabelKey, Label);

            this.VoteCreator   = (VoteCreator != null) ? VoteCreator
                                                       : () => new VetoVote();

            if (PropertiesInitializer != null)
                PropertiesInitializer(this);

        }

        #endregion

        #endregion


        #region IDynamicGraphObject<PropertyVertex> Members

        #region GetDynamicMemberNames()

        /// <summary>
        /// Returns an enumeration of all property keys.
        /// </summary>
        public virtual IEnumerable<String> GetDynamicMemberNames()
        {

            return PropertyData.Keys.
                                Select(key => key.ToString());

        }

        #endregion


        #region SetMember(Binder, Object)

        /// <summary>
        /// Sets a new property or overwrites an existing.
        /// </summary>
        /// <param name="Binder">The property key</param>
        /// <param name="Object">The property value</param>
        public virtual Object SetMember(String Binder, Object Object)
        {
            return Set((TKey) (Object) Binder, (TValue) Object);
        }

        #endregion

        #region GetMember(Binder)

        /// <summary>
        /// Returns the value of the given property key.
        /// </summary>
        /// <param name="Binder">The property key.</param>
        public virtual Object GetMember(String Binder)
        {
            TValue _Object;
            TryGetProperty((TKey) (Object) Binder, out _Object);
            return _Object as Object;
        }

        #endregion

        #region DeleteMember(Binder)

        /// <summary>
        /// Tries to remove the property identified by the given property key.
        /// </summary>
        /// <param name="Binder">The property key.</param>
        public virtual Object DeleteMember(String Binder)
        {

            try
            {
                PropertyData.Remove((TKey) (Object) Binder);
                return true;
            }
            catch
            { }

            return false;

        }

        #endregion

        #endregion

        #region IProperties Members

        #region Set(Key, Value)

        /// <summary>
        /// Add a KeyValuePair to the graph element.
        /// If a value already exists for the given key, then the previous value is overwritten.
        /// </summary>
        /// <param name="Key">A key.</param>
        /// <param name="newValue">A value.</param>
        public virtual IProperties<TKey, TValue> Set(TKey Key, TValue newValue)
        {

            #region Initial Checks

            if (Key.Equals(IdKey))
                throw new IdentificationChangeException();

            if (Key.Equals(RevIdKey))
                throw new RevIdentificationChangeException();

            if (Key.Equals(Label))
                throw new RevIdentificationChangeException();

            #endregion

            TValue oldValue;

            if (PropertyData.TryGetValue(Key, out oldValue))
            {
                if (SendPropertyChangingVote(Key, oldValue, newValue))
                {
                    PropertyData[Key] = newValue;
                    SendPropertyChangedNotification(Key, oldValue, newValue);
                }
            }

            else
            {
                if (SendPropertyAdditionVote(Key, newValue))
                {
                    PropertyData.Add(Key, newValue);
                    SendPropertyAddedNotification(Key, newValue);
                }
            }

            return this;

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

            return PropertyData.Values.
                                Any(v => v.Equals(Value));

        }

        #endregion

        #region Contains(Key, Value)

        /// <summary>
        /// Determines if the given key and value exists.
        /// </summary>
        /// <param name="Key">A key.</param>
        /// <param name="Value">A value.</param>
        public Boolean Contains(TKey Key, TValue Value)
        {

            TValue _Value;

            if (PropertyData.TryGetValue(Key, out _Value))
                return Value.Equals(_Value);

            return false;

        }

        #endregion

        #region Contains(KeyValuePair)

        /// <summary>
        /// Determines if the given KeyValuePair exists.
        /// </summary>
        /// <param name="KeyValuePair">A KeyValuePair.</param>
        public Boolean Contains(KeyValuePair<TKey, TValue> KeyValuePair)
        {
            return PropertyData.Contains(KeyValuePair);
        }

        #endregion


        #region this[Key]

        /// <summary>
        /// Return the value associated with the given key.
        /// </summary>
        /// <param name="Key">A key.</param>
        public virtual TValue this[TKey Key]
        {
            get
            {

                TValue _Object;

                PropertyData.TryGetValue(Key, out _Object);

                return _Object;

            }
        }

        #endregion

        #region TryGetProperty(Key, out Value)

        /// <summary>
        /// Return the value associated with the given key.
        /// </summary>
        /// <param name="Key">A key.</param>
        /// <param name="Value">The associated value.</param>
        /// <returns>True if the returned value is valid. False otherwise.</returns>
        public virtual Boolean TryGetProperty(TKey Key, out TValue Value)
        {
            return PropertyData.TryGetValue(Key, out Value);
        }

        #endregion

        #region TryGetProperty<T>(Key, out Value)

        /// <summary>
        /// Return the value associated with the given key.
        /// </summary>
        /// <param name="Key">A key.</param>
        /// <param name="Value">The associated value.</param>
        /// <returns>True if the returned value is valid. False otherwise.</returns>
        public virtual Boolean TryGetProperty<T>(TKey Key, out T Value)
        {

            TValue _Value;

            if (PropertyData.TryGetValue(Key, out _Value))
            {

                try
                {
                    Value = (T) (Object) _Value;
                }
                catch (Exception)
                {
                    Value = default(T);
                    return false;
                }

                return true;

            }

            Value = default(T);
            return false;

        }

        #endregion

        #region GetProperties(Include = null)

        /// <summary>
        /// Return a filtered enumeration of all KeyValuePairs.
        /// </summary>
        /// <param name="Include">A delegate to filter properties based on their keys and values.</param>
        /// <returns>A enumeration of all key/value pairs matching the given KeyValueFilter.</returns>
        public virtual IEnumerable<KeyValuePair<TKey, TValue>> GetProperties(KeyValueFilter<TKey, TValue> Include = null)
        {

            return (Include == null) ? PropertyData.Where(kvp => kvp.Value != null)
                                     : PropertyData.Where(kvp => kvp.Value != null && Include(kvp.Key, kvp.Value));

        }

        #endregion

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


        #region Remove(Key)

        /// <summary>
        /// Removes all KeyValuePairs associated with the given key.
        /// </summary>
        /// <param name="Key">A key.</param>
        /// <returns>The value associated with that key prior to the removal.</returns>
        public virtual TValue Remove(TKey Key)
        {

            #region Initial Checks

            if (Key.Equals(IdKey))
                throw new ArgumentException("Removing the Id property is not allowed!");

            if (Key.Equals(RevIdKey))
                throw new ArgumentException("Removing the RevId property is not allowed!");

            #endregion

            TValue _Value;

            if (PropertyData.TryGetValue(Key, out _Value))
            {
                if (SendPropertyRemovalVote(Key, _Value))
                {

                    if (!PropertyData.Remove(Key))
                        throw new Exception("Could not delete '" + Key + "'!");

                    SendPropertyRemovedNotification(Key, _Value);

                }
            }

            return _Value;

        }

        #endregion

        #region Remove(Key, Value)

        /// <summary>
        /// Remove the given key and value pair.
        /// </summary>
        /// <param name="Key">A key.</param>
        /// <param name="Value">A value.</param>
        /// <returns>The value associated with that key prior to the removal.</returns>
        public TValue Remove(TKey Key, TValue Value)
        {

            #region Initial Checks

            if (Key.Equals(IdKey))
                throw new ArgumentException("Removing the Id property is not allowed!");

            if (Key.Equals(RevIdKey))
                throw new ArgumentException("Removing the RevId property is not allowed!");

            #endregion

            TValue _Value;

            if (TryGetProperty(Key, out _Value))
            {
                if (_Value.Equals(Value))
                {
                    if (SendPropertyRemovalVote(Key, _Value))
                    {
                        Remove(Key);
                        SendPropertyRemovedNotification(Key, _Value);
                        return _Value;
                    }
                }
            }

            return default(TValue);
            
        }

        #endregion

        #region Remove(KeyValueFilter = null)

        /// <summary>
        /// Remove all KeyValuePairs specified by the given KeyValueFilter.
        /// Removing the Id or RevId property is not supported!
        /// </summary>
        /// <param name="KeyValueFilter">A delegate to remove properties based on their keys and values.</param>
        /// <returns>A enumeration of all key/value pairs removed by the given KeyValueFilter before their removal.</returns>
        public IEnumerable<KeyValuePair<TKey, TValue>> Remove(KeyValueFilter<TKey, TValue> KeyValueFilter = null)
        {

            var ToDelete = new List<KeyValuePair<TKey, TValue>>();
            var Deleted  = new List<KeyValuePair<TKey, TValue>>();

            // Collect KeyValuePair to delete...
            if (KeyValueFilter != null)
                foreach (var KeyValuePair in PropertyData)
                    if (KeyValueFilter(KeyValuePair.Key, KeyValuePair.Value))
                        if (KeyValuePair.Key.Equals(IdKey))
                            throw new ArgumentException("Removing the Id property is not allowed!");
                        else if (KeyValuePair.Key.Equals(RevIdKey))
                            throw new ArgumentException("Removing the RevId property is not allowed!");
                        else
                            ToDelete.Add(KeyValuePair);


            // Delete them now...
            foreach (var KeyValuePair in ToDelete)
                if (SendPropertyRemovalVote(KeyValuePair.Key, KeyValuePair.Value))
                {
                    Remove(KeyValuePair.Key);
                    Deleted.Add(KeyValuePair);
                    SendPropertyRemovedNotification(KeyValuePair.Key, KeyValuePair.Value);
                }


            // Return deleted KeyValuePairs
            return Deleted;

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

        #region IComparable Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public virtual Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given Object must not be null!");

            // Check if the given object can be casted to a AGraphElement
            var AGraphElement = Object as AGraphElement<TId, TRevId, TLabel, TKey, TValue>;

            if ((Object) AGraphElement == null)
                throw new ArgumentException("The given object is not a AGraphElement!");

            return CompareTo(AGraphElement);

        }

        #endregion

        #region CompareTo(Id)

        /// <summary>
        /// Compares this object to an identification.
        /// </summary>
        /// <param name="Id">An Id to compare with.</param>
        public Int32 CompareTo(TId Id)
        {

            if ((Object) Id == null)
                throw new ArgumentNullException("The given Id must not be null!");

            return Id.CompareTo(Id);

        }

        #endregion

        #region CompareTo(IReadOnlyGraphElement)

        /// <summary>
        /// Compares this object to another readonly GraphElement.
        /// </summary>
        /// <param name="IReadOnlyGraphElement">An IReadOnlyGraphElement to compare with.</param>
        public Int32 CompareTo(IReadOnlyGraphElement<TId, TRevId, TLabel, TKey, TValue> IReadOnlyGraphElement)
        {

            if ((Object) IReadOnlyGraphElement == null)
                throw new ArgumentNullException("The given GraphElement must not be null!");

            return Id.CompareTo(IReadOnlyGraphElement.Id);

        }

        #endregion

        #region CompareTo(IGraphElement)

        /// <summary>
        /// Compares this object to another GraphElement.
        /// </summary>
        /// <param name="IGraphElement">An IGraphElement to compare with.</param>
        public Int32 CompareTo(IGraphElement<TId, TRevId, TLabel, TKey, TValue> IGraphElement)
        {

            if ((Object) IGraphElement == null)
                throw new ArgumentNullException("The given GraphElement must not be null!");

            return Id.CompareTo(IGraphElement.Id);

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

            // Check if the given object can be casted to a AGraphElement
            var AGraphElement = Object as AGraphElement<TId, TRevId, TLabel, TKey, TValue>;

            if ((Object) AGraphElement == null)
                return false;

            return this.Equals(AGraphElement);

        }

        #endregion

        #region Equals(Id)

        /// <summary>
        /// Compares this object to an identification.
        /// </summary>
        /// <param name="Id">An Id to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(TId Id)
        {

            if ((Object) Id == null)
                return false;

            return Id.Equals(Id);

        }

        #endregion

        #region Equals(IReadOnlyGraphElement)

        /// <summary>
        /// Compares this object to an IReadOnlyGraphElement.
        /// </summary>
        /// <param name="IReadOnlyGraphElement">An IReadOnlyGraphElement to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(IReadOnlyGraphElement<TId, TRevId, TLabel, TKey, TValue> IReadOnlyGraphElement)
        {

            if ((Object) IReadOnlyGraphElement == null)
                return false;

            return Id.Equals(IReadOnlyGraphElement.Id);

        }

        #endregion

        #region Equals(IGraphElement)

        /// <summary>
        /// Compares this object to an IGraphElement.
        /// </summary>
        /// <param name="IGraphElement">An IGraphElement to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(IGraphElement<TId, TRevId, TLabel, TKey, TValue> IGraphElement)
        {

            if ((Object)IGraphElement == null)
                return false;

            return Id.Equals(IGraphElement.Id);

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
            return Id.GetHashCode();
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {

            var IdString = (Id == null) ? "<null>" : Id.ToString();

            return this.GetType().Name + "(Id = " + IdString + ")";

        }

        #endregion

    }

}
