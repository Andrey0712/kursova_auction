using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAuction.Model
{
    public class LotViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Неуказано наименование лота")]
        public string Name { get; set; }
        /// <summary>
        /// название лота
        /// </summary>
        [Required(ErrorMessage = "Неуказанна начальная стоимость лота")]
        public int Prise { get; set; }
        /// <summary>
        /// стоимость
        /// </summary>
        [Required( ErrorMessage = "Неуказанна срок акционна")]
        public int End { get; set; }
        /// <summary>
        /// количество дней
        /// </summary>
        [Required(ErrorMessage = "Добавьте описание лота")]
        public string Description { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        [StringLength(255)]
        public string Image { get; set; }
        public DateTime Begin { get; set; }
    }
}
