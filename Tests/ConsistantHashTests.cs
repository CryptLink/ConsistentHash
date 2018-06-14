using System;
using System.Collections.Generic;
using CryptLink.SigningFramework;
using NUnit.Framework;
using CryptLink.ConsistentHash;

namespace CryptLink.ConsistentHashTests
{
    [TestFixture]
    class ConsistantHashTests {

        [Test, Category("ConsistantHash")]
        public void UnweightedAddRemove() {
            var c = new ConsistentHash<HashableString>(HashProvider.SHA384);
            var firstItem = new HashableString(Guid.NewGuid().ToString());
            firstItem.ComputeHash(c.Provider, null);

            c.Add(firstItem, true, 0);
            Assert.True(c.ContainsNode(firstItem.ComputedHash));

            Assert.True(c.NodeCount == 1);

            c.Remove(firstItem, firstItem.ComputedHash);
            Assert.False(c.ContainsNode(firstItem.ComputedHash));
            Assert.True(c.NodeCount == 0);
        }

        [Test, Category("ConsistantHash")]
        public void NextNode() {
            var c = new ConsistentHash<HashableBytes>(HashProvider.SHA384);
            var firstItem = new HashableBytes(new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 });
            var secondItem = new HashableBytes(new byte[] { 0x10, 0x10, 0x10, 0x10, 0x10, 0x10 });
            var thirdItem = new HashableBytes(new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 });

            firstItem.ComputeHash(c.Provider, null);

            c.Add(secondItem, true, 0);

            var founditem = c.GetNode(firstItem.ComputedHash);
            var founditem2 = c.GetNode(thirdItem.ComputedHash);

            Assert.Equals(secondItem, founditem);
            Assert.Equals(secondItem, founditem2);

        }

        [Test, Category("ConsistantHash")]
        public void UpdateKeyArray() {
            var c = new ConsistentHash<HashableString>(HashProvider.SHA384);
            var firstItem = new HashableString(Guid.NewGuid().ToString());
            var secondItem = new HashableString(Guid.NewGuid().ToString());

            firstItem.ComputeHash(c.Provider, null);
            secondItem.ComputeHash(c.Provider, null);

            c.Add(firstItem, true, 0);
            c.Add(secondItem, false, 0);
            Assert.False(c.ContainsNode(secondItem.ComputedHash));

            c.UpdateKeyArray();
            Assert.True(c.ContainsNode(firstItem.ComputedHash));
        }

        [Test, Category("ConsistantHash")]
        public void AddRange() {
            var c = new ConsistentHash<HashableString>(HashProvider.SHA384);
            var items = new List<HashableString>() {
                new HashableString(Guid.NewGuid().ToString()),
                new HashableString(Guid.NewGuid().ToString()),
                new HashableString(Guid.NewGuid().ToString())
            };

            foreach (var item in items) {
                item.ComputeHash(c.Provider, null);
            }

            c.AddRange(items, true, 0);

            foreach (var item in items) {
                Assert.True(c.ContainsNode(item.ComputedHash));
            }

            Assert.True(c.NodeCount == items.Count);

            foreach (var item in items) {
                c.Remove(item, item.ComputedHash);
                Assert.False(c.ContainsNode(item.ComputedHash));
            }

            Assert.True(c.NodeCount == 0);
        }

        [Test, Category("ConsistantHash")]
        public void WeightedAddRemove() {
            int rounds = 100;
            var c = new ConsistentHash<HashableString>(HashProvider.SHA384);
            var firstItem = new HashableString(Guid.NewGuid().ToString());
            firstItem.ComputeHash(c.Provider, null);

            c.Add(firstItem, true, rounds);
            Assert.True(c.ContainsNode(firstItem.ComputedHash));
            Assert.True(c.NodeCount == rounds);

            c.Remove(firstItem, firstItem.ComputedHash);
            Assert.False(c.ContainsNode(firstItem.ComputedHash));
            Assert.True(c.NodeCount == 0);

            var rehashed = firstItem.ComputedHash.Rehash();
            for (int i = 1; i < rounds; i++) {
                rehashed = firstItem.ComputedHash.Rehash();
                Assert.False(c.ContainsNode(rehashed));
            }
        }

    }
}
