namespace DotNetCoreBootstrap.DIDemo
{
    using System;

    using Microsoft.Extensions.Logging;

    public sealed class DemoClass : IDemoClass
    {
        private readonly ILogger<DemoClass> logger;

        public DemoClass(ILogger<DemoClass> logger, string name, string value)
        {
            this.Name = name;
            this.Value = value;
            this.logger = logger;

            this.logger.LogDebug(
                $"Resolved object {this}");
        }

        public string Name { get; private set; }

        public string TypeName => this.GetType().Name;

        public string Value { get; private set; }

        public void DemoMethod()
        {
            this.logger.LogDebug($"Run demo method from {this}");
            Console.WriteLine($"Run demo method from {this}");
        }

        public override string ToString()
            => $"[TypeName = '{this.TypeName}', Name = '{this.Name}', Value = '{this.Value}']";
    }
}