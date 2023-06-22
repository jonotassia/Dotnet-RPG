using AutoMapper;
using Dotnet_RPG.Dtos.Character;
using Dotnet_RPG.Models;
using System.Collections.Generic;

namespace Dotnet_RPG.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character>
        {
            new Character {Name = "Mønkëy", Strength = 50, Defense = 1, Handsomeness = 100},
            new Character {Id = 1, Name = "Wald", Intelligence = 50, Defense = 30, Handsomeness = 95, Class = RpgClass.Mage}
        };
        private readonly IMapper _mapper;

        public CharacterService(IMapper mapper)
        {
            this._mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character = _mapper.Map<Character>(newCharacter);
            character.Id = characters.Max(c => c.Id) + 1;
            characters.Add(character);
            serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetChracterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var character = characters.FirstOrDefault(c => c.Id == id);
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            try
            {
                var character = characters.FirstOrDefault(c => c.Id == updateCharacter.Id);

                if (character == null)
                {
                    throw new NullReferenceException($"Character with Id '{updateCharacter.Id}' not found.");
                }

                _mapper.Map<Character>(updateCharacter);
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (NullReferenceException ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            
            try
            {
                var character = characters.FirstOrDefault(c => c.Id == id);
                characters.Remove(character);

                if (character is null)
                {
                    throw new NullReferenceException($"Character with Id '{id}' not found.");
                }

                serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            }
            catch(NullReferenceException ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Success = false;
            }
            
            return serviceResponse;
        }
    }
}
