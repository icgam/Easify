// This software is part of the Easify framework
// Copyright (C) 2019 Intermediate Capital Group
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;

namespace Easify.Testing
{
    public class FixtureBase
    {
        public FixtureBase()
        {
            Fixture = new Fixture();
            Fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            Fixture.Customize(new AutoNSubstituteCustomization());
        }

        public Fixture Fixture { get; }

        public ILogger<T> Logger<T>()
        {
            return Substitute.For<ILogger<T>>();
        }

        public T Fake<T>() where T : class
        {
            return Substitute.For<T>();
        }

        public T FakeEntity<T>()
        {
            return Fixture.Create<T>();
        }

        public List<T> FakeEntityList<T>(int count = 3)
        {
            return Fixture.CreateMany<T>(count).ToList();
        }

        protected static T LoadSampleData<T>(string path) where T : class, new()
        {
            var content = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}