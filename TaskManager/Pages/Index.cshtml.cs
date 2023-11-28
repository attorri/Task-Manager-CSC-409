﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManager.Pages
{
    public class IndexModel : PageModel
    {
        

        // Sample in-memory storage for tasks
        private static List<TaskItem> tasks = new List<TaskItem>
        {
            new TaskItem { Id = 1, Description = "Sample Task 1", DueDate = DateTime.Now.AddDays(7) },
            new TaskItem { Id = 2, Description = "Sample Task 2", DueDate = DateTime.Now.AddDays(14) }
        };


        public List<TaskItem> Tasks => tasks;

        public void OnGet()
        {
            // No action needed for now
        }

        // Add a new task
        public IActionResult OnPostAddTask(TaskItem newTask)
        {
            if (newTask.Id == 0)
                newTask.Id = tasks.Count + 1;
            tasks.Add(newTask);
            return RedirectToPage();
        }

        // ...
        //[BindProperty]
        //public TaskItem EditedTask { get; set; }

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
            for (int i=0; i< tasks.Count; i++)
            {
                if (tasks[i].Id == SearchTask.Id)
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

        // API: Getting all tasks
        public List<TaskItem> GetAllTasks() => tasks;

        // API: Getting all completed tasks
        public List<TaskItem> GetAllCompletedTasks() => tasks.Where(t => t.IsCompleted).ToList();

        // API: Getting a task by Id
        public TaskItem GetTaskById(int taskId) => tasks.FirstOrDefault(t => t.Id == taskId);

        // API: Changing a task's description by Id
        public IActionResult ChangeTaskDescriptionById(int taskId, string newDescription)
        {
            var taskToEdit = tasks.FirstOrDefault(t => t.Id == taskId);
            if (taskToEdit != null)
            {
                taskToEdit.Description = newDescription;
                return new OkResult();
            }
            return new NotFoundResult();
        }

        // API: Mark a task as completed by Id
        public IActionResult MarkTaskAsCompletedById(int taskId)
        {
            var taskToComplete = tasks.FirstOrDefault(t => t.Id == taskId);
            if (taskToComplete != null)
            {
                taskToComplete.IsCompleted = true;
                taskToComplete.CompletionDate = DateTime.Now;
                return new OkResult();
            }
            return new NotFoundResult();
        }
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
