using System;
using System.Collections.Generic;
using CryptLink.SigningFramework;
using NUnit.Framework;

namespace CryptLink.ConsistentHashTests
{
    [TestFixture]
    class ConsistantHashTests {

        [Test, Category("Hashing")]
        public void ConsistentUnweightedRoundtrip() {
            var c = new ConsistentHash<HashableString>(HashProvider.SHA384);
            var firstItem = new HashableString(Guid.NewGuid().ToString());
            firstItem.ComputeHash(c.Provider, null);

            c.Add(firstItem, true, 0);
            Assert.True(c.ContainsNode(firstItem.ComputedHash));

            c.Remove(firstItem, firstItem.ComputedHash);
            Assert.False(c.ContainsNode(firstItem.ComputedHash));
        }

    }
}
