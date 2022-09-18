using RetroMikeMiningTools.DO;
using System.ComponentModel.DataAnnotations;

namespace RetroMikeMiningTools.DTO
{
    public class HiveOsRigCoinConfig
    {
        public HiveOsRigCoinConfig()
        {
            this.Groups = new List<LinkedGroup>();
        }

        [Key]
        public int Id { get; set; }

        public int WorkerId { get; set; }

        [UIHint("MiningCoinEditor")]
        public string Ticker { get; set; }

        [UIHint("MiningFlighsheetEditor")]
        public long? Flightsheet { get; set; }

        public string FlightsheetName { get; set; }

        public bool Enabled { get; set; }

        public double? DesiredBalance { get; set; }

        [UIHint("GroupListEditor")]
        public List<LinkedGroup>? Groups { get; set; }
    }
}
