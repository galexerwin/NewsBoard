using System;

namespace NetBoard.Models
{
    public class ResponseModel
    {
        public bool success { get; set; }
        public string error { get; set; }
        public dynamic data { get; set; }
    }
}
