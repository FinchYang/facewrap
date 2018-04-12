namespace face
{
    public partial class FormFace
    {
        public class result
        {
            public bool ok { get; set; }
            public float score { get; set; }
            public CompareStatus status { get; set; }
            public mgverror errcode { get; set; }
        }
    }
}
