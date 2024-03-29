﻿using AdOut.Planning.Model.Enum;

namespace AdOut.Planning.Model.Dto
{
    public class AdListDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public ContentType ContentType { get; set; }
        public AdStatus Status { get; set; }
        public string PreviewPath { get; set; }
    }
}
