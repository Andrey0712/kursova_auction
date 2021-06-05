using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAuction.Entities;
using WebAuction.Model;

namespace WebAuction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LotController : ControllerBase
    {
        private readonly MyContext _context;
        public LotController(MyContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("search")]
        public IActionResult SearchLot()
        {
            var list = _context.Lot.ToList();
            return Ok(list);
           
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddCar([FromBody] LotViewModel lot)
        {
            var rez = new Lot
            {
                Name = lot.Name,
                Prise = lot.Prise,
                Image = lot.Image,
                End = DateTime.Now.AddDays(lot.End),
                Description = lot.Description,
                

            };
            _context.Lot.Add(rez);
            _context.SaveChanges();
            return Ok(new { message = "Додано" });
        }
    }
}
