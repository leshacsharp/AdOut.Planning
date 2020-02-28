using System.Collections.Generic;

namespace AdOut.Planning.Model.Classes
{
    public class ContentValidationResult
    {
        public bool IsValid
        {
            get
            {
                return Errors?.Count == 0;
            }
        }

        public List<ContentError> Errors { get; set; }
        
        public ContentValidationResult()
        {
            Errors = new List<ContentError>();
        }
    }
}
