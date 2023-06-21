using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.Dtos.Character
{
    public class GetCharacterResponseDto
    {
        public int CharacterID { get; set; }

        public string? CharacterName { get; set; }

        public int HitPoints { get; set; } = 100;

        public int Strength { get; set; } = 10;

        public int Defence { get; set; } = 10;

        public int Intelligence { get; set; } = 10;

        public RpgClass Class { get; set; } = RpgClass.Knight;
    }
}