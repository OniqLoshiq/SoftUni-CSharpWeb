using System.ComponentModel.DataAnnotations;

namespace SIS.MVCFramework.Attributes.Property
{
    public class NumberRangeAttribute : ValidationAttribute
    {
        private readonly int minValue;
        private readonly int maxValue;

        public NumberRangeAttribute(int minValue, int maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public override bool IsValid(object value)
        {
            return this.minValue <= (int)value && this.maxValue >= (int)value;
        }
    }
}
