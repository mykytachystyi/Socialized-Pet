﻿using Domain.Admins;
using Domain.Appeals;
using Domain.Appeals.Repositories;

namespace Infrastructure.Repositories
{
    public class AppealMessageRepository : IAppealMessageRepository
    {
        public Context _context;
        public AppealMessageRepository(Context context)
        {
            _context = context;
        }
        public AppealMessage Create(AppealMessage message)
        {
            _context.AppealMessages.Add(message);
            _context.SaveChanges();
            return message;
        }
        public AppealMessage GetBy(long messageId)
        {
            return _context.AppealMessages.FirstOrDefault(m => m.Id == messageId);
        }
        public void Update(AppealMessage message)
        {
            _context.AppealMessages.Update(message);
            _context.SaveChanges();
        }
    }
}
