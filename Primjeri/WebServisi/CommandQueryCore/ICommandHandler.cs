using System.Threading.Tasks;

namespace CommandQueryCore
{
  public interface ICommandHandler<TCommand>
  {
    void Handle(TCommand command);
    Task HandleAsync(TCommand command);
  }
}
