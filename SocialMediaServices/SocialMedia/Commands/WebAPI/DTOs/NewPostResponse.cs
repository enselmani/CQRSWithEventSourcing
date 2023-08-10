using SharedKernel.DTOs;
using System;

namespace WebAPI.DTOs
{
    public class NewPostResponse : BaseResponse
    {
        public Guid Id { get; set; }
    }
}