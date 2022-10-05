using RetroMikeMiningTools.Enums;
using System.ComponentModel.DataAnnotations;

namespace RetroMikeMiningTools.DTO
{
    public class GoldshellAsicConfig
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }
        public bool Enabled { get; set; }
        public string? ApiBasePath { get; set; }

        [DataType(DataType.Password)]
        public string? ApiPassword { get; set; }
        public string? WhatToMineEndpoint { get; set; }

        [UIHint("MiningModeEditor")]
        public MiningMode MiningMode { get; set; }

        public string? DonationAmount { get; set; }

        public DateTime? EnabledDateTime { get; set; }
        public bool DonationRunning { get; set; }
        public DateTime? DonationStartTime { get; set; }
        public DateTime? DonationEndTime { get; set; }
        public string? Username { get; set; }

        [DataType(DataType.Currency)]
        public decimal Profit { get; set; }
        
    }
}
