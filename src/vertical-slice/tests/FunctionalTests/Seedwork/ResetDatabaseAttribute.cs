using System.Reflection;
using Xunit.Sdk;

namespace FunctionalTests.Seedwork
{
    internal class ResetDatabaseAttribute
        :BeforeAfterTestAttribute
    {
        public override void Before(MethodInfo methodUnderTest)
        {
            SliceFixture.ResetDatabase();
        }
    }
}
