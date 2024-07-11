using ZahimarProject.DTOS.MemmoriesDto;
using ZahimarProject.Helpers.SaveImages;
using ZahimarProject.Models;
using ZahimarProject.Repos.MemmoriesRepo;

namespace ZahimarProject.Services.MemoriesServices
{
    public class MemoryServices : Service<Memmories> , IMemoriesServices
    {
        private readonly IMemmoriesRepository memmoriesRepository;

        public MemoryServices(IMemmoriesRepository memmoriesRepository)
        {
            this.memmoriesRepository = memmoriesRepository;
        }
        public async Task<Memmories> GetMemmories(AddMemmoryDto addMemmoryDto, int patientID)
           {
                Memmories memmory = new Memmories()
                {
                    Description = addMemmoryDto.Description,
                    PatientId = patientID,
                    Person_Name = addMemmoryDto.Person_Name,
                    PicURL = await ImageHelper.SaveImageAsync(addMemmoryDto.Image),

                };
                return memmory;
                
           }

            public async void UpdateMemmory(Memmories memmory, AddMemmoryDto editMemmoryDto)
            {
                memmory.PicURL = await ImageHelper.SaveImageAsync(editMemmoryDto.Image);
                memmory.Description = editMemmoryDto.Description;
                memmory.Person_Name = editMemmoryDto.Person_Name;
            }

            public List<MapMemmoryToDto> mapMemmoryToDto(List<Memmories> memmories)
            {
                List<MapMemmoryToDto> memmoryDtos = new List<MapMemmoryToDto>();
                foreach(Memmories m in memmories)
                {
                    MapMemmoryToDto memmoryDto = new MapMemmoryToDto() { 
                    Id = m.Id,
                    PatientId = m.PatientId,
                    Description = m.Description,
                    PicURL = m.PicURL,
                    Person_Name = m.Person_Name,
                    };
                    memmoryDtos.Add(memmoryDto);
                }
                return memmoryDtos;
            }

        public MapMemmoryToDto mapMemmoryToDtoToDelete(Memmories m)
        {
            MapMemmoryToDto mapMemmoryToDto = new MapMemmoryToDto()
            {
                Id = m.Id,
                PatientId = m.PatientId,
                Description = m.Description,
                Person_Name = m.Person_Name,
                PicURL = m.PicURL
            };
            return mapMemmoryToDto;
        }

        public GeneralResponse GetMemory(int id)
        {
            Memmories memmories = memmoriesRepository.Get(m=>m.Id==id);
            if(memmories == null)
            {
                return new GeneralResponse()
                { 
                    IsSuccess = false,
                    Data="Not Found"
                };
            }else
            {
                MapMemmoryToDto mapMemmoryToDto = new MapMemmoryToDto()
                {
                    Id = memmories.Id,
                    PatientId = memmories.PatientId,
                    Description = memmories.Description,
                    Person_Name = memmories.Person_Name,
                    PicURL = memmories.PicURL
                };

                return new GeneralResponse()
                {
                    IsSuccess=true,
                    Data=mapMemmoryToDto,
                };
            }
        }
    }
}
