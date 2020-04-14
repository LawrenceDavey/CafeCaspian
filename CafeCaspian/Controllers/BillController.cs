using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CafeCaspian.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CafeCaspian.Controllers
{
    [ApiController]
    [Route("api/bill/")]
    public class BillController : ControllerBase
    {
        private readonly IBillService _billService;

        public BillController(IBillService billService)
        {
            _billService = billService;
        }

        // GET api/bill/getbilltotal
        [HttpPost("getbilltotal")]
        public IActionResult GetBillTotal([FromBody] List<string> products)
        {
            if (products.Any())
            {
                try
                {
                    var result = _billService.GetTotalBill(products);
                    return Ok(result);
                }
                catch (Exception)
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                return BadRequest("No products");
            }
        }
    }
}
