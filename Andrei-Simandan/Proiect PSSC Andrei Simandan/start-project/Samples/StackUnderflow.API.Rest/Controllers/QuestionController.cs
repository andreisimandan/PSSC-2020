using Access.Primitives.EFCore;
using Access.Primitives.IO;
using Jaeger.Thrift.Agent;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using StackUnderflow.Domain.Core.Contexts.Question;
using StackUnderflow.Domain.Core.Contexts.Question.CreateQuestionOp;
using StackUnderflow.EF.Models;
using System.Linq;
using System.Threading.Tasks;

namespace StackUnderflow.API.AspNetCore.Controllers
{

    [ApiController]
    [Route("question")]
    public class QuestionController : Controller
    {
        private readonly IInterpreterAsync _interpreter;
        private readonly StackUnderflowContext _dbContext;
        private readonly IClusterClient _client; 

        public QuestionController(IInterpreterAsync interpreter, StackUnderflowContext dbContext, IClusterClient client)
        {
            _interpreter = interpreter;
            _dbContext = dbContext;
            _client = client;
        }

        [HttpPost("createquestion")]
        public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionCmd createQuestionCmd)
        {
            QuestionWriteContext ctx = new QuestionWriteContext(
                new EFList<Post>(_dbContext.Post),
                new EFList<User>(_dbContext.User));

            var expr = from createQuestionResult in QuestionDomain.CreateQuestion(createQuestionCmd)
                       select createQuestionResult;

            var r = await _interpreter.Interpret(expr, ctx, new object());
            _dbContext.SaveChanges();
            return r.Match(
                created => Ok(created.Question.PostId),
                 notCreated => StatusCode(StatusCodes.Status500InternalServerError, "Question could not be created."),
            invalidRequest => BadRequest("Invalid request."));
        }


       
        
    }
}

