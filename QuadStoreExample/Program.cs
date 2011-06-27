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
using System.Dynamic;
using System.Linq.Expressions;

#endregion

namespace de.ahzf.Blueprints.QuadStore
{

    public class Program
    {

        public static void Main(String[] myArgs)
        {

            IQuad<String> s1, s2, s3, s4, s5;

            var _QuadStore = new QuadStore<String>(
                                     SystemId:        "BlueQuad0001",
                                     QuadIdConverter: (QuadId) => QuadId.ToString(),
                                     DefaultContext:  ()       => "0");

            // Note: Add repositories!

            using (var _Transaction = _QuadStore.BeginTransaction())
            {

                using (var _NestedTransaction = _Transaction.BeginNestedTransaction())
                {
                    s1 = _QuadStore.Add("Alice", "knows", "Bob");
                    _NestedTransaction.Commit();
                }

                s2 = _QuadStore.Add("Alice", "knows", "Dave");
                s3 = _QuadStore.Add("Bob",   "knows", "Carol");
                s4 = _QuadStore.Add("Eve",   "loves", "Alice");
                s5 = _QuadStore.Add("Carol", "loves", "Alice");

                _Transaction.Commit();

            }


            var q1 = _QuadStore.GetQuad(s2.QuadId);

        }

    }

}