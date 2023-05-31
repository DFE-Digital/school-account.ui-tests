namespace DFE.SA.UITests.Settings
{
    public sealed class WebSettings
    {
        public string? BaseUrl { get; set; }
        public BrowserSettings? Chrome { get; set; }
        public String? RecordVideoPath { get; set; }
        public int ElementWaitTimeout { get; set; }
        public bool HeadLess { get; set; }
        public int SlowMo { get; set; }
        public String userName { get; set; }
        public String password { get; set; }
    }
}
