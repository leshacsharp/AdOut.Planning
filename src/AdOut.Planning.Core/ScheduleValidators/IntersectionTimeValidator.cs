using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Interfaces.Schedule;
using System;
using System.Threading.Tasks;

namespace AdOut.Planning.Core.ScheduleValidators
{
    public class IntersectionTimeValidator : IScheduleValidator
    {
        private IScheduleValidator _nextValidator;

        public Task ValidAsync(ScheduleValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return _nextValidator?.ValidAsync(context);
        }

        public void SetNextValidator(IScheduleValidator validator)
        {
            if (validator == null)
                throw new ArgumentNullException(nameof(validator));

            _nextValidator = validator;
        }
    }
}
