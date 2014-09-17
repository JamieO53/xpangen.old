using System;
using NUnit.Framework;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Parameter;

namespace org.xpangen.Generator.Test
{
    [TestFixture]
    public class GenDataReferenceCacheTests
    {
        /// <summary>
        /// Ensure that the 'self' reference is automatically cached.
        /// </summary>
        [TestCase(Description = "Ensure that the 'self' reference is automatically cached")]
        public void ReferenceCacheSelfTest()
        {
            var d = GenDataDef.CreateMinimal().AsGenData();
            d.Cache.Check("Minimal", "self");
            var self = d.Cache["self"];
            Assert.AreSame(d, self);
        }

        /// <summary>
        /// Ensure that simple named references are correctly cached
        /// </summary>
        [TestCase(Description = "Ensure that the 'self' simple named reference cannot be cached internally")]
        [ExpectedException(exceptionType: typeof(ArgumentException), ExpectedMessage = "The 'self' generator data cannot be added explicitly to the cache\r\nParameter name: name")]
        public void ReferenceCacheSelfLocalPathTest()
        {
            var d = GenDataDef.CreateMinimal().AsGenData();
            var d0 = GenDataDef.CreateMinimal().AsGenData();
            d.Cache.Internal("Minimal", "self", d0);
        }

        [TestCase(Description = "Ensure that the base data references and their definitions can be retrieved.")]
        public void GetGenDataBaseReferencesTest()
        {
            var d = GenDataDef.CreateMinimal().AsGenData();
            d.GenDataBase.References.Add("Data", "Def");
            d.GenDataBase.References.Add("Data", "Def");
            var references = d.GenDataBase.References.ReferenceList;
            Assert.AreEqual(3, references.Count);
            Assert.AreEqual("data", references[0].Data);
            Assert.AreEqual("def", references[0].Definition);
            Assert.AreEqual("def", references[1].Data);
            Assert.AreEqual("minimal", references[1].Definition);
            Assert.AreEqual("minimal", references[2].Data);
            Assert.AreEqual("minimal", references[2].Definition);
        }

        [TestCase(Description = "Ensure that the base data references and their definitions are correctly loaded.")]
        public void GetGenDataBaseReferencesLoadTest()
        {
            var d = GenDataDef.CreateMinimal().AsGenData();
            d.GenDataBase.References.Add("Data/Definition", "Minimal");
            d.LoadCache();
            Assert.That(d.Cache.Contains("minimal"));
            Assert.That(d.Cache.Contains("data\\definition"));
        }

        /// <summary>
        /// Ensure that simple named references are correctly cached
        /// </summary>
        [TestCase(Description = "Ensure that simple named references are correctly cached")]
        public void ReferenceCacheLocalPathTest()
        {
            var d = GenDataDef.CreateMinimal().AsGenData();
            var d0 = GenDataDef.CreateMinimal().AsGenData();
            var d1 = d0.DuplicateContext();
            var lookup0 = d.Cache.Internal("Minimal", "d0", d0);
            var lookup1 = d.Cache.Internal("Minimal", "d1", d1);
            Assert.AreSame(d0, lookup0);
            Assert.AreSame(d1, lookup1);
            lookup0 = d.Cache["d0"];
            lookup1 = d.Cache["d1"];
            Assert.AreSame(d0, lookup0);
            Assert.AreSame(d1, lookup1);
            Assert.AreNotSame(lookup1, lookup0);
        }

        /// <summary>
        /// Ensure that simple named references are correctly cached
        /// </summary>
        [TestCase(Description = "Ensure that simple named references are correctly cached")]
        public void ReferenceCacheFilePathTest()
        {
            var d = GenDataDef.CreateMinimal().AsGenData();
            d.GenDataBase.References.Add("Data/Minimal", "Data/Minimal");
            d.GenDataBase.References.Add("Data/Definition", "Data/Minimal");
            d.LoadCache();
            var minimal = d.Cache["Data/Minimal"];
            var definition = d.Cache["Data/Definition"];
            Assert.AreNotSame(minimal, definition);
        }

        /// <summary>
        /// Set up the Generator data definition tests
        /// </summary>
        [TestFixtureSetUp]
        public void SetUp()
        {
            GenDataLoader.Register();
        }

        /// <summary>
        /// Tear down the Generator data definition tests
        /// </summary>
        [TestFixtureTearDown]
        public void TearDown()
        {

        }
    }
}