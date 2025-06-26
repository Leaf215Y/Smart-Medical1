using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;
using Microsoft.Extensions.DependencyInjection;
using Smart_Medical.Until.Redis;
using Smart_Medical.Redis;
using Smart_Medical.Dictionarys.DictionaryDatas;
using System.Collections.Generic; // 引入实现

namespace Smart_Medical;
[DependsOn(
typeof(Smart_MedicalDomainModule),
typeof(AbpCachingStackExchangeRedisModule), // 确保 Redis 缓存已启用
typeof(Smart_MedicalApplicationContractsModule),
typeof(AbpSettingManagementApplicationModule)

    )]
public class Smart_MedicalApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {

        // 注册泛型接口实现（推荐方式，支持所有T）
        context.Services.AddScoped(typeof(IRedisHelper<>), typeof(RedisHelper<>));

        // 或者针对特定类型单独注册（如果有特殊需求）
        //context.Services.AddScoped<IRedisHelper<List<GetDictionaryDataDto>>, RedisHelper<List<GetDictionaryDataDto>>>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<Smart_MedicalApplicationModule>();
        });
        // 如果 MyRedisHelper 和 DictionaryTypeCacheHelper 没有使用 [Dependency] 特性，则需要在这里手动注册
        // context.Services.AddScoped(typeof(IMyRedisHelper<>), typeof(MyRedisHelper<>));
        // context.Services.AddScoped<IDictionaryTypeCacheHelper, DictionaryTypeCacheHelper>();

        // 配置 ABP 分布式缓存选项
        Configure<AbpDistributedCacheOptions>(options =>
        {
            options.KeyPrefix = "SmartMedical:"; // 统一的 Redis 键前缀
        });
    }
}
