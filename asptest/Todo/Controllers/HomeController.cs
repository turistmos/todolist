using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Todo.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Data.Sqlite;
using System;
using Todo.Models.ViewModels;
namespace Todo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var todoListViewModel = GetAllTodos();
        return View(todoListViewModel);
    }

    [HttpGet]
    public JsonResult PopulateForm(int id)
    {
        var todo=GetById(id);
        return Json(todo);
    }

// skriver ut bord
    internal TodoViewModel GetAllTodos()
    {
        List<TodoItem> todoList = new();

        using (SqliteConnection con = 
        new SqliteConnection("Data Source=db.sqlite"))
        {
            using (var tableCmd = con.CreateCommand())
            {
                con.Open();
                tableCmd.CommandText= "SELECT * FROM todo2";

                using (var reader = tableCmd.ExecuteReader())
                {
                    if(reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            todoList.Add(
                                new TodoItem
                                {
                                    Id= reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Person = reader.GetString(2)
                                });
                        }
                    }
                    else 
                    {
                        return new TodoViewModel
                        {
                            TodoList=todoList
                        };
                    }
                };
            }
        }

        return new TodoViewModel 
        {
            TodoList= todoList
        };
    }

         internal TodoItem GetById(int id)
    {
        TodoItem todo = new();

        using (var connection = 
        new SqliteConnection("Data Source=db.sqlite"))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();
                tableCmd.CommandText= $"SELECT * FROM todo2 Where Id = '{id}'";

                using (var reader = tableCmd.ExecuteReader())
                {
                    if(reader.HasRows)
                    {
                       reader.Read();
                        todo.Id= reader.GetInt32(0);
                        todo.Name = reader.GetString(1);
                        todo.Person = reader.GetString(2);
                    }
                    else 
                    {
                        return todo;
                       
                    }
                };
            }
        }

        return todo;
    }

   public RedirectResult Insert(TodoItem todo)
   {
       using (SqliteConnection con = 
       new SqliteConnection("Data Source=db.sqlite"))
       {
           using (var tableCmd = con.CreateCommand())
           {
               con.Open();
               tableCmd.CommandText = $"INSERT INTO todo2 (person,name) VALUES ('{todo.Person}','{todo.Name}')";
               try
               {
                   tableCmd.ExecuteNonQuery();
               }
               catch (Exception ex)
               {
                   
                   Console.WriteLine(ex.Message);
               }
           }
       }
       return Redirect("https://localhost:7229/");
   }

    public JsonResult Delete(int id)
    {
        using (SqliteConnection con=
        new SqliteConnection("Data Source=db.sqlite"))
        {
            using (var tableCmd = con.CreateCommand())
            {
                con.Open();
                tableCmd.CommandText = $"DELETE from todo2 WHERE Id = '{id}'";
                tableCmd.ExecuteNonQuery();
            }
        }
        return Json(new {});
    }

    public RedirectResult Update(TodoItem todo)
    {
        using (SqliteConnection con = 
        new SqliteConnection("Data Source=db.sqlite"))
        {
            using (var tableCmd = con.CreateCommand())
            {
                con.Open();
                tableCmd.CommandText = $"UPDATE todo2 SET name = '{todo.Name}'WHERE Id = '{todo.Id}'";
                tableCmd.CommandText = $"UPDATE todo2 SET person = '{todo.Person}'WHERE Id = '{todo.Id}'";
                try
                {
                    tableCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        return Redirect("https://localhost:7229/");
    }
}