using RetroMikeMiningTools.DO;
using RetroMikeMiningTools.Enums;
using System.ComponentModel.DataAnnotations;

namespace RetroMikeMiningTools.DTO
{
    public class HiveOsRigConfig
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }
        public bool Enabled { get; set; }
        public string? DonationAmount { get; set; }
        public string? WhatToMineEndpoint { get; set; }

        [UIHint("MiningModeEditor")]
        public MiningMode MiningMode { get; set; }
        public string? HiveWorkerId { get; set; }
        public DateTime? EnabledDateTime { get; set; }
        public bool DonationRunning { get; set; }
        public DateTime? DonationStartTime { get; set; }
        public DateTime? DonationEndTime { get; set; }

        [UIHint("MiningCoinEditor")]
        public string? PinnedTicker { get; set; }
    }
}