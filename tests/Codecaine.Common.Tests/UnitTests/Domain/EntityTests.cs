using Codecaine.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System;

namespace Codecaine.Common.Tests.UnitTests.Domain
{
     

    [TestFixture]
    public class EntityTests
    {
        // Concrete implementation for testing purposes
        private class TestEntity : Entity
        {
            public TestEntity(Guid id) : base(id) { }
            public TestEntity() : base() { }
        }

        private class DerivedEntity : TestEntity
        {
            public DerivedEntity(Guid id) : base(id) { }
        }

        [Test]
        public void Constructor_WithValidGuid_SetsId()
        {
            var id = Guid.NewGuid();
            var entity = new TestEntity(id);

            Assert.That(entity.Id, Is.EqualTo(id));
        }

        [Test]
        public void Constructor_WithEmptyGuid_ThrowsException()
        {
            var ex = Assert.Throws<ArgumentException>(() => new TestEntity(Guid.Empty));
            Assert.That(ex.ParamName, Is.EqualTo("id"));
        }

        [Test]
        public void DefaultConstructor_SetsIdToEmpty()
        {
            var entity = new TestEntity();
            Assert.That(entity.Id, Is.EqualTo(Guid.Empty));
        }

        [Test]
        public void Equals_WithSameReference_ReturnsTrue()
        {
            var entity = new TestEntity(Guid.NewGuid());
            Assert.That(entity.Equals(entity), Is.True);
        }

        [Test]
        public void Equals_WithSameId_ReturnsTrue()
        {
            var id = Guid.NewGuid();
            var entity1 = new TestEntity(id);
            var entity2 = new TestEntity(id);

            Assert.That(entity1.Equals(entity2), Is.True);
        }

        [Test]
        public void Equals_WithDifferentId_ReturnsFalse()
        {
            var entity1 = new TestEntity(Guid.NewGuid());
            var entity2 = new TestEntity(Guid.NewGuid());

            Assert.That(entity1.Equals(entity2), Is.False);
        }

        [Test]
        public void Equals_WithNullEntity_ReturnsFalse()
        {
            var entity = new TestEntity(Guid.NewGuid());
            Assert.That(entity.Equals((Entity)null), Is.False);
        }

        [Test]
        public void Equals_WithNullObject_ReturnsFalse()
        {
            var entity = new TestEntity(Guid.NewGuid());
            Assert.That(entity.Equals((object)null), Is.False);
        }

        [Test]
        public void Equals_WithDifferentType_ReturnsFalse()
        {
            var entity = new TestEntity(Guid.NewGuid());
            Assert.That(entity.Equals(new object()), Is.False);
        }

        [Test]
        public void Equals_WithDerivedTypeSameId_ReturnsTrue()
        {
            var id = Guid.NewGuid();
            Entity entity1 = new TestEntity(id);
            Entity entity2 = new DerivedEntity(id);

            Assert.That(entity1.Equals(entity2), Is.True);
        }        

        [Test]
        public void GetHashCode_ReturnsConsistentValue()
        {
            var id = Guid.NewGuid();
            var entity = new TestEntity(id);

            var expectedHash = id.GetHashCode() * 41;
            Assert.That(entity.GetHashCode(), Is.EqualTo(expectedHash));
        }

        [Test]
        public void GetHashCode_WithEmptyId_ReturnsConsistentValue()
        {
            var entity = new TestEntity();
            var expectedHash = Guid.Empty.GetHashCode() * 41;
            Assert.That(entity.GetHashCode(), Is.EqualTo(expectedHash));
        }

        [Test]
        public void EqualityOperator_BothNull_ReturnsTrue()
        {
            TestEntity a = null;
            TestEntity b = null;

            Assert.That(a == b, Is.True);
        }

        [Test]
        public void EqualityOperator_FirstNull_ReturnsFalse()
        {
            TestEntity a = null;
            var b = new TestEntity(Guid.NewGuid());

            Assert.That(a == b, Is.False);
        }

        [Test]
        public void EqualityOperator_SecondNull_ReturnsFalse()
        {
            var a = new TestEntity(Guid.NewGuid());
            TestEntity b = null;

            Assert.That(a == b, Is.False);
        }

        [Test]
        public void EqualityOperator_SameId_ReturnsTrue()
        {
            var id = Guid.NewGuid();
            var entity1 = new TestEntity(id);
            var entity2 = new TestEntity(id);

            Assert.That(entity1 == entity2, Is.True);
        }

        [Test]
        public void EqualityOperator_DifferentIds_ReturnsFalse()
        {
            var entity1 = new TestEntity(Guid.NewGuid());
            var entity2 = new TestEntity(Guid.NewGuid());

            Assert.That(entity1 == entity2, Is.False);
        }

        [Test]
        public void InequalityOperator_BothNull_ReturnsFalse()
        {
            TestEntity a = null;
            TestEntity b = null;

            Assert.That(a != b, Is.False);
        }

        [Test]
        public void InequalityOperator_FirstNull_ReturnsTrue()
        {
            TestEntity a = null;
            var b = new TestEntity(Guid.NewGuid());

            Assert.That(a != b, Is.True);
        }

        [Test]
        public void InequalityOperator_SecondNull_ReturnsTrue()
        {
            var a = new TestEntity(Guid.NewGuid());
            TestEntity b = null;

            Assert.That(a != b, Is.True);
        }

        [Test]
        public void InequalityOperator_SameId_ReturnsFalse()
        {
            var id = Guid.NewGuid();
            var entity1 = new TestEntity(id);
            var entity2 = new TestEntity(id);

            Assert.That(entity1 != entity2, Is.False);
        }

        [Test]
        public void InequalityOperator_DifferentIds_ReturnsTrue()
        {
            var entity1 = new TestEntity(Guid.NewGuid());
            var entity2 = new TestEntity(Guid.NewGuid());

            Assert.That(entity1 != entity2, Is.True);
        }

        [Test]
        public void Equals_WithSameTypeDifferentId_ReturnsFalse()
        {
            var entity1 = new TestEntity(Guid.NewGuid());
            var entity2 = new TestEntity(Guid.NewGuid());

            Assert.That(entity1.Equals(entity2), Is.False);
        }

        [Test]
        public void Equals_WhenOtherIsNotEntity_ReturnsFalse()
        {
            var entity = new TestEntity(Guid.NewGuid());
            var notEntity = new object();

            Assert.That(entity.Equals(notEntity), Is.False);
        }
    }
}
