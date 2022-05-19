using System.Collections.Generic;
using NUnit.Framework;
using TryGet;

namespace Tests
{
    public class DictionaryExtensionTests
    {
        private static readonly IDictionary<string, object> TestSet = new Dictionary<string, object>
        {
            { "string", "string" },
            { "bool", true },
            { "int", 10 }
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
        public void Test_Positive_TryGet_Int_As_Int_OrDefault()
        {
            var result = new TryGetResult(true, 10).As<int>().OrDefault();

            Assert.That(result, Is.EqualTo(10));
        }
    }
}
