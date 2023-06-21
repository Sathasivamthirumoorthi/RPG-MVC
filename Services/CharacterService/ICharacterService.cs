using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<ServiceResponse<List<GetCharacterResponseDto>>> GetAllCharacters(int userid);
        Task<ServiceResponse<GetCharacterResponseDto>> GetCharacterById(int id);
        Task<ServiceResponse<List<GetCharacterResponseDto>>> AddCharacter(AddCharacterRequestDto newCharacter);
        Task<ServiceResponse<GetCharacterResponseDto>> PutCharacterById(int id,AddCharacterRequestDto character);
        Task<ServiceResponse<List<GetCharacterResponseDto>>> DeleteCharacterById(int id);

    }
}