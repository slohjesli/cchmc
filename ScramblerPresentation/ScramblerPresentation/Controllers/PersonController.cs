using ScramblerPresentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ScramblerPresentation.Controllers
{
    public class PersonController : ApiController
    {
        // GET: api/Person
        public InternalClass Get()
        {
            return InternalHelper.AllThePeople.First();
        }
    }
}
