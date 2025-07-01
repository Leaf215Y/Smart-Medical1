import request from '@/utils/request';

// API地址 (根据你的Swagger)
const api_name = '/api/app/drug';

/**
 * 封装药品管理相关API
 */
const DrugAPI = {
  /**
   * 获取药品分页列表
   * @param params 查询参数，包含pageIndex, pageSize, drugName, drugType等
   */
  getDrugList(params) {
    return request({
      url: api_name,
      method: 'get',
      params: params
    });
  },

  /**
   * TODO: 获取药品详情
   */
  getDrugDetail(id) {
    return request({
      url: `${api_name}/${id}`,
      method: 'get'
    });
  },

  /**
   * TODO: 新增药品
   */
  addDrug(data) {
    return request({
      url: api_name,
      method: 'post',
      data: data
    });
  },

  /**
   * TODO: 修改药品
   */
  updateDrug(id, data) {
    return request({
      url: `${api_name}/${id}`,
      method: 'put',
      data: data
    });
  },

  /**
   * TODO: 删除药品
   */
  deleteDrug(id) {
    return request({
      url: `${api_name}/${id}`,
      method: 'delete'
    });
  }
};

export default DrugAPI;
