﻿namespace Virgil.SDK.Helpers
{
    using System;

    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class ValidatedNotNullAttribute : Attribute
    {
    }
}