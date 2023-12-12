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

        /*
         * 
         * 
            Make a web application/api for managing tasks.
            A task should have the followings:
            Id (C# data type: int)
            Description (C# data type: string)
            Due date (C# data type: DateTime)
            Is completed? (C# data type: bool)
            Completion date (C# data type: DateTime)
            Required functionalities:
            Showing all tasks
            Adding a new task
            Removing a task
            Editing a task
            Mark a task as completed
            Saving the date in the completion date property
            API for the followings:
            Getting all tasks
            Getting all completed tasks
            Getting a task by Id
            Changing a task's description by Id
            Mark a task as completed by Id
            Optional functionalities (for extra points):
            A mean to sort tasks (by any property)
            Search by description
            Filtering by completion status and due/completion dates
            Saving data into a database (any database)


         * 
        */

        // Demo

        public string Hello()
        {
            return "hi hi hi ";  // returned if you type 'hello'
        }

        // Mark Task As Completed by ID

        [Route("{id:int}")]
        public string Complete(int id) // Type "Complete/{id}" i.e.: if you want to mark task 3 as completed, "Complete/3"
        {
            var task  = IndexModel.tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                return "Coudln't find the task";

            task.IsCompleted = true;
            task.CompletionDate = DateTime.Now;
            return "Success! Task "+ id + " is now complete!";
        }

        [Route("{date:DateTime}/{description}")]
        public String AddTask(DateTime date, String description)
        {
            int id = IndexModel.tasks.Count + 1;
            bool isCompleted = false;
            TaskItem newTask = new TaskItem { Id = id, Description = description, DueDate = date };
            IndexModel.tasks.Add(newTask);
            return "Success New Task - id: " + newTask.Id.ToString() + ", Description: " + newTask.Description + ", Due Date: " + newTask.DueDate.ToString(); 
        }


        // Changing Task's Description by Id

        [Route("{id:int}/{newDescription}")]

        public string ChangeDescriptionByID(int id, String newDescription)
        {
            var task = IndexModel.tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                return "Coudln't find the task";

            task.Description = newDescription; // Changing the description to the 'newDescription

            String CompletionStatus = "Incomplete";

            String taskToReturn = "{ ID: " + task.Id.ToString() + ", Description: " + task.Description + ", Due Date: " + task.DueDate.ToString() + ", Completion Status: " + CompletionStatus + " }";

            if (task.IsCompleted == true)
            {
                CompletionStatus = "Complete";
                taskToReturn = "{ ID: " + task.Id.ToString() + ", Description: " + task.Description + ", Due Date: " + task.DueDate.ToString() + ", Completion Status: " + CompletionStatus + ", Completion Date: " + task.CompletionDate.ToString() + " }";
            }

            

            return "Success! New Task #" + id + "- " + taskToReturn;
        }

        // Search by Description (BONUS)

        [Route("{description}", Order = 1)]
        public string SearchDescription(String description)
        {
            var task = IndexModel.tasks.FirstOrDefault(t => t.Description == description);
            if (task == null)
                return "Coudln't find the task";

            task.Description = description;

            String CompletionStatus = "Incomplete";

            String taskToReturn = "{ ID: " + task.Id.ToString() + ", Description: " + task.Description + ", Due Date: " + task.DueDate.ToString() + ", Completion Status: " + CompletionStatus + " }";

            if (task.IsCompleted == true)
            {
                CompletionStatus = "Complete";
                taskToReturn = "{ ID: " + task.Id.ToString() + ", Description: " + task.Description + ", Due Date: " + task.DueDate.ToString() + ", Completion Status: " + CompletionStatus + ", Completion Date: " + task.CompletionDate.ToString() + " }";
            }

            return taskToReturn;
        }

        // Getting a task by ID

        [Route("{id:int}")]
        public string SearchID(int id)
        {
            var task = IndexModel.tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                return "Coudln't find the task";

            String CompletionStatus = "Incomplete";

            String taskToReturn = "{ ID: " + task.Id.ToString() + ", Description: " + task.Description + ", Due Date: " + task.DueDate.ToString() + ", Completion Status: " + CompletionStatus + " }";

            if (task.IsCompleted == true)
            {
                CompletionStatus = "Complete";
                taskToReturn = "{ ID: " + task.Id.ToString() + ", Description: " + task.Description + ", Due Date: " + task.DueDate.ToString() + ", Completion Status: " + CompletionStatus + ", Completion Date: " + task.CompletionDate.ToString() + " }";
            }

            return taskToReturn;
        }


        // Getting All Tasks

        public String GetAllTasks()
        {
            //int numTasks = IndexModel.tasks.Count;

            Stack<String> AllTasksStack = new Stack<String>();

            foreach (var task in IndexModel.tasks)
            {

                String CompletionStatus = "Incomplete";
                
                String taskToAdd = "{ ID: " + task.Id.ToString() + ", Description: " + task.Description + ", Due Date: " + task.DueDate.ToString() + ", Completion Status: " + CompletionStatus + " }, ";

                if (task.IsCompleted == true)
                {
                    CompletionStatus = "Complete";
                    taskToAdd = "{ ID: " + task.Id.ToString() + ", Description: " + task.Description + ", Due Date: " + task.DueDate.ToString() + ", Completion Status: " + CompletionStatus + ", Completion Date: " + task.CompletionDate.ToString() + " }, ";
                }

                AllTasksStack.Push(taskToAdd);
            }

            String AllTasks = "";

            if (AllTasksStack.Count > 0)
            {
                /*
                 * Adding it in reverse, because Stacks are structured with the last string being the first element
                 * And the first input string being the last element
                 */

                for (int i = AllTasksStack.Count-1; i >=0; i--) 
                {
                    AllTasks += AllTasksStack.ElementAt(i);
                }
                return AllTasks;
            }

            return ("no current tasks");
        }

        // Getting All Completed Tasks

        public String GetCompletedTasks()
        {
            //int numTasks = IndexModel.tasks.Count;

            Stack<String> CompletedTasksStack = new Stack<String>();

            foreach( var task in IndexModel.tasks)
            {
                if (task.IsCompleted == true)
                {
                    String taskToAdd = "{ ID: " + task.Id.ToString() + ", Description: " + task.Description + ", Due Date: " + task.DueDate.ToString() + ", Completion Status: Complete" + ", Completion Date: " + task.CompletionDate.ToString() + " }, ";
                    CompletedTasksStack.Push(taskToAdd);
                }
            }

            String CompleteTasks = "";

            if (CompletedTasksStack.Count>0)
            {
                /*
                 * Adding it in reverse, because Stacks are structured with the last string being the first element
                 * And the first input string being the last element
                 */

                for (int i = CompletedTasksStack.Count-1; i>=0; i--)
                {
                    CompleteTasks += CompletedTasksStack.ElementAt(i);
                }
                return CompleteTasks;
            }

            return ("no completed tasks");
        }


    }
}

