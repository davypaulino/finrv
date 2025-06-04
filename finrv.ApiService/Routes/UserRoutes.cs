namespace finrv.ApiService.Routes;

public static class UserRoutes
{
    public static RouteGroupBuilder MapUsers(this RouteGroupBuilder builder)
    {
        builder.MapGet("", () =>
        {
            return "Hello World!";
        })
        .WithName("Get All Users")
        .WithTags("users")
        .WithDescription("Get all Users of system.")
        .WithDisplayName("Get All Users");

        return builder;
    }
}

