namespace Dotnet_RPG.Models
{
    public class Character
    {        
        // Declare properties
        public int Id { get; set; }
        public int Name { get; set; } = "Monkey";
        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defense { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public RpgClass Class { get; set; } = RpgClass.Knight;
    }
}
