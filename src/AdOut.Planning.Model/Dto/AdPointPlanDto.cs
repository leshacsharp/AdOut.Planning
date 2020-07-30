using System.Collections.Generic;

namespace AdOut.Planning.Model.Dto
{
    public class AdPointPlanDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public IEnumerable<ScheduleDto> Schedules { get; set; }
    }
}
