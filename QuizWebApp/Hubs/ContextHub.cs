using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using QuizWebApp.Models;

namespace QuizWebApp.Hubs
{
    /// <summary>
    /// Hub is only used as a notification mechanism.
    /// The actual actions are put into the controllers, where security is checked
    /// </summary>
    [HubName("Context")]
    public class ContextHub : Hub
    {
        public void UpdateCurrentState(ContextStateType state)
        {            
            Clients.All.CurrentStateChanged(state.ToString());
        }

        public void PlayerSelectedOptionIndex()
        {
            Clients.Others.PlayerSelectedOptionIndex();
        }
    }
}