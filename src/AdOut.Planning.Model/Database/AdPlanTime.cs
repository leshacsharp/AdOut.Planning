using AdOut.Planning.Model.Enum;

namespace AdOut.Planning.Model.Database
{
    public class AdPlanTime
    {
        public string Title { get; set; }
        public string Path { get; set; }
        public ContentType ContentType { get; set; }
        public int? Order { get; set; }
    }
}
