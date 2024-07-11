using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZahimarProject.DTOS.ToDoListDtos;
using ZahimarProject.Helpers;
using ZahimarProject.Helpers.Enums;
using ZahimarProject.Helpers.UnitOfWorkFolder;
using ZahimarProject.Models;

namespace ZahimarProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ToDoListController(IUnitOfWork unitOfWork)
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
        [Authorize(Policy = UserRoles.Relative)]
        public ActionResult<GeneralResponse> AddToDoList(ToDoListDto toDoListDto)
        {
            if (ModelState.IsValid)
            {
                Patient patient = GetPatient();
                ToDoList toDoList = unitOfWork.ToDoListService.AddToDoList(toDoListDto, patient.Id);
                unitOfWork.ToDoListRepository.Insert(toDoList);
                unitOfWork.Save();
                //ReturnToDoListDto returnToDoList = unitOfWork.ToDoListService.mapToDto(toDoList);
                return new GeneralResponse() { IsSuccess = true, Data = "Note Added Successfully" };
            }
            return new GeneralResponse() { IsSuccess = false, Data = ModelState };
        }

        [HttpPut]
        [Authorize(Policy = UserRoles.Relative)]
        public ActionResult<GeneralResponse> EditToDoList(int id, ToDoListDto toDoListDto)
        {
            ToDoList toDoListDb = unitOfWork.ToDoListRepository.Get(t => t.Id == id);
            if (toDoListDb == null)
            {
                return new GeneralResponse() { IsSuccess = false, Data = $"NO ToDoList with id : {id}" };
            }
            unitOfWork.ToDoListService.Edit(toDoListDb, toDoListDto);
            unitOfWork.Save();
            ReturnToDoListDto returnToDoList = unitOfWork.ToDoListService.mapToDto(toDoListDb);
            return new GeneralResponse() { IsSuccess = true, Data = returnToDoList };
        }

        [HttpGet]
        [Authorize(Policy = UserRoles.Relative)]
        public ActionResult<GeneralResponse> GetAll()
        {
            Patient patient = GetPatient();
            List<ToDoList> toDoLists = unitOfWork.ToDoListRepository.GetAll2(patient.Id);
            if (toDoLists.Count == 0)
            {
                return new GeneralResponse() { IsSuccess = false, Data = $"No ToDoLists" };
            }
            List<ReturnToDoListDto> returnToDoListDtos = unitOfWork.ToDoListService.mapListToDto(toDoLists, patient.Id);
            return new GeneralResponse() { IsSuccess = true, Data = returnToDoListDtos };
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = UserRoles.Relative)]
        public ActionResult<GeneralResponse> GetByID(int id)
        {
            Patient patient = GetPatient();

            ToDoList toDoListDb = unitOfWork.ToDoListRepository.Get(t => t.Id == id && t.PatientId == patient.Id);
            if (toDoListDb == null)
            {
                return new GeneralResponse() { IsSuccess = false, Data = $"No ToDoList with Id : {id}" };
            }
            ReturnToDoListDto returnToDoListDto = unitOfWork.ToDoListService.mapToDto(toDoListDb);
            return new GeneralResponse() { IsSuccess = true, Data= returnToDoListDto };
        }

        [HttpDelete]
        [Authorize(Policy = UserRoles.Relative)]
        public ActionResult<GeneralResponse> Delete(int id)
        {
            Patient patient = GetPatient();
            ToDoList toDoListDb = unitOfWork.ToDoListRepository.Get(t => t.Id==id && t.PatientId == patient.Id);
            if (toDoListDb == null)
            {
                return new GeneralResponse() { IsSuccess = false, Data = $"No ToDoList with Id : {id}" };
            }
            unitOfWork.ToDoListRepository.Delete(toDoListDb);
            unitOfWork.Save();
            return new GeneralResponse() { IsSuccess = true, Data = $"Deleted Successfully" };
        }

        [HttpPost("MarkDone/{id:int}")]
        [Authorize(Policy = UserRoles.Relative)]
        public ActionResult<GeneralResponse> MarkDone(int id)
        {
            ToDoList toDoList = unitOfWork.ToDoListRepository.Get(t=>t.Id==id); 
            if (toDoList != null)
            {
                if(toDoList.Status== ToDoListStatus.Done)
                {
                    return new GeneralResponse()
                    {
                        IsSuccess = false,
                        Data = "You Already did it"
                    };
                }
                toDoList.Status=ToDoListStatus.Done;
                unitOfWork.ToDoListRepository.Update(toDoList);
                unitOfWork.Save();
                return new GeneralResponse()
                {
                    IsSuccess = true,
                    Data = "Great Work You Did it"
                };
            }
            else
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = "this ToDo Not Found"
                };
            }
        }
    }
}
