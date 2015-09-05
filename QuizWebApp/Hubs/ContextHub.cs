using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using QuizWebApp.Models;
using System;

namespace QuizWebApp.Hubs
{
    [HubName("Context")]
    public class ContextHub : Hub
    {
        public void UpdateCurrentState(ContextStateType state)
        {
            using (var db = new QuizWebAppDb())
            {
                var context = db.Contexts.First();
                context.CurrentState = state;

                // if change state to "3:show answer", judge to all players.
                if (state == ContextStateType.ShowCorrectAnswer)
                {
                    var answers = db
                        .Answers
                        .Where(a => a.QuestionID == context.CurrentQuestionID)
                        .ToList();
                    var currentQuestion = db.Questions.Find(context.CurrentQuestionID);

                    answers
                        .ForEach(a => a.Status =
                            a.ChoosedOptionIndex == currentQuestion.IndexOfCorrectOption
                            ? AnswerStateType.Correct : AnswerStateType.Incorrect);
                }

                db.SaveChanges();
            }

            Clients.All.CurrentStateChanged(state.ToString());
        }

        public void PlayerSelectedOptionIndex(int answerIndex)
        {
            using (var db = new QuizWebAppDb())
            {
                var context = db.Contexts.First();
                if(context.CurrentState != ContextStateType.ChooseTheAnswer)
                {
                    throw new InvalidOperationException("Changing your answer is disabled at this time!");
                }
                var playerId = Context.User.Identity.UserId();
                var questionId = context.CurrentQuestionID;
                var answer = db.Answers.First(a => a.PlayerID == playerId && a.QuestionID == questionId);
                answer.ChoosedOptionIndex = answerIndex;
                answer.Status = AnswerStateType.Pending;/*entried*/

                db.SaveChanges();
            }

            Clients.Others.PlayerSelectedOptionIndex();
        }
    }
}