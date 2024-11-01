using System.Collections.Generic;
namespace src.app.module.Models
{
    //модель возвращаемых данных черех api
    public class OutputConfigurationModel
    {
        public string Uid { get; set; }
        public string Type { get; set; }
        public Dictionary<string, object> Properties { get; set; }
    }
}
