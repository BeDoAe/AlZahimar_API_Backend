using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZahimarProject.DTOS.MemmoriesDto;
using ZahimarProject.Helpers;
using ZahimarProject.Helpers.UnitOfWorkFolder;
using ZahimarProject.Models;

namespace ZahimarProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemmoriesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public MemmoriesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        Patient GetPatient()
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Patient patient = unitOfWork.PatientRepository.GetPatient(LoggedInUserId);
            return patient;
        }

        [HttpPost]
        [Authorize(policy: UserRoles.Relative)]
        public async Task<ActionResult<GeneralResponse>> AddMemmory(AddMemmoryDto memmoryDto)
        {
            if (ModelState.IsValid)
            {
                Patient patient = GetPatient();
                Memmories memmories = await unitOfWork.MemoriesServices.GetMemmories(memmoryDto, patient.Id);
                unitOfWork.MemmoriesRepository.Insert(memmories);
                unitOfWork.Save();
                return new GeneralResponse() { IsSuccess = true, Data = "added successfully" };
            }
            return new GeneralResponse() { IsSuccess = false, Data = ModelState };
        }


        [HttpPut]
        [Authorize(policy: UserRoles.Relative)]
        public async Task<ActionResult<GeneralResponse>> EditMemmory(int id, AddMemmoryDto editmemmoryDto)
        {
            Memmories memmory = unitOfWork.MemmoriesRepository.Get(m => m.Id == id);
            if (memmory == null)
            {
                return new GeneralResponse() { IsSuccess = false, Data = $"No Memmory with ID : {id}" };
            }
            
            unitOfWork.MemoriesServices.UpdateMemmory(memmory, editmemmoryDto);

            unitOfWork.Save();
            return new GeneralResponse() { IsSuccess = true, Data = "Updated Successfully" };
        }

        [HttpGet]
       // [Authorize(policy: UserRoles.Relative)]
        public ActionResult<GeneralResponse> GetAllMemmories()
        {
            Patient patient = GetPatient();
            List<Memmories> memmories = unitOfWork.MemmoriesRepository.GetAllMemoriesForPatient(patient.Id);
            if(memmories.Count == 0)
            {
                return new GeneralResponse() { IsSuccess = false, Data = $"No Memmories"};
            }
            List<MapMemmoryToDto> MemmoriesDtos = unitOfWork.MemoriesServices.mapMemmoryToDto(memmories);
            return new GeneralResponse() { IsSuccess = true, Data = MemmoriesDtos };
        }


        [HttpDelete]
        [Authorize(policy: UserRoles.Relative)]
        public ActionResult<GeneralResponse> DeleteMemmory(int id)
        {
            Memmories memmory = unitOfWork.MemmoriesRepository.Get(m => m.Id == id);
            if(memmory is null)
            {
                return new GeneralResponse() { IsSuccess = false, Data = $"No Memmory with id : {id}" };
            }
            bool success = unitOfWork.MemmoriesRepository.CustomMemmoryDelete(memmory);
            if (!success)
            {
                return new GeneralResponse() { IsSuccess = false, Data = "No image with this url" };
            }
            unitOfWork.Save();
            MapMemmoryToDto mapMemmoryToDto = unitOfWork.MemoriesServices.mapMemmoryToDtoToDelete(memmory);
            return new GeneralResponse() { IsSuccess = true, Data =  "Deleted Succesfully" };
        }


        [HttpGet("{id:int}")]
        [Authorize(policy: UserRoles.Relative)]
        public ActionResult<GeneralResponse> GetMemory(int id)
        {
            GeneralResponse generalResponse = unitOfWork.MemoriesServices.GetMemory(id);
            return generalResponse;
        }
    }

}
