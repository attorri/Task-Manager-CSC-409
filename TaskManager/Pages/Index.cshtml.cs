using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace TaskManager.Pages
{
    public class IndexModel : PageModel
    {
        

        // Sample in-memory storage for tasks
        private static List<TaskItem> tasks = new List<TaskItem>
        {
            new TaskItem { Id = 1, Description = "Task 1", DueDate = DateTime.Now.AddDays(14) },
            new TaskItem { Id = 2, Description = "Task 2", DueDate = DateTime.Now.AddDays(31) }
        };


        public List<TaskItem> Tasks => tasks;

        

        // Add a new task
        public IActionResult OnPostAddTask(TaskItem newTask)
        {
            if (newTask.Id == 0)
                newTask.Id = tasks.Count + 1;
            tasks.Add(newTask);
            return RedirectToPage();
        }


        public IActionResult OnPostEditTask(TaskItem editedTask, int editedId, string editedDescription, DateTime editedDueDate, int taskId)
        {
            
                var existingTask = tasks.FirstOrDefault(t => t.Id == taskId);

                if (existingTask != null)
                {
                    editedTask.Id = editedId;
                    editedTask.Description = editedDescription;
                    editedTask.DueDate = editedDueDate;

                    // Update the existing task with the edited values
                    existingTask.Id = editedTask.Id;
                    existingTask.Description = editedTask.Description;
                    existingTask.DueDate = editedTask.DueDate;

                    // Optionally, update other properties as needed

                    // Clear the edited task property
                    editedTask = null;

                    return RedirectToPage();
                }
                else
                {
                    return NotFound();
                }

            // old edit button (on frontend) -  <!-- <button type="button" class="edit-button" onclick="editTask('@task.Id', '@task.Description', '@task.DueDate.ToString("yyyy-MM-dd")')">Edit</button> -->

        }

        public TaskItem SearchTask = new TaskItem();

        public IActionResult OnPostSearchTask(int SearchedID)
        {
            SearchTask.Id = SearchedID;
            int maxTasks = tasks.Count - 1;
            bool matchTasks = false;
            var searchedTask = tasks.FirstOrDefault(t => t.Id == SearchTask.Id);
            for (int i=0; i< tasks.Count; i++)
            {
                if (tasks[i].Id == searchedTask.Id)
                {
                    matchTasks = true;
                    SearchTask.Description = tasks[i].Description;
                    SearchTask.DueDate = tasks[i].DueDate;
                    SearchTask.IsCompleted = tasks[i].IsCompleted;
                    SearchTask.CompletionDate = tasks[i].CompletionDate;
                    SearchTask.Description = SearchTask.Description;
                    SearchTask.DueDate = SearchTask.DueDate;
                    SearchTask.IsCompleted = SearchTask.IsCompleted;
                    SearchTask.CompletionDate = SearchTask.CompletionDate;
                }
                if (i == maxTasks && matchTasks == false)
                    return RedirectToPage(); // Originally I had return NotFound(), but that was impractical
            }
            return RedirectToPage();
        }
        //Testing Testingggg

        // ...


        // Remove a task
        public IActionResult OnPostRemoveTask(int taskId)
        {
            var taskToRemove = tasks.FirstOrDefault(t => t.Id == taskId);
            if (taskToRemove != null)
            {
                tasks.Remove(taskToRemove);
            }
            return RedirectToPage();
        }
        // Mark a task as completed
        public IActionResult OnPostMarkCompleted(int taskId)
        {
            var taskToComplete = tasks.FirstOrDefault(t => t.Id == taskId);
            if (taskToComplete != null)
            {
                taskToComplete.IsCompleted = true;
                taskToComplete.CompletionDate = DateTime.Now;
            }
            return RedirectToPage();
        }

        [BindProperty]
        public int searchedID { get; set; }

        [BindProperty]
        public string searchedDescription { get; set; }

        public TaskItem TaskDetails { get; set; }

        [BindProperty]
        public TaskItem searchedTask { get; set; }

        public List<TaskItem> searchedTasksList { get; set; } = new List<TaskItem>();
        

        public static TaskItem searchedTaskByID = new TaskItem();
        public TaskItem searchedTasksIDMethod => searchedTaskByID;

        public IActionResult OnPostSearchTaskByID()
        {
            if (searchedID>0 && searchedID<=tasks.Count)
            {
                TaskDetails = tasks.FirstOrDefault(t => t.Id == searchedID);
                if (TaskDetails!= null)
                {
                    /*
                     * In case I convert it back to string stack
                    String Id = TaskDetails.Id.ToString();
                    String Description = TaskDetails.Description;
                    String DueDate = TaskDetails.DueDate.ToString();
                    String CompletionStatus = "Incomplete";
                    if (TaskDetails.IsCompleted==true)
                    {
                        CompletionStatus = "Complete";
                    }
                    searchedTaskByID.Push(Id);
                    searchedTaskByID.Push(Description);
                    searchedTaskByID.Push(DueDate);
                    searchedTaskByID.Push(CompletionStatus);
                    */
                    searchedTaskByID.Id = searchedID;
                    searchedTaskByID.Description = TaskDetails.Description;
                    searchedTaskByID.DueDate = TaskDetails.DueDate;
                    searchedTaskByID.IsCompleted = TaskDetails.IsCompleted;
                }
                
                if (searchedTaskByID.Id>0 && !(searchedTaskByID.Description.Equals(""))) // Second part of if statement is to check if it's a null task
                    return RedirectToPage();
            }
            return RedirectToPage();

            // returning "Task Not Found" is very sloppy and unprofessional, so i'm not currently doing that
            //return Content("Task Not Found");
        }

        private static TaskItem searchedTaskByDescription = new TaskItem();
        public TaskItem searchedTaskByDescriptionMethod => searchedTaskByDescription;

        public IActionResult OnPostSearchTaskByDescription()
        {
            TaskDetails = tasks.FirstOrDefault(t => t.Description.Equals(searchedDescription));
            if (TaskDetails != null)
            {
                searchedTaskByDescription.Id = TaskDetails.Id;
                searchedTaskByDescription.Description = searchedDescription;
                searchedTaskByDescription.DueDate = TaskDetails.DueDate;
                searchedTaskByDescription.IsCompleted = TaskDetails.IsCompleted;
            }

            if (searchedTaskByDescription.Id > 0 && !(searchedTaskByDescription.Description.Equals(""))) // Second part of if statement is to check if it's a null task
            {
                return RedirectToPage();
            }
            return Content("not found");
        }

        /*
         * OnPost method to return a JSON file
         * Can also use this functionality to return a .txt file
        */

        /*

        public IActionResult OnPostGetTaskByDescription()
        {
            TaskDetails = tasks.FirstOrDefault(t => t.Description == searchedDescription);
            if (TaskDetails != null)
            {
                // Convert task details to a JSON object and return
                var json = JsonSerializer.Serialize(new
                {
                    Id = TaskDetails.Id,
                    Description = TaskDetails.Description,
                    DueDate = TaskDetails.DueDate.ToString("yyyy-MM-dd"),
                    IsCompleted = TaskDetails.IsCompleted,
                    CompletionDate = TaskDetails.CompletionDate.HasValue ? TaskDetails.CompletionDate.Value.ToString("yyyy-MM-dd") : null
                });
                //TaskDetails = null;

                // Convert JSON to bytes
                var bytes = Encoding.UTF8.GetBytes(json);

                // Return a FileResult with the JSON data
                return new FileContentResult(bytes, "application/json")
                {
                    FileDownloadName = $"task_{searchedID}.json"
                };
            }
            else
            {
                return Content("Task not found");
            }
        }
        */



    }

    public class TaskItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletionDate { get; set; }
    }
}
