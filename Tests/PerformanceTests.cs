using CryptLink.SigningFramework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryptLink.ConsistentHashTests
{
    [TestFixture]
    class PerformanceTests
    {
        private TimeSpan TestLength = new TimeSpan(0, 0, 0, 2, 500);
        private Random random = new Random();
        private int replicationCount = 50;

        [Test, Category("Performance"), Category("Optional")]
        public void AddSinglePerformance() {
            var results = "";

            foreach (HashProvider provider in Enum.GetValues(typeof(HashProvider))) {

                long itemCount = 0;
                DateTime startTime = DateTime.Now;
                var c = new ConsistentHash<HashableString>(provider);

                while ((startTime + TestLength) > DateTime.Now) {
                    var item = new HashableString(Guid.NewGuid().ToString());
                    item.ComputeHash(provider);
                    c.Add(item, true, 0);
                    itemCount++;
                }

                results += GetResultString(provider, itemCount);
            }

            Assert.Pass(results);
        }

        [Test, Category("Performance"), Category("Optional")]
        public void AddThenGetPerformance() {
            var results = "";

            foreach (HashProvider provider in Enum.GetValues(typeof(HashProvider))) {

                long itemCount = 0;
                DateTime startTime = DateTime.Now;
                var c = new ConsistentHash<HashableString>(provider);

                while ((startTime + TestLength) > DateTime.Now) {
                    var item = new HashableString(Guid.NewGuid().ToString());
                    item.ComputeHash(provider);
                    c.Add(item, true, 0);
                    c.GetNode(item.ComputedHash);
                    itemCount++;
                }

                results += GetResultString(provider, itemCount);
            }

            Assert.Pass(results);
        }

        [Test, Category("Performance"), Category("Optional")]
        public void AddBulkPerformance() {
            var results = "";

            foreach (HashProvider provider in Enum.GetValues(typeof(HashProvider))) {

                long itemCount = 0;
                DateTime startTime = DateTime.Now;
                var c = new ConsistentHash<HashableString>(provider);

                while ((startTime + TestLength) > DateTime.Now) {
                    var item = new HashableString(Guid.NewGuid().ToString());
                    item.ComputeHash(provider);
                    c.Add(item, false, 0);
                    itemCount++;
                }

                c.UpdateKeyArray();

                results += GetResultString(provider, itemCount);
            }

            Assert.Pass(results);
        }

        [Test, Category("Performance"), Category("Optional")]
        public void AddRemoveSinglePerformance() {
            var results = "";

            foreach (HashProvider provider in Enum.GetValues(typeof(HashProvider))) {

                long itemCount = 0;
                DateTime startTime = DateTime.Now;
                var c = new ConsistentHash<HashableString>(provider);

                while ((startTime + TestLength) > DateTime.Now) {
                    var item = new HashableString(Guid.NewGuid().ToString());
                    item.ComputeHash(provider);
                    c.Add(item, true, 0);
                    itemCount++;
                    c.Remove(item, true);
                }

                results += GetResultString(provider, itemCount);
            }

            Assert.Pass(results);
        }

        private string GetResultString(HashProvider Provider, long ItemCount) {
            var perSec = (ItemCount / TestLength.TotalSeconds);
            return $"{Provider}: added {ItemCount.ToString("n0")} items, replication count: {replicationCount.ToString("n0")}, ({perSec.ToString("n0")} unique per sec, {(perSec * replicationCount).ToString("n0")} replicated)\r\n";
        }

    }
}
