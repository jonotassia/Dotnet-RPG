﻿namespace Dotnet_RPG.Dtos.Character
{
    public class UpdateCharacterDto
    {
        // Properties
        public int Id { get; set; }
        public string Name { get; set; } = "Character";
        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defense { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public int Handsomeness { get; set; } = 10;
        public RpgClass Class { get; set; } = RpgClass.Knight;
    }
}