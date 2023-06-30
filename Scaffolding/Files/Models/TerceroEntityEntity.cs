using SARA.Server.Shared.Attribute;

namespace SARA.Server.Models.Example
{
    [Table("scheme.tableName")]
    public class TerceroEntityEntity : BaseEntity
    {
        [Key]
        public string cd_example1 { get; set; }
        public string ds_example2 { get; set; }
    }
}