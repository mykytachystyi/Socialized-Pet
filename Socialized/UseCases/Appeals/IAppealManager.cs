using Domain.Admins;
using UseCases.Appeals.Commands;
using UseCases.Response.Appeals;

namespace UseCases.Appeals
{
    public interface IAppealManager
    {
        Appeal Create(CreateAppealCommand command);
        ICollection<AppealResponse> GetAppealsByUser(string userToken, int since, int count);
        ICollection<AppealResponse> GetAppealsByAdmin(int since, int count);
        void UpdateAppealToClosed(long appealId);
        void UpdateAppealToAnswered(long appealId);
    }
}
