using System.Collections.Generic;

namespace src.app.module.Models
{
    //модель того, как данные могут быть сохранены в базе
    public class DbConfigurationModel
    {
        public string Uid { get; set; }
        public string Type { get; set; }
        public List<Property> Properties { get; set; }
    }

    public class Property
    {
        public string Id { get; set; }
        public string ValueString { get; set; }
    }
}
