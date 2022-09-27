using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.Utilities;
using System.ComponentModel.DataAnnotations;

namespace RetroMikeMiningTools.DTO
{
    public class ZergAlgoConfig
    {
        public ZergAlgoConfig()
        {
            this.Groups = new List<LinkedGroup>();
        }

        [Key]
        public int Id { get; set; }

        public int WorkerId { get; set; }

        [UIHint("ZergAlgoEditor")]
        public string Algo { get; set; }

        [UIHint("MiningFlighsheetEditor")]
        public long? Flightsheet { get; set; }

        public string FlightsheetName { get; set; }

        public bool Enabled { get; set; }

        [UIHint("GroupListEditor")]
        public List<LinkedGroup>? Groups { get; set; }

        public decimal HashRateMH { get; set; }
        public int Power { get; set; }

        [DataType(DataType.Currency)]
        public decimal Profit { get; set; }

        public string? Username { get; set; }
    }
}
