using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VolgaIT2023
{
    public class ModelStateExceptionFilter : IActionResult
    {
        public async Task ExecuteResultAsync(ActionContext context)
        {
            var state = context.ModelState;
            context.HttpContext.Features.Set<ModelStateFeature>(new ModelStateFeature(state));
            return;
        }
    }
    public class ModelStateFeature
    {
        public ModelStateDictionary ModelState { get; set; }

        public ModelStateFeature(ModelStateDictionary state)
        {
            this.ModelState = state;
        }
    }
}