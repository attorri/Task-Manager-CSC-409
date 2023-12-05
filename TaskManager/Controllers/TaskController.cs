using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManager.Controllers
{
    using Pages;

    [ApiController]
    [Route("[controller]/[action]")]
    public class TaskController : ControllerBase
    {
        // GET: /<controller>/
        public string Hello()
        {
            return "hello";
        }

        [Route("{id:int}")]
        public string Complete(int id)
        {
            var task  = IndexModel.tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                return "Coudln't find the task";

            task.IsCompleted = true;
            task.CompletionDate = DateTime.Now;
            return "Success";
        }
    }
}

