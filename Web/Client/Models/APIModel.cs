using System;

namespace NetBoard.Models
{
    public class APIModel
    {
        public bool           responseOkay { get; set; }
        public string         responseData { get; set; }
        public ResponseModel  responseJSON { get; set; }  
    }
}
