using System;
using System.Collections.Generic;

namespace watchintranet.dbmodel
{
    public partial class Noidlog
    {
        public int Idnoidlog { get; set; }
        public string Idcardno { get; set; }
        public JsonObject<PictureInfo> Capturephoto { get; set; }
        public sbyte? Compared { get; set; }
        public short? Result { get; set; }
        public string Businessnumber { get; set; }
        public sbyte? Notified { get; set; }
    }
}
