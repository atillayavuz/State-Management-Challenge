using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StateManagement.WebApi.Requests
{
    public class StartProcessRequest
    {
        public Guid FlowId { get; set; }

        public Guid TaskId { get; set; }

    }
}
