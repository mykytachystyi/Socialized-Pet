using Domain.Admins;

namespace Domain.Appeals.Repositories
{
    public interface IAppealMessageReplyRepository
    {
        void Create(AppealMessageReply reply);
        void Update(AppealMessageReply reply);
        AppealMessageReply Get(long id);
    }
}
