using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAuction.Model;

namespace WebAuction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LotController : ControllerBase
    {
        [HttpGet]
        [Route("search")]
        public IActionResult Search()
        {
            var list = new List<LotVM>()
            {

                new LotVM
                {
                Name = "Манета 1 рубль",
                Prise = 500,
                Description = "1 рубль 1882 г. в идеальном состоянии",
                Image = "1.png"
                },
                new LotVM
                {
                Name = "Картина К.Малевича",
                Prise = 50000,
                Description = "Оригинал",
                Image = "2.png"
                },
            };
            return Ok(list);
        }
    }
}
