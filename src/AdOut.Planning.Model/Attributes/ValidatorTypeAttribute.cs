using AdOut.Planning.Model.Enum;
using System;

namespace AdOut.Planning.Model.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ValidatorTypeAttribute : Attribute
    {
        public ValidatorType Type { get; set; }

        public ValidatorTypeAttribute(ValidatorType type)
        {
            Type = type;
        }
    }
}
