﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using NUnit.Framework;
using TryGet.Extensions;

namespace TryGet.Tests
{
    public class Tests
    {
        private static readonly IDictionary<string, object> TestSet = new Dictionary<string, object>
        {
            { "string", "string" },
            { "bool", true },
            { "false", false },
            { "int", 10 }
        };

        private static readonly IDictionary<string, Animal> Animals = new Dictionary<string, Animal>
        {
            { "cat", new Cat() },
            { "dog", new Dog() }
        };

        [Test]
        public void Test_Positive_TryGet_String()
        {
            var result = TestSet.TryGet("string");

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.EqualTo("string"));
        }

        [Test]
        public void Test_Negative_TryGet_String()
        {
            var result = TestSet.TryGet("stringg");

            Assert.That(result.IsSuccess, Is.False);
            Assert.Throws<NoValueException>(() => { var val = result.Value; });
        }

        [Test]
        public void Test_Positive_TryGet_String_Cast()
        {
            var result = TestSet.TryGet("string");

            var val = (string)result;
            Assert.That(val, Is.EqualTo("string"));
        }

        [Test]
        public void Test_Positive_TryGet_Bool_Exists_Cast_True()
        {
            var result = TestSet.TryGet("bool");

            if (result)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        [Test]
        public void Test_Negative_TryGet_Bool_NotExist_Cast_False()
        {
            var result = TestSet.TryGet("boolz");

            if (!result)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        [Test]
        public void Test_Positive_TryGet_Bool_ExistsCorrectType_Cast()
        {
            var result = TestSet.TryGet("string").As<string>();

            if (result)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        [Test]
        public void Test_Negative_TryGet_Bool_ExistsWrongType_Cast()
        {
            var result = TestSet.TryGet("string").As<int>();

            if (!result)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        [Test]
        public void Test_Positive_TryGet_String_As_String()
        {
            var result = new TryGetResult(true, "string").As<string>();

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.EqualTo("string"));
        }

        [Test]
        public void Test_Negative_TryGet_String_As_String()
        {
            var result = new TryGetResult(false, default).As<string>();

            Assert.That(result.IsSuccess, Is.False);
            Assert.Throws<NoValueException>(() => { var val = result.Value; });
        }

        [Test]
        public void Test_Negative_TryGet_String_As_Bool()
        {
            var result = new TryGetResult(true, "string").As<bool>();

            Assert.That(result.IsSuccess, Is.False);
            Assert.Throws<NoValueException>(() => { var val = result.Value; });
        }

        [Test]
        public void Test_Positive_TryGet_String_As_Int_OrDefault()
        {
            var result = new TryGetResult(true, "string").As<int>().OrDefault();

            Assert.That(result, Is.Zero);
        }

        [Test]
        public void Test_Positive_TryGet_String_As_Int_OrDefault_CustomValue()
        {
            var result = new TryGetResult(true, "string").As<int>().OrDefault(5);

            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        public void Test_Positive_TryGet_Int_As_Int_OrDefault()
        {
            var result = new TryGetResult(true, 10).As<int>().OrDefault();

            Assert.That(result, Is.EqualTo(10));
        }

        [Test]
        public void Test_Positive_TryGet_Polymorphism()
        {
            var result = Animals.TryGet("cat");

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.AssignableTo<Animal>());
        }

        [Test]
        public void Test_Positive_TryGet_As_Polymorphism()
        {
            var result = Animals.TryGet("cat").As<Cat>();

            Assert.That(result.IsSuccess, Is.True);
        }

        [Test]
        public void Test_Negative_TryGet_As_Polymorphism()
        {
            var result = Animals.TryGet("cat").As<Dog>();

            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public void Test_Positive_TryGet_As_Interface()
        {
            var result = Animals.TryGet("cat").As<IHasNineLives>();

            Assert.That(result.IsSuccess, Is.True);
        }

        [Test]
        public void Test_Negative_TryGet_As_Interface()
        {
            var result = Animals.TryGet("dog").As<IHasNineLives>();

            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public void Test_Positive_TryGet_As_Chain()
        {
            var result = Animals.TryGet("cat").As<object>().As<Animal>().As<Cat>();

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.AssignableTo<Cat>());
        }

        [Test]
        public void Test_ReadOnlyDictionary()
        {
            if (((IReadOnlyDictionary<string, object>)new ReadOnlyDictionary<string, object>(TestSet)).TryGet("string"))
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        [Test]
        public void Test_Positive_TryGet_As_Out()
        {
            var result = TestSet.TryGet("string").As<string>(out var stringValue);

            Assert.That(result, Is.True);
            Assert.That(stringValue, Is.EqualTo("string"));
        }

        [Test]
        public void Test_Negative_TryGet_As_Out()
        {
            var result = TestSet.TryGet("bool").As<string>(out var stringValue);

            Assert.That(result, Is.False);
            Assert.That(stringValue, Is.Null);
        }

        [Test]
        public void Test_Positive_TryGet_AsUntyped_Out()
        {
            var result = TestSet.TryGet("string").As(out var objectValue);

            Assert.That(result, Is.True);
            Assert.That(objectValue, Is.EqualTo("string"));
        }

        [Test]
        public void Test_Negative_TryGet_AsUntyped_Out()
        {
            var result = TestSet.TryGet("stringz").As(out var objectValue);

            Assert.That(result, Is.False);
            Assert.That(objectValue, Is.Null);
        }

        [Test]
        public void Test_Positive_TryGet_As_Out_ValueType()
        {
            var dictionary = new Dictionary<string, int>
            {
                { "one", 1 },
                { "two", 2 }
            };

            var result = dictionary.TryGet("one").As(out var intValue);

            Assert.That(result, Is.True);
            Assert.That(intValue, Is.EqualTo(1));
        }

        [Test]
        public void Test_Negative_TryGet_As_Out_ValueType()
        {
            var dictionary = new Dictionary<string, int>
            {
                { "one", 1 },
                { "two", 2 }
            };

            var result = dictionary.TryGet("four").As(out var intValue);

            Assert.That(result, Is.False);
            Assert.That(intValue, Is.Zero);
        }
    }

    class Animal { }
    class Cat : Animal, IHasNineLives { }
    class Dog : Animal { }
    interface IHasNineLives { }
}
