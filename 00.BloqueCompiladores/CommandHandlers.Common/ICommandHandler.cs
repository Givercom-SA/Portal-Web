namespace CommandHandlers.Common
{
    public interface ICommandHandler<in T> where T : Command
    {
        CommandResult Handle(T command);
    }
}
