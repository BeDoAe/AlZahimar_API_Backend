using ZahimarProject.DTOS.MemmoriesDto;
using ZahimarProject.Models;

namespace ZahimarProject.Services.MemoriesServices
{
    public interface IMemoriesServices : IService<Memmories>
    {
        public Task<Memmories> GetMemmories(AddMemmoryDto addMemmoryDto, int patientID);
        public void UpdateMemmory(Memmories memmory, AddMemmoryDto editMemmoryDto);

        public List<MapMemmoryToDto> mapMemmoryToDto(List<Memmories> memmories);

        public MapMemmoryToDto mapMemmoryToDtoToDelete(Memmories m);
        public GeneralResponse GetMemory(int id);

    }
}
