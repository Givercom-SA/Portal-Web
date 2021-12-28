using Autofac;
using Microsoft.Extensions.Logging;
using System;

namespace CommandHandlers.Common
{
    public class CommandDispatcher
    {
        private readonly ILifetimeScope container;
        private readonly ILogger<CommandDispatcher> logger;

        public CommandDispatcher(ILifetimeScope container, ILogger<CommandDispatcher> logger)
        {
            this.container = container;
            this.logger = logger;
        }

        public CommandResult Dispatch<T>(T command) where T : Command
        {
            using (var scope = container.BeginLifetimeScope())
            {
                try
                {
                    var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
                    dynamic handler = scope.ResolveOptional(handlerType);

                    var result = handler.Handle(command as dynamic);

                    return result;
                }
                catch (Exception e)
                {
                    e = this.Unwrap(e);
                    logger.LogError(e, e.Message);

                    throw e;
                }
            }
        }

        private Exception Unwrap(Exception ex)
        {
            while (null != ex.InnerException)
            {
                ex = ex.InnerException;
            }

            return ex;
        }
    }
}
