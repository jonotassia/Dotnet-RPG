namespace Dotnet_RPG.Dtos.Fight
{
    public class AttackResultDto
    {
        public string Attacker { get; set; } = string.Empty;
        public string Opponent { get; set; } = string.Empty;
        public string WeaponUsed { get; set; } = string.Empty;
        public string SkillUsed { get; set;} = string.Empty;
        public int AttackerHP { get; set; }
        public int OpponentHP { get; set; }
        public int Damage { get; set; }
        public bool Defeated { get; set; } = false;
    }
}
