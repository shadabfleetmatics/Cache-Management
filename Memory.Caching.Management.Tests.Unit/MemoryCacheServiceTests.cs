// Copyright 2023.

using System;
using System.Collections.Generic;
using FluentAssertions;
using Memory.Caching.Management.Repository;
using Memory.Caching.Management.Service;
using NUnit.Framework;

namespace Memory.Caching.Management.Tests.Unit
{
    public class MemoryCacheServiceTests
    {
        private static MemoryCacheService BuildCache()
        {
            return new MemoryCacheService(new MemoryCacheRepository());
        }

        private IMemoryCacheService sut;

        private TestObject testObject = new TestObject();

        private class TestObject
        {
            public readonly IList<object> SomeItems = new List<object> { 1, 2, 3, "test123" };
            public string SomeMessage = "test123";
        }

        private const string TestKey = "testKey";

        [SetUp]
        public void BeforeEachTest()
        {
            sut = BuildCache();
            testObject = new TestObject();
        }


        [Test]
        public  void AddAndGetReturnsTheCachedItem()
        {
             sut.Add(TestKey,   "SomeValue");
             var cachedResult = sut.GetAsync<string>(TestKey).GetAwaiter().GetResult();
            Assert.IsNotNull(cachedResult);
            Assert.AreEqual("SomeValue", cachedResult);
        }

        [Test]
        public void RemoveReturnsNull()
        {
            sut.Add(TestKey, "SomeValue");
            sut.Remove(TestKey);
            var cachedResult = sut.Get<string>(TestKey);
            Assert.IsNull(cachedResult);
        }


        [Test]
        public void AddComplexObjectThenGetReturnsCachedObject()
        {
            sut.Add(TestKey, testObject);
            var actual = sut.Get<object>(TestKey) as TestObject;
            var expected = testObject;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddComplexObjectThenGetGenericReturnsCachedObject()
        {
            testObject.SomeItems.Add("AnotherObj");
            testObject.SomeMessage = "changed-object";
            sut.Add(TestKey, testObject);
            var actual = sut.Get<TestObject>(TestKey);
            var expected = testObject;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual);
            testObject.SomeItems.Should().Contain("AnotherObj");
            testObject.SomeMessage.Should().Be("changed-object");
        }

        [Test]
        public void AddEmptyKeyThrowsException()
        {
            Action act = () => sut.Add("", new object());
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Test]
        public void AddNullThrowsException()
        {
            Action act = () => sut.Add<object>(TestKey, null);
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
