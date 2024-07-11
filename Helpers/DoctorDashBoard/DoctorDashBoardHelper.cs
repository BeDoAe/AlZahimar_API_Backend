using ZahimarProject.Helpers.UnitOfWorkFolder;

namespace ZahimarProject.Helpers.DoctorDashBoard
{
    public class DoctorDashBoardHelper
    {
        private readonly IUnitOfWork unitOfWork;

        public DoctorDashBoardHelper(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


    }
}
