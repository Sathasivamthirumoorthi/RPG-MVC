using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace web.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        

        private static List<Character> characters = new List<Character>(){
        new Character(),
        new Character{
            CharacterID = 1,
            CharacterName = "paul"
        },
        new Character{
            CharacterID = 2,
            CharacterName = "ss"
        },
        new Character{
            CharacterID = 3,
            CharacterName = "cc"
        },
        };
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper , DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<List<GetCharacterResponseDto>>> AddCharacter(AddCharacterRequestDto newCharacter)
        {   
            var serviceResponse = new ServiceResponse<List<GetCharacterResponseDto>>();
            await _context.Characters.AddAsync(_mapper.Map<Character>(newCharacter));
            await _context.SaveChangesAsync();
            var dbCharacters = await _context.Characters.ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c  => _mapper.Map<GetCharacterResponseDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterResponseDto>>> DeleteCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterResponseDto>>();
            try{
            var character = await _context.Characters.FirstAsync(c => c.CharacterID == id);
            Console.WriteLine("me",character);
            if(character is null){
                throw new Exception($"Character with id : {id} not found ");
            }

            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();
            var dbCharacters = await _context.Characters.ToListAsync();
            // mapper choose map function 
            // which function it should to mapped to type (GetCharacterResponseDto)
            // and parameter that is going to mapped
            serviceResponse.Data = dbCharacters.Select(c  => _mapper.Map<GetCharacterResponseDto>(c)).ToList();
            }
            catch(Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterResponseDto>>> GetAllCharacters(int userid)
        {   
            var serviceResponse = new ServiceResponse<List<GetCharacterResponseDto>>();
            var dbCharacters = await _context.Characters.Where(c => c.User.Id == userid).ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterResponseDto>(c)).ToList();
            return serviceResponse;
        }   

        public async Task<ServiceResponse<GetCharacterResponseDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterResponseDto>();
            try{
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.CharacterID == id);
            
            if(character is null){
                throw new Exception($"Character with id : {id} not found ");
            }

            serviceResponse.Data = _mapper.Map<GetCharacterResponseDto>(character);
            }
            catch(Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }
            return serviceResponse;

        }

        public async Task<ServiceResponse<GetCharacterResponseDto>> PutCharacterById(int id, AddCharacterRequestDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterResponseDto>();
            try{
            var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.CharacterID == id);
            
            if(dbCharacter is null){
                throw new Exception($"Character with id : {id} not found ");
            }

            dbCharacter.CharacterName = newCharacter.CharacterName; // Corrected property name
            dbCharacter.Class = newCharacter.Class;
            dbCharacter.Defence = newCharacter.Defence;
            dbCharacter.Intelligence = newCharacter.Intelligence;
            dbCharacter.Strength = newCharacter.Strength;
            dbCharacter.HitPoints = newCharacter.HitPoints; 
            await _context.SaveChangesAsync();
            // mapper choose map function 
            // which function it should to mapped to type (GetCharacterResponseDto)
            // and parameter that is going to mapped
            serviceResponse.Data = _mapper.Map<GetCharacterResponseDto>(dbCharacter);
            }
            catch(Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }
            return serviceResponse;   
        }
    }
}