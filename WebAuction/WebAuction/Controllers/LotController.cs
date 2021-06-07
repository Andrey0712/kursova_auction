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
                Begin=DateTime.Now
                

            };
            _context.Lot.Add(rez);
            _context.SaveChanges();
            return Ok(new { message = "Додано" });
        }

        [HttpPut]
        [Route("edit")]
        public IActionResult Update([FromBody] LotViewModel lot)
        {


            var res = _context.Lot.FirstOrDefault(x => x.Id == lot.Id);

            if (res != null)
            {
                res.Id = lot.Id;
                res.Name = lot.Name;
                res.Prise = lot.Prise;
                res.Image = lot.Image;
                res.Description = lot.Description;
                res.End = DateTime.Now.AddDays(lot.End);
               
                _context.SaveChanges();
            }

            return Ok(new { result = $"Отредактированно автомобиль под ID № {lot.Id}" });
        }

        [HttpDelete]
        [Route("del")]
        public IActionResult DeleteCar([FromBody] int id)
        {
            var del_car = _context.Lot.FirstOrDefault(x => x.Id == id);

            if (del_car != null)
            {
                _context.Lot.Remove(del_car);
                _context.SaveChanges();

            }

            return Ok(new { message = "Удалено" });


        }
    }
}
