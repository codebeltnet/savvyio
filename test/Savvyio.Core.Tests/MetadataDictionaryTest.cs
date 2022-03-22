using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon;
using Cuemon.Extensions.Xunit;
using Cuemon.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio
{
    public class MetadataDictionaryTest : Test
    {
        public MetadataDictionaryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Indexer_ShouldGetAndSetElementWithSpecifiedKey()
        {
            var sut = new MetadataDictionary();
            sut["key1"] = "value";
            sut["key2"] = Guid.Empty;
            sut["key3"] = int.MaxValue;

            Assert.Equal(sut["key1"], "value");
            Assert.Equal(sut["key2"], Guid.Empty);
            Assert.Equal(sut["key3"], int.MaxValue);
        }

        [Fact]
        public void ReservedKeywords_ShouldMatchNumberOfPublicConst()
        {
            var sut = typeof(MetadataDictionary);
            var mr = new MemberReflection(o =>
            {
                o.ExcludeInheritancePath = true;
            });

            var reservedKeywords = sut.GetField("ReservedKeywords", mr.Flags).GetValue(null) as IEnumerable<string>;
            var keywordConstants = sut.GetFields(mr.Flags).Where(fi => fi.IsPublic).Select(fi => fi.Name);

            Assert.Equal(reservedKeywords, keywordConstants);
        }

        [Fact]
        public void Count_ShouldMatchNumberOfElements()
        {
            var sut = new MetadataDictionary();
            for (var i = 0; i < 10; i++)
            {
                sut.Add(i.ToString(), i);
            }

            Assert.Equal(sut.Count, 10);
        }

        [Fact]
        public void IsReadOnly_ShouldBeFalse()
        {
            var sut = new MetadataDictionary();

            Assert.False(sut.IsReadOnly);
        }

        [Fact]
        public void Keys_ShouldAllBeNumbers()
        {
            var sut = new MetadataDictionary();
            for (var i = 0; i < 10; i++)
            {
                sut.Add(i.ToString(), i);
            }

            Assert.Equal(Generate.RangeOf(10, i => i.ToString()), sut.Keys);
        }

        [Fact]
        public void Values_ShouldAllBeNumbers()
        {
            var sut = new MetadataDictionary();
            for (var i = 0; i < 10; i++)
            {
                sut.Add(i.ToString(), i);
            }

            Assert.Equal(Generate.RangeOf(10, i => i), sut.Values.Cast<int>());
        }

        [Fact]
        public void Add_ShouldAddElementWithSpecifiedKey()
        {
            var sut = new MetadataDictionary();
            sut.Add("key1", "value");
            sut.Add("key2", Guid.Empty);
            sut.Add(new KeyValuePair<string, object>("key3", int.MaxValue));

            Assert.Equal(sut["key1"], "value");
            Assert.Equal(sut["key2"], Guid.Empty);
            Assert.Equal(sut["key3"], int.MaxValue);
        }

        [Fact]
        public void Clear_ShouldRemoveAllElements()
        {
            var sut = new MetadataDictionary();
            sut.Add("key1", "value");
            sut.Add("key2", Guid.Empty);
            sut.Add(new KeyValuePair<string, object>("key3", int.MaxValue));
            sut.Clear();

            Assert.Empty(sut);
        }

        [Fact]
        public void CopyTo_ShouldCopyElementsToArray()
        {
            var sut = new MetadataDictionary();
            sut.Add("key1", "value");
            sut.Add("key2", Guid.Empty);
            sut.Add(new KeyValuePair<string, object>("key3", int.MaxValue));

            var col = new KeyValuePair<string, object>[3];
            sut.CopyTo(col, 0);

            Assert.Equal(sut, col);
        }

        [Fact]
        public void Contains_ShouldFindMatchingItems()
        {
            var sut = new MetadataDictionary();
            sut.Add("key1", "value");
            sut.Add("key2", Guid.Empty);
            sut.Add(new KeyValuePair<string, object>("key3", int.MaxValue));

            Assert.True(sut.Contains(new KeyValuePair<string, object>("key1", "value")));
            Assert.True(sut.Contains(new KeyValuePair<string, object>("key2", Guid.Empty)));
            Assert.True(sut.Contains(new KeyValuePair<string, object>("key3", int.MaxValue)));
            Assert.False(sut.Contains(new KeyValuePair<string, object>("key4", DateTime.MaxValue)));
        }

        [Fact]
        public void ContainsKey_ShouldFindMatchingItems()
        {
            var sut = new MetadataDictionary();
            sut.Add("key1", "value");
            sut.Add("key2", Guid.Empty);
            sut.Add(new KeyValuePair<string, object>("key3", int.MaxValue));

            Assert.True(sut.ContainsKey("key1"));
            Assert.True(sut.ContainsKey("key2"));
            Assert.True(sut.ContainsKey("key3"));
            Assert.False(sut.ContainsKey("key4"));
        }

        [Fact]
        public void Remove_ShouldRemoveItemByKey()
        {
            var sut = new MetadataDictionary();
            sut.Add("key1", "value");
            sut.Add("key2", Guid.Empty);
            sut.Add(new KeyValuePair<string, object>("key3", int.MaxValue));

            sut.Remove("key1");

            Assert.False(sut.ContainsKey("key1"));
            Assert.True(sut.Count == 2, "sut.Count == 2");
        }

        [Fact]
        public void Remove_ShouldRemoveItemByKeyValuePair()
        {
            var kvp = new KeyValuePair<string, object>("key1", "value");
            var sut = new MetadataDictionary();
            sut.Add("key1", "value");
            sut.Add("key2", Guid.Empty);
            sut.Add(new KeyValuePair<string, object>("key3", int.MaxValue));

            sut.Remove(kvp);

            Assert.False(sut.Contains(kvp));
            Assert.True(sut.Count == 2, "sut.Count == 2");
        }

        [Fact]
        public void TryGetValue_ShouldRetrieveValue()
        {
            var sut = new MetadataDictionary();
            sut.Add("key1", "value");
            sut.Add("key2", Guid.Empty);
            sut.Add(new KeyValuePair<string, object>("key3", int.MaxValue));

            sut.TryGetValue("key2", out var key2Value);

            Assert.Equal(Guid.Empty, key2Value);
        }
    }
}
