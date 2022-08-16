using Microsoft.AspNetCore.Mvc;
using iMessengerCoreAPI.Models;

namespace iMessengerCoreAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RGDialogsClientsController : ControllerBase
    {
        private readonly RGDialogsClients _rGDialogsClients;
        private readonly ILogger<RGDialogsClientsController> _logger;
        public RGDialogsClientsController(ILogger<RGDialogsClientsController> logger)
        {
            _rGDialogsClients = new RGDialogsClients();
            _logger = logger;
        }

        [Route("GetDialogByClients")]
        [HttpPost]
        public IActionResult GetDialogByClients(List<Guid> idClients)
        {

            List<RGDialogsClients> data = _rGDialogsClients.Init();

            var trueId = from d1 in data
                         join i in (
                         from d2 in data
                         where !idClients.Contains(d2.IDClient)
                         group d2 by d2.IDRGDialog into g
                         select new { IDRGDialog = g.Key }
                         ) on d1.IDRGDialog equals i.IDRGDialog into ps
                         from i in ps.DefaultIfEmpty()
                         where i == null
                         group d1 by d1.IDRGDialog into g
                         where g.Count() == idClients.Count()
                         select new { IDRGDialog = g.Key };

            if (trueId.Count() == 0)
            {
                return Ok(Guid.Empty);
            }

            return Ok(trueId.FirstOrDefault());

        }

    }
}
