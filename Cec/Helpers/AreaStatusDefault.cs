using Cec.Models;
using System;
using System.Linq;

namespace Cec.Helpers
{
    public class DefaultStatus
    {
        //Private Properties
        private ApplicationDbContext _db = new ApplicationDbContext();
        private readonly string _defaultStatus;

        public DefaultStatus()
        {
            _defaultStatus = "Plan";
        }

        public Guid GetDefault()
        {
            return _db.Statuses.Where(s => s.Designation == _defaultStatus).First().StatusId;
        }
    }
}