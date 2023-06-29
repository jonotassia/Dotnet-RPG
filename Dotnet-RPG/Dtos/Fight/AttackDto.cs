namespace Dotnet_RPG.Dtos.Fight
{
    public class AttackDto
    {
        public int AttackerId { get; set; }
        public int OpponentId { get; set; }
        public int SkillId { get; set; } = 0;
    }
}
