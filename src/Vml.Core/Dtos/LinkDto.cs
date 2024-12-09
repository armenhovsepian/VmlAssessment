namespace Vml.Core.Dtos
{
    public class LinkDto
    {
        public int Id { get; set; }
        public string Rel { get; set; }
        public string Href { get; set; }
        public string Action { get; set; }
        //private string _types { get; set; }

        //public string[] Types
        //{
        //    get
        //    {
        //        return _types.Split(";");
        //    }
        //    set
        //    {
        //        _types = string.Join(";", value);
        //    }
        //}
    }
}
