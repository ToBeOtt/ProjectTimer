﻿namespace ProjectTimer.Entities
{
    public class Clock
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime Started { get; set; }
        public DateTime Ended { get; set; }
        public double TotalMinutes { get; set; }

        public Project Project { get; set; }


        public Clock() { }

        public Clock(string? description, DateTime started)
        {
            Description = description;
            Started = started;
        }
    }
}
