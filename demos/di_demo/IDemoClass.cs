namespace DotNetCoreBootstrap.DIDemo
{
    public interface IDemoClass
    {
        string Name { get; }

        string TypeName { get; }

        string Value { get; }

        void DemoMethod();
    }
}