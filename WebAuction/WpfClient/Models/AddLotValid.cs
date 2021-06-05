using System;
using System.Collections.Generic;
using System.Text;

namespace WpfClient.Models
{
    public abstract class BaseValidation
    {
        public int Status { get; set; }
    }
    public class ErrorsAddLot
    {
        public List<string> Name { get; set; }
        public List<int> Prise { get; set; }
        public List<int> End { get; set; }
        public List<string> Description { get; set; }

    }

    public class AddLotValid : BaseValidation
    {
        public ErrorsAddLot Errors { get; set; }
    }
    public class GetAllErrors
    {
        public string GetAllError { get; set; }
    }
}
