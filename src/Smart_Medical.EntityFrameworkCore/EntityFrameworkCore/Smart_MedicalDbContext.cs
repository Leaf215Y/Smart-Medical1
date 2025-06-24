using Microsoft.EntityFrameworkCore;
using Smart_Medical.Dictionarys;
using Smart_Medical.DoctorvVsit;
using Smart_Medical.Medical;
using Smart_Medical.Patient;
using Smart_Medical.Pharmacy;
using Smart_Medical.Pharmacy.InAndOutWarehouse;
using Smart_Medical.Prescriptions;
using Smart_Medical.RBAC;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace Smart_Medical.EntityFrameworkCore;

[ConnectionStringName("Default")]
public class Smart_MedicalDbContext :
    AbpDbContext<Smart_MedicalDbContext>
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    public DbSet<PrescriptionAs> Prescriptions { get; set; }
    public DbSet<Medication> Medications { get; set; }


    #region

    public DbSet<DoctorAccount> DoctorAccounts { get; set; }
    public DbSet<DoctorClinic> DoctorClinics { get; set; }
    public DbSet<DoctorDepartment> DoctorDepartments { get; set; }
    public DbSet<BasicPatientInfo> BasicPatientInfos { get; set; }
    public DbSet<PatientPrescription> PatientPrescriptions { get; set; }
   
    public DbSet<PharmaceuticalCompany> PharmaceuticalCompanies { get; set; }
    public DbSet<DrugInStock> DrugInStocks { get; set; }
    #endregion

    public DbSet<Drug> Drugs { get; set; }
    public DbSet<Sick> Medicals { get; set; }

    public DbSet<DictionaryData> DictionaryDatas { get; set; }
    public DbSet<DictionaryType> DictionaryTypes { get; set; }


    public Smart_MedicalDbContext(DbContextOptions<Smart_MedicalDbContext> options)
        : base(options)
    {

    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(Smart_MedicalConsts.DbTablePrefix + "YourEntities", Smart_MedicalConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});


        builder.Entity<PrescriptionAs>(b =>
        {
            b.ToTable(Smart_MedicalConsts.DbTablePrefix + "Prescriptions",
                Smart_MedicalConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.PrescriptionName).IsRequired().HasMaxLength(128);

        });
       
        builder.Entity<Medication>(b =>
        {
            b.ToTable(Smart_MedicalConsts.DbTablePrefix + "Medications",
                Smart_MedicalConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.MedicationName).IsRequired().HasMaxLength(128);

        });

        // PatientPrescription 配置
        builder.Entity<PatientPrescription>(b =>
        {
            b.ToTable(Smart_MedicalConsts.DbTablePrefix + "PatientPrescriptions", Smart_MedicalConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
                                       //长度限制、必填项等
            b.Property(x => x.MedicationName).IsRequired().HasMaxLength(128);
            b.Property(x => x.Specification).HasMaxLength(128);
            b.Property(x => x.DosageUnit).IsRequired().HasMaxLength(20);
            b.Property(x => x.Dosage).IsRequired();
            b.Property(x => x.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
            b.Property(x => x.PrescriptionTemplateNumber).IsRequired().HasDefaultValue(0);

        });

        // BasicPatientInfo 配置
        builder.Entity<BasicPatientInfo>(b =>
        {
            b.ToTable(Smart_MedicalConsts.DbTablePrefix + "BasicPatientInfos", Smart_MedicalConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.VisitId).IsRequired().HasMaxLength(20);
            b.Property(x => x.PatientName).IsRequired().HasMaxLength(50);
            b.Property(x => x.AgeUnit).HasMaxLength(10);
            b.Property(x => x.ContactPhone).HasMaxLength(20);
            b.Property(x => x.IdNumber).HasMaxLength(18);
            b.Property(x => x.VisitType).IsRequired().HasMaxLength(20);
            b.Property(x => x.VisitStatus).HasMaxLength(20);
        });

        // DoctorAccount 配置
        builder.Entity<DoctorAccount>(b =>
        {
            b.ToTable(Smart_MedicalConsts.DbTablePrefix + "DoctorAccounts", Smart_MedicalConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.AccountId).IsRequired().HasMaxLength(20);
            b.Property(x => x.EmployeeId).IsRequired().HasMaxLength(10);
            b.Property(x => x.EmployeeName).IsRequired().HasMaxLength(20);
            b.Property(x => x.InstitutionName).IsRequired().HasMaxLength(50);
            b.Property(x => x.DepartmentName).HasMaxLength(30);
        });

        // DoctorClinic 配置
        builder.Entity<DoctorClinic>(b =>
        {
            b.ToTable(Smart_MedicalConsts.DbTablePrefix + "DoctorClinics", Smart_MedicalConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.PatientId).IsRequired();
            b.Property(x => x.DoctorId).IsRequired();
            b.Property(x => x.VisitDateTime).IsRequired();
            b.Property(x => x.DepartmentName).IsRequired().HasMaxLength(50);
            b.Property(x => x.ChiefComplaint).HasMaxLength(500);
            b.Property(x => x.PreliminaryDiagnosis).HasMaxLength(1000);
            b.Property(x => x.VisitType).IsRequired().HasMaxLength(20);
            b.Property(x => x.Remarks).HasMaxLength(1000);
        });

        // DoctorDepartment 配置
        builder.Entity<DoctorDepartment>(b =>
        {
            b.ToTable(Smart_MedicalConsts.DbTablePrefix + "DoctorDepartments", Smart_MedicalConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.DepartmentName).IsRequired().HasMaxLength(50);
            b.Property(x => x.DepartmentCategory).HasMaxLength(30);
            b.Property(x => x.Address).HasMaxLength(100);
            b.Property(x => x.DirectorName).HasMaxLength(20);
            b.Property(x => x.Type).HasMaxLength(20);
        });



           builder.Entity<Drug>(b =>
        {
            b.ToTable(Smart_MedicalConsts.DbTablePrefix + "Drugs", Smart_MedicalConsts.DbSchema);
            b.ConfigureByConvention(); 
            b.Property(x => x.DrugName).IsRequired().HasMaxLength(128);
            b.Property(x => x.DrugType).IsRequired().HasMaxLength(32);
            b.Property(x => x.FeeName).IsRequired().HasMaxLength(32);
            b.Property(x => x.DosageForm).IsRequired().HasMaxLength(32);
            b.Property(x => x.Specification).IsRequired().HasMaxLength(64);
            b.Property(x => x.Effect).IsRequired().HasMaxLength(256);
            b.Property(x => x.Category).IsRequired();
            b.Property(x => x.PurchasePrice).HasColumnType("decimal(18,2)");
            b.Property(x => x.SalePrice).HasColumnType("decimal(18,2)");
        });

        builder.Entity<Sick>(b =>
        {
            b.ToTable(Smart_MedicalConsts.DbTablePrefix + "Medicals", Smart_MedicalConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Status).IsRequired().HasMaxLength(32);
            b.Property(x => x.InpatientNumber).IsRequired().HasMaxLength(32);
            b.Property(x => x.Name).IsRequired().HasMaxLength(32);
            b.Property(x => x.DischargeDepartment).IsRequired().HasMaxLength(64);
            b.Property(x => x.Gender).IsRequired().HasMaxLength(8);
            b.Property(x => x.DischargeTime).IsRequired();
            b.Property(x => x.AdmissionDiagnosis).IsRequired().HasMaxLength(128);
            b.Property(x => x.DischargeDiagnosis).IsRequired().HasMaxLength(128);
        });

        builder.Entity<PharmaceuticalCompany>(b =>
        {
            b.ToTable(Smart_MedicalConsts.DbTablePrefix + "PharmaceuticalCompanies", Smart_MedicalConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.CompanyName).IsRequired().HasMaxLength(128);
            b.Property(x => x.ContactPerson).HasMaxLength(64);
            b.Property(x => x.ContactPhone).HasMaxLength(32);
            b.Property(x => x.Address).HasMaxLength(256);
        });

        builder.Entity<DrugInStock>(b =>
        {
            b.ToTable(Smart_MedicalConsts.DbTablePrefix + "DrugInStocks", Smart_MedicalConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.BatchNumber).IsRequired().HasMaxLength(64);

            b.HasOne<Drug>().WithMany().HasForeignKey(x => x.Id).IsRequired();
            b.HasOne<PharmaceuticalCompany>().WithMany().HasForeignKey(x => x.Id).IsRequired();
        });

        builder.Entity<DictionaryType>(b =>
        {
            b.ToTable(Smart_MedicalConsts.DbTablePrefix + "DictionaryTypes",
                Smart_MedicalConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<Medication>(b =>
        {
            b.ToTable(Smart_MedicalConsts.DbTablePrefix + "Medications",
                Smart_MedicalConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.MedicationName).IsRequired().HasMaxLength(128);

        });

    }
}
