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
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

using de.ahzf.Blueprints.PropertyGraph.InMemory;

using NUnit.Framework;

#endregion

namespace de.ahzf.Blueprints.UnitTests.SimplePropertyGraphTests
{

    /// <summary>
    /// SimplePropertyGraph unit tests for creating vertices.
    /// </summary>
    [TestFixture]
    public class CreateGraphTests : InitGraph
    {

        #region EmptyConstructorTest()

        /// <summary>
        /// A test for an empty VertexId constructor.
        /// </summary>
        [Test]
        public void EmptyConstructorTest()
        {
            var _Graph = CreateGraph();
            Assert.IsTrue(_Graph != null);
        }

        #endregion

    }

}