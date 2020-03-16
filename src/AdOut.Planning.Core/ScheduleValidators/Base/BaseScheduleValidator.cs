using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Interfaces.Schedule;
using System;

namespace AdOut.Planning.Core.ScheduleValidators.Base
{
    public abstract class BaseScheduleValidator : IScheduleValidator
    {
        protected IScheduleValidator _nextValidator;

        public void SetNextValidator(IScheduleValidator validator)
        {
            if (validator == null)
                throw new ArgumentNullException(nameof(validator));

            _nextValidator = validator;
        }

        public abstract void Validate(ScheduleValidationContext context);
    }
}
