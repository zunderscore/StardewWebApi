using System.Text.Json.Serialization;
using StardewModdingAPI.Enums;
using StardewValley.Menus;
using StardewWebApi.Types;

namespace StardewWebApi.Game.Players;

public class BasicSkill
{
    private readonly List<int> _professionIds;

    public BasicSkill(int id, int level, List<int>? professionIds = null)
    {
        Id = id;
        Level = level;
        _professionIds = professionIds ?? new();
    }

    public int Id { get; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SkillType Name => (SkillType)Id;

    public int Level { get; }

    public List<NumericIdNameDescription> Professions => _professionIds.Select(id =>
        {
            var description = LevelUpMenu.getProfessionDescription(id);
            return new NumericIdNameDescription(id, description[0], description[1]);
        }).ToList();
}
