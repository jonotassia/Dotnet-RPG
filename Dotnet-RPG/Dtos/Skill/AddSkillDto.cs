namespace Dotnet_RPG.Dtos.Skill
{
    public class AddSkillDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Damage { get; set; }
    }
}
