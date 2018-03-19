using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebApplication2.Models
{
    public class StrategyVM
    {

        public IEnumerable<Strategy> AllStrategy { set; get; }
        public Strategy Strategy { set; get; }
        public List<SelectListItem> StratList { get; set; }

    }
}