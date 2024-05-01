using Cocona.Filters;

namespace ByReplace.Exceptions;

internal class GlobalHandleExceptionAttribute : CommandFilterAttribute
{
    readonly IPrint print = new PrintConsole();

    private readonly Dictionary<Type, Action<Exception>> handles;

    public GlobalHandleExceptionAttribute()
    {
        handles = new Dictionary<Type, Action<Exception>>
        {
            { typeof(NotFoundException), NotFoundHandle }
        };
    }

    private void NotFoundHandle(Exception ex)
    {
        print.Warning(ex.Message);
    }

    public override async ValueTask<int> OnCommandExecutionAsync(CoconaCommandExecutingContext ctx, CommandExecutionDelegate next)
    {
        try
        {
            return await next(ctx);
        }
        catch (Exception ex)
        {
            handles[typeof(NotFoundException)].Invoke(ex);

            return 1;
        }
    }
}
