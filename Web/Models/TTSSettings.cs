using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class TTSSettings
    {
        public TTSSettings()
        {
            ByWord = true;
            ByChar = false;
            CursorMode = true;
            State = true;
            //MathMLMode = true;
            PdfMode = true;
        }
        public bool ByChar { get; set; }
        public bool ByWord { get; set; }
        public bool CursorMode { get; set; }
        public bool State { get; set; }
        //public bool MathMLMode { get; set; }
        public bool PdfMode { get; set; }
    }
}
