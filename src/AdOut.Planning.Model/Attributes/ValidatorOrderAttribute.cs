using System;

namespace AdOut.Planning.Model.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ValidatorOrderAttribute : Attribute
    {
        public int Order { get; set; }

        public ValidatorOrderAttribute(int order)
        {
            Order = order;
        }
    }
}
