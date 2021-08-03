namespace BarcodePrinter
{
    public class Record
    {
        public int id { get; set; }
        public int rowIndex { get; set; }
        public string serial_numbers { get; set; }
        public string model { get; set; }
        public string qty { get; set; }
        public string weight { get; set; }
        public string date { get; set; }
        public string batch_no { get; set; }
        public string case_no { get; set; }
        public string qr { get; set; }
        public string is_end { get; set; }
    }
}
