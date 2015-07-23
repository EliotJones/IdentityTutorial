 // ReSharper disable once CheckNamespace
namespace JetBrains.Annotations
{
    using System;

    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Parameter |
        AttributeTargets.Property | AttributeTargets.Delegate |
        AttributeTargets.Field)]
    public sealed class NotNullAttribute : Attribute
    {
    }
}
