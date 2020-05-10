using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "Custom Style Settings",
    Author = "LefeWare Solutions",
    Website = "",
    Version = "1.0.0",
    Description = "Custom Tenant Site Style Settings",
    Category = "Settings",
    Dependencies = new[]
    {
        "OrchardCore.CustomSettings"
    }
)]