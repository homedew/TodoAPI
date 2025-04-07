using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TodoAPI.Helper
{
    public class BaseController: ControllerBase
    {
        protected IActionResult ApiOk<T>(T data) => Ok( new ApiResponse<T>{Success = true, Data = data});
        protected IActionResult ApiNotFound(string message) => NotFound(new ApiResponse<string> {Success = false, Message = message});
    }
}