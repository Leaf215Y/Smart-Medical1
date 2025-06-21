using Microsoft.EntityFrameworkCore;
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

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    // Tenant Management
    #endregion
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Medication> Medications { get; set; }


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


        builder.Entity<Prescription>(b =>
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
    }
}
