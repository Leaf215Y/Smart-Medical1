using AutoMapper;
using Smart_Medical.Application.Contracts.RBAC.Permissions;
using Smart_Medical.Application.Contracts.RBAC.RolePermissions;
using Smart_Medical.Application.Contracts.RBAC.Roles;
using Smart_Medical.Application.Contracts.RBAC.UserRoles;
using Smart_Medical.Application.Contracts.RBAC.Users;
using Smart_Medical.Appointment;
using Smart_Medical.Dictionarys;
using Smart_Medical.Dictionarys.DictionaryDatas;
using Smart_Medical.Dictionarys.DictionaryTypes;
using Smart_Medical.DoctorvVsit;
using Smart_Medical.DoctorvVsit;
using Smart_Medical.DoctorvVsit.DockerDepartments;
using Smart_Medical.DoctorvVsit.DockerDepartments;
using Smart_Medical.Medical;
using Smart_Medical.OutpatientClinic.Dtos;
using Smart_Medical.OutpatientClinic.Dtos.Parameter;
using Smart_Medical.Patient;
using Smart_Medical.Pharmacy;
using Smart_Medical.Pharmacy.InAndOutWarehouse;
using Smart_Medical.Prescriptions;
using Smart_Medical.RBAC;
using Smart_Medical.UserLoginECC;
using System.Collections.Generic;

namespace Smart_Medical;

public class Smart_MedicalApplicationAutoMapperProfile : Profile
{
    public Smart_MedicalApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        //用户
        CreateMap<User, UserDto>();
        CreateMap<CreateUpdateUserDto, User>();
        //角色
        CreateMap<Role, RoleDto>();
        CreateMap<CreateUpdateRoleDto, Role>();
        //权限
        CreateMap<Permission, PermissionDto>();
        CreateMap<CreateUpdatePermissionDto, Permission>();
        //用户角色关联
        CreateMap<UserRole, UserRoleDto>();
        CreateMap<CreateUpdateUserRoleDto, UserRole>();
        //角色权限关联
        CreateMap<RolePermission, RolePermissionDto>();
        CreateMap<CreateUpdateRolePermissionDto, RolePermission>();
        //处方
        CreateMap<PrescriptionDto, Prescription>().ReverseMap();
        CreateMap<CreateUpdateMedicationDto, Medication>();
        CreateMap<Medication, MedicationDto>().ReverseMap();

        #region 

        CreateMap<DoctorClinic, InsertPatientDto>().ReverseMap();
        CreateMap<BasicPatientInfo, InsertPatientDto>().ReverseMap();
        CreateMap<Sick, InsertPatientDto>().ReverseMap();
        CreateMap<BasicPatientInfo, GetVisitingDto>().ReverseMap();
        CreateMap<BasicPatientInfo, BasicPatientInfoDto>().ReverseMap();
        CreateMap<User, ResultLoginDto>().ReverseMap();
        CreateMap<User, ResultLoginDtor>().ReverseMap();
        #endregion

        //CreateMap<List<Medication>, List<MedicationDto>>().ReverseMap();
        //科室
        CreateMap<CreateUpdateDoctorDepartmentDto, DoctorDepartment>().ReverseMap();
        CreateMap<DoctorDepartment, GetDoctorDepartmentListDto>().ReverseMap();
        CreateMap<GetDoctorDepartmentSearchDto, DoctorDepartment>().ReverseMap();


     
           
        CreateMap<MedicalHistory, PharmaceuticalCompanyDto>();
        CreateMap<CreateUpdatePharmaceuticalCompanyDto, MedicalHistory>();

        // 药品相关映射
        CreateMap<CreateUpdateDrugDto, Drug>();
        CreateMap<Drug, DrugDto>();
        //数据字典
        CreateMap<CreateUpdateDictionaryDataDto, DictionaryData>().ReverseMap();
        CreateMap<DictionaryData, GetDictionaryDataDto>().ReverseMap();
        CreateMap<CreateUpdateDictionaryTypeDto, DictionaryType>().ReverseMap();
        CreateMap<DictionaryType, GetDictionaryTypeDto>().ReverseMap();
        CreateMap<DrugInStock, DrugInStockDto>();
        CreateMap<CreateUpdateDrugInStockDto, DrugInStock>();

        CreateMap<Sick, SickDto>().ReverseMap();
        CreateMap<CreateUpdateSickDto, Sick>().ReverseMap();


        CreateMap<MakeAppointmentDto, BasicPatientInfo>().ReverseMap();


    }
}
