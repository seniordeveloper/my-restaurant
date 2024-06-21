using System.Diagnostics.CodeAnalysis;

namespace MyRestaurant.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BaseSpecification
    {
        [TestInitialize]
        public void Setup()
        {
            Given();
            When();
        }

        protected virtual void Given()
        {
        }

        protected virtual void When()
        {
        }
    }
}
