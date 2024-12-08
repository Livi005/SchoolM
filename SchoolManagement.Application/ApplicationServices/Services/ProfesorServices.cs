using AutoMapper;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.MapsDto;
using SchoolManagement.Infrastructure.DataAccess.IProfesorRepository;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Application.ApplicationServices.MapsDto.Profesor;

namespace SchoolManagement.Application.ApplicationServices.Services
{
    public class ProfesorService : IProfesorService
    {
        private readonly IProfesorRepository _profesorRepository;
        private readonly IMapper _mapper;

        public ProfesorService(IProfesorRepository profesorRepository, IMapper mapper)
        {
            _profesorRepository = profesorRepository;
            _mapper = mapper;
        }

        public async Task<ProfesorDto> CreateProfesorAsync(ProfesorDto profesorDto)
        {
            var profesor = _mapper.Map<Domain.Entities.Profesor>(profesorDto);
            var savedAgency = await _profesorRepository.CreateAsync(profesor);
            return _mapper.Map<ProfesorDto>(savedAgency);
        }

        public async Task DeleteProfesorByIdAsync(int profesorDto)
        {
            await _profesorRepository.DeleteByIdAsync(profesorDto);
        }

        public async Task<IEnumerable<ProfesorDto>> ListProfesorAsync()
        {
            var profesors = await _profesorRepository.ListAsync();
            var list = profesors.ToList();
            List<ProfesorDto> Profesors_List = new();
            for (int i = 0; i < profesors.Count(); i++)
            {
                Profesors_List.Add(_mapper.Map<ProfesorDto>(list[i]));
            }

            return Profesors_List;
        }

        public async Task<ProfesorDto> UpdateProfesorAsync(ProfesorDto profesorDto)
        {
            var profesor = _profesorRepository.GetById(profesorDto.ID);
            _mapper.Map(profesorDto, profesor);
            await _profesorRepository.UpdateAsync(profesor);
            return _mapper.Map<ProfesorDto>(profesor);
        }
    }
}