using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IMGSortLib.Sort;
using System.Collections.Generic;
using IMGSortLib.Dummies;

namespace IMGSortLib.Tests
{
    [TestClass]
    public class SortLogicTests
    {
        [TestMethod]
        public void RemoveDuplicatesWithDuplicates()
        {
            var logic = new SortLogic("X:\\Test");
            logic._fileItems = new List<DeltaItem>()
            {
                new DeltaItem() {SourceFile=new FileDataDummy("Bild01.jpg","X:\\Bilder\\",new DateTime(2016,10,01),DateTime.Now)},
                new DeltaItem() {SourceFile=new FileDataDummy("Bild01.jpg","X:\\Bilder\\Duble\\",new DateTime(2016,10,01),DateTime.Now)}
            };
            logic.CalcTarget();
            logic.RemoveDuplicates();
        }
    }
}
