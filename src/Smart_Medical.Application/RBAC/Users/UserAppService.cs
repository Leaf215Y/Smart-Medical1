﻿using AutoMapper.Internal.Mappers;
using MD5Hash;
using Microsoft.AspNetCore.Mvc;
using Smart_Medical.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Smart_Medical.RBAC.Users
{
    //[ApiExplorerSettings(GroupName = "用户管理")]
    public class UserAppService : ApplicationService, IUserAppService
    {
        private readonly IRepository<User, Guid> _userRepository;

        public UserAppService(IRepository<User, Guid> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ApiResult> CreateAsync(CreateUpdateUserDto input)
        {
            input.UserPwd = input.UserPwd.GetMD5();
            var user = ObjectMapper.Map<CreateUpdateUserDto, User>(input);
            var result = await _userRepository.InsertAsync(user);
            return ApiResult.Success(ResultCode.Success);
        }

        public async Task<ApiResult> DeleteAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                return ApiResult.Fail("用户不存在", ResultCode.NotFound);
            }
            await _userRepository.DeleteAsync(user);
            return ApiResult.Success(ResultCode.Success);
        }

        public async Task<ApiResult<UserDto>> GetAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                return ApiResult<UserDto>.Fail("用户不存在", ResultCode.NotFound);
            }
            var userDto = ObjectMapper.Map<User, UserDto>(user);
            return ApiResult<UserDto>.Success(userDto, ResultCode.Success);
        }

        public async Task<ApiResult<PageResult<List<UserDto>>>> GetListAsync([FromQuery] Seach seach)
        {
            var list = await _userRepository.GetQueryableAsync();

            var totalCount = list.Count();
            var totalPage = (int)Math.Ceiling((double)totalCount / seach.PageSize);
            var pagedList = list.Skip((seach.PageIndex - 1) * seach.PageSize).Take(seach.PageSize).ToList();
            var userDtos = ObjectMapper.Map<List<User>, List<UserDto>>(pagedList);

            var pageResults = new PageResult<List<UserDto>>
            {
                TotleCount = totalCount,
                TotlePage = totalPage,
                Data = userDtos
            };

            return ApiResult<PageResult<List<UserDto>>>.Success(pageResults, ResultCode.Success);
        }

        public async Task<ApiResult<UserDto>> LoginAsync(LoginDto loginDto)
        {
            // 根据用户名查找用户
            var users = await _userRepository.GetQueryableAsync();
            var user = users.FirstOrDefault(u => u.UserName == loginDto.UserName);

            // 检查用户是否存在
            if (user == null)
            {
                return ApiResult<UserDto>.Fail("用户名不存在", ResultCode.NotFound);
            }

            // 验证密码
            if (user.UserPwd != loginDto.UserPwd.GetMD5())
            {
                return ApiResult<UserDto>.Fail("密码错误", ResultCode.ValidationError);
            }

            // 登录成功，返回用户信息
            var userDto = ObjectMapper.Map<User, UserDto>(user);
            return ApiResult<UserDto>.Success(userDto, ResultCode.Success);
        }

        public async Task<ApiResult> UpdateAsync(Guid id, CreateUpdateUserDto input)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                return ApiResult.Fail("用户不存在", ResultCode.NotFound);
            }



            var updatedUser = ObjectMapper.Map(input, user);
            updatedUser.UserPwd= user.UserPwd.GetMD5(); // 确保密码被加密
            await _userRepository.UpdateAsync(updatedUser);
            return ApiResult.Success(ResultCode.Success);
        }
    }
}
