namespace face
{
    public partial class FormFace
    {
        public class UpdateInfo
        {
            public string Name { get; set; }
            public string Date { get; set; }
            public byte[] FileContent { get; set; }
            public UpdateInfo()
            {
            }
        }
    }
}
