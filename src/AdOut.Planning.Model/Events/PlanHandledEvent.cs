﻿using AdOut.Extensions.Communication;
using AdOut.Extensions.Communication.Attributes;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Database;
using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Events
{
    //todo: check work
    [IgnoreEventDeclaration]
    //[ExchangeType(ExchangeTypeEnum.Headers)]
    public class PlanHandledEvent : IntegrationEvent
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public DateTime StartDateTime { get; set; }

        public IEnumerable<AdPlanTime> Ads { get; set; }

        public IEnumerable<SchedulePeriod> Schedules { get; set; }
    }
}
