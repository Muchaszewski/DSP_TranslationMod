using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using TranslationCommon.Fonts;

namespace Tests.UIFixes
{
    [TestFixture]
    public class RangeValueTests
    {
        [TestCase("-7", -7, 0, 0)]
        [TestCase("-7", -7, -10, 1)]
        [TestCase("0", 0, 0, 1)]
        [TestCase("0", 0, -1, 1)]
        [TestCase("1", 1)]
        [TestCase("4", 4)]
        public void RangeValue_Equal_Test(string test, int expected, int startRange = 0, int expectedCount = 1)
        {
            var range = RangeValue.Parse(test);
            range.Reset(startRange);
            var count = 0;
            while (range.TryGetNextMatch(out var match))
            {
                Assert.AreEqual(expected, match);
                count++;
            }

            Assert.AreEqual(expectedCount, count);
        }
        
        [TestCase("<-7", new []{-10, -9, -8}, -10)]
        [TestCase("<0", new int[]{}, 0)]
        [TestCase("<4", new []{0, 1, 2, 3})]
        public void RangeValue_LessThan_Test(string test, int[] expected, int startRange = 0)
        {
            RangeValueTestRange(test, expected, startRange);
        }
        
        [TestCase("<=-7", new []{-10, -9, -8, -7}, -10)]
        [TestCase("<=0", new int[]{0})]
        [TestCase("<=4", new []{0, 1, 2, 3, 4})]
        public void RangeValue_LessThanEqual_Test(string test, int[] expected, int startRange = 0)
        {
            RangeValueTestRange(test, expected, startRange);
        }

        [TestCase(">-7", new []{-6, -5, -4,}, -10)]
        [TestCase(">0", new int[]{1, 2, 3})]
        [TestCase(">4", new []{5, 6, 7})]
        public void RangeValue_GreaterThen_Test(string test, int[] expected, int startRange = 0, int iterations = 3)
        {
            RangeValueTestRange(test, expected, startRange, iterations); 
        }
        
        [TestCase(">=-7", new []{-7, -6, -5}, -10)]
        [TestCase(">=0", new int[]{0, 1, 2})]
        [TestCase(">=4", new []{4, 5, 6})]
        public void RangeValue_GreaterThenEqual_Test(string test, int[] expected, int startRange = 0, int iterations = 3)
        {
            RangeValueTestRange(test, expected, startRange, iterations); 
        }
        
        [TestCase("-7..-5", new []{-7, -6, -5}, -10)]
        [TestCase("0..0", new int[]{0})]
        [TestCase("3..0", new int[]{0, 1, 2, 3})]
        [TestCase("0..3", new int[]{0, 1, 2, 3})]
        [TestCase("4..6", new []{4, 5, 6})]
        public void RangeValue_Between_Test(string test, int[] expected, int startRange = 0)
        {
            RangeValueTestRange(test, expected, startRange);
        }
        
        [TestCase("-7;-5", new []{-7, -5}, -10)]
        [TestCase("0;0", new int[]{0})]
        [TestCase("3;0", new int[]{0, 3})]
        [TestCase("0;3", new int[]{0, 3})]
        [TestCase("4;6", new []{4, 6})]
        public void RangeValue_Separator_Test(string test, int[] expected, int startRange = 0)
        {
            RangeValueTestRange(test, expected, startRange);
        }
        
        [TestCase("3..5; 8; >12", new []{3,4,5,8,13,14}, 6)]
        public void RangeValue_Complex_Test(string test, int[] expected, int iterations)
        {
            RangeValueTestRange(test, expected, 0, iterations);
        }
        
        private static void RangeValueTestRange(string test, int[] expected, int startRange, int iterations = int.MaxValue)
        {
            var range = RangeValue.Parse(test);
            range.Reset(startRange);
            var count = 0;
            while (range.TryGetNextMatch(out var match) && count < iterations)
            {
                Assert.AreEqual(true, expected.Contains(match), 
                    $"Expected [{expected.Select(x => x.ToString()).Aggregate((x,y) => $"{x}, {y}")}]\n" +
                    $"But was [{match}] at Count: {count}");
                count++;
            }

            Assert.AreEqual(expected.Length, count);
        }

    }
}