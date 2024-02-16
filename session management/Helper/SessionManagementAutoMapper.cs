using AutoMapper;
using session_management.DTO;
using session_management.Models;
namespace session_management.Helper
{
    public class SessionManagementAutoMapper:Profile
    {
        public SessionManagementAutoMapper()
        {
            CreateMap<AdminModel, AdminModelDTO>();
            CreateMap<KeyExtensionModel, KeyExtensionModelDTO>();
            CreateMap<KeyModel, KeyModelDTO>();
            CreateMap<UserKeyModel, UserKeyModelDTO>();
            CreateMap<UserModel, UserModelDTO>();
            CreateMap<AdminModelDTO, AdminModel>();
            CreateMap<UserModelUpdateDTO, UserModel>();
            CreateMap<KeyExtensionModelDTO, KeyExtensionModel>();
            CreateMap<KeyModelDTO, KeyModel>();
            CreateMap<UserKeyModelDTO, UserKeyModel>();
            CreateMap<UserModelDTO, UserModel>();
        }
    }
}
