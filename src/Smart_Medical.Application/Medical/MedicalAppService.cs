using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Smart_Medical.Medical
{
    [ApiExplorerSettings(GroupName = "医疗管理")]
    public class MedicalAppService : ApplicationService
    {
        private readonly IRepository<Sick, Guid> _repository;

        public MedicalAppService(IRepository<Sick, Guid> repository)
        {
            _repository = repository;
        }

        // 查询单个
        public async Task<SickDto> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(id);
            return ObjectMapper.Map<Sick, SickDto>(entity);
        }

        // 分页查询
        public async Task<PagedResultDto<SickDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _repository.GetQueryableAsync();
            var totalCount = await AsyncExecuter.CountAsync(queryable);
            var items = await AsyncExecuter.ToListAsync(
                queryable.Skip(input.SkipCount).Take(input.MaxResultCount)
            );
            return new PagedResultDto<SickDto>(
                totalCount,
                ObjectMapper.Map<List<Sick>, List<SickDto>>(items)
            );
        }

        // 新增
        public async Task<SickDto> CreateAsync(CreateUpdateSickDto input)
        {
            var entity = ObjectMapper.Map<CreateUpdateSickDto, Sick>(input);
            entity = await _repository.InsertAsync(entity);
            return ObjectMapper.Map<Sick, SickDto>(entity);
        }

        // 修改
        public async Task<SickDto> UpdateAsync(Guid id, CreateUpdateSickDto input)
        {
            var entity = await _repository.GetAsync(id);
            ObjectMapper.Map(input, entity);
            entity = await _repository.UpdateAsync(entity);
            return ObjectMapper.Map<Sick, SickDto>(entity);
        }

        // 删除
        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

    }
}
