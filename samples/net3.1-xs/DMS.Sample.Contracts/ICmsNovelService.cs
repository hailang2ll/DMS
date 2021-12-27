using DMS.Common.Model.Result;
using DMS.Sample.Contracts.Param;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Sample.Contracts
{
    public interface ICmsNovelService
    {
        Task<ResponseResult> Add(AddCmsNovelParam param);
    }
}
