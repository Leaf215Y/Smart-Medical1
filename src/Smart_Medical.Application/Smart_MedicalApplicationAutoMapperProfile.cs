using AutoMapper;
using Smart_Medical.DoctorvVsit;
using Smart_Medical.Medical;
using Smart_Medical.OutpatientClinic.Dtos;
using Smart_Medical.Patient;
using Smart_Medical.Prescriptions;
using Smart_Medical.RBAC;
using Smart_Medical.RBAC.Roles;
using Smart_Medical.RBAC.Users;
using System.Collections.Generic;

namespace Smart_Medical;

public class Smart_MedicalApplicationAutoMapperProfile : Profile
{
    public Smart_MedicalApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<CreateUpdateUserDto, User>().ReverseMap();

        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<CreateUpdateRoleDto, Role>().ReverseMap();

        CreateMap<PrescriptionDto, Prescription>().ReverseMap();
        CreateMap<CreateUpdateMedicationDto, Medication>().ReverseMap();
        CreateMap<Medication, MedicationDto>().ReverseMap();

        #region 

        CreateMap<DoctorClinic, InsertPatientDto>().ReverseMap();
        CreateMap<BasicPatientInfo, InsertPatientDto>().ReverseMap();
        CreateMap<Sick, InsertPatientDto>().ReverseMap();
        CreateMap<BasicPatientInfo, GetVisitingDto>().ReverseMap();

        #endregion

        //CreateMap<List<Medication>, List<MedicationDto>>().ReverseMap();
    }
}
