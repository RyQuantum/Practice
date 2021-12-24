namespace FobRegister
{
    public class FobObj
    {
        public string fobNumber { get; set; }
        public string key { get; set; }
        public FobObj(string id, string key)
        {
            this.fobNumber = id;
            this.key = key;
        }
    }
}
