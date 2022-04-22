using System;

namespace Todo.Models;

public class TodoItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Person {get;set;}

    public DateTime Time {get;set;}

}
