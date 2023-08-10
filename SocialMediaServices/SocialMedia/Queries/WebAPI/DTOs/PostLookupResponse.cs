using Domain.Entities;
using SharedKernel.DTOs;
using System.Collections.Generic;

namespace WebAPI.DTOs
{
    public class PostLookupResponse : BaseResponse
    {
        public List<Post> Posts { get; set; }
    }
}