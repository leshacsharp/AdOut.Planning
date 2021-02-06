using AdOut.Planning.Model.Attributes;
using AdOut.Planning.Model.Enum;
using AdOut.Planning.Model.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;

namespace AdOut.Planning.Core.Validators.Schedule
{
    public class ScheduleValidatorFactory : IScheduleValidatorFactory
    {
        private readonly IEnumerable<IScheduleValidator> _validators;
        public ScheduleValidatorFactory(IEnumerable<IScheduleValidator> validators)
        {
            _validators = validators;
        }

        public IScheduleValidator CreateChainOfAllValidators()
        {
            var sorteredValidators = _validators.OrderBy(v =>
            {
                var validatorTypeAttr = v.GetType().GetCustomAttributes(typeof(ValidatorTypeAttribute), false).SingleOrDefault();
                if (validatorTypeAttr != null)
                {
                    var validatorType = ((ValidatorTypeAttribute)validatorTypeAttr).Type;
                    var validatorPriority = (int)validatorType;
                    return validatorPriority;
                }

                return int.MaxValue;
            }).ToList();

            var chainOfValidators = CreateChainOfValidators(sorteredValidators);
            return chainOfValidators;
        }

        public IScheduleValidator CreateChainOfValidators(ValidatorType type)
        {
            var specificValidators = _validators.Where(v =>
            {
                var validatorTypeAttr = v.GetType().GetCustomAttributes(typeof(ValidatorTypeAttribute), false).SingleOrDefault();
                if (validatorTypeAttr != null)
                {
                    var validatorType = ((ValidatorTypeAttribute)validatorTypeAttr).Type;
                    return validatorType == type;
                }

                return false;
            }).ToList();

            var chainOfValidators = CreateChainOfValidators(specificValidators);
            return chainOfValidators;
        }

        private IScheduleValidator CreateChainOfValidators(List<IScheduleValidator> validators)
        {
            for (int i = 0; i < validators.Count - 1; i++)
            {
                var currentValidator = validators[i];
                var nextValidator = validators[i + 1];

                currentValidator.SetNextValidator(nextValidator);
            }

            return validators.FirstOrDefault();
        }
    }
}
