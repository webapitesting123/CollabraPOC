
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FabrikamResidences_Activities.Models;

namespace FabrikamResidences_Activities.Data
{
    public interface IPortalRepository
    {
        IQueryable<PortalActivity> GetActivities();
        PortalActivity GetActivity(int id);
        void AddActivity(PortalActivity activity);
        void UpdateActivity(PortalActivity activity);
        void DeleteActivity(int id);

        void AddAttendee(Attendee attendee);
        IQueryable<Attendee> GetActivityAttendees(int activityId);

        void InitializeDatabase();
    }
}