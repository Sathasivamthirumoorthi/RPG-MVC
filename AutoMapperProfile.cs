using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character,GetCharacterResponseDto>();
            CreateMap<AddCharacterRequestDto,Character>();
        }
    }
}