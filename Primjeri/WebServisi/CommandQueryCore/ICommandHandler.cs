using System.Threading.Tasks;

namespace CommandQueryCore
{
  public interface ICommandHandler<TCommand>
  {    
    Task HandleAsync(TCommand command);
  }
}
