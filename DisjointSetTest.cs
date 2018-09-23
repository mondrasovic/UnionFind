using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnionFind
{
    [TestFixture]
    public class DisjointSetTest
    {
		DisjointSet<int> disjointSet;

		[Test]
        public void TestCountCorrespondsInsertedValuesNum()
		{
			GivenValues(10, 20, 30, 40, 50, 60);
			ThenCountEquals(6);
		}

		[Test]
        public void TestIsEmptyAfterClear()
		{
			GivenValues(-5, 100, 0, 89, 50);
			WhenClearIsCalled();
			ThenIsEmpty();
		}

		[Test]
        public void TestIsEmptyAfterCreation()
		{
			ThenIsEmpty();
		}

		[Test]
        public void TestDuplicateItemsAreCountedJustOnce()
		{
			GivenValues(2, 2, 4, 4, 8, 8, 16, 16, 32, 32, 32, 32);
			ThenCountEquals(5);
		}

        [Test]
        public void TestUnionPutsTwoElementsToTheSameSet()
		{
			GivenValues(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
			WhenUnionIsCalled(1, 2);
			ThenValuesAreInTheSameSet(1, 2);
		}

		[Test]
        public void TestUnionPutsMultipleElementsToTheSameSet()
        {
            GivenValues(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            WhenUnionIsCalled(1, 2, 6, 8);
            ThenValuesAreInTheSameSet(1, 2, 6, 8);
        }

        [Test]
        public void TestAllElementsAreInUniqueSetWithoutUnion()
		{
			GivenValues(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
			ThenValuesSetsAreDisjoint(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
		}

		[Test]
        public void TestUnionWithItselfDoesNothing()
		{
			GivenValues(100, 200, 300);
			WhenUnionIsCalled(100, 100);
			ThenValuesSetsAreDisjoint(100, 200, 300);
		}

        [Test]
        public void TestRandomUnionOfAllValuesCreatesOneSet()
		{
			GivenValues(1, 2, 3, 4, 5, 6);
			WhenUnionIsCalled(1, 2);
			WhenUnionIsCalled(3, 4);
			WhenUnionIsCalled(4, 5);
			WhenUnionIsCalled(5, 6);
			WhenUnionIsCalled(1, 5);
			ThenValuesAreInTheSameSet(1, 2, 3, 4, 5, 6);
		}

        [Test]
        public void TesUnitingElementsIntoTwoHalvesResultsInTwoSets()
		{
			GivenValues(1, 2, 3, 4, 5, 6);
			WhenUnionIsCalled(1, 2, 3);
			WhenUnionIsCalled(4, 5, 6);
			ThenNumberOfUniqueSetsEquals(2, 1, 2, 3, 4, 5, 6);
		}

        [Test]
        public void TestIteratorProducesTheSameValues()
		{
			GivenValues(-1, -2, -3, -4, -5);
			var iteratedSet = WhenStructureIsIterated();
			var targetSet = new HashSet<int>(new int[]{-1, -2, -3, -4, -5});
			ThenTwoSetsAreEqual(iteratedSet, targetSet);

		}

		[SetUp]
		protected void SetUp()
		{
			var testName = TestContext.CurrentContext.Test.Name;
			Console.WriteLine($"Running test '{testName}'.");

			disjointSet = new DisjointSet<int>();         
		}

		[TearDown]
		protected void TearDown()
		{
			disjointSet = null;
		}

		void GivenValues(params int[] values)
		{
			foreach (var value in values)
				disjointSet.MakeSet(value);
		}

        void WhenClearIsCalled()
		{
			disjointSet.Clear();
		}

        void WhenUnionIsCalled(params int[] values)
		{
			if (values.Length < 2)
				throw new ArgumentException("number of values is less than 2");

			var anchorValue = values[0];
			for (int i = 1; i < values.Length; ++i) {
				var currValue = values[i];
				disjointSet.Union(anchorValue, currValue);
			}
		}

        HashSet<int> WhenStructureIsIterated()
		{
			var values = new HashSet<int>();
			foreach (var value in disjointSet)
				values.Add(value);
			return values;
		}

        void ThenCountEquals(int value)
		{
			Assert.AreEqual(disjointSet.Count, value);
		}

        void ThenIsEmpty()
		{
			Assert.True(disjointSet.IsEmpty());
		}

        void ThenValuesAreInTheSameSet(params int[] values)
		{
			if (values.Any())
			{
				var uniqueSets = ExtractUniqueSets(values);
				Assert.AreEqual(uniqueSets.Count, 1);
			}
		}

		void ThenValuesSetsAreDisjoint(params int[] values)
        {
			var uniqueSets = ExtractUniqueSets(values);
			Assert.AreEqual(uniqueSets.Count, values.Length);
        }

        void ThenNumberOfUniqueSetsEquals(int count, params int[] values)
		{
			var uniqueSets = ExtractUniqueSets(values);
			Assert.AreEqual(uniqueSets.Count, count);
		}

		void ThenTwoSetsAreEqual(HashSet<int> firstSet, HashSet<int> secondSet)
		{
			Assert.True(firstSet.SetEquals(secondSet));
		}

		HashSet<int> ExtractUniqueSets(int[] values)
		{
			var sets = values.Select((arg) => disjointSet.FindSet(arg)).
                                 ToArray();
            return new HashSet<int>(sets);
		}
    }
}
