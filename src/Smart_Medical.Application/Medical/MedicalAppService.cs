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

        /// <summary>
        /// 查询单个病历
        /// Get a single medical record by ID
        /// </summary>
        /// <remarks>
        /// GET /api/app/medical/{id}
        /// 根据病历ID获取单个病历详细信息。
        /// </remarks>
        /// <param name="id">病历ID</param>
        /// <returns>病历详细信息</returns>
        public async Task<SickDto> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(id);
            return ObjectMapper.Map<Sick, SickDto>(entity);
        }

        /// <summary>
        /// 分页查询病历列表
        /// Get paged list of medical records
        /// </summary>
        /// <remarks>
        /// GET /api/app/medical/list
        /// 分页获取病历信息列表。
        /// </remarks>
        /// <param name="input">分页和排序参数</param>
        /// <returns>病历分页列表</returns>
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

        /// <summary>
        /// 新增病历
        /// Create a new medical record
        /// </summary>
        /// <remarks>
        /// POST /api/app/medical
        /// 新增一条病历信息。
        /// </remarks>
        /// <param name="input">病历创建参数</param>
        /// <returns>新增的病历详细信息</returns>
        public async Task<SickDto> CreateAsync(CreateUpdateSickDto input)
        {
            var entity = ObjectMapper.Map<CreateUpdateSickDto, Sick>(input);
            entity = await _repository.InsertAsync(entity);
            return ObjectMapper.Map<Sick, SickDto>(entity);
        }

        /// <summary>
        /// 修改病历
        /// Update a medical record
        /// </summary>
        /// <remarks>
        /// PUT /api/app/medical/{id}
        /// 根据ID修改病历信息。
        /// </remarks>
        /// <param name="id">病历ID</param>
        /// <param name="input">病历修改参数</param>
        /// <returns>修改后的病历详细信息</returns>
        public async Task<SickDto> UpdateAsync(Guid id, CreateUpdateSickDto input)
        {
            var entity = await _repository.GetAsync(id);
            ObjectMapper.Map(input, entity);
            entity = await _repository.UpdateAsync(entity);
            return ObjectMapper.Map<Sick, SickDto>(entity);
        }

        /// <summary>
        /// 删除病历
        /// Delete a medical record
        /// </summary>
        /// <remarks>
        /// DELETE /api/app/medical/{id}
        /// 根据ID删除病历信息。
        /// </remarks>
        /// <param name="id">病历ID</param>
        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

    }
}
