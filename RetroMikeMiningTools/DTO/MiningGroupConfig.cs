using System.ComponentModel.DataAnnotations;

namespace RetroMikeMiningTools.DTO
{
    public class MiningGroupConfig
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }
        public bool? Enabled { get; set; }

        ////[DataType(DataType.Time)]
        //public string? StartTime { get; set; }

        ////[DataType(DataType.Time)]
        //public string? EndTime { get; set; }

        ////[DataType(DataType.Currency)]
        //public double? PowerCost { get; set; }
    }
}
