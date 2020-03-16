using AdOut.Planning.Model.Attributes;
using AdOut.Planning.Model.Interfaces.Schedule;
using System.Collections.Generic;
using System.Linq;

namespace AdOut.Planning.Core.ScheduleValidators.Factories
{
    public class ScheduleValidatorFactory : IScheduleValidatorFactory
    {
        private IEnumerable<IScheduleValidator> _validators;
        public ScheduleValidatorFactory(IEnumerable<IScheduleValidator> validators)
        {
            _validators = validators;
        }

        public IScheduleValidator CreateValidator()
        {
            var sorteredValidators = _validators.OrderBy(v =>
            {
                var validatorOrderAttr = v.GetType().GetCustomAttributes(typeof(ValidatorOrderAttribute), false).SingleOrDefault();
                if (validatorOrderAttr != null)
                {
                    return ((ValidatorOrderAttribute)validatorOrderAttr).Order;
                }

                return int.MaxValue;
            }).ToList();

            for (int i = 0; i < sorteredValidators.Count - 1; i++)
            {
                var currentValidator = sorteredValidators[i];
                var nextValidator = sorteredValidators[i + 1];

                currentValidator.SetNextValidator(nextValidator);
            }

            return sorteredValidators.FirstOrDefault();
        }
    }
}
