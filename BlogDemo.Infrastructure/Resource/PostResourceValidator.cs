using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using BlogDemo.Infrastructure.Resource;

namespace BlogDemo.Infrastructure.Resource
{
    public class PostResourceValidator:AbstractValidator<PostResource>
    {
        public PostResourceValidator()
        {
            RuleFor(x => x.Author)
                .NotNull()
                .WithName("作者")
                .WithMessage("{PropertyName}是必需的")
                .MaximumLength(50)
                .WithMessage("{PropertyName}的最大长度是{MaxLength}");
        }
    }
}
