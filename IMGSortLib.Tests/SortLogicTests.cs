using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IMGSortLib.Sort;
using System.Collections.Generic;
using IMGSortLib.Dummies;
using System.Linq;

namespace IMGSortLib.Tests
{
    [TestClass]
    public class SortLogicTests
    {
        [TestMethod]
        public void RemoveDuplicates_Duplicates_1Item()
        {
            var logic = new SortLogic();
            logic.SourcePaths = new HashSet<string>() { "X:\\Test" };
            logic._fileItems = new List<DeltaItem>()
            {
                new DeltaItem() {SourceFile=new FileDataDummy("Bild01.jpg","X:\\Bilder\\",new DateTime(2016,10,01),DateTime.Now)},
                new DeltaItem() {SourceFile=new FileDataDummy("Bild01.jpg","X:\\Bilder\\Duble\\",new DateTime(2016,10,01),DateTime.Now)},
            };
            logic.CalcTarget();
            logic.RemoveDuplicates();
            Assert.AreEqual(1, logic._fileItems.Count);
        }
        [TestMethod]
        public void RemoveDuplicates_DuplicatesAndHash_3Items()
        {
            var logic = new SortLogic();
            logic.SourcePaths = new HashSet<string>() { "X:\\Test" };
            logic._fileItems = new List<DeltaItem>()
            {
                new DeltaItem() {SourceFile=new FileDataDummy("Bild01.jpg","X:\\Bilder\\",new DateTime(2016,10,01),DateTime.Now)},
                new DeltaItem() {SourceFile=new FileDataDummy("Bild01.jpg","X:\\Bilder\\Duble\\",new DateTime(2016,10,01),DateTime.Now)},
                new DeltaItem() {SourceFile=new FileDataDummy("Bild02.jpg","X:\\Bilder\\",new DateTime(2016,10,01),DateTime.Now), FileHash = "12345"},
                new DeltaItem() {SourceFile=new FileDataDummy("Bild02.jpg","X:\\Bilder\\Duble\\",new DateTime(2016,10,01),DateTime.Now), FileHash = "12345"},
                new DeltaItem() {SourceFile=new FileDataDummy("Bild02.jpg","X:\\Bilder\\Duble\\",new DateTime(2016,10,01),DateTime.Now), FileHash = "145"}
            };
            logic.CalcTarget();
            logic.RemoveDuplicates();
            Assert.AreEqual(3, logic._fileItems.Count);
        }
        [TestMethod]
        public void RemoveDuplicates_DifferntHashTakenInSameSecond()
        {
            var logic = new SortLogic();
            logic.SourcePaths = new HashSet<string>() { "X:\\Test" };
            logic._fileItems = new List<DeltaItem>()
            {
                new DeltaItem() {SourceFile=new FileDataDummy("Bild02.jpg","X:\\Bilder\\",new DateTime(2016,10,01),DateTime.Now), FileHash = "12345"},
                new DeltaItem() {SourceFile=new FileDataDummy("Bild02.jpg","X:\\Bilder\\Duble\\",new DateTime(2016,10,01),DateTime.Now), FileHash = "12346"},
            };
            logic.CalcTarget();
            logic.RemoveDuplicates();
            Assert.AreEqual(2, logic._fileItems.Count);
            Assert.AreNotEqual(logic._fileItems.ToList()[0].TargetFullPath, logic._fileItems.ToList()[1].TargetFullPath);
        }

    }
}
