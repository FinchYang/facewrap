using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsFormsApp1
{
    public class IdInfo
    {
        private string Name = "";
        private string Sex = "";
        private string Folk = "";
        private string FolkId = "";
        private string BirthDay = "";
        private string Address = "";
        private string Id = "";
        private string IssueOrgan = "";
        private string AvailityBegin = "";
        private string AvailityEnd = "";
        private string Newaddress = "";
        private string IdBase64Photo = "";
        private string bmpPath = "";
        public byte[] abName = new byte[30];
        public byte[] abSex = new byte[2];
        public byte[] abFolk = new byte[4];
        public byte[] abBirth = new byte[16];
        public byte[] abAddress = new byte[70];
        public byte[] abId = new byte[36];
        public byte[] abIssueOrgan = new byte[30];
        public byte[] abAvailityBegin = new byte[16];
        public byte[] abAvailityEnd = new byte[16];
        public byte[] abOther = new byte[36];
        public byte[] abPhoto = new byte[1024];
        public byte[] abNewaddress = new byte[70];
        
        public void setName(string name)
        {
            Name = name;
        }
        public string getName()
        {
            return Name;
        }
        public void setSex(string sex)
        {
            Sex = sex;
        }
        public string getSex()
        {
            return Sex;
        }
        public void setFolk(string folk)
        {
            Folk = folk;
        }
        public string getFolk()
        {
            return Folk;
        }
        public string getFolkId()
        {
            return FolkId;
        }
        public void setFolkId(string folkid)
        {
            FolkId = folkid;
        }

        public void setBirthDay(string birthDay)
        {
            BirthDay = birthDay;
        }
        public string getBirthDay()
        {
            return BirthDay;
        }
        public void setAddress(string address)
        {
            Address = address;
        }
        public string getAddress()
        {
            return Address;
        }

        public void setId(string id)
        {
            Id = id;
        }
        public string getId()
        {
            return Id;
        }
        public void setIssueOrgan(string issueOrgan)
        {
            IssueOrgan = issueOrgan;
        }
        public string getIssueOrgan()
        {
            return IssueOrgan;
        }
        public void setAvailityBegin(string availityBegin)
        {
            AvailityBegin = availityBegin;
        }
        public string getAvailityBegin()
        {
            return AvailityBegin;
        }

        public void setAvailityEnd(string availityEnd)
        {
            AvailityEnd = availityEnd;
        }
        public string getAvailityEnd()
        {
            return AvailityEnd;
        }
        public void setNewaddress(string newaddress)
        {
            Newaddress = newaddress;
        }
        public string getNewaddress()
        {
            return Newaddress;
        }

        public void setbmpPath(string path)
        {
            bmpPath = path;
        }
        public string getbmpPath()
        {
            return bmpPath;
        }
        public void setIdBase64Photo(string idBase64Photo)
        {
            IdBase64Photo = idBase64Photo;
        }
        public string getIdBase64Photo()
        {
            return IdBase64Photo;
        }

       

    }
}
