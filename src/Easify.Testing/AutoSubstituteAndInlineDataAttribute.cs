﻿using AutoFixture.Xunit2;

 namespace Easify.Testing
{
    public class AutoSubstituteAndInlineDataAttribute : InlineAutoDataAttribute
    {
        public AutoSubstituteAndInlineDataAttribute(params object[] values)
            : base(new AutoSubstituteAndDataAttribute(), values)
        {
        }
    }
}
