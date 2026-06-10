using System.Collections;

public interface IAction
{
    IEnumerator Execute(ActionContext context);
}
