using System;
using System.Collections.Generic;
using System.Data;
using NUnit.Framework;

namespace UnitTesting.Stubs
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    [TestFixture]
    class StubDataReaderTests
    {
        private StubDataReader _sut;
        [SetUp]
        public void BeforeEachTest()
        {
            var guids = new[]
            {
                "96ce47ba-0eae-4d57-893d-cfdb2ac800c0", "740d7202-0920-4222-bae9-422a981b68f5",
                "a79685db-44ec-452c-8ae6-730f25a434ea", "54a65889-000e-475c-8323-f32eb43d3eb5",
                "df616c87-c5fb-4dea-a200-1caa5736013e", "500edbfc-b0a8-43b6-9c18-79075c8a5911",
                "b7d71037-63a2-4c28-bfb1-19afb95724d2", "e5468c50-e93e-4831-9d77-054d4ec10280",
                "a934d004-41f1-46d4-bc54-6be94f460a57", "33b99e79-0ed2-49b9-95ca-df764a6e83b2"
            };

            IList<object[]> records = new List<object[]>();
            for (int i = 0; i < guids.Length; i++)
            {
                var row = new object[]
                {
                    Guid.Parse(guids[i]),
                    "Name"+i,
                    // ReSharper disable once RedundantExplicitNullableCreation
                    i%2 == 0 ? new int?() : new int?(i*10),
                    i%3 == 0,
                };
                records.Add(row);
            }

            _sut = new StubDataReader(new[]
            {
                ("Id", typeof(Guid)),
                ("Name", typeof(string)),
                ("Age", typeof(int?)),
                ("IsStudent", typeof(bool))
            }, records);
        }

        [TearDown]
        public void AfterEachTest() => _sut.Dispose();

        [Test]
        public void GetSchemaTable_ShouldYieldProperSchema()
        {
            DataTable dt = _sut.GetSchemaTable();


            Assert.That(dt != null);
            Assert.That(dt.Rows.Count, Is.EqualTo(4));
            Assert.That((string)dt.Rows[0]["ColumnName"] == "Id" && (Type)dt.Rows[0]["DataType"] == typeof(Guid));
            Assert.That((string)dt.Rows[1]["ColumnName"] == "Name" && (Type)dt.Rows[1]["DataType"] == typeof(string));
            Assert.That((string)dt.Rows[2]["ColumnName"] == "Age" && (Type)dt.Rows[2]["DataType"] == typeof(int) && (bool)dt.Rows[2]["AllowDBNull"]);
            Assert.That((string)dt.Rows[3]["ColumnName"] == "IsStudent" && (Type)dt.Rows[3]["DataType"] == typeof(bool) && (bool)dt.Rows[3]["AllowDBNull"] == false);
        }

        [Test]
        public void Read_ShouldReturnProperValues()
        {
            _sut.Read();
            Tuple<Guid, string, int?, bool> materializedTuple = ReadQuadruple<Guid, string, int?, bool>(_sut);

            Assert.That(materializedTuple != null);
            Assert.That(materializedTuple.Item1.ToString() == "96ce47ba-0eae-4d57-893d-cfdb2ac800c0");
            Assert.That(materializedTuple.Item2 == "Name0");
            Assert.That(materializedTuple.Item3 == null);
            Assert.That(materializedTuple.Item4);
        }

        [Test]
        public void Read_ReadingShouldBePossibleOnlyAfterCallToRead()
        {
            Assert.Throws<InvalidOperationException>(() => _sut.GetValue(0), "Reading did not start");
            _sut.Read();
            Assert.DoesNotThrow(() => _sut.GetValue(0));
        }

        [Test]
        public void Read_ReadingShouldNotBePossibleWhenReaderIsClosed()
        {
            _sut.Read();
            Assert.DoesNotThrow(() => _sut.GetValue(0));
            _sut.Close();

            Assert.That(_sut.IsClosed);
            Assert.Throws<InvalidOperationException>(() => _sut.GetValue(0), "Reader is closed");
        }

        private static Tuple<T1, T2, T3, T4> ReadQuadruple<T1, T2, T3, T4>(IDataRecord reader) => new Tuple<T1, T2, T3, T4>((T1)reader.GetValue(0), (T2)reader.GetValue(1), (T3)reader.GetValue(2), (T4)reader.GetValue(3));
    }
}
