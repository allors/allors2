using System.Web.Mvc;

using Common.Logging;

public class NLogExceptionFilter : IExceptionFilter
{
    private readonly ILog log;

    public NLogExceptionFilter()
    {
        this.log = LogManager.GetLogger(this.GetType());
    }

    public void OnException(ExceptionContext context)
    {
        this.log.Error(context.Exception);
    }
}