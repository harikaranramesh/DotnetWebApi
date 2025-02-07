using Microsoft.AspNetCore.Mvc;
using IDMApi.Models;

public interface ITaskServices
{
    Task<Employee> GetAssignedTask(int id);
    Task AssignTask(int id,string task);
    

}