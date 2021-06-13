using System;
using System.Collections.Generic;
using System.Text;

namespace UiHelper.Constants
{
    public static class UriConstant
    {
        static string uri = "http://localhost:1782/api/";
        //static string uri = "https://kondr.tk/api/";

       
        public static string Get = $"{uri}Lot/search";
        public static string Add = $"{uri}Lot/add";
        public static string Edit = $"{uri}Lot/edit";
        public static string Del = $"{uri}Lot/del";
        public static string Rate = $"{uri}Lot/rate";
        public static string Account = $"{uri}Account";
    }
}
