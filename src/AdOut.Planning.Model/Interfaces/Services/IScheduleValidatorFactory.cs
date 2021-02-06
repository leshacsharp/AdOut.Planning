using AdOut.Planning.Model.Enum;

namespace AdOut.Planning.Model.Interfaces.Services
{
    public interface IScheduleValidatorFactory
    {
        IScheduleValidator CreateChainOfAllValidators();

        IScheduleValidator CreateChainOfValidators(ValidatorType type);
    }
}
