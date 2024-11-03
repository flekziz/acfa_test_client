namespace repository.module.Models.Output
{
    public class Configuration
    {
        public string Uid { get; set; }
        public string DisplayName { get; set; }
        public string InternalType { get; set; }
        public Configuration[] Configurations { get; set; }
        public Property[] Properties { get; set; }
    }
}
