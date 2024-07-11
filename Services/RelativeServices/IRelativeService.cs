using ZahimarProject.Authentication;
using ZahimarProject.DTOS.RelativeDTOs;
using ZahimarProject.Models;
using ZahimarProject.Repos.RelativeRepo;

namespace ZahimarProject.Services.RelativeServices
{
    public interface IRelativeService :IService<Relative>
    {
        public IRelativeRepository RelativeRepository { get; }
        //public RelativeGetDTO GetRelativeDTO(Relative relative);
        public RelativeGetProfileDTO RelativeGetProfile(Relative relative);
        public void GetUpdatedRelative(RelativeDTO relativeDTO, Relative UpdatedRelative);
        public void GetUpdatedAppUser(Relative relative, AppUser appUser);
        public List<RelativeGetDTO> GetRelativeDTOs(List<Relative> relatives);
    }
}
