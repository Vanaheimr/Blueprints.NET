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

using de.ahzf.Blueprints.PropertyGraphs;

using Jurassic;
using Jurassic.Library;

#endregion

namespace de.ahzf.Blueprints.HTTP.Server
{

    /// <summary>
    /// A wrapper to access a graph element from JavaScript.
    /// </summary>
    public class JSGraphElement : ObjectInstance
    {

        #region Data

        /// <summary>
        /// The internal graph element.
        /// </summary>
        private readonly IGraphElement<UInt64, Int64, String, Object> GraphElement;

        /// <summary>
        /// The internal JavaScript engine;
        /// </summary>
        private readonly ScriptEngine JavaScriptEngine;

        #endregion

        #region Constructor(s)

        #region JSGraphElement(JavaScriptEngine)

        /// <summary>
        /// Create a new graph element wrapper for JavaScript.
        /// </summary>
        /// <param name="GraphElement">A graph element.</param>
        /// <param name="JavaScriptEngine">An instance of a JavaScript engine.</param>
        public JSGraphElement(IGraphElement<UInt64, Int64, String, Object> GraphElement, ScriptEngine JavaScriptEngine)
            : base(JavaScriptEngine)
        {

            this.GraphElement     = GraphElement;
            this.JavaScriptEngine = JavaScriptEngine;

            // Int32 is a Jurassic limitation as JavaScript only supports "number"
            // http://jurassic.codeplex.com/wikipage?title=Supported%20types&referringTitle=Exposing%20a%20.NET%20class%20to%20JavaScript
            this.DefineProperty("Id",      new PropertyDescriptor((Int32) GraphElement.Id,  PropertyAttributes.Sealed), true);
            this.DefineProperty("RevId",   new PropertyDescriptor(GraphElement.RevId,       PropertyAttributes.Sealed), true);

        }

        #endregion

        #endregion


        #region (override) GetMissingPropertyValue(PropertyKey)

        /// <summary>
        /// Retrieves the value of a property which doesn't exist on the object.  This method can
        /// be overridden to effectively construct properties on the fly.  The default behavior is
        /// to return <c>undefined</c>.
        /// </summary>
        /// <param name="PropertyKey">The name of the missing property.</param>
        /// <returns>The value of the missing property.</returns>
        protected override Object GetMissingPropertyValue(String PropertyKey)
        {

            Object PropertyValue;

            if (GraphElement.TryGetProperty(PropertyKey, out PropertyValue))
                return PropertyValue;

            else
                return null;

        }

        #endregion

    }

}