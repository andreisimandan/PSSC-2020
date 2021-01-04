using EarlyPay.Primitives.ValidationAttributes;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StackUnderflow.Domain.Core.Contexts.Question.CreateQuestionOp
{
    public struct CreateQuestionCmd
    {
        public CreateQuestionCmd(int questionId, int userId, string title, string postText)
        {
            PostId = questionId;
            TenantId = userId;
            Title = title;
            PostText = postText;
        }

        [Required]
        public int PostId { get; set; }
        [Required]
        public int TenantId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string PostText { get; set; }
         
    }
}

