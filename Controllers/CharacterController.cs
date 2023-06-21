
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace web.Controllers
{   
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {   
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterResponseDto>>>> GetAll()
        {
            // get id by cliams principle obj
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            return Ok(await _characterService.GetAllCharacters(userId));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterResponseDto>>> Get(int id)
        {
            var character = await _characterService.GetCharacterById(id);
            if(character.Data is null){
                return NotFound(character);
            }
            return Ok(await _characterService.GetCharacterById(id));
        }

        [AllowAnonymous]
        [HttpPost("Create")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterResponseDto>>>> AddCharacter(AddCharacterRequestDto newCharacter){
            var character = _characterService.AddCharacter(newCharacter);
            return Ok(await character);
        }

       [HttpPut("Edit/{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterResponseDto>>> PutCharacter(int id,AddCharacterRequestDto request)
        {   
            var character = await _characterService.PutCharacterById(id,request);
            if(character.Data is null){
                return NotFound(character);
            }
            return Ok(character);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterResponseDto>>>> DeleteCharacter(int id)
        {
            var character = await _characterService.DeleteCharacterById(id);
            if(character.Data is null){
                return NotFound(character);
            }
            return Ok(character);
        }


        
    }
}