using System.ComponentModel.DataAnnotations.Schema;

namespace V4HubTester
{
    [Table("Hub")]
    public class Hub
    {
        public int id { get; set; }
        public string HubMac { get; set; }
        public string Log { get; set; }
    }
}
