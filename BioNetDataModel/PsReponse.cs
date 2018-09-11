using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BioNetModel
{
    public class PsReponse
    {
        public bool Result { get; set; }
        public string StringError { get; set; }
    }
    public class PsReponseSMS
    {
        public bool Result { get; set; }
        public string MobileNumber { get; set; }
        public string StringError { get; set; }
    }
}
