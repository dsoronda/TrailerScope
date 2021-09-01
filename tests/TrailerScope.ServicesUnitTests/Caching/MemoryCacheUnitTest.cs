using NUnit.Framework;

using FluentAssertions;
using TrailerScope.Services.Caching;

namespace TrailerScope.ServicesUnitTests.Caching
{
    public class MemoryCacheUnitTest
    {
        
        [SetUp]
        public void Setup() { }

        [Test]
        public void ContainKey_Fails_On_Init()
        {
            var cache = new MemoryCache<int>();
            var key = nameof(Cache_ContainsKeyAndValue_AfterInsert);

            var containsKey = cache.Contains(key);
            containsKey.Should().BeFalse();
        }
        
        [Test]
        public void Cache_ContainsKey_AfterInsert()
        {
            var cache = new MemoryCache<int>();
            var value = 42;
            var key = nameof(Cache_ContainsKey_AfterInsert);

            var containsKey_false = cache.Contains(key);
            containsKey_false.Should().BeFalse();

            cache.AddItem(key, value);
            
            var containsKey_true = cache.Contains(key);
            containsKey_true.Should().BeTrue();
        }
        
        [Test]
        public void Cache_ContainsKeyAndValue_AfterInsert()
        {
            var cache = new MemoryCache<int>();
            var value = 42;
            var key = nameof(Cache_ContainsKeyAndValue_AfterInsert);
            
            var containsKey_false = cache.Contains(key);
            containsKey_false.Should().BeFalse();

            cache.AddItem(key, value);
            
            var containsKey_true = cache.Contains(key);
            containsKey_true.Should().BeTrue();
            cache.GetItem(key).Should().Be(value);
        }
    }
}