using CarMinder.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, UserManager<User> userManager)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != "" && token != null)
            await AttachUserToContext(context, userManager, token);

        await _next(context);
    }

    private async Task AttachUserToContext(HttpContext context, UserManager<User> userManager, string token)
    {
        // przetwarzanie tokena
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        if (jsonToken == null)
        {
            return; // Nieprawidłowy format tokenu
        }

        // wyciaganie userId z tokena (na podstawie Subject)
        var userId = jsonToken.Subject;

        if (userId == null)
        {
            return;
        }

        // wyszukiwanie użytkownika na podstawie jego id
        var user = await userManager.FindByIdAsync(userId);

        User userData = user;
        context.Items["UserData"] = userData; // Przekazanie użytkownika do kontekstu
    }
}