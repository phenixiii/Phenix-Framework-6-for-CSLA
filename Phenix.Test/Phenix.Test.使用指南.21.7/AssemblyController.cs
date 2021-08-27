using System;
using System.Net.Http;
using System.Web.Http;

namespace Phenix.Test.使用指南._21._7.Plugin
{
    /*
     * 直接继承 System.Web.Http.ApiController 实现公共服务
     */
    //public sealed class AssemblyController : System.Web.Http.ApiController
    /*
     * 打上[AllowAnonymous]后继承 Phenix.Core.Web.ApiController<T> 实现公共服务
     */
    [AllowAnonymous]
    public sealed class AssemblyController : Phenix.Core.Web.ApiController<AssemblyController>
    {
        public HttpResponseMessage Get(string name)
        {
            try
            {
                /*
                 * 普通写法
                 */
                //return Phenix.Core.Web.Utilities.PackResponse(Request, AssemblyEasy.FetchList(new AssemblyCriteria() {Name = name}));
                /*
                 * 直接返回JSON字符串, 省去了构造实体对象和序列化的过程
                 */
                //return FetchList<AssemblyEasy>(new AssemblyCriteria() {Name = name});
                /*
                 * 直接传入表达式查询条件
                 */
                return FetchList<AssemblyEasy>(p => p.Name.Contains(name));
            }
            catch (Exception ex)
            {
                return Phenix.Core.Web.Utilities.PackErrorResponse(Request, ex);
            }
        }
    }
}
