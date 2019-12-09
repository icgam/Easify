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