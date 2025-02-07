using System.Security.Principal;
using IDMApi.Models;

public interface IUserServices
{
    Task<(string Token, string Role, string Username, int Id)> AuthenticateUser(User user);

}