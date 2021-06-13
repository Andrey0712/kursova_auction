using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UiHelper;
using UiHelper.Constants;
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
        [Authorize(Roles = Roles.Admin)]
        public IActionResult AddCar([FromBody] LotViewModel lot)
        {
            var dir = Directory.GetCurrentDirectory();
            var dirSave = Path.Combine(dir, "foto");
            var imageName = Path.GetRandomFileName() + ".jpg";
            var imageSaveFolder = Path.Combine(dirSave, imageName);
            var image = lot.Image.LoadBase64();
            image.Save(imageSaveFolder);

            var shema = Request.Scheme;
            var port = Request.Host.Port;
            var domain = Request.Host.Host;
            if (port != null)
                domain += ":" + port.ToString();
            string url = $"{shema}://{domain}/img/{imageName}";
            var rez = new Lot
            {
                Name = lot.Name,
                Prise = lot.Prise,
                //Image = imageSaveFolder,
                End = DateTime.Now.AddDays(lot.End),
                Description = lot.Description,
                Begin = DateTime.Now,
                Image = url

            };
            _context.Lot.Add(rez);
            _context.SaveChanges();
            return Ok(new { message = "Додано" });
        }

        [HttpPut]
        [Route("edit")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Update([FromBody] LotViewModel lot)
        {


            var res = _context.Lot.FirstOrDefault(x => x.Id == lot.Id);
            var dir = Directory.GetCurrentDirectory();
            var dirSave = Path.Combine(dir, "foto");
            var imageName = Path.GetRandomFileName() + ".jpg";
            var imageSaveFolder = Path.Combine(dirSave, imageName);
            var image = lot.Image.LoadBase64();
            image.Save(imageSaveFolder);
            if (res != null)
            {
                res.Id = lot.Id;
                res.Name = lot.Name;
                res.Prise = lot.Prise;
                res.Image = imageSaveFolder;
                res.Description = lot.Description;
                res.End = DateTime.Now.AddDays(lot.End);
               
                _context.SaveChanges();
            }

            return Ok(new { result = $"Отредактированно лот под ID № {lot.Id}" });
        }

        [HttpDelete]
        [Route("del")]
        [Authorize(Roles = Roles.Admin)]
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
        [HttpPut]
        [Route("rate")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult UpRate([FromBody] LotViewModelRate lot)
        {


            var res = _context.Lot.FirstOrDefault(x => x.Id == lot.Id);

            if (res != null)
            {
                res.Id = lot.Id;
                res.Description = lot.Description;
                res.Prise = lot.Prise;
                
                _context.SaveChanges();
            }

            return Ok(new { result = $"Отредактированно лот под ID № {lot.Id}" });
        }
        
    }
}
