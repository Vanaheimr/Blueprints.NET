﻿/*
 * Copyright (c) 2010-2015, Achim 'ahzf' Friedland <achim@graphdefined.org>
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
using System.Dynamic;
using System.Collections.Generic;

#endregion

namespace org.GraphDefined.Vanaheimr.Balder
{

    /// <summary>
    /// An interface for simplified interaction with dynamic objects.
    /// </summary>
    /// <typeparam name="CompileTimeType">The compile time type of the DynamicMetaObject.</typeparam>
    public interface IDynamicGraphElement<CompileTimeType> : IDynamicMetaObjectProvider
    {

        /// <summary>
        /// Return all binder names.
        /// </summary>
        IEnumerable<String> GetDynamicMemberNames();

        /// <summary>
        /// Assign the given value to the given binder name.
        /// </summary>
        /// <param name="Binder">A binder name.</param>
        /// <param name="Object">A value.</param>
        Object SetMember   (String Binder, Object Object);

        /// <summary>
        /// Return the value of the given binder name.
        /// </summary>
        /// <param name="Binder">A binder name.</param>
        Object GetMember   (String Binder);

        /// <summary>
        /// Delete the given binder name.
        /// </summary>
        /// <param name="Binder">A binder name.</param>
        Object DeleteMember(String Binder);

    }

}
