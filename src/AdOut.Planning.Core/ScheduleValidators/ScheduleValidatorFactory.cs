using AdOut.Planning.Model.Interfaces.Schedule;

namespace AdOut.Planning.Core.ScheduleValidators
{
    public class ScheduleValidatorFactory : IScheduleValidatorFactory
    {
        public IScheduleValidator CreateValidator()
        {
            var adPointTimeValidator = new AdPointTimeValidator();
            var intersectionTimeValidator = new IntersectionTimeValidator();

            adPointTimeValidator.SetNextValidator(intersectionTimeValidator);

            return adPointTimeValidator;
        }
    }
}
