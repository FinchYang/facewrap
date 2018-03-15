using System;
using System.Collections.Generic;
using FaceRepository.Models;

namespace FaceRepository.dbmodel
{
    public partial class Noidloghis
    {
        public int Noidloghis1 { get; set; }
        public string Idcardno { get; set; }
        public JsonObject<PictureInfo> Capturephoto { get; set; }
        public sbyte Compared { get; set; }
        public short Result { get; set; }
        public string Businessnumber { get; set; }
        public DateTime Stamp { get; set; }
    }
}
