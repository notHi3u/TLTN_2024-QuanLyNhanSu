using Common.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Configurations
{
    public static class ApiResponseExtensions
    {
        public static RouteHandlerBuilder ConfigureApiResponses(this RouteHandlerBuilder builder)
        {
            return builder
                .Produces<BaseResponse<string>>(StatusCodes.Status200OK)
                .Produces<BaseResponse<string>>(StatusCodes.Status201Created)
                .Produces<BaseResponse<string>>(StatusCodes.Status202Accepted)
                .Produces<BaseResponse<string>>(StatusCodes.Status204NoContent)
                .Produces<BaseResponse<string>>(StatusCodes.Status400BadRequest)
                .Produces<BaseResponse<string>>(StatusCodes.Status401Unauthorized)
                .Produces<BaseResponse<string>>(StatusCodes.Status404NotFound)
                .Produces<BaseResponse<string>>(StatusCodes.Status500InternalServerError);
                
        }
    }

}
