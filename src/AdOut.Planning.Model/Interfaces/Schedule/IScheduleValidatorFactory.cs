using AdOut.Planning.Model.Enum;

namespace AdOut.Planning.Model.Interfaces.Schedule
{
    public interface IScheduleValidatorFactory
    {
        IScheduleValidator CreateChainOfValidators();

        IScheduleValidator CreateChainOfValidators(ValidatorType type);
    }
}
