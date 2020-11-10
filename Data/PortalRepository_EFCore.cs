
using System.Collections.Generic;
using System.Linq;
using FabrikamResidences_Activities.Models;
using Microsoft.EntityFrameworkCore;

namespace FabrikamResidences_Activities.Data
{
    public class PortalRepository_EFCore : IPortalRepository
    {
        private PortalContext _portalContext;
        public PortalRepository_EFCore(PortalContext portalContext)
        {
            _portalContext = portalContext;
        }

        #region Manage Database Schema
      
        public void InitializeDatabase()
        {
            // _portalContext.Database.Migrate();
        }
        
        #endregion 
       
        #region IPortalRepository Members
        public void AddActivity(PortalActivity activity)
        {
            _portalContext.PortalActivity.Add(activity);
            _portalContext.SaveChanges();
        }

        public void DeleteActivity(int id)
        {
            var activity = GetActivity(id);
            _portalContext.Remove(activity);
            _portalContext.SaveChanges();
        }

        public IQueryable<PortalActivity> GetActivities()
        {
            return _portalContext.PortalActivity.OrderBy(a => a.Date);
        }

        public PortalActivity GetActivity(int id)
        {
            return _portalContext.PortalActivity
                .Include(activity => activity.Attendees)
                .Where(a => a.Id == id)
                .FirstOrDefault();
        }

        public void UpdateActivity(PortalActivity activity)
        {
            _portalContext.PortalActivity.Update(activity);
            _portalContext.SaveChanges();
        }

        public void AddAttendee(Attendee attendee)
        {
            var activity = GetActivity(attendee.PortalActivityId);
            activity.Attendees.Add(attendee);
            _portalContext.SaveChanges();
        }

        public IQueryable<Attendee> GetActivityAttendees(int activityId)
        {
            return _portalContext.Attendee
                .Where(a => a.PortalActivityId == activityId)
                .OrderBy(a => a.FirstName);                           
            
        }
    
        #endregion 
        
    }
}