using Xunit;

namespace ReactionRace.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            int a = 2;
            int b = 2;
            Assert.Equal(4, a + b);
        }

        [Fact]
        public void Test2()
        {
            int a = 3;
            int b = 2;
            Assert.Equal(5, a + b);
        }
    }
}