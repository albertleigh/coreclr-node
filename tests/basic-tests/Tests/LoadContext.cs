namespace TestApp.Tests
{
    using System.Reflection;
    using System.Runtime.Loader;
    using FluentAssertions;
    using CoreclrNodeHost;

    public sealed class LoadContext : MochaTest
    {
        public void It_should_not_reload_assembly()
        {
            var ownAssembly = GetType().Assembly;
            var loaded = Assembly.Load(ownAssembly.FullName);

            loaded.Should().BeSameAs(ownAssembly);
        }

        // Cannot force using asm default context
        // For Default LoadContext, this override always returns null since Default Context cannot override itself.
        // https://github.com/dotnet/coreclr/blob/master/Documentation/design-docs/assemblyloadcontext.md
        public void It_should_not_be_the_default_context()
        {
            var nheAlc = AssemblyLoadContext.GetLoadContext(typeof(IBridgeToNode).Assembly);
            var myAlc = AssemblyLoadContext.GetLoadContext(typeof(LoadContext).Assembly);
            var defaultAlc = AssemblyLoadContext.Default;
        
            nheAlc.Should().NotBeSameAs(defaultAlc);
            myAlc.Should().NotBeSameAs(defaultAlc);
        }
    }
}
